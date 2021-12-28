using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using Application.Common.Exceptions;
using Application.Common.Utils;
using AutoMapper.QueryableExtensions;

namespace Application.Products.Queries
{
    public class getProductsListQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
    public class getProductsListQueryHandler : IRequestHandler<getProductsListQuery, PaginatedList<ProductDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public getProductsListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ProductDTO>> Handle(getProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products
            .AsQueryable()
            .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.pageNumber, request.pageSize); ;
            return products;
        }
    }
}