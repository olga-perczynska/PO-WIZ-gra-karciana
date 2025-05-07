using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using PO_WIZ_gra_karciana.Models;
namespace PO_WIZ_gra_karciana.Views
{
    public partial class piotrek : Window
    {
        
        private List<string> _deck = new();
        private List<string> _player1Hand = new();
        private List<string> _player2Hand = new();
        private Random _random = new();
        private string _piotrusCard = "ghost";
        private int _turn = 1;
        private MainWindow _mainWindow;

        private readonly Dictionary<string, string> _emojiMap = new()
        {
            { "dog", "🐶" }, { "cat", "🐱" }, { "fox", "🦊" },
            { "rabbit", "🐰" }, { "mouse", "🐭" }, { "frog", "🐸" },
            { "panda", "🐼" }, { "ghost", "👻" }
        };

        public piotrek(MainWindow mainWindow)
        {
            InitializeComponent();
            InitGame();
            _mainWindow = mainWindow;
        }


        private void InitGame()
        {
            _deck.Clear();
            _player1Hand.Clear();
            _player2Hand.Clear();
            _turn = 1;

            var cards = new List<string> { "dog", "cat", "fox", "rabbit", "mouse", "frog", "panda" };
            _deck = cards.Concat(cards).Append(_piotrusCard).OrderBy(_ => Guid.NewGuid()).ToList();

            for (int i = 0; i < _deck.Count; i++)
            {
                if (i % 2 == 0)
                    _player1Hand.Add(_deck[i]);
                else
                    _player2Hand.Add(_deck[i]);
            }

            RemovePairs(_player1Hand);
            RemovePairs(_player2Hand);
            UpdateUI();
        }

        private void RemovePairs(List<string> hand)
        {
            var grouped = hand.GroupBy(c => c).Where(g => g.Count() >= 2).ToList();
            foreach (var pair in grouped)
            {
                hand.Remove(pair.Key);
                hand.Remove(pair.Key);
            }
        }

        private void UpdateUI()
        {
            Player1Panel.Children.Clear();
            Player2Panel.Children.Clear();

            var currentPlayerHand = _turn == 1 ? _player1Hand : _player2Hand;
            var opponentHand = _turn == 1 ? _player2Hand : _player1Hand;
            var targetPanel = _turn == 1 ? Player2Panel : Player1Panel;

            foreach (var card in currentPlayerHand)
            {
                var text = new TextBlock
                {
                    Text = _emojiMap[card],
                    FontSize = 24,
                    Margin = new Thickness(4),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };
                if (_turn == 1)
                    Player1Panel.Children.Add(text);
                else
                    Player2Panel.Children.Add(text);
            }
            var drawButton = new Button
            {
                Content = "Losuj kartę",
                Margin = new Thickness(10)
            };
            drawButton.Click += DrawRandomCard_Click;
            targetPanel.Children.Add(drawButton);

            StatusLabel.Text = _turn == 1
                ? "Tura: Gracz 1"
                : "Tura: Gracz 2";
        }

        private void DrawRandomCard_Click(object? sender, RoutedEventArgs e)
        {
            var myHand = _turn == 1 ? _player1Hand : _player2Hand;
            var opponentHand = _turn == 1 ? _player2Hand : _player1Hand;

            if (opponentHand.Count == 0)
                return;

            int index = _random.Next(opponentHand.Count);
            var drawnCard = opponentHand[index];
            opponentHand.RemoveAt(index);

            if (myHand.Contains(drawnCard))
            {
                myHand.Remove(drawnCard); 
            }
            else
            {
                myHand.Add(drawnCard); 
            }

            if (CheckEnd()) return;

            _turn = _turn == 1 ? 2 : 1;
            UpdateUI();
        }

        private bool CheckEnd()
        {
            if (_player1Hand.Count == 1 && _player1Hand.Contains(_piotrusCard))
            {
                _ = ShowResult("Gracz 1 przegral!");
                return true;
            }
            else if (_player2Hand.Count == 1 && _player2Hand.Contains(_piotrusCard))
            {
                _ = ShowResult("Gracz 2 przegral!");
                return true;
            }
            return false;
        }

        private async System.Threading.Tasks.Task ShowResult(string message)
        {
            var dialog = new Window
            {
                Width = 300,
                Height = 100,
                Content = new TextBlock
                {
                    Text = message,
                    FontSize = 16,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                }
            };

            dialog.ShowDialog(this);
            await System.Threading.Tasks.Task.Delay(3000);
            dialog.Close();
            InitGame();
            string wyniko = "Wygral Jaco";
            _mainWindow?.ZapiszHistorieGry("Piotruś", wyniko);
        }
    }
}
