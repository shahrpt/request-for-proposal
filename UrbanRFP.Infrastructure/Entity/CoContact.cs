
using System;
/// <summary>
/// Created on  : Wednesday, August 7, 2019
/// Created by  : Waheed Iqbal
/// Updated on  : Wednesday, August 7, 2019
/// Updated by  : Waheed Iqbal
/// Description : Declare datamodel object for  co_contact
/// </summary>
namespace UrbanRFP.Infrastructure.Entity
{
    public class CoContact
    {
        public Guid ct_key { get; set; }
        public long? ct_id { get; set; }
        public string ct_first_name { get; set; }
        public string ct_last_name { get; set; }
        public string ct_email { get; set; }
        public string ct_phone { get; set; }
        public string ct_fax { get; set; }
        public string ct_password { get; set; }
        public byte? ct_delete_flag { get; set; }
        public string ct_add_date { get; set; }
        public string ct_add_user { get; set; }
        public string ct_change_date { get; set; }
        public string ct_change_user { get; set; }
        public Guid ct_activation_code { get; set; }
        public Int16 ct_active { get; set; }
    }
}