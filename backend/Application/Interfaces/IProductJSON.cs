using System;

using Application;
using Domain;

namespace Application
{
    public class ProductJSON : IMapFrom<Product>
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public long price { get; set; }
        public string? brand { get; set; }
        public int qunatityInStock { get; set; }
    }
}