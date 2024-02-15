

using Crud.Domain.Common;

namespace Crud.Application.Common.Interfaces;

public interface IBaseEventService
{
    Task Publish(BaseEvent domainEvent);
}
