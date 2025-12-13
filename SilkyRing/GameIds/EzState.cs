// 

namespace SilkyRing.GameIds;

public static class EzState
{
    public class TalkCommand(int commandId, int[] @params)
    {
        public int CommandId { get; } = commandId;
        public int[] Params { get; } = @params;
    }

    public static class TalkCommands
    {
        public static readonly TalkCommand OpenKaleShop = new(22, [100500, 100524]);
    }
}