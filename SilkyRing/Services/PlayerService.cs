using System;
using System.Numerics;
using SilkyRing.Interfaces;
using SilkyRing.Memory;
using SilkyRing.Models;
using SilkyRing.Utilities;
using static SilkyRing.Memory.Offsets;

namespace SilkyRing.Services
{
    public class PlayerService(MemoryService memoryService, HookManager hookManager) : IPlayerService
    {
        public Vector3 GetPlayerPos() =>
            ReadVector3(GetChrPhysicsPtr() + (int)ChrIns.ChrPhysicsOffsets.Coords);

        public void SavePos()
        {
        }

        public void RestorePos()
        {
        }

        public PosWithHurtbox GetPosWithHurtbox()
        {
            var physPtr = GetChrPhysicsPtr();
            var position = ReadVector3(physPtr + (int)ChrIns.ChrPhysicsOffsets.Coords);
            var capsuleRadius = memoryService.ReadFloat(physPtr + (int)ChrIns.ChrPhysicsOffsets.HurtCapsuleRadius);
            return new PosWithHurtbox(position, capsuleRadius);
        }

        public void SetHp(int hp) =>
            memoryService.WriteInt32(GetChrDataPtr() + (int)ChrIns.ChrDataOffsets.Health, hp);

        public int GetCurrentHp() =>
            memoryService.ReadInt32(GetChrDataPtr() + (int)ChrIns.ChrDataOffsets.Health);

        public int GetMaxHp() =>
            memoryService.ReadInt32(GetChrDataPtr() + (int)ChrIns.ChrDataOffsets.MaxHealth);

        public void SetFullHp()
        {
            var full = memoryService.ReadInt32(GetChrDataPtr() + (int)ChrIns.ChrDataOffsets.MaxHealth);
            memoryService.WriteInt32(GetChrDataPtr() + (int)ChrIns.ChrDataOffsets.Health, full);
        }

        public void SetRtsr()
        {
            var full = memoryService.ReadInt32(GetChrDataPtr() + (int)ChrIns.ChrDataOffsets.MaxHealth);
            memoryService.WriteInt32(GetChrDataPtr() + (int)ChrIns.ChrDataOffsets.Health, (full * 20) / 100 - 1);
        }

        // public int GetSp() =>
        //     _memoryIo.ReadInt32(GetPlayerCtrlField(GameManagerImp.ChrCtrlOffsets.Stamina));
        //
        // public void SetSp(int sp) =>
        //     _memoryIo.WriteInt32(GetPlayerCtrlField(GameManagerImp.ChrCtrlOffsets.Stamina), sp);

        public float GetSpeed() =>
            memoryService.ReadFloat(GetChrBehaviorPtr() + (int)ChrIns.ChrBehaviorOffsets.AnimSpeed);

        public void SetSpeed(float speed) =>
            memoryService.WriteFloat(GetChrBehaviorPtr() + (int)ChrIns.ChrBehaviorOffsets.AnimSpeed, speed);

        public void ToggleInfinitePoise(bool isInfinitePoiseEnabled)
        {
            var code = CodeCaveOffsets.Base + CodeCaveOffsets.InfinitePoise;

            if (isInfinitePoiseEnabled)
            {
                var origin = Hooks.InfinitePoise;
                var hook = Hooks.InfinitePoise;
                var codeBytes = AsmLoader.GetAsmBytes("InfinitePoise");
                var bytes = BitConverter.GetBytes(WorldChrMan.Base.ToInt64());
                Array.Copy(bytes, 0, codeBytes, 0x1 + 2, 8);
                AsmHelper.WriteJumpOffsets(codeBytes, new[]
                {
                    (hook, 7, code + 0x1D, 0x1D + 1),
                    (hook, 7, code + 0x2A, 0x2A + 1),
                });
                memoryService.WriteBytes(code, codeBytes);
                hookManager.InstallHook(code.ToInt64(), hook, new byte[]
                    { 0x80, 0xBF, 0x5F, 0x02, 0x00, 0x00, 0x00 });
            }
            else
            {
                hookManager.UninstallHook(code.ToInt64());
            }
        }

