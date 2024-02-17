
using MediatR;

namespace Crud.Application.Customers.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand(int Id,string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string BankAccountNumber) : IRequest;

