using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MediatR;

using Application.Seed;

namespace API.Controllers
{
    public class SeedController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;


        [HttpGet("reseed")]
        public async Task<ActionResult<Unit>> Reseed()
        {
            var command = new SeedCommand();
            return await Mediator.Send(command);
            // return Ok();
            // if (_hostEnvironment.IsDevelopment())
            // {
            //     // if (await SmallSeed.ReSeedData(_context, _mapper))
            //     //     return Ok();

            //     // return BadRequest("Failed to reseed database");
            // }
            // return NotFound();
        }
    }
}