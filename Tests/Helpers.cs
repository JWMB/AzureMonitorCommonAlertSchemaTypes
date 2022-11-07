using Newtonsoft.Json;
using Shouldly;
using Types;
using Types.Serialization;

namespace Tests
{
    internal class Helpers
    {
        public static Alert Deserialize(string str)
        {
            return AlertJsonSerializerSettings.DeserializeOrThrow(str);
        }

        public static Alert DeserializeFile(string filename) => Deserialize(File.ReadAllText(ResolveFilename(filename)));

        public static string ResolveFilename(string filename)
        {
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var found = GetFullnameIfExists(filename, currentDir);
            if (found != null)
                return found;

            while (currentDir != null)
            {
                if (currentDir.Name == "bin" && currentDir.Parent != null)
                {
                    found = GetFullnameIfExists(filename, currentDir.Parent);
                    if (found != null)
                        return found;
                }
                currentDir = currentDir.Parent;
            }
            //var binSearch = $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
            //var index = currentDir.FullName.LastIndexOf(binSearch);
            //if (index > 0)
            //{
            //    var joined = Path.Join(currentDir.FullName.Remove(index), filename);
            //    if (File.Exists(joined))
            //        return joined;
            //    throw new FileNotFoundException($"{filename} - Current dir:{Directory.GetCurrentDirectory()} (joined:'{joined}')");
            //}

            throw new FileNotFoundException($"{filename} - Current dir:'{Directory.GetCurrentDirectory()}'"); // (binSearch:'{binSearch}')");
        }

        private static string? GetFullnameIfExists(string filepath, DirectoryInfo dir)
        {
            var result = new FileInfo(Path.Join(dir.FullName, filepath));
            return result.Exists ? result.FullName : null;
        }
    }
}
