using Crud.Application.Common.Mappings;
using Crud.Domain.Entities;

namespace Crud.Application.Customers.Queries;

public sealed class GetCustomersResponse : IMapFrom<Customer>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string BankAccountNumber { get; set; } = default!;
}
