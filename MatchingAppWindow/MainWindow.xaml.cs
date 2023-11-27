using KBS_project;
using MatchingApp.DataAccess.SQL;
using MatchingAppWindow.Views;
using Microsoft.VisualBasic;
using Microsoft.Win32;
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
        public static Profile? profile;
        private MatchingAppRepository repository;

        private StartScreen startScreen = new();
        private RegisterScreen registerScreen;
        private Matchingquiz matchingquiz = new();
        private Navigation navigation = new();

        public MainWindow()
        {
            repository = new MatchingAppRepository();

            registerScreen = new(repository);

            InitializeComponent();

            startScreen.RegisterButton.Click += (object sender, RoutedEventArgs e) => Content = registerScreen;
            registerScreen.CreateAccountButton.Click += registerAccount;

            Content = startScreen;
        }

        private void registerAccount(Object? sender, EventArgs args)
        {
            Content = navigation;
        }


    }
}
