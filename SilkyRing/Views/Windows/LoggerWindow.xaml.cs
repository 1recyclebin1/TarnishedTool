using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SilkyRing.ViewModels;

namespace SilkyRing.Views.Windows
{
    public partial class LoggerWindow : Window
    {
        private readonly LoggerViewModel _loggerViewModel;
        public LoggerWindow(LoggerViewModel viewModel)
        {
            InitializeComponent();
            _loggerViewModel = viewModel;
            DataContext = _loggerViewModel;
        }

        private void ClearUniqueSetEvents_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.ClearUniqueSetEvents();
        }

        private void ClearUniqueSpEffects_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.ClearUniqueSpEffects();
        }

        private void ClearConsole_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.ClearConsole();
        }

        private void PauseAllLogging_Click(object sender, RoutedEventArgs e)
        {
            _loggerViewModel.PauseAllLogging();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Maximized;
            }
            else
            {
                DragMove();
            }
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
        
    }
}