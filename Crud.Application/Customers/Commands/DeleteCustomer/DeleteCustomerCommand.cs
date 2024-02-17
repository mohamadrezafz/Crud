

using MediatR;

namespace Crud.Application.Customers.Commands.DeleteCustomer;

public sealed record DeleteCustomerCommand(int Id) : IRequest;