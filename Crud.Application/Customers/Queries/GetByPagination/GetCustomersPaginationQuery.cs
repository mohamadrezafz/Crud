using Crud.Application.Common.Models;
using MediatR;

namespace Crud.Application.Customers.Queries.GetByPagination;

public sealed record GetCustomersPaginationQuery(int Page = 1 , int Count = 5) : IRequest<PaginationList<GetCustomersPaginationPaginationResponse>>;
