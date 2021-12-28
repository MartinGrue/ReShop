using Domain.Entities;
namespace Application.Products.Queries
{
    public class ProductDTO : IMapFrom<Product>
    {
        public string name { get; set; }

    }
}