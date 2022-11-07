namespace Types
{
    public class Data
    {
        public Essentials Essentials { get; set; } = new Essentials();
        public IAlertContext? AlertContext { get; set; }
    }
}
