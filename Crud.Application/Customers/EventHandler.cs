
using Crud.Application.Common.Models;
using Crud.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crud.Application.Customers;

public class CustomerCreatedEventHandler : INotificationHandler<BaseEventNotify<CustomerCreatedEvent>>
{
    private readonly ILogger<CustomerCreatedEventHandler> _logger;

    public CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(BaseEventNotify<CustomerCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var baseEvent = notification.BaseEvent;

        _logger.LogInformation("Crud Event: {BaseEvent}", baseEvent.GetType().Name);

        return Task.CompletedTask;
    }
}