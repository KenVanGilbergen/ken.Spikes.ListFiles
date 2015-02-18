using System.Collections.Generic;
using System.IO;
using System.Linq;
using ken.Windows;

namespace ken.Spikes.ListFiles.Implementations
{
    public partial class Implementation
    {
        public static IEnumerable<string> FindFilesEnumerateFilesPath(string rootDirectory)
        {
            return FindFiles.EnumerateFiles(rootDirectory, "*", SearchOption.AllDirectories).Select(_ => _.Path);
        }

        public static IEnumerable<FileData> FindFilesEnumerateFiles(string rootDirectory)
        {
            return FindFiles.EnumerateFiles(rootDirectory, "*", SearchOption.AllDirectories);
        }
    }
}
