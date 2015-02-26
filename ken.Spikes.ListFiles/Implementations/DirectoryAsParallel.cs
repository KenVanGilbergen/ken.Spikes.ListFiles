using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ken.Spikes.ListFiles.Implementations
{
    /// <summary>
    /// Iterate File Directories with PLINQ
    /// https://msdn.microsoft.com/en-us/library/ff462679(v=vs.110).aspx
    /// </summary>
    public partial class Implementation
    {
        public static IEnumerable<string> DirectoryAsParallel(string rootDirectory)
        {
            var files = Directory.EnumerateFiles(rootDirectory, "*.*", SearchOption.AllDirectories).AsParallel();
            return files;
        }
    }


}
