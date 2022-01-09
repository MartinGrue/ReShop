using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Seed
{
    public class InputJSON
    {
        public List<InputJSON.Product> products { get; set; }
        public List<InputJSON.Category> categories { get; set; }
        public List<InputJSON.Product_Category> product_Categories { get; set; }

        public class Product
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
            public string description { get; set; }
            public long price { get; set; }
            public string brand { get; set; }
            public int qunatityInStock { get; set; }
            public List<Badge> badges { get; set; }
            public List<Review> reviews { get; set; }
            public List<Image> images { get; set; }
            public List<FilterAttribute> attributes { get; set; }
        }
        public class Category
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }
        public class Product_Category
        {
            public Guid productid { get; set; }
            public Guid categoryid { get; set; }
        }
    }
}