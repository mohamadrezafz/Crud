
namespace Crud.Domain.Common;

public interface IHasBaseEvent
{
    public List<BaseEvent> BaseEvents { get; set; }
}
public abstract class BaseEvent
{
    public bool IsPublish { get; set; }
    public DateTime DateTimeOccurred { get; set; } = DateTime.Now;
}

