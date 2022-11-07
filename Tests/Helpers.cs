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
            if (File.Exists(filename))
                return filename;

            var binSearch = $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
            var dir = Directory.GetCurrentDirectory();
            var index = dir.LastIndexOf(binSearch);
            if (index > 0)
            {
                var joined = Path.Join(dir.Remove(index), filename);
                if (File.Exists(joined))
                    return joined;
                throw new FileNotFoundException($"{filename} - Current dir:{Directory.GetCurrentDirectory()} (joined:'{joined}')");
            }

            throw new FileNotFoundException($"{filename} - Current dir:'{Directory.GetCurrentDirectory()}' (binSearch:'{binSearch}')");
        }
    }
}
