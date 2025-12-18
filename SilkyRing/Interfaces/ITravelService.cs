// 

using SilkyRing.Models;

namespace SilkyRing.Interfaces;

public interface ITravelService
{
    void Warp(Grace grace);
    void WarpToBlockId(Position position);
}