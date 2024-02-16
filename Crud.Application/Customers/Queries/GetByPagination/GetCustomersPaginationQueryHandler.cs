

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crud.Application.Common.Interfaces;
using Crud.Application.Common.Mappings;
using Crud.Application.Common.Models;
using MediatR;

namespace Crud.Application.Customers.Queries.GetByPagination;

public class GetCustomersPaginationQueryHandler : IRequestHandler<GetCustomersPaginationQuery,PaginationList<GetCustomersPaginationPaginationResponse>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetCustomersPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public Task<PaginationList<GetCustomersPaginationPaginationResponse>> Handle(GetCustomersPaginationQuery request, CancellationToken cancellationToken) =>
        _context.Customers
            .ProjectTo<GetCustomersPaginationPaginationResponse>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Count);
}