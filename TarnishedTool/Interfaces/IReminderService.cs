// 

namespace TarnishedTool.Interfaces;

public interface IReminderService
{
    void TrySetReminder();
    void SetPlayerIconActive(bool active);
    void SetTargetIconActive(bool active);

}