

using Crud.Domain.Common;
using MediatR;

namespace Crud.Application.Common.Models;

public class BaseEventNotify<T> : INotification where T : BaseEvent
{
    public BaseEventNotify(T baseEvent)
    {
        this.BaseEvent = baseEvent;
    }

    public T BaseEvent { get; }
}
