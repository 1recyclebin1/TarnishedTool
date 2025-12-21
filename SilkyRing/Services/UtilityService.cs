using System;
using SilkyRing.Interfaces;
using SilkyRing.Memory;
using SilkyRing.Utilities;
using static SilkyRing.Memory.Offsets;

namespace SilkyRing.Services
{
    public class UtilityService(MemoryService memoryService, HookManager hookManager, IPlayerService playerService)
        : IUtilityService
    {
        public void ForceSave() =>
            memoryService.WriteUInt8((IntPtr)memoryService.ReadInt64(GameMan.Base) + GameMan.ForceSave, 1);
        
        public void ToggleCombatMap(bool isEnabled)
        {
            memoryService.WriteUInt8(Patches.OpenMap, isEnabled ? 0xEB : 0x74);
            memoryService.WriteBytes(Patches.CloseMap, isEnabled ? [0x90, 0x90, 0x90] : [0xff, 0x50, 0x60]);
        }

        public void ToggleDungeonWarp(bool isEnabled) =>
            memoryService.WriteUInt8(Patches.DungeonWarp, isEnabled ? 0xEB : 0x74);

        public void ToggleNoClip(bool isNoClipEnabled)
        {
            var kbCode = CodeCaveOffsets.Base + (int)CodeCaveOffsets.NoClip.Kb;
            var triggersCode = CodeCaveOffsets.Base + (int)CodeCaveOffsets.NoClip.Triggers;
            var updateCoordsCode = CodeCaveOffsets.Base + (int)CodeCaveOffsets.NoClip.UpdateCoords;

            if (isNoClipEnabled)
            {
                WriteKbCode(kbCode);
                WriteTriggerCode(triggersCode);
                WriteUpdateCoords(updateCoordsCode);
                
                hookManager.InstallHook(kbCode.ToInt64(), Hooks.NoClipKb, new byte[]
                    { 0xF6, 0x84, 0x08, 0xE8, 0x07, 0x00, 0x00, 0x80 });
                hookManager.InstallHook(triggersCode.ToInt64(), Hooks.NoClipTriggers, new byte[]
                    { 0x0F, 0xB6, 0x44, 0x24, 0x36 });
                hookManager.InstallHook(updateCoordsCode.ToInt64(), Hooks.UpdateCoords, new byte[]
                    { 0x0F, 0x11, 0x43, 0x70, 0xC7, 0x43, 0x7C, 0x00, 0x00, 0x80, 0x3F });
            }
            else
            {
                hookManager.UninstallHook(kbCode.ToInt64());
                hookManager.UninstallHook(triggersCode.ToInt64());
                hookManager.UninstallHook(updateCoordsCode.ToInt64());

                playerService.EnableGravity();
            }
        }

        
        private void WriteKbCode(IntPtr kbCode)
        {
            var codeBytes = AsmLoader.GetAsmBytes("NoClip_Keyboard");
            var zDirection = CodeCaveOffsets.Base + (int)CodeCaveOffsets.NoClip.ZDirection;

            AsmHelper.WriteRelativeOffsets(codeBytes, new[]
            {
                (kbCode.ToInt64() + 0x10, zDirection.ToInt64(), 7, 0x10 + 2),
                (kbCode.ToInt64() + 0x29, zDirection.ToInt64(), 7, 0x29 + 2),
                (kbCode.ToInt64() + 0x34, Hooks.NoClipKb + 0x8, 5, 0x34 + 1),
                (kbCode.ToInt64() + 0x41, Hooks.NoClipKb + 0x8, 5, 0x41 + 1),
            });

            memoryService.WriteBytes(kbCode, codeBytes);
        }

        private void WriteTriggerCode(IntPtr triggersCode)
        {
            var codeBytes = AsmLoader.GetAsmBytes("NoClip_Triggers");
            var zDirection = CodeCaveOffsets.Base + (int)CodeCaveOffsets.NoClip.ZDirection;
            AsmHelper.WriteRelativeOffsets(codeBytes, new[]
            {
                (triggersCode.ToInt64() + 0x7, zDirection.ToInt64(), 7, 0x7 + 2),
                (triggersCode.ToInt64() + 0x1C, zDirection.ToInt64(), 7, 0x1C + 2),
                (triggersCode.ToInt64() + 0x2D, Hooks.NoClipTriggers + 0x5, 5, 0x2D + 1),
            });
            memoryService.WriteBytes(triggersCode, codeBytes);
        }

        private void WriteUpdateCoords(IntPtr updateCoordsCode)
        {
            var codeBytes = AsmLoader.GetAsmBytes("NoClip_UpdateCoords");
            var zDirection = CodeCaveOffsets.Base + (int)CodeCaveOffsets.NoClip.ZDirection;

            AsmHelper.WriteRelativeOffsets(codeBytes, new[]
            {
                (updateCoordsCode.ToInt64() + 0x7, WorldChrMan.Base.ToInt64(), 7, 0x7 + 3),
                (updateCoordsCode.ToInt64() + 0x42, Functions.ChrInsByHandle, 5, 0x42 + 1),
                (updateCoordsCode.ToInt64() + 0x7E, InputManager.Base.ToInt64(), 7, 0x7E + 3),
                (updateCoordsCode.ToInt64() + 0xE8, FieldArea.Base.ToInt64(), 7, 0xE8 + 3),
                (updateCoordsCode.ToInt64() + 0x104, Functions.MatrixVectorProduct, 5, 0x104 + 1),
                (updateCoordsCode.ToInt64() + 0x13A, zDirection.ToInt64(), 6, 0x13A + 2),
                (updateCoordsCode.ToInt64() + 0x164, zDirection.ToInt64(), 7, 0x164 + 2),
                (updateCoordsCode.ToInt64() + 0x19B, Hooks.UpdateCoords + 0xB, 5, 0x19B + 1)
            });
            memoryService.WriteBytes(updateCoordsCode, codeBytes);
        }

        public float GetSpeed() =>
            memoryService.ReadFloat((IntPtr)memoryService.ReadInt64(CSFlipperImp.Base) + CSFlipperImp.GameSpeed);

        public void SetSpeed(float speed) =>
            memoryService.WriteFloat((IntPtr)memoryService.ReadInt64(CSFlipperImp.Base) + CSFlipperImp.GameSpeed, speed);
        
        
        public void ToggleDrawHitbox(bool isDrawHitboxEnabled) =>
            memoryService.WriteUInt8((IntPtr)memoryService.ReadInt64(DamageManager.Base) + DamageManager.HitboxView,
                isDrawHitboxEnabled ? 1 : 0);

        public void ToggleWorldHitDraw(int offset, bool isEnabled) =>
            memoryService.WriteUInt8(WorldHitMan.Base + offset, isEnabled ? 1 : 0);

        public void SetColDrawMode(int val) =>
            memoryService.WriteUInt8(WorldHitMan.Base + WorldHitMan.Mode, (byte)val);
    }
}