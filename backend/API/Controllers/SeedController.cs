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
        public SeedController(IWebHostEnvironment HostEnvironment)
        {
            _hostEnvironment = HostEnvironment;
        }

        [HttpGet("reseed")]
        public async Task<ActionResult<Unit>> Reseed()
        {
            if (_hostEnvironment.IsDevelopment())
                return await Mediator.Send(new SeedCommand());
            return NotFound();
        }
    }
}