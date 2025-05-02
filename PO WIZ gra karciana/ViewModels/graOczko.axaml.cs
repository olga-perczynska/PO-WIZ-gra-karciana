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
            WynikText.Text = $"Przegra�e�! Masz {punktyGracza} punkt�w.";

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
            wynik = "Wygra�e�!";
        else if (punktyGracza == punktyKomputera)
            wynik = "Remis!";
        else
            wynik = "Przegra�e�!";

        WynikText.Text = $"Komputer: {punktyKomputera} pkt � {wynik}";

        //ZablokujPrzyciski();
    }

    private void ZablokujPrzyciski()
    {
        this.FindControl<Button>("Dobierz_Click").IsEnabled = false;
        this.FindControl<Button>("Pas_Click").IsEnabled = false;
    }
}