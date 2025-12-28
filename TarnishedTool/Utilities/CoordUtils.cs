// 

using System.Numerics;

namespace TarnishedTool.Utilities;

public static class CoordUtils
{
    public const int BaseGameOverworldId = 0x3C;
    public const int DlcOverworldId = 0x3D;
    public const int GridSize = 256;
    
    public static Vector3 ToAbsolute(Vector3 globalCoords, uint blockId)
    {
        byte area = (byte)((blockId >> 24) & 0xFF);

        if (area == BaseGameOverworldId || area == DlcOverworldId)
        {
            byte gridX = (byte)((blockId >> 16) & 0xFF);
            byte gridZ = (byte)((blockId >> 8) & 0xFF);

            return new Vector3(
                globalCoords.X + GridSize * gridX,
                globalCoords.Y,
                globalCoords.Z + GridSize * gridZ
            );
        }

        return globalCoords;
    }
}