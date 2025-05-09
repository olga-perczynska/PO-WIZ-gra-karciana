using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace PO_WIZ_gra_karciana.Views
{
    public partial class GameOverWindow : Window
    {
        public GameOverWindow(string message)
        {
            InitializeComponent();
            this.FindControl<TextBlock>("MessageText").Text = message;
        }

        private void Restart_Click(object? sender, RoutedEventArgs e)
        {
            Close("restart");
        }

        private void Exit_Click(object? sender, RoutedEventArgs e)
        {
            Close("exit");
        }
    }
}
