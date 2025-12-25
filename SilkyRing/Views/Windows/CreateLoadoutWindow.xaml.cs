using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SilkyRing.Models;
using SilkyRing.ViewModels;

namespace SilkyRing.Views;

public partial class CreateLoadoutWindow : Window
{
    public CreateLoadoutWindow(
        Dictionary<string, List<Item>> itemsByCategory,
        List<AshOfWar> ashesOfWar,
        Dictionary<string, LoadoutTemplate> customLoadoutTemplates,
        bool hasDlc)
    {
        InitializeComponent();
        
        var viewModel = new CreateLoadoutViewModel(
            itemsByCategory,
            ashesOfWar,
            customLoadoutTemplates,
            hasDlc,
            ShowInputDialog);
        
        DataContext = viewModel;
        
        if (Application.Current.MainWindow != null)
        {
            Application.Current.MainWindow.Closing += (_, _) => Close();
        }
    }

    private string ShowInputDialog(string prompt, string defaultValue)
    {
        var dialog = new Window
        {
            Title = "Input",
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = this,
            WindowStyle = WindowStyle.None,
            AllowsTransparency = true,
            Background = (System.Windows.Media.Brush)Application.Current.Resources["BackgroundBrush"],
            Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextBrush"]
        };

        var panel = new StackPanel { Margin = new Thickness(10) };
        panel.Children.Add(new TextBlock { Text = prompt, Margin = new Thickness(0, 0, 0, 10) });

        var textBox = new TextBox { Text = defaultValue, Margin = new Thickness(0, 0, 0, 10) };
        panel.Children.Add(textBox);

        var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
        
        var okButton = new Button { Content = "OK", Width = 60, IsDefault = true, Margin = new Thickness(0, 0, 5, 0) };
        okButton.Click += (_, _) => dialog.DialogResult = true;
        buttonPanel.Children.Add(okButton);

        var cancelButton = new Button { Content = "Cancel", Width = 60, IsCancel = true };
        cancelButton.Click += (_, _) => dialog.DialogResult = false;
        buttonPanel.Children.Add(cancelButton);

        panel.Children.Add(buttonPanel);
        dialog.Content = panel;

        return dialog.ShowDialog() == true ? textBox.Text : string.Empty;
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            WindowState = WindowState == WindowState.Maximized 
                ? WindowState.Normal 
                : WindowState.Maximized;
        }
        else
        {
            DragMove();
        }
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    
    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}