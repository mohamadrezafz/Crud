
using Crud.Application.Common.Interfaces;
using Crud.Application.Common.Models;
using Crud.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crud.Infrastructure.Services;

/// <summary>
/// Service responsible for publishing base events using MediatR.
/// </summary>
public class BaseEventService : IBaseEventService
{
    private readonly ILogger<BaseEventService> _logger;
    private readonly IPublisher _mediator;

    public BaseEventService(ILogger<BaseEventService> logger, IPublisher mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Publishes the provided base event and logs the event type.
    /// </summary>
    /// <param name="baseEvent">The base event to be published.</param>
    public async Task Publish(BaseEvent baseEvent)
    {
        _logger.LogInformation("event published - {0}", baseEvent.GetType().Name);
        await _mediator.Publish(NotificationToBaseEvent(baseEvent));
    }

    private INotification NotificationToBaseEvent(BaseEvent baseEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(BaseEventNotify<>).MakeGenericType(baseEvent.GetType()), baseEvent)!;
    }
}
