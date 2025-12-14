// 

namespace SilkyRing.Interfaces;

public interface IDlcService
{
    void CheckDlc();
    public bool IsDlcAvailable { get; }
}