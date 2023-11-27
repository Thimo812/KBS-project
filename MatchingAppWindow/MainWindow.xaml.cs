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
using MatchingAppWindow;

namespace MatchingAppWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StartScreen startScreen = new();
        private RegisterScreen registerScreen = new();
        private Matchingquiz matchingquiz = new();
        private Navigation navigation = new();

        public static Profile? profile;
        private MatchingAppRepository repository;
        public MainWindow()
        {
            repository = new MatchingAppRepository();

            InitializeComponent();

            startScreen.RegisterButton.Click += (object sender, RoutedEventArgs e) => Content = registerScreen;
            registerScreen.CreateAccountButton.Click += registerAccount;


            Content = startScreen;
        }

        private void registerAccount(Object? sender, EventArgs args)
        {
            profile = registerScreen.Profile;
            Content = navigation;
            repository.SaveProfile(profile);
        }

        public void SwitchToProfileScreen(Object sender, RoutedEventArgs e)
        {
            var profileEditScreen = new ProfileEditScreen();
            profileEditScreen.PhotoScreenButton.Click += SwitchToPhotoScreen;
            profileEditScreen.AccountScreenButton.Click += SwitchToAccountScreen;

            Content = profileEditScreen;
        }

        public void SwitchToAccountScreen(Object sender, RoutedEventArgs e)
        {
            var accountScreen = new AccountEditScreen();
            accountScreen.PhotoScreenButton.Click += SwitchToPhotoScreen;
            accountScreen.ProfileEditButton.Click += SwitchToProfileScreen;

            Content = accountScreen;
        }

        public void SwitchToPhotoScreen(Object sender, RoutedEventArgs e)
        {
            var photoScreen = new PhotoEditScreen();
            photoScreen.ProfileEditButton.Click += SwitchToProfileScreen;
            photoScreen.AccountScreenButton.Click += SwitchToAccountScreen;

            Content = photoScreen;
        }

    }
}
