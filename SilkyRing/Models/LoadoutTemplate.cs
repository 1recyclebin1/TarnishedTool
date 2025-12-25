// 

using System.Collections.Generic;

namespace SilkyRing.Models;

public class LoadoutTemplate
{
    public string Name { get; set; }
    public List<ItemTemplate> Items { get; set; } = new();
}