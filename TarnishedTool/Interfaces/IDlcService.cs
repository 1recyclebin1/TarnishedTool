// 

namespace TarnishedTool.Interfaces;

public interface IDlcService
{
    void CheckDlc();
    public bool IsDlcAvailable { get; }
}