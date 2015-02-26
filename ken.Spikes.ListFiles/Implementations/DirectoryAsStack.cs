using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ken.Spikes.ListFiles.Implementations
{
    /// <summary>
    /// Iterate File Directories with the Parallel Class 
    /// https://msdn.microsoft.com/en-us/library/ff477033(v=vs.110).aspx
    /// </summary>
    public partial class Implementation
    {
        static readonly int ProcCount = Environment.ProcessorCount;
        
        public static IEnumerable<string> DirectoryAsStack(string rootDirectory)
        {
            var allFiles = new ConcurrentBag<string>();
            var dirs = new Stack<string>();
            dirs.Push(rootDirectory);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                var subDirs = Directory.GetDirectories(currentDir);
                var files = Directory.GetFiles(currentDir);
                if (files.Length < ProcCount)
                {
                    foreach (var file in files)
                    {
                        allFiles.Add(file);
                    }
                }
                else
                {
                    Parallel.ForEach(files, file => allFiles.Add(file));
                }
                foreach (string str in subDirs) dirs.Push(str);
            }
            return allFiles;
        }
    }
}
