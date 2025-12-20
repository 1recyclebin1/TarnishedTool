// 

using System.Windows.Controls;
using SilkyRing.ViewModels;

namespace SilkyRing.Views.Tabs;

public partial class ItemTab
{
    public ItemTab(ItemViewModel itemViewModel)
    {
        InitializeComponent();
        DataContext = itemViewModel;
    }
}