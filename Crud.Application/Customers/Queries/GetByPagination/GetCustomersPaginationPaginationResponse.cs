

using Crud.Application.Common.Mappings;
using Crud.Domain.Entities;

namespace Crud.Application.Customers.Queries.GetByPagination;

public sealed record GetCustomersPaginationPaginationResponse(int Id, string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string BankAccountNumber) : IMapFrom<Customer>;
