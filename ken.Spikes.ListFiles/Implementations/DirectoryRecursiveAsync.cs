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
        public static async Task<IEnumerable<FileInfo>> DirectoryRecursiveAsync(string rootDirectory)
        {
            var di = new DirectoryInfo(rootDirectory);
            var allFiles = new List<FileInfo>();
            allFiles.AddRange(await DirectoryRecursiveAsync(di, "*"));
            return allFiles;
        }

        public static async Task<IEnumerable<FileInfo>> DirectoryRecursiveAsync(DirectoryInfo dr, string searchPattern)
        {
            var files = dr.GetFiles(searchPattern).ToList();
            foreach (var di in dr.GetDirectories())
            {
                files.AddRange(await DirectoryRecursiveAsync(di, searchPattern));
            }
            return files;
        }

        public static async Task<IEnumerable<FileInfo>> DirectoryRecursiveAsyncWithBag(string rootDirectory)
        {
            var di = new DirectoryInfo(rootDirectory);
            var bag = new ConcurrentBag<FileInfo>();
            await DirectoryRecursiveAsync(di, "*", bag);
            return bag;
        }

        public static async Task DirectoryRecursiveAsync(DirectoryInfo dr, string searchPattern, ConcurrentBag<FileInfo> bag)
        {
            foreach (var fi in dr.GetFiles(searchPattern))
            {
                bag.Add(fi);
            }
            foreach (var di in dr.GetDirectories())
            {
                await DirectoryRecursiveAsync(di, searchPattern, bag);
            }
        }
    }
}
