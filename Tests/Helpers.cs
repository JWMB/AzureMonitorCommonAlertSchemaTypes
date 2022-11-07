using Newtonsoft.Json;
using Shouldly;
using Types;

namespace Tests
{
    internal class Helpers
    {
        public static Alert Deserialize(string str)
        {
            return AlertJsonSerializerSettings.DeserializeOrThrow(str);
            //var alert = JsonConvert.DeserializeObject<Alert>(str, new AlertJsonSerializerSettings());
            //alert.ShouldNotBeNull();
            //return alert;
        }

        public static Alert DeserializeFile(string filename) => Deserialize(File.ReadAllText(filename));

    }
}
