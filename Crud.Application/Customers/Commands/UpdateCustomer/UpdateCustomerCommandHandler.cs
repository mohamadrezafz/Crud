using Crud.Application.Common.Exceptions;
using Crud.Application.Common.Interfaces;
using MediatR;


namespace Crud.Application.Customers.Commands.UpdateCustomer;


public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException();

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.DateOfBirth = request.DateOfBirth;
        entity.PhoneNumber = request.PhoneNumber;
        entity.Email = request.Email;
        entity.BankAccountNumber = request.BankAccountNumber;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
