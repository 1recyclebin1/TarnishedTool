// 

using TarnishedTool.ViewModels;

namespace TarnishedTool.Views.Tabs;

public partial class ItemTab
{
    public ItemTab(ItemViewModel itemViewModel)
    {
        InitializeComponent();
        DataContext = itemViewModel;
    }
}