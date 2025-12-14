// 

using SilkyRing.Interfaces;

namespace SilkyRing.ViewModels;

public class ItemViewModel
{
    private readonly IItemService _itemService;

    public ItemViewModel(IItemService itemService)
    {
        _itemService = itemService;
        
        
    }
}