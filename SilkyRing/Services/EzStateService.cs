// 

using SilkyRing.Interfaces;
using SilkyRing.Memory;
using SilkyRing.Utilities;
using static SilkyRing.GameIds.EzState;
using static SilkyRing.Memory.Offsets;

namespace SilkyRing.Services;

public class EzStateService(MemoryService memoryService) : IEzStateService
{
    public void ExecuteTalkCommand(TalkCommand command) => ExecuteTalkCommand(command, 0);

    public void ExecuteTalkCommand(TalkCommand command, long chrHandle)
    {
        var code = CodeCaveOffsets.Base + CodeCaveOffsets.EzStateTalkCode;
        var paramsLoc = CodeCaveOffsets.Base + CodeCaveOffsets.EzStateTalkParams;
        
        for (int i = 0; i < command.Params.Length; i++)
        {
            memoryService.WriteInt32(paramsLoc + i * 4, command.Params[i]);
        }
        
        var bytes = AsmLoader.GetAsmBytes("ExecuteTalkCommand");
        AsmHelper.WriteRelativeOffsets(bytes, new []
        {
            (code.ToInt64() + 0x16, Functions.ExternalEventTempCtor, 5, 0x16 + 1),
            (code.ToInt64() + 0x5A, paramsLoc.ToInt64(), 7, 0x5A + 3),
            (code.ToInt64() + 0x9A, Functions.ExecuteTalkCommand, 5, 0x9A + 1),
        });

        AsmHelper.WriteImmediateDwords(bytes, new[]
        {
            (command.CommandId, 0x11 + 1),
            (command.Params.Length, 0x4D + 1)
        });
        
        AsmHelper.WriteAbsoluteAddress(bytes, chrHandle, 0x3F + 2);
        
        memoryService.WriteBytes(code, bytes);
        memoryService.RunThread(code);
    }
}