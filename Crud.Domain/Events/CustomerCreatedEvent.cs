
using Crud.Domain.Common;
using Crud.Domain.Entities;

namespace Crud.Domain.Events;

public class CustomerCreatedEvent : BaseEvent
{
    public CustomerCreatedEvent(Customer customer)
    {
        this.Customer = customer;
    }
    public Customer Customer { get; set; }
}
