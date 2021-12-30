using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product_FilterAttribute
    {
        public Guid Productid { get; set; }
        public Guid FilterAttributeid { get; set; }
        public virtual Product Product { get; set; }
        public virtual FilterAttribute FilterAttribute { get; set; }
    }
}