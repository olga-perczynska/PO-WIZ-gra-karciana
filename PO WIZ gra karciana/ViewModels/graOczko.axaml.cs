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
        public string Figura { get; set; }  = "";
        public string Kolor { get; set; }   = "";
        public int Wartosc { get; set; }  = 0;

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
        TuraText.Text = "Tura: GRACZA";

        InicjalizujTalie();

        GraczKartyPanel.Children.Clear();
        KomputerKartyPanel.Children.Clear();

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
    private void Dobierz_Click(object? sender, RoutedEventArgs e)
    {
        var kartaGracza = taliaKart[0];  
        taliaKart.RemoveAt(0);
        kartyGracza.Add(kartaGracza);
        punktyGracza += kartaGracza.Wartosc;
        GraczKartyPanel.Children.Add(WczytajObrazKarty(kartaGracza));

        if (punktyGracza > 21)
        {
            WynikText.Text = $"Przegra³eœ! Masz {punktyGracza} punktów.";

           // ZablokujPrzyciski();
        }
    }

    private async void Pas_Click(object? sender, RoutedEventArgs e)
    {
        // ZablokujPrzyciski();
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

        if (punktyKomputera > 21 || punktyGracza > punktyKomputera)
            wynik = "Wygra³eœ!";
        else if (punktyGracza == punktyKomputera)
            wynik = "Remis!";
        else
            wynik = "Przegra³eœ!";

        WynikText.Text = $"Komputer: {punktyKomputera} pkt — {wynik}";
        string wyniko = "Wygra? gracz: Piotrek";  // lub np. "Remis", "Gracz przegra?", itd.
        _mainWindow?.ZapiszHistorieGry("Oczko", wyniko);
    }

    private void ZablokujPrzyciski()
    {
        this.FindControl<Button>("Dobierz_Click").IsEnabled = false;
        this.FindControl<Button>("Pas_Click").IsEnabled = false;
    }
}