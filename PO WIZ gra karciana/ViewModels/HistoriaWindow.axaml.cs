using Avalonia.Controls;
using PO_WIZ_gra_karciana.Models;
using System.Collections.Generic;

namespace PO_WIZ_gra_karciana.Views
{
    public partial class HistoriaWindow : Window
    {
        public HistoriaWindow(List<GraHistoryczna> historia)
        {
            InitializeComponent();
            HistoriaListBox.ItemsSource = historia;
        }
    }
}
