

using MediatR;

namespace Crud.Application.Customers.Queries.GetById;

public sealed record GetCustomerByIdQuery(int Id) : IRequest<GetCustomerByIdResponse>;
