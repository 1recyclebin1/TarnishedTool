// 

using System.Windows;

namespace TarnishedTool.Views.Windows;

public partial class SpEffectsWindow : Window
{
    public SpEffectsWindow()
    {
        InitializeComponent();
        
        if (Application.Current.MainWindow != null)
        {
            Application.Current.MainWindow.Closing += (sender, args) => { Close(); };
        }
    }
}