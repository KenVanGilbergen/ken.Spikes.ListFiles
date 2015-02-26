using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
            foreach (FileInfo fi in dr.GetFiles(searchPattern))
            {
                bag.Add(fi.FullName);
            }
            Parallel.ForEach(dr.GetDirectories(), di => DirectoryRecursive(di, searchPattern, bag));
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
            foreach (FileInfo fi in dr.GetFiles(searchPattern))
            {
                bag.Add(fi);
            }
            Parallel.ForEach(dr.GetDirectories(), di => DirectoryRecursive(di, searchPattern, bag));
        }
    }
}
