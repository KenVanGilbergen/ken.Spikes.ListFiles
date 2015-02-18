using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ken.Spikes.ListFiles.Implementations
{
    public partial class Implementation
    {
        public static IEnumerable<string> DirectoryRecursiveFullName(string rootDirectory)
        {
            var di = new DirectoryInfo(rootDirectory);
            var allFiles = new ConcurrentBag<string>();
            DirectoryRecursive(di, "*", allFiles);
            return allFiles;
        }

        private static void DirectoryRecursive(DirectoryInfo dr, string searchPattern, ConcurrentBag<string> bag)
        {
            var files = dr.GetFiles(searchPattern);
            foreach (FileInfo fi in files)
            {
                bag.Add(fi.FullName);
            }
            var subDirs = dr.GetDirectories();
            Parallel.ForEach(subDirs, di => DirectoryRecursive(di, searchPattern, bag));
        }

        public static IEnumerable<FileInfo> DirectoryRecursive(string rootDirectory)
        {
            var di = new DirectoryInfo(rootDirectory);
            var allFiles = new ConcurrentBag<FileInfo>();
            DirectoryRecursive(di, "*", allFiles);
            return allFiles;
        }

        private static void DirectoryRecursive(DirectoryInfo dr, string searchPattern, ConcurrentBag<FileInfo> bag)
        {
            var files = dr.GetFiles(searchPattern);
            foreach (FileInfo fi in files)
            {
                bag.Add(fi);
            }
            var subDirs = dr.GetDirectories();
            Parallel.ForEach(subDirs, di => DirectoryRecursive(di, searchPattern, bag));
        }
    }
}
