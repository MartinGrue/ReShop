using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
namespace Application.Seed
{
    public class OutputJSON
    {
        public List<OutputJSON.Product> products { get; set; }
        public List<OutputJSON.Category> categories { get; set; }
        public List<OutputJSON.Product_Category> product_Categories { get; set; }

        public class Product : BaseJSON.Product, IMapFrom<Domain.Entities.Product>
        {
            public Guid id { get; set; }
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
        public class Category : BaseJSON.Category, IMapFrom<Domain.Entities.Category>
        {
            public Guid id { get; set; }
            public string slug { get; set; }
        }

        public class Product_Category : IMapFrom<Domain.Entities.Product_Category>
        {
            public Guid productid { get; set; }
            public Guid categoryid { get; set; }
        }
    }

}