// 

using System.Numerics;

namespace SilkyRing.Models;

public class Position(uint blockId, Vector3 globalCoords, float globalAngle)
{
    public uint BlockId { get; set; } = blockId;
    public Vector3 GlobalCoords { get; set; } = globalCoords;
    public float GlobalAngle { get; set; } = globalAngle;
}