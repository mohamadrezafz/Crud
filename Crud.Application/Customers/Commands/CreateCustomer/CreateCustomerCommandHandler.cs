using Crud.Application.Common.Interfaces;
using Crud.Domain.Entities;
using MediatR;
using Crud.Domain.Events;

namespace Crud.Application.Customers.Commands.CreateCustomer;


public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {

        var entity = new Customer()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            BankAccountNumber = request.BankAccountNumber

        };

        entity.BaseEvents.Add(new CustomerCreatedEvent(entity));

        _context.Customers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
