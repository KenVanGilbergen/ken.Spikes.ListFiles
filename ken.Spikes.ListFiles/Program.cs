using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ken.Core;
using ken.Spikes.ListFiles.Implementations;
using ken.Windows;

namespace ken.Spikes.ListFiles
{
    //TODO: retry/skip access denied errrors - https://msdn.microsoft.com/en-us/library/dd997370(v=vs.110).aspx
    class Program
    {
        //private const string DefaultDir = @"D:\Websites\foo";
        private const string DefaultDir = @"D:\Websites\multiple_sites_red";
        //private const string DefaultDir = @"D:\Websites";
        private const bool DefaultWriteToConsole = false;

        private static void Main(string[] args)
        {
            Console.Write("Directory to enumerate [{0}]: ", DefaultDir);
            var sourceDir = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(sourceDir)) sourceDir = DefaultDir;

            Console.WriteLine("Write to console [{0}]: ", DefaultWriteToConsole ? "y" : "n");
            var press = Console.ReadKey();
            var writeToConsole = DefaultWriteToConsole;
            if ((press.KeyChar == 'Y') || (press.KeyChar == 'y')) writeToConsole = true;
            else if ((press.KeyChar == 'N') || (press.KeyChar == 'n')) writeToConsole = false;

            var sw = new Stopwatch();

            var results = new Dictionary<string, long>();
            var actions = new List<Func<string, IEnumerable<string>>>
            {
                Implementation.DirectoryGetFiles,
                Implementation.DirectoryGetFilesWithYield,
                Implementation.DirectoryInfoEnumerateFilesFullName,
                Implementation.FindFilesEnumerateFilesPath,
                Implementation.DirectoryRecursiveFullName,
                Implementation.DirectoryAsParallel,
                Implementation.DirectoryAsStack,
            };

            var actionsFileInfo = new List<Func<string, IEnumerable<FileInfo>>>
            {
                Implementation.DirectoryInfoEnumerateFiles,
                Implementation.DirectoryRecursive
            };

            var actionsFileData = new List<Func<string, IEnumerable<FileData>>>
            {
                Implementation.FindFilesEnumerateFiles
            };

            for (var i = 1; i <= 3; i++)
            {
                foreach (var action in actions)
                {
                    Console.WriteLine("===== {0} =====", action.Method.Name + i);
                    sw.Restart();
                    var files = action.Invoke(sourceDir).ToList();
                    foreach (var file in files)
                    {
                        if (writeToConsole) Console.WriteLine(file);
                    }
                    sw.Stop();
                    results.Add(action.Method.Name + i, sw.ElapsedMilliseconds);
                    Console.WriteLine("> {0}ms for {1} files.", sw.ElapsedMilliseconds, files.Count);
                }
                if (i == 1) actions.Reverse();
                if (i == 2) actions.Shuffle();
            }

            for (var i = 1; i <= 3; i++)
            {
                foreach (var action in actionsFileInfo)
                {
                    Console.WriteLine("===== {0} =====", action.Method.Name);
                    sw.Restart();
                    var fileInfos = action.Invoke(sourceDir).ToList();
                    foreach (var fileInfo in fileInfos)
                    {
                        if (writeToConsole) Console.WriteLine("{0} {1}", fileInfo.FullName, fileInfo.LastWriteTimeUtc);
                    }
                    sw.Stop();
                    results.Add(action.Method.Name + i, sw.ElapsedMilliseconds);
                    Console.WriteLine("> {0}ms for {1} files.", sw.ElapsedMilliseconds, fileInfos.Count);
                }
                if (i == 1) actions.Reverse();
                if (i == 2) actions.Shuffle();
            }

            for (var i = 1; i <= 3; i++)
            {
                foreach (var action in actionsFileData)
                {
                    Console.WriteLine("===== {0} =====", action.Method.Name);
                    sw.Restart();
                    var fileDatas = action.Invoke(sourceDir).ToList();
                    foreach (var fileData in fileDatas)
                    {
                        if (writeToConsole) Console.WriteLine("{0} {1}", fileData.Path, fileData.LastWriteTimeUtc);
                    }
                    sw.Stop();
                    results.Add(action.Method.Name + i, sw.ElapsedMilliseconds);
                    Console.WriteLine("> {0}ms for {1} files.", sw.ElapsedMilliseconds, fileDatas.Count);
                }
                if (i == 1) actions.Reverse();
                if (i == 2) actions.Shuffle();
            }

            Console.WriteLine("===== {0} =====", "Async");
            sw.Restart();
            var fis = Implementation.DirectoryRecursiveAsync(sourceDir).GetAwaiter().GetResult().ToList();
            foreach (var fileData in fis)
            {
                if (writeToConsole) Console.WriteLine("{0} {1}", fileData.Name, fileData.LastWriteTimeUtc);
            }
            sw.Stop();
            results.Add("Async", sw.ElapsedMilliseconds);
            Console.WriteLine("> {0}ms for {1} files.", sw.ElapsedMilliseconds, fis.Count);

            Console.WriteLine("===== {0} =====", "AsyncWithBag");
            sw.Restart();
            fis = Implementation.DirectoryRecursiveAsyncWithBag(sourceDir).GetAwaiter().GetResult().ToList();
            foreach (var fileData in fis)
            {
                if (writeToConsole) Console.WriteLine("{0} {1}", fileData.Name, fileData.LastWriteTimeUtc);
            }
            sw.Stop();
            results.Add("AsyncWithBag", sw.ElapsedMilliseconds);
            Console.WriteLine("> {0}ms for {1} files.", sw.ElapsedMilliseconds, fis.Count);

            Console.WriteLine();
            Console.WriteLine();
            foreach (var result in results.OrderBy(_ => _.Value))
            {
                Console.WriteLine("{0}: {1}", result.Key, result.Value);
            }

            Console.ReadKey();
        }
    }
}


//@"D:\Websites" for 174317 files.
//
//DirectoryRecursiveFullName1: 2131ms
//DirectoryRecursive3: 2185
//DirectoryRecursive2: 2196
//DirectoryRecursiveFullName3: 2271
//DirectoryRecursiveFullName2: 2411
//DirectoryRecursive1: 3203
//DirectoryAsParallel1: 3910
//DirectoryAsParallel2: 3915
//DirectoryAsParallel3: 3933
//DirectoryGetFilesWithYield1: 3949
//DirectoryGetFiles3: 3951
//DirectoryGetFilesWithYield2: 3991
//DirectoryGetFilesWithYield3: 4000
//DirectoryGetFiles2: 4009
//FindFilesEnumerateFilesPath3: 4556
//FindFilesEnumerateFilesPath2: 4563
//FindFilesEnumerateFilesPath1: 4586
//DirectoryInfoEnumerateFiles1: 4618
//FindFilesEnumerateFiles3: 4643
//DirectoryInfoEnumerateFiles3: 4644
//FindFilesEnumerateFiles1: 4667
//DirectoryInfoEnumerateFiles2: 4696
//FindFilesEnumerateFiles2: 4763
//DirectoryAsStack3: 5255
//DirectoryAsStack1: 5260
//DirectoryAsStack2: 5282
//DirectoryInfoEnumerateFilesFullName2: 5819
//DirectoryInfoEnumerateFilesFullName1: 5830
//DirectoryInfoEnumerateFilesFullName3: 5834
//Async: 5892
//AsyncWithBag: 6091
//DirectoryGetFiles1: 75543
