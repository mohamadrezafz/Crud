

using MediatR;

namespace Crud.Application.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(string FirstName, string LastName , DateTime DateOfBirth , string PhoneNumber , string Email, string BankAccountNumber) : IRequest<int>;
