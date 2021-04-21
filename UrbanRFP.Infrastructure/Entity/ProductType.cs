using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UrbanRFP.Infrastructure.Entity
{
    /// <summary>
    /// Created on  :Friday, June 28, 2019
    /// Created by  : Waheed Iqbal
    /// Updated on  : Waheed Iqbal
    /// Updated by  : Friday, June 28, 2019
    /// Description : Declare datamodel object for  rfp_product_type
    /// </summary>
    public class ProductType
    {
        public ProductType()
        {
            SubTypes = new List<ProductSubType>();
        }

        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public List<ProductSubType> SubTypes { get; set; }
        public bool IsActive { get; set; }
    }
}
