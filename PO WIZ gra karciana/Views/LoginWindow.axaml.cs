using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PO_WIZ_gra_karciana.Views
{
    public partial class LoginWindow : Window
    {
        public string Nick { get; private set; } = "Gracz1";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void OnLoginClick(object? sender, RoutedEventArgs e)
        {
            var wpisanyNick = NickTextBox.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(wpisanyNick))
                Nick = wpisanyNick;

            Close();
        }
    }
}
