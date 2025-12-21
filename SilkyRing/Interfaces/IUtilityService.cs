// 

namespace SilkyRing.Interfaces;

public interface IUtilityService
{
    void ForceSave();
    void ToggleCombatMap(bool isEnabled);
    void ToggleDungeonWarp(bool isEnabled);
    void ToggleNoClip(bool isEnabled);
    float GetSpeed();
    void SetSpeed(float speed);

}