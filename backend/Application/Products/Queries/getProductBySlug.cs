using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using Application.Common.Exceptions;

namespace Application.Products.Queries
{
    public class getProductBySlugQuery : IRequest<ProductDTO>
    {
        public string slug { get; set; }
    }
    public class getProductBySlugQueryHandler : IRequestHandler<getProductBySlugQuery, ProductDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public getProductBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProductDTO> Handle(getProductBySlugQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.name == request.slug);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.slug);
            }
            return _mapper.Map<Product, ProductDTO>(product);
        }
    }
}