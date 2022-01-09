using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FilterAttribute
    {
        public Guid id { get; set; }

        public string? slug { get; set; }
        public string? value { get; set; }
        public virtual List<Product_FilterAttribute>? product_FilterAttribute { get; set; }
    }
}