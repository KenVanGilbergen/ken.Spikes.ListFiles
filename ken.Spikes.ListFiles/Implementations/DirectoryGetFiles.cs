using System.Collections.Generic;
using System.IO;

namespace ken.Spikes.ListFiles.Implementations
{
    public partial class Implementation
    {
        public static IEnumerable<string> DirectoryGetFiles(string rootDirectory)
        {
            return Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
        }
    }
}
