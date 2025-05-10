using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System;

namespace PO_WIZ_gra_karciana;

public partial class GraWojna : Window
{
    private readonly List<string> _cards = new();
    private readonly Random _rng = new();

    public GraWojna()
    {
        InitializeComponent();
  
    }

   
}