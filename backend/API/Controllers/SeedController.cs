using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace API.Controllers
{
    public class SeedController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public SeedController(DataContext context, IWebHostEnvironment HostEnvironment, IMapper mapper)
        {
            _hostEnvironment = HostEnvironment;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("reseed")]
        public async Task<IActionResult> Reseed()
        {
            // if (_hostEnvironment.IsDevelopment())
            // {
            //     if (_context.Products.whe)
            //     {
            //         if (await SmallSeed.PurgeDb(_context, _userManager, _photoAccessor))
            //         {
            //             if (await SmallSeed.ReSeedData(_context, _userManager, _photoAccessor, _mapper))
            //             {
            //                 return Ok();
            //             }
            //             return BadRequest("Failed to reseed database after purge");
            //             // }
            //         }
            //         return BadRequest("Failed to purge database");
            //     }
            //     if (await SmallSeed.ReSeedData(_context, _mapper))
            //     {
            //         return Ok();
            //     }
            // }
            return NotFound();
        }
    }
}