// 

using static TarnishedTool.GameIds.EzState;

namespace TarnishedTool.Interfaces;

public interface IEzStateService
{
    void ExecuteTalkCommand(TalkCommand command);
    void ExecuteTalkCommand(TalkCommand command, long chrHandle);
}