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
    class Program
    {
        //private const string SourceDir = @"D:\Websites";
        private const string SourceDir = @"D:\Websites\multiple_sites_red";
        private const bool WriteToConsole = false;
        //private const string SourceDir = @"D:\Websites\foo";
        //private const bool WriteToConsole = true;

        private static void Main(string[] args)
        {
            var sw = new Stopwatch();

            var results = new Dictionary<string, long>();
            var actions = new List<Func<string, IEnumerable<string>>>
            {
                Implementation.DirectoryGetFiles,
                Implementation.DirectoryGetFilesWithYield,
                Implementation.DirectoryInfoEnumerateFilesFullName,
                Implementation.FindFilesEnumerateFilesPath,
            };

            var actionsFileInfo = new List<Func<string, IEnumerable<FileInfo>>>
            {
                Implementation.DirectoryInfoEnumerateFiles
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
                    foreach (var file in action.Invoke(SourceDir))
                    {
                        if (WriteToConsole) Console.WriteLine(file);
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
                foreach (var fileInfo in action.Invoke(SourceDir))
                {
                    if (WriteToConsole) Console.WriteLine("{0} {1}", fileInfo.FullName, fileInfo.LastWriteTimeUtc);
                }
                sw.Stop();
                results.Add(action.Method.Name, sw.ElapsedMilliseconds);
                Console.WriteLine("> {0}", sw.ElapsedMilliseconds);
            }

            foreach (var action in actionsFileData)
            {
                Console.Write("===== {0} =====", action.Method.Name);
                sw.Restart();
                foreach (var fileData in action.Invoke(SourceDir))
                {
                    if (WriteToConsole) Console.WriteLine("{0} {1}", fileData.Path, fileData.LastWriteTimeUtc);
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