        public void ApplySpEffect(long spEffectId)
        {
            var bytes = AsmLoader.GetAsmBytes("SetSpEffect");
            var playerIns =
                memoryService.ReadInt64((IntPtr)memoryService.ReadInt64(WorldChrMan.Base) + WorldChrMan.PlayerIns);
            AsmHelper.WriteAbsoluteAddresses(bytes, new[]
            {
                (playerIns, 0x0 + 2),
                (spEffectId, 0xA + 2),
                (Funcs.SetSpEffect, 0x18 + 2)
            });
            memoryService.AllocateAndExecute(bytes);
        }

        public void ToggleChrDataFlag(int offset, byte bitmask, bool isEnabled)
        {
            var chrData = GetChrDataPtr();

            memoryService.SetBitValue(chrData + offset, bitmask, isEnabled);
        }

        public void ToggleDebugFlag(int offset, bool isEnabled) =>
            memoryService.WriteUInt8(WorldChrManDbg.Base + offset, isEnabled ? 1 : 0);

        public void ToggleNoRuneGain(bool isNoRuneGainEnabled) =>
            memoryService.WriteBytes(Patches.NoRunesFromEnemies,
                isNoRuneGainEnabled
                    ? [0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90]
                    : [0x41, 0xFF, 0x91, 0xC8, 0x05, 0x00, 0x00]);

        public void ToggleNoRuneArcLoss(bool isNoRuneArcLossEnabled) =>
            memoryService.WriteUInt8(Patches.NoRuneArcLoss, isNoRuneArcLossEnabled ? 0xEB : 0x74);

        public void ToggleNoRuneLoss(bool isNoRuneLossEnabled) =>
            memoryService.WriteBytes(Patches.NoRuneLossOnDeath,
                isNoRuneLossEnabled
                    ? [0x90, 0x90, 0x90]
                    : [0x89, 0x45, 0x6C]);

        public void SetNewGame(int value) =>
            memoryService.WriteInt32((IntPtr)memoryService.ReadInt64(GameDataMan.Base) + GameDataMan.NewGame, value);

        public int GetNewGame() =>
            memoryService.ReadInt32((IntPtr)memoryService.ReadInt64(GameDataMan.Base) + GameDataMan.NewGame);

        public void GiveRunes(int runes)
        {
            var bytes = AsmLoader.GetAsmBytes("GiveRunes");
            var playerGameData =
                memoryService.ReadInt64((IntPtr)memoryService.ReadInt64(GameDataMan.Base) +
                                        GameDataMan.PlayerGameData);
            AsmHelper.WriteAbsoluteAddresses(bytes, new[]
            {
                (playerGameData, 0x0 + 2),
                (runes, 0xA + 2),
                (Funcs.GiveRunes, 0x14 + 2)
            });

            memoryService.AllocateAndExecute(bytes);
        }

        public int GetRuneLevel() =>
            memoryService.ReadInt32((IntPtr)memoryService.ReadInt64(GameDataMan.Base) + GameDataMan.RuneLevel);

        private IntPtr GetChrDataPtr() =>
            memoryService.FollowPointers(WorldChrMan.Base, [WorldChrMan.PlayerIns, ..ChrIns.ChrDataModule], true);

        private IntPtr GetChrPhysicsPtr() =>
            memoryService.FollowPointers(WorldChrMan.Base, [WorldChrMan.PlayerIns, ..ChrIns.ChrPhysicsModule], true);

        private IntPtr GetChrBehaviorPtr() =>
            memoryService.FollowPointers(WorldChrMan.Base, [WorldChrMan.PlayerIns, ..ChrIns.ChrBehaviorModule], true);

        private Vector3 ReadVector3(IntPtr address)
        {
            byte[] coordBytes = memoryService.ReadBytes(address, 12);
            return new Vector3(
                BitConverter.ToSingle(coordBytes, 0),
                BitConverter.ToSingle(coordBytes, 4),
                BitConverter.ToSingle(coordBytes, 8)
            );
        }
    }
}