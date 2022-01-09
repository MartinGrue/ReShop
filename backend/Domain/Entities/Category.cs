using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public Guid? parentid { get; set; } = null;

        public virtual List<Product_Category>? product_Category { get; set; }

    }
}