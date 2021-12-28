using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Products.Queries;
using Application.Common.Utils;

namespace API.Controllers
{
    public class ProductsController : BaseController
    {
        [HttpGet("{slug}")]
        public async Task<ActionResult<ProductDTO>> getProductBySlug(string slug)
        {
            return await Mediator.Send(new getProductBySlugQuery { slug = slug });
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProductDTO>>> getProducts([FromQuery] getProductsListQuery productParams)
        {
            return await Mediator.Send(productParams);
        }
    }
}
