﻿using MatchingAppWindow.Views;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchingAppWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var startScreen = new StartScreen();

            startScreen.RegisterButton.Click += SwitchToRegisterScreen;

            Content = startScreen;

        }

        private void SwitchToRegisterScreen(Object? sender, EventArgs args)
        {
            Content = new RegisterScreen();
        }
    }
}
