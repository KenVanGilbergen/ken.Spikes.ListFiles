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
        private const bool DefaultWriteToConsole = true;

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
                    Console.Write("===== {0} =====", action.Method.Name + i);
                    sw.Restart();
                    foreach (var file in action.Invoke(sourceDir))
                    {
                        if (writeToConsole) Console.WriteLine(file);
                    }
                    sw.Stop();
                    results.Add(action.Method.Name + i, sw.ElapsedMilliseconds);
                    Console.WriteLine("> {0}", sw.ElapsedMilliseconds);
                }
                if (i == 1) actions.Reverse();
                if (i == 2) actions.Shuffle();
            }

            foreach (var action in actionsFileInfo)
            {
                Console.Write("===== {0} =====", action.Method.Name);
                sw.Restart();
                foreach (var fileInfo in action.Invoke(sourceDir))
                {
                    if (writeToConsole) Console.WriteLine("{0} {1}", fileInfo.FullName, fileInfo.LastWriteTimeUtc);
                }
                sw.Stop();
                results.Add(action.Method.Name, sw.ElapsedMilliseconds);
                Console.WriteLine("> {0}", sw.ElapsedMilliseconds);
            }

            foreach (var action in actionsFileData)
            {
                Console.Write("===== {0} =====", action.Method.Name);
                sw.Restart();
                foreach (var fileData in action.Invoke(sourceDir))
                {
                    if (writeToConsole) Console.WriteLine("{0} {1}", fileData.Path, fileData.LastWriteTimeUtc);
                }
                sw.Stop();
                results.Add(action.Method.Name, sw.ElapsedMilliseconds);
                Console.WriteLine("> {0}", sw.ElapsedMilliseconds);
            }

            foreach (var result in results.OrderBy(_ => _.Key))
            {
                Console.WriteLine("{0}: {1}", result.Key, result.Value);
            }

            Console.ReadKey();
        }
    }
}
