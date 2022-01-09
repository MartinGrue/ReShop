using System;
using Application;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Seed
{
    public class SeedData
    {
#nullable enable
        public List<Product>? products { get; set; }
        public List<Category>? categories { get; set; }
#nullable disable
    }
}
