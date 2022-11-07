using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Types
{
    //public class Alert<T>
    //    where T : new()
    public class Alert
    {
        public string SchemaId { get; set; } = string.Empty;
        //public Data<T> Data { get; set; } = new Data<T>();
        public Data Data { get; set; } = new Data();
    }

    //public class Data<T>
    //    where T : new()
    public class Data
    {
        public Essentials Essentials { get; set; } = new Essentials();
        //public T AlertContext { get; set; } = new T();
        public IAlertContext? AlertContext { get; set; }
    }
}
