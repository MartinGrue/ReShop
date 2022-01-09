using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product
    {
        public Guid id { get; set; }
        public string name { get; set; }

        public string slug { get; set; }

        public string description { get; set; }
        public long price { get; set; }
        public string? brand { get; set; }
        public int qunatityInStock { get; set; }

        public virtual List<Badge>? badges { get; set; }
        public virtual List<Image>? images { get; set; }
        public virtual List<Review>? reviews { get; set; }
        public virtual List<Product_FilterAttribute>? product_FilterAttribute { get; set; }
        public virtual List<Product_Category>? product_Category { get; set; }


        // //features
        // public string? slug { get; set; }
        // public virtual ICollection<Image> images { get; set; }
        // public int? rating { get; set; }
        // public int? review { get; set; }

        // //string | string[] | enum?
        // public string? categories { get; set; }
        // public ICollection<Attributes>? Name { get; set; }

        // //more features
        // public long? compareAtPrice { get; set; }
        // public ProductBadges? badges { get; set; }

    }
}