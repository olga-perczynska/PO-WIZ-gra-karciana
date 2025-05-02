using Avalonia.Controls;

namespace PO_WIZ_gra_karciana.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnOpenNewWindowClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var newWindow = new graOczko();
            newWindow.Show();
        }
    }
}