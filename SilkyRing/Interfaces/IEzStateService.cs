// 

using static SilkyRing.GameIds.EzState;

namespace SilkyRing.Interfaces;

public interface IEzStateService
{
    void ExecuteTalkCommand(TalkCommand command);
    void ExecuteTalkCommand(TalkCommand command, long chrHandle);
}