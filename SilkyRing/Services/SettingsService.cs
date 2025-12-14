// 

using System;
using SilkyRing.Interfaces;
using SilkyRing.Memory;
using static SilkyRing.Memory.Offsets;

namespace SilkyRing.Services;

public class SettingsService(MemoryService memoryService, HookManager hookManager) : ISettingsService
{
    public void Quitout() =>
        memoryService.WriteUInt8((IntPtr)memoryService.ReadInt64(GameMan.Base) + GameMan.ShouldQuitout, 1);

    public void ForceSave() =>
        memoryService.WriteUInt8((IntPtr)memoryService.ReadInt64(GameMan.Base) + GameMan.ForceSave, 1);
}