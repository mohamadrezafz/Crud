

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crud.Application.Common.Exceptions;
using Crud.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crud.Application.Customers.Queries.GetById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetCustomerByIdQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetCustomerByIdResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .Where(c => c.Id == request.Id)
            .ProjectTo<GetCustomerByIdResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new NotFoundException();

        return entity;
    }
}
