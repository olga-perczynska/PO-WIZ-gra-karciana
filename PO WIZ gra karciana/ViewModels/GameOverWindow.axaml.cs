using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PO_WIZ_gra_karciana.Views
{
    public partial class GameOverWindow : Window
    {
        public GameOverWindow(string wynik)
        {
            InitializeComponent();
            ResultText.Text = wynik;
        }

        private void OnRestartClick(object? sender, RoutedEventArgs e)
        {
            Close("restart");
        }

        private void OnExitClick(object? sender, RoutedEventArgs e)
        {
            Close("exit");
        }
    }
}
