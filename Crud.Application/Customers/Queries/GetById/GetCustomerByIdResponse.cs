
using Crud.Application.Common.Mappings;
using Crud.Domain.Entities;

namespace Crud.Application.Customers.Queries.GetById;

public sealed record GetCustomerByIdResponse(int Id, string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Email, string BankAccountNumber) : IMapFrom<Customer>;
