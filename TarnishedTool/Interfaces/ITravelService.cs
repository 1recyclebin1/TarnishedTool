// 

using TarnishedTool.Models;

namespace TarnishedTool.Interfaces;

public interface ITravelService
{
    void Warp(Grace grace);
    void WarpToBlockId(Position position);
}