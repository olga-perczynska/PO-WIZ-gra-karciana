using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PO_WIZ_gra_karciana.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PO_WIZ_gra_karciana;

public partial class GraWojna : Window
{
    private MainWindow _mainWindow;
    private Queue<string> _playerDeck = new();
    private Queue<string> _computerDeck = new();
    private readonly List<string> _allCards = new();
    private readonly Random _rng = new();

    public GraWojna(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        InitDecks();
        _ = PlayRoundAsync();
    }

    private void InitDecks()
    {
        var suits = new[] { "hearts", "diamonds", "clubs", "spades" };

        for (int val = 2; val <= 14; val++)
        {
            string name = val switch
            {
                11 => "jack",
                12 => "queen",
                13 => "king",
                14 => "ace",
                _ => val.ToString()
            };

            foreach (var suit in suits)
            {
                _allCards.Add($"{val}_{name}_of_{suit}");
            }
        }

        var shuffled = _allCards.OrderBy(_ => _rng.Next()).ToList();
        _playerDeck = new Queue<string>(shuffled.Take(26));
        _computerDeck = new Queue<string>(shuffled.Skip(26));
        UpdatePoints();
    }

    private async void OnPlayClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await PlayRoundAsync();
    }

    private async Task PlayRoundAsync()
    {
        if (_playerDeck.Count == 0)
        {
            StatusText.Text = "💀 Gracz przegrał – komputer wygrał grę!";
            UpdatePoints();
            _mainWindow?.ZapiszHistorieGry("Wojna", "Komputer wygrał");
            return;
        }

        if (_computerDeck.Count == 0)
        {
            StatusText.Text = "🎉 Gracz wygrał grę!";
            UpdatePoints();
            _mainWindow?.ZapiszHistorieGry("Wojna", "Gracz wygrał");
            return;
        }

        var pile = new List<string>();
        string playerCard = _playerDeck.Dequeue();
        string computerCard = _computerDeck.Dequeue();
        pile.Add(playerCard);
        pile.Add(computerCard);

        ShowCardImages(playerCard, computerCard);

        int pVal = GetCardValue(playerCard);
        int cVal = GetCardValue(computerCard);

        if (pVal > cVal)
        {
            StatusText.Text = $"✅ Gracz wygrał rundę kartą {FormatCard(playerCard)} przeciwko {FormatCard(computerCard)}";
            AddToWinner(_playerDeck, pile);
        }
        else if (pVal < cVal)
        {
            StatusText.Text = $"❌ Komputer wygrał rundę kartą {FormatCard(computerCard)} przeciwko {FormatCard(playerCard)}";
            AddToWinner(_computerDeck, pile);
        }
        else
        {
            StatusText.Text = $"⚔️ WOJNA! Obaj gracze zagrali {FormatCard(playerCard)}!";
            UpdatePoints();
            await Task.Delay(800);
            ResolveWar(pile);
            return;
        }

        UpdatePoints();
    }

    private void ResolveWar(List<string> pile)
    {
        int maxDraw = Math.Min(4, Math.Min(_playerDeck.Count, _computerDeck.Count));

        if (maxDraw == 0)
        {
            StatusText.Text = "😵 Ktoś nie miał kart do wojny – koniec gry!";
            UpdatePoints();

            if (_playerDeck.Count == 0)
                _mainWindow?.ZapiszHistorieGry("Wojna", "Komputer wygrał");
            else
                _mainWindow?.ZapiszHistorieGry("Wojna", "Gracz wygrał");

            return;
        }

        var pExtra = DrawCards(_playerDeck, maxDraw);
        var cExtra = DrawCards(_computerDeck, maxDraw);

        pile.AddRange(pExtra);
        pile.AddRange(cExtra);

        string finalP = pExtra.Last();
        string finalC = cExtra.Last();

        ShowCardImages(finalP, finalC);

        int pVal = GetCardValue(finalP);
        int cVal = GetCardValue(finalC);

        if (pVal > cVal)
        {
            StatusText.Text = $"🏆 Gracz wygrał WOJNĘ kartą {FormatCard(finalP)}!";
            AddToWinner(_playerDeck, pile);
        }
        else if (pVal < cVal)
        {
            StatusText.Text = $"🤖 Komputer wygrał WOJNĘ kartą {FormatCard(finalC)}!";
            AddToWinner(_computerDeck, pile);
        }
        else
        {
            StatusText.Text = $"😮 Kolejna wojna! Obie ostatnie karty to {FormatCard(finalP)}";
            
        }

        if (_playerDeck.Count == 0)
        {
            StatusText.Text = "💀 Gracz przegrał – komputer wygrał grę!";
            UpdatePoints();
            _mainWindow?.ZapiszHistorieGry("Wojna", "Komputer wygrał");
            return;
        }

        if (_computerDeck.Count == 0)
        {
            StatusText.Text = "🎉 Gracz wygrał grę!";
            UpdatePoints();
            _mainWindow?.ZapiszHistorieGry("Wojna", "Gracz wygrał");
            return;
        }

        UpdatePoints();
    }

    private List<string> DrawCards(Queue<string> deck, int count)
    {
        var drawn = new List<string>();
        for (int i = 0; i < count && deck.Count > 0; i++)
            drawn.Add(deck.Dequeue());
        return drawn;
    }

    private void AddToWinner(Queue<string> winnerDeck, List<string> pile)
    {
        foreach (var card in pile.OrderBy(_ => _rng.Next()))
            winnerDeck.Enqueue(card);
    }

    private void ShowCardImages(string playerCard, string computerCard)
    {
        PlayerImage.Source = LoadImage(playerCard);
        ComputerImage.Source = LoadImage(computerCard);
    }

    private int GetCardValue(string card)
    {
        return int.Parse(card.Split('_')[0]);
    }

    private Bitmap LoadImage(string rawCard)
    {
        string[] parts = rawCard.Split('_');
        string fileName = $"{parts[1]}_of_{parts.Last()}.png";
        var uri = new Uri($"avares://PO_WIZ_gra_karciana/Assets/Karty/{fileName}");
        return new Bitmap(AssetLoader.Open(uri));
    }

    private string FormatCard(string rawCard)
    {
        var parts = rawCard.Split('_');
        string val = parts[1].ToUpper();
        string suit = parts.Last().ToUpper();
        return $"{val} of {suit}";
    }

    private void UpdatePoints()
    {
        PlayerPoints.Text = $"Karty: {_playerDeck.Count}";
        ComputerPoints.Text = $"Karty: {_computerDeck.Count}";
    }
}
