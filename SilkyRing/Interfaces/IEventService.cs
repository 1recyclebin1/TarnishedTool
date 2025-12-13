// 

namespace SilkyRing.Interfaces;

public interface IEventService
{
    void SetEvent(long eventId);
    bool GetEvent(long eventId);
}