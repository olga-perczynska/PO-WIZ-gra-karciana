using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;

namespace PO_WIZ_gra_karciana;

public partial class graOczko : Window
{
    private int punktyGracza = 0;
    private int punktyKomputera = 0;
    private Random random = new Random();
    public graOczko()
    {
        InitializeComponent();
    }
    private void Dobierz_Click(object? sender, RoutedEventArgs e)

    {
        int karta = random.Next(2, 12);
        punktyGracza += karta;
        GraczPunktyText.Text = $"Twoje punkty: {punktyGracza}";

        if (punktyGracza > 21)
        {
            WynikText.Text = $"Przegra³eœ! Masz {punktyGracza} punktów.";

            //ZablokujPrzyciski();
        }
    }

    private void Pas_Click(object? sender, RoutedEventArgs e)
    {

        while (punktyKomputera < 17)
        {
            int karta = random.Next(2, 12);
            punktyKomputera += karta;
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