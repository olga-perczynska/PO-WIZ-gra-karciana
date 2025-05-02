using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PO_WIZ_gra_karciana;

public partial class graOczko : Window
{
    private int punktyGracza = 0;
    private int punktyKomputera = 0;
    private Random random = new Random();
    private List<int> kartyGracza = new List<int>();
    private List<int> kartyKomputer = new List<int>();

    public graOczko()
    {
        InitializeComponent();
    }

    private void Rozdanie_Click(object? sender, RoutedEventArgs e)
    {
      
        for(int i=0; i<2;i++)
        {
            int karta = random.Next(2, 12);
            kartyGracza.Add(karta);
            punktyGracza += karta;

            int kartaKomputer = random.Next(2, 12);
            kartyKomputer.Add(kartaKomputer);
            punktyKomputera += karta;
        }

        GraczPunktyText.Text = "Karty gracza: " + string.Join(" ", kartyGracza);
        KomputerPunktyText.Text = $"Karty krupiera: {kartyKomputer[0]}  zakryta ";


    }
    private void Dobierz_Click(object? sender, RoutedEventArgs e)
    {
        int karta = random.Next(2, 12);
        kartyGracza.Add(karta);
        punktyGracza += karta;
        GraczPunktyText.Text = "Karty gracza: " + string.Join(" ", kartyGracza);

        if (punktyGracza > 21)
        {
            WynikText.Text = $"Przegra³eœ! Masz {punktyGracza} punktów.";

            //ZablokujPrzyciski();
        }
    }

    private async void Pas_Click(object? sender, RoutedEventArgs e)
    {
        KomputerPunktyText.Text = "Karty krupiera: " + string.Join(" ", kartyKomputer);

        while (punktyKomputera < 17)
        {
            await Task.Delay(3000);
            int karta = random.Next(2, 12);
            kartyKomputer.Add(karta);
            punktyKomputera += karta;
            KomputerPunktyText.Text = "Karty krupiera: " + string.Join(" ", kartyKomputer);
        }

        string wynik = "";

        if (punktyKomputera > 21 || punktyGracza > punktyKomputera)
            wynik = "Wygra³eœ!";
        else if (punktyGracza == punktyKomputera)
            wynik = "Remis!";
        else
            wynik = "Przegra³eœ!";

        WynikText.Text = $"Komputer: {punktyKomputera} pkt — {wynik}";

        //ZablokujPrzyciski();
    }

    private void ZablokujPrzyciski()
    {
        this.FindControl<Button>("Dobierz_Click").IsEnabled = false;
        this.FindControl<Button>("Pas_Click").IsEnabled = false;
    }
}