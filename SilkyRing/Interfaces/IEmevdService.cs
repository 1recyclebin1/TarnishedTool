// 

using static SilkyRing.GameIds.Emevd;

namespace SilkyRing.Interfaces;

public interface IEmevdService
{
    void ExecuteEmevdCommand(EmevdCommand command);
}