
using Crud.Domain.Common;

namespace Crud.Domain.Entities;

public class Customer :BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string BankAccountNumber { get; set; } = default!;
    public List<BaseEvent> BaseEvents { get; set; } = default!;
}

