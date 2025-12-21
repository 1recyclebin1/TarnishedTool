// 

using System.IO;

namespace SilkyRing.GameIds;

public class Emevd
{
    public class EmevdCommand
    {
        public int GroupId { get; }
        public int CommandId { get; }
        public byte[] ParamData { get; }

        public EmevdCommand(int groupId, int commandId, params object[] args)
        {
            GroupId = groupId;
            CommandId = commandId;
            ParamData = Pack(args);
        }

        private static byte[] Pack(object[] args)
        {
            if (args.Length == 0) return [];

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            int offset = 0;

            foreach (var arg in args)
            {
                int alignment = arg is sbyte or byte ? 1 : arg is short or ushort ? 2 : 4;
                int padding = (alignment - (offset % alignment)) % alignment;
                offset += padding + alignment;
            
                for (int i = 0; i < padding; i++) bw.Write((byte)0);

                switch (arg)
                {
                    case sbyte v:  bw.Write(v); break;
                    case byte v:   bw.Write(v); break;
                    case short v:  bw.Write(v); break;
                    case ushort v: bw.Write(v); break;
                    case int v:    bw.Write(v); break;
                    case uint v:   bw.Write(v); break;
                    case float v:  bw.Write(v); break;
                }
            }

            return ms.ToArray();
        }
    }

    public static class EmevdCommands
    {
        public static readonly EmevdCommand Rest = new EmevdCommand(2004, 47);
    }
}