using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PO_WIZ_gra_karciana.Views
{
    public partial class gramemory : Window
    {
        private Dictionary<Button, string> _cardMap = new();
        private List<Button> _flipped = new();
        private int _moves = 0;

        public gramemory()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            _cardMap.Clear();
            _flipped.Clear();
            _moves = 0;
            MovesLabel.Text = "Ruchy: 0";
            CardGrid.Children.Clear();

            var emojis = new List<string> { "kot", "pies", "mysz", "lis", "krolik", "zolw", "chomik", "waz" };
            var allCards = emojis.Concat(emojis).OrderBy(_ => Guid.NewGuid()).ToList();

            for (int i = 0; i < 16; i++)
            {
                var btn = new Button
                {
                    FontSize = 24,
                    Tag = i
                };
                btn.Click += Card_Click;
                _cardMap[btn] = allCards[i];
                CardGrid.Children.Add(btn);
            }
        }

        private async void Card_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || _flipped.Contains(btn) || !btn.IsEnabled)
                return;

            btn.Content = _cardMap[btn];
            _flipped.Add(btn);

            if (_flipped.Count == 2)
            {
                _moves++;
                MovesLabel.Text = $"Ruchy: {_moves}";

                await Task.Delay(1000);

                var first = _flipped[0];
                var second = _flipped[1];

                if (_cardMap[first] == _cardMap[second])
                {
                    first.IsEnabled = false;
                    second.IsEnabled = false;
                }
                else
                {
                    first.Content = null;
                    second.Content = null;
                }

                _flipped.Clear();

                if (_cardMap.Keys.All(b => !b.IsEnabled))
                {
                    await Task.Delay(500);
                    var dialog = new Window
                    {
                        Width = 200,
                        Height = 100,
                        Content = new TextBlock
                        {
                            Text = "Wygrales!",
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                        }
                    };
                    dialog.ShowDialog(this);
                    await Task.Delay(2000);
                    dialog.Close();
                    InitGame();
                }
            }
        }
    }
}
