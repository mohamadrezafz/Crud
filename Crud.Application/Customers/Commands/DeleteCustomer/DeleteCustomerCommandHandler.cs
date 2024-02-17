
using Crud.Application.Common.Exceptions;
using Crud.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crud.Application.Customers.Commands.DeleteCustomer;


public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == request.Id); 

        if (entity == null)
            throw new NotFoundException();
        

        _context.Customers.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

    }

}