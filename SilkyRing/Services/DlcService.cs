// 

using System;
using SilkyRing.Interfaces;
using static SilkyRing.Memory.Offsets;

namespace SilkyRing.Services;

public class DlcService(MemoryService memoryService) : IDlcService
{
    
    public void CheckDlc()
    {
        var flags = memoryService.ReadInt64(CsDlcImp.Base) + CsDlcImp.ByteFlags;
        IsDlcAvailable = memoryService.ReadUInt8((IntPtr)flags + (int)CsDlcImp.Flags.DlcCheck) == 1;
    }

    public bool IsDlcAvailable { get; private set; }
}