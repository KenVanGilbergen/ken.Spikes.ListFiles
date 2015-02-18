using System.Collections.Generic;
using System.IO;

namespace ken.Spikes.ListFiles.Implementations
{
    public partial class Implementation
    {
        public static IEnumerable<string> DirectoryGetFilesWithYield(string rootDirectory)
        {
            foreach (var file in Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories))
            {
                yield return file;
            }
        }
    }
}
