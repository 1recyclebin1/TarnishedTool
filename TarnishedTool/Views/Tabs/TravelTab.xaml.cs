using System.Windows.Controls;
using TarnishedTool.ViewModels;

namespace TarnishedTool.Views.Tabs
{
    public partial class TravelTab : UserControl
    {
        public TravelTab(TravelViewModel travelViewModel)
        {
            InitializeComponent();
            DataContext = travelViewModel;
        }
    }
}