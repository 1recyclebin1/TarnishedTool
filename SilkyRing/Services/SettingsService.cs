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

    public void ToggleStutterFix(bool isEnabled) =>
        memoryService.WriteUInt8(
            (IntPtr)memoryService.ReadInt64(UserInputManager.Base) + UserInputManager.SteamInputEnum,
            isEnabled ? 1 : 0);

    public void ToggleDisableAchievements(bool isEnabled)
    {
        var isAwardAchievementsEnabledFlag = memoryService.FollowPointers(CSTrophy.Base, [
            CSTrophy.CSTrophyPlatformImp_forSteam,
            CSTrophy.IsAwardAchievementEnabled
        ], false);
        memoryService.WriteUInt8(isAwardAchievementsEnabledFlag, isEnabled ? 0 : 1);
    }

    public void ToggleNoLogo(bool isEnabled) =>
        memoryService.WriteBytes(Patches.NoLogo, isEnabled ? [0x90, 0x90] : [0x74, 0x53]);
}