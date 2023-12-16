using KBS_project;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
        public ProfileScreen? ProfileScreen { get; set; }
        private ProfileEditScreen profileEditScreen = new();
        private AccountEditScreen accountEditScreen = new();
        private PhotoEditScreen photoEditScreen = new();
        private ChatScreen chatScreen;

        public Navigation()
        {
            InitializeComponent();

            DataContext = this;
        }

        public void SwitchToProfileScreen(object? sender, EventArgs e)
        {
            if (ProfileScreen == null) ProfileScreen = new();
            contentFrame.Content = ProfileScreen;
        }

        public void SwitchToChatScreen(object? sender, EventArgs e)
        {
            if (chatScreen == null) chatScreen = new();
            contentFrame.Content = chatScreen;
        }

        public void InitScreens()
        {
            profileEditScreen.InitializePage();
            accountEditScreen.InitializePage();

            profilesButton.Click += (s, e) => contentFrame.Content = ProfileScreen;
            messagesButton.Click += SwitchToChatScreen;

            profileEditScreen.matchingQuizButton.Click += (s, e) => contentFrame.Content = matchingQuiz;

            editProfileButton.Click += (s, e) =>
            {
                contentFrame.Content = profileEditScreen;
                editProfileButton.Visibility = Visibility.Hidden;
                editAccountButton.Visibility = Visibility.Hidden;
            };

            editAccountButton.Click += (s, e) =>
            {
                contentFrame.Content = accountEditScreen;
                editProfileButton.Visibility = Visibility.Hidden;
                editAccountButton.Visibility = Visibility.Hidden;
            };

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
            }
            else if (editProfileButton.Visibility == Visibility.Visible)
            {
                editProfileButton.Visibility = Visibility.Hidden;
                editAccountButton.Visibility = Visibility.Hidden;
            }
        }

        private void LogoutButtonFocus(object? sender, MouseEventArgs e)
        {
            logoutButton.Source = new BitmapImage(new Uri("/Views/LogoutIconFocus.png", UriKind.Relative));
        }

        private void LogoutButtonFocusLost(object? sender, MouseEventArgs e)
        {
            logoutButton.Source = new BitmapImage(new Uri("/Views/LogoutIcon.png", UriKind.Relative));
        }
    }
}
