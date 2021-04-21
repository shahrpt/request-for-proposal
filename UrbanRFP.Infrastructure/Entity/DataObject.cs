using System;

namespace UrbanRFP.Infrastructure.Entity
{
    public class DataObject
    {
        public string primary_key_name { get; set; }
        public string primary_key_value { get; set; }
        public string data_field_name { get; set; }
        public dynamic data_field_value { get; set; }
        public Type data_type { get; set; }
        public string user_name { get; set; }
        public string table_name { get; set; }
    }
}