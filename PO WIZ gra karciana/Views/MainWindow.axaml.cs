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
            var memoryWindow = new Views.gramemory(this);
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
        private void OpenGraWojnaClick(object? sender, RoutedEventArgs e)
        {
            var graWojnaWindow = new GraWojna(); 
            graWojnaWindow.Show();
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
                Wynik = $"{NickGracza}: {wynik}"
            };

            historiaGier.Add(graHistoryczna);
        }


        public string NickGracza { get; set; } = "Gracz1";

        private async void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            await login.ShowDialog(this);

            NickGracza = login.Nick;
            Console.WriteLine($"Zalogowano jako: {NickGracza}");
        }


    }
}
