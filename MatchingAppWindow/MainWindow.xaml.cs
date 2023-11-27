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

        public MainWindow()
        {
            InitializeComponent();

            var startScreen = new StartScreen();

            var matchingQuiz = new Matchingquiz();

            startScreen.RegisterButton.Click += SwitchToRegisterScreen;
            startScreen.LoginButton.Click += SwitchToFilterScreen;

            Content = matchingQuiz;
        }

        public void SwitchToRegisterScreen(Object sender, RoutedEventArgs e)
        {
            SwitchToProfileScreen(sender, e);
            //Content = new RegisterScreen();
        }
        private void SwitchToFilterScreen(Object? sender, EventArgs args)
        {
            Content = new FilterScreen();
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
