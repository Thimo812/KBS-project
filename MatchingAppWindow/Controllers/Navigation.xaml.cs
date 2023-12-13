using KBS_project;
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

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : Page
    {
        private Matchingquiz matchingQuiz = new();
        private ProfileScreen profileScreen;
        private ProfileEditScreen profileEditScreen = new();
        private AccountEditScreen accountEditScreen = new();
        private PhotoEditScreen photoEditScreen = new();

        public Navigation()
        {
            InitializeComponent();

            DataContext = this;
        }

        public void SwitchToProfileScreen(object? sender, EventArgs e)
        {
            if (profileScreen == null) profileScreen = new();
            profileScreen.matchingQuizButton.Click += (object sender, RoutedEventArgs e) => contentFrame.Content = matchingQuiz;
            profileScreen.loginButton.Click += (object sender, RoutedEventArgs e) => contentFrame.Content = profileEditScreen;
            contentFrame.Content = profileScreen;
        }

        public void InitScreens()
        {
            profileEditScreen.InitializePage();
            accountEditScreen.InitializePage();
            photoEditScreen.InitializePage();

            profilesButton.Click += (s, e) => contentFrame.Content = profileScreen;

            profileEditScreen.matchingQuizButton.Click += (s, e) => contentFrame.Content = matchingQuiz;

            editProfileButton.Click += ToggleProfileButtons;
            editProfileButton.Click += (sender, e) => contentFrame.Content = profileEditScreen;
            editAccountButton.Click += ToggleProfileButtons;
            editAccountButton.Click += (sender, e) => contentFrame.Content = accountEditScreen;
            editPhotosButton.Click += ToggleProfileButtons;
            editPhotosButton.Click += (sender, e) => contentFrame.Content = photoEditScreen;

            matchingQuiz.ExitPage += (sender, e) => contentFrame.Content = profileEditScreen;

            SwitchToProfileScreen(this, EventArgs.Empty);
        }

        private void ProfileButtonFocus(object? sender, MouseEventArgs e)
        {
            profileButton.Source = new BitmapImage(new Uri("/Views/AccountIconFocus.png", UriKind.Relative));
        }

        private void ProfileButtonFocusLost(object? sender, MouseEventArgs e)
        {
            profileButton.Source = new BitmapImage(new Uri("/Views/AccountIcon.png", UriKind.Relative));
        }

        private void ToggleProfileButtons(object? sender, RoutedEventArgs e)
        {
            if (editProfileButton.Visibility == Visibility.Hidden)
            {
                editProfileButton.Visibility = Visibility.Visible;
                editAccountButton.Visibility = Visibility.Visible;
                editPhotosButton.Visibility = Visibility.Visible;
            }
            else if (editProfileButton.Visibility == Visibility.Visible)
            {
                editProfileButton.Visibility = Visibility.Hidden;
                editAccountButton.Visibility = Visibility.Hidden;
                editPhotosButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
