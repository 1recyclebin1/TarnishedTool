// 

namespace SilkyRing.Interfaces;

public interface ISettingsService
{
    void Quitout();
    void ToggleStutterFix(bool isStutterFixEnabled);
    void ToggleDisableAchievements(bool isEnabled);
}