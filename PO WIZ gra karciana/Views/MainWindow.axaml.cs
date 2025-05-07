using Avalonia.Controls;
using Avalonia.Interactivity;
using PO_WIZ_gra_karciana.Models;
using PO_WIZ_gra_karciana.Views;
using System;
using System.Collections.Generic;

namespace PO_WIZ_gra_karciana.Views
{
    public partial class MainWindow : Window
    {
        public List<GraHistoryczna> historiaGier = new List<GraHistoryczna>();

        public MainWindow()
        {
            InitializeComponent();
            HistoriaButton.Click += OtworzHistorieGier_Click;
        }
        private void OpenMemoryGame_Click(object? sender, RoutedEventArgs e)
        {
            var memoryWindow = new Views.gramemory();
            memoryWindow.Show();
        }

        private void OpenPiotrusGame_Click(object? sender, RoutedEventArgs e)
        {
            var piotrusWindow = new Views.piotrek(this);
            piotrusWindow.Show(); 
        }

        private void OnOpenNewWindowClick(object? sender, RoutedEventArgs e)
        {
            var newWindow = new graOczko(this);
            newWindow.Show();
        }

        private void OtworzHistorieGier_Click(object? sender, RoutedEventArgs e)
        {
            var historiaWindow = new HistoriaWindow(historiaGier);
            historiaWindow.Show();
        }

        public void ZapiszHistorieGry(string gra, string wynik)
        {
            var graHistoryczna = new GraHistoryczna
            {
                Data = DateTime.Now.ToString("dd-MM-yyyy"),
                Godzina = DateTime.Now.ToString("HH:mm:ss"),
                Gra = gra,
               Wynik = wynik
            };

            historiaGier.Add(graHistoryczna);
        }
    }
}
