using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ken.Spikes.ListFiles.Implementations
{
    public partial class Implementation
    {
        public static IEnumerable<string> DirectoryInfoEnumerateFilesFullName(string rootDirectory)
        {
            var di = new DirectoryInfo(rootDirectory);
            return di.EnumerateFiles("*", SearchOption.AllDirectories).Select(_ => _.FullName);
        }

        public static IEnumerable<FileInfo> DirectoryInfoEnumerateFiles(string rootDirectory)
        {
            var di = new DirectoryInfo(rootDirectory);
            return di.EnumerateFiles("*", SearchOption.AllDirectories);
        }
    }
}
