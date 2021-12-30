using System;
using Application;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Seed
{
    public class ProductJSON : IMapFrom<Product>
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }

        public string description { get; set; }
        public long price { get; set; }
        public string? brand { get; set; }
        public int qunatityInStock { get; set; }
        public List<Badge> badges { get; set; }
        public List<Review> reviews { get; set; }
        public List<Image> images { get; set; }
        public List<FilterAttribute> attributes { get; set; }



    }
}