using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Products.Queries;

namespace API.Controllers
{
    public class ProductsController : BaseController
    {
        [HttpGet("{slug}")]
        public async Task<ActionResult<ProductDTO>> getProductBySlug(string slug)
        {
            Console.WriteLine("Slug: " + slug);
            return await Mediator.Send(new getProductBySlugQuery { slug = slug });
        }
    }
}
