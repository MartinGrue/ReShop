using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product_Category
    {   
        public Guid productid { get; set; }
        public Guid categoryid { get; set; }
        public virtual Product product { get; set; }
        public virtual Category category { get; set; }
    }
}