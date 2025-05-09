using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Avalonia.Platform;
using PO_WIZ_gra_karciana.Models;
using PO_WIZ_gra_karciana.Views;

namespace PO_WIZ_gra_karciana;

public partial class graOczko : Window
{
    public class Karta
    {
        public string Figura { get; set; } = "";
        public string Kolor { get; set; } = "";
        public int Wartosc { get; set; } = 0;
    }

    private List<string> gracze;
    private int maxGraczy;

    private int punktyGracza = 0;
    private int punktyKomputera = 0;
    private Random random = new Random();
    private List<Karta> kartyGracza = new List<Karta>();
    private List<Karta> kartyKomputer = new List<Karta>();
    private List<Karta> taliaKart;
    private MainWindow _mainWindow;

    public graOczko(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
    }

    private Image WczytajObrazKarty(Karta karta)
    {
        var uri = new Uri($"avares://PO_WIZ_gra_karciana/Assets/Karty/{karta.Figura}_of_{karta.Kolor}.png");
        var stream = AssetLoader.Open(uri);

        return new Image
        {
            Width = 80,
            Height = 120,
            Source = new Avalonia.Media.Imaging.Bitmap(stream),
            Margin = new Thickness(5)
        };
    }

    private void InicjalizujTalie()
    {
        string[] figury = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king", "ace" };
        string[] kolory = { "hearts", "diamonds", "clubs", "spades" };

        taliaKart = new List<Karta>();

        foreach (var kolor in kolory)
        {
            foreach (var figura in figury)
            {
                int wartosc = figura switch
                {
                    "jack" => 10,
                    "queen" => 10,
                    "king" => 10,
                    "ace" => 11,
                    _ => int.Parse(figura)
                };

                taliaKart.Add(new Karta { Figura = figura, Kolor = kolor, Wartosc = wartosc });
            }
        }

        taliaKart = taliaKart.OrderBy(x => random.Next()).ToList();
    }

    private void Rozdanie_Click(object? sender, RoutedEventArgs e)
    {
        this.FindControl<Button>("RozdanieButton").IsEnabled = false;

        TuraText.Text = "Tura: GRACZA";

        InicjalizujTalie();

        GraczKartyPanel.Children.Clear();
        KomputerKartyPanel.Children.Clear();

        punktyGracza = 0;
        punktyKomputera = 0;
        kartyGracza.Clear();
        kartyKomputer.Clear();

        for (int i = 0; i < 2; i++)
        {
            var kartaGracza = taliaKart[0];
            taliaKart.RemoveAt(0);
            kartyGracza.Add(kartaGracza);
            punktyGracza += kartaGracza.Wartosc;
            GraczKartyPanel.Children.Add(WczytajObrazKarty(kartaGracza));

            var kartaKomputer = taliaKart[0];
            taliaKart.RemoveAt(0);
            kartyKomputer.Add(kartaKomputer);
            punktyKomputera += kartaKomputer.Wartosc;

            if (i == 0)
            {
                KomputerKartyPanel.Children.Add(WczytajObrazKarty(kartaKomputer));
            }
            else
            {
                var uriZakryta = new Uri("avares://PO_WIZ_gra_karciana/Assets/Karty/zakryta.png");
                var streamZakryta = Avalonia.Platform.AssetLoader.Open(uriZakryta);

                KomputerKartyPanel.Children.Add(new Image
                {
                    Width = 80,
                    Height = 120,
                    Source = new Avalonia.Media.Imaging.Bitmap(streamZakryta),
                    Margin = new Thickness(5)
                });
            }
        }
    }

    private async void Dobierz_Click(object? sender, RoutedEventArgs e)
    {
        var kartaGracza = taliaKart[0];
        taliaKart.RemoveAt(0);
        kartyGracza.Add(kartaGracza);
        punktyGracza += kartaGracza.Wartosc;
        GraczKartyPanel.Children.Add(WczytajObrazKarty(kartaGracza));

        if (punktyGracza > 21)
        {
            var gameOverWindow = new GameOverWindow($"Gracz: {punktyGracza} pkt\nPrzegra³eœ!");
            var result = await gameOverWindow.ShowDialog<string>(this);

            _mainWindow?.ZapiszHistorieGry("Oczko", "Gracz przegra³");

            if (result == "restart")
            {
                RestartGame();
            }
            else if (result == "exit")
            {
                Close();
            }
        }
    }


    private async void Pas_Click(object? sender, RoutedEventArgs e)
    {
        ZablokujPrzyciski();
        TuraText.Text = "Tura: KRUPIERA";
        DobieraText.Text = "Krupier dobiera";

        KomputerKartyPanel.Children.Clear();
        foreach (var karta in kartyKomputer)
        {
            KomputerKartyPanel.Children.Add(WczytajObrazKarty(karta));
        }

        while (punktyKomputera < 17)
        {
            await Task.Delay(3000);

            var kartaKomputer = taliaKart[0];
            taliaKart.RemoveAt(0);
            kartyKomputer.Add(kartaKomputer);
            punktyKomputera += kartaKomputer.Wartosc;
            KomputerKartyPanel.Children.Add(WczytajObrazKarty(kartaKomputer));
        }

        DobieraText.Text = "";
        string wynik = "";

        if (punktyGracza > 21)
            wynik = "Przegra³eœ!";
        else if (punktyKomputera > 21 || punktyGracza > punktyKomputera)
            wynik = "Wygra³eœ!";
        else if (punktyGracza == punktyKomputera)
            wynik = "Przegraleœ";
        else
            wynik = "Przegra³eœ!";

        _mainWindow?.ZapiszHistorieGry("Oczko", $"Gracz: {punktyGracza}, Komputer: {punktyKomputera}, {wynik}");

        await Task.Delay(3000);

        var gameOverWindow = new GameOverWindow($"Gracz: {punktyGracza} pkt\nKomputer: {punktyKomputera} pkt\n{wynik}");
        var result = await gameOverWindow.ShowDialog<string>(this);

        if (result == "restart")
        {
            RestartGame();
        }
        else if (result == "exit")
        {
            Close();
        }
    }

    private void ZablokujPrzyciski()
    {
        this.FindControl<Button>("DobierzButton").IsEnabled = false;
        this.FindControl<Button>("PasButton").IsEnabled = false;
    }

    private void RestartGame()
    {
        punktyGracza = 0;
        punktyKomputera = 0;
        kartyGracza.Clear();
        kartyKomputer.Clear();
        GraczKartyPanel.Children.Clear();
        KomputerKartyPanel.Children.Clear();
        WynikText.Text = "";
        TuraText.Text = "Tura: GRACZA";
        DobieraText.Text = "";
        InicjalizujTalie();

        this.FindControl<Button>("DobierzButton").IsEnabled = true;
        this.FindControl<Button>("PasButton").IsEnabled = true;
        this.FindControl<Button>("RozdanieButton").IsEnabled = true;
    }
}
