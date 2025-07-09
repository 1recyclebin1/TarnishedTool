using System;
using System.Collections.Generic;
using SilkyRing.Memory;
using SilkyRing.Utilities;
using static SilkyRing.Memory.Offsets;
using Array = System.Array;

namespace SilkyRing.Services
{
    public class EnemyService
    {
        private readonly MemoryIo _memoryIo;
        private readonly HookManager _hookManager;

        public EnemyService(MemoryIo memoryIo, HookManager hookManager)
        {
            _memoryIo = memoryIo;
            _hookManager = hookManager;
        }


        public void ToggleRykardMega(bool isRykardNoMegaEnabled)
        {
            var code = CodeCaveOffsets.Base + CodeCaveOffsets.Rykard;
            if (isRykardNoMegaEnabled)
            {
                var hook = Hooks.HasSpEffect;
                var codeBytes = AsmLoader.GetAsmBytes("RykardNoMega");
                var bytes = AsmHelper.GetJmpOriginOffsetBytes(hook, 7, code + 0x17);
                Array.Copy(bytes, 0, codeBytes, 0x12 + 1, 4);
                _memoryIo.WriteBytes(code, codeBytes);
                _hookManager.InstallHook(code.ToInt64(), hook, new byte[]
                    { 0x48, 0x8B, 0x49, 0x08, 0x48, 0x85, 0xC9 });
            }
            else
            {
                _hookManager.UninstallHook(code.ToInt64());
            }
            
          
        }
    }
}