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
        public ProfileScreen profileScreen;
        private LikesMatchesScreen matchScreen;
        private ProfileEditScreen profileEditScreen = new();
        private AccountEditScreen accountEditScreen = new();
        public ChatScreen ChatScreen { get; private set; }

        public Navigation()
        {
            InitializeComponent();

            DataContext = this;
        }

        public void SwitchToProfileScreen(object? sender, EventArgs e)
        {
            if (profileScreen == null) profileScreen = new();
            contentFrame.Content = profileScreen;
        }

        public void SwitchToLikesMatchesScreen(object? sender, EventArgs e)
        {
            if (matchScreen == null) matchScreen = new();
            contentFrame.Content = matchScreen;
        }

        public void InitScreens()
        {
            ChatScreen = new ChatScreen();

            profileEditScreen.InitializePage();
            accountEditScreen.InitializePage();

            profileEditScreen.confirmButton.Click += (sender, e) => contentFrame.Content = profileScreen;
            profileEditScreen.cancelButton.Click += (sender, e) => contentFrame.Content = profileScreen;
            profileEditScreen.quizButton.Click += (sender, e) => contentFrame.Content = matchingQuiz;

            messageButton.Click += (s, e) => contentFrame.Content = ChatScreen;
            profilesButton.Click += (s, e) => contentFrame.Content = profileScreen;
            matchesButton.Click += (s, e) =>
            {
                matchScreen.UpdateLikes();
                contentFrame.Content = matchScreen;
            };

            editProfileButton.Click += (sender, e) => contentFrame.Content = profileEditScreen;
            editAccountButton.Click += (sender, e) => contentFrame.Content = accountEditScreen;

            contentFrame.ContentRendered += HideProfileButton;

            matchingQuiz.ExitPage += (sender, e) => contentFrame.Content = profileEditScreen;
            accountEditScreen.ExitPage += (sender, e) => contentFrame.Content = profileScreen;

            SwitchToLikesMatchesScreen(this, EventArgs.Empty);
            SwitchToProfileScreen(this, EventArgs.Empty);
        }

        private void MatchesButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ProfileButtonFocus(object? sender, MouseEventArgs e)
        {
            profileButton.Source = new BitmapImage(new Uri("/Images/AccountIconFocus.png", UriKind.Relative));
        }

        private void ProfileButtonFocusLost(object? sender, MouseEventArgs e)
        {
            profileButton.Source = new BitmapImage(new Uri("/Images/AccountIcon.png", UriKind.Relative));
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

        private void HideProfileButton(object? sender, EventArgs e)
        {
            editProfileButton.Visibility = Visibility.Hidden;
            editAccountButton.Visibility = Visibility.Hidden;
        }

        private void LogoutButtonFocus(object? sender, MouseEventArgs e)
        {
            logoutButton.Source = new BitmapImage(new Uri("/Images/LogoutIconFocus.png", UriKind.Relative));
        }

        private void LogoutButtonFocusLost(object? sender, MouseEventArgs e)
        {
            logoutButton.Source = new BitmapImage(new Uri("/Images/LogoutIcon.png", UriKind.Relative));
        }
    }
}
