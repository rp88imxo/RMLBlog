using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;

namespace RmlBlogMvc.utils
{
    public static class RmlUtils
    {
        // Create a folder with a given path if it doesnt exist
        public static void EnsureFolderCreated(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }
}
