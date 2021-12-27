using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using Application.Interfaces;
using Domain.Entities;
namespace Application.Seed
{

    public class SeedCommand : IRequest<Unit> { }

    public class SeedCommandHandler : IRequestHandler<SeedCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SeedCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(SeedCommand request, CancellationToken cancellationToken)
        {
            var success = await SeedHelpers.ReSeedData(_context, _mapper, cancellationToken);
            if (success) return Unit.Value;
            throw new Exception("Problem in create handler");
        }
    }
}