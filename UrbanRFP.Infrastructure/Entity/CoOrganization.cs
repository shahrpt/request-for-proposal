using System;
using System.ComponentModel.DataAnnotations;

namespace UrbanRFP.Infrastructure.Entity
{
    public class CoOrganization
    {
        public Guid org_key { get; set; }
        public int org_id { get; set; }
        [Display(Name ="ORGANIZATION")]
        public string org_legal_name { get; set; }
        [Display(Name = "ORG TYPE")]
        public string org_description { get; set; }
        [Display(Name = "DIVISION")]
        public string org_division { get; set; }
        [Display(Name = "DEPARTMENT")]
        public string org_department { get; set; }
        [Display(Name = "SPECIALITIES")]
        public string org_specialities { get; set; }
        [Display(Name = "VENDOR FLAG")]
        public byte org_vendor_flag { get; set; }
        [Display(Name = "ORG CREATED ON")]
        public DateTime? org_date_created { get; set; }
        [Display(Name = "TIME ZONE")]
        public string org_timezone { get; set; }
        [Display(Name = "ENTITY TYPE")]
        public string org_entity_type { get; set; }
        [Display(Name = "SPECIAL TYPE")]
        public byte org_special_type_flag { get; set; }
        [Display(Name = "QUALIFIED")]
        public byte org_qualified_LG_flag { get; set; }
        [Display(Name = "FEDERAL TAX Id")]
        public string org_federal_tax_id { get; set; }
        [Display(Name = "WEBSITE")]
        public string org_website { get; set; }
        [Display(Name = "ANNUAL REVENUE")]
        public decimal org_ann_revenue { get; set; }
        [Display(Name = "NUMBER OF EMPLOYEES")] 
        public int org_vendor_number_of_employees { get; set; }
        [Display(Name = "POPULATION RANGE")]
        public int org_LG_population_range { get; set; }
        [Display(Name = "CREATED ON")]
        public DateTime? org_add_date { get; set; }
        [Display(Name = "CREATED BY")]
        public string org_add_user { get; set; }
        [Display(Name = "MODIFIED ON")]
        public DateTime? org_change_date { get; set; }
        [Display(Name = "MODIFIED BY")]
        public string org_change_user { get; set; }
        [Display(Name = "DELETED?")]
        public byte org_delete_flag { get; set; }
        [Display(Name = "ORGANIZATION DOMAIN")]
        public string org_domain { get; set; }
    }
}
