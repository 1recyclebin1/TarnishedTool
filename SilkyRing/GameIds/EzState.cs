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
        public static readonly TalkCommand OpenUpgrade = new(24, [1]);
        public static readonly TalkCommand LevelUp = new(31, []);
        public static readonly TalkCommand OpenAttunement = new(28, [-1, -1]);
        public static readonly TalkCommand OpenChest = new(30, []);
        public static readonly TalkCommand OpenSell = new(46, [-1, -1]);
        public static readonly TalkCommand OpenAow = new(48, []);
        public static readonly TalkCommand OpenAllot = new(105, []); 
        public static readonly TalkCommand Rebirth = new(113, []); 
        public static readonly TalkCommand OpenPhysick = new(130, []);
        public static readonly TalkCommand AcquireGesture = new(131, [108]); // TODO call with all gesture ids
        public static readonly TalkCommand OpenGreatRunes = new(137, []);
        public static readonly TalkCommand OpenAlterGarments = new(142, [111000, 111399]);
        
        public static readonly TalkCommand[] UpgradeMenuFlags = [
            new(49, [6001, 232]),
            new(49, [6001, 233]),
            new(49, [6001, 234]),
            new(49, [6001, 235]),
        ];
    }
}