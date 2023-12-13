using KBS_project;
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
            profileEditScreen.PhotoScreenButton.Click += (Object sender, RoutedEventArgs e) => contentFrame.Content = photoEditScreen;
            profileEditScreen.AccountScreenButton.Click += (Object sender, RoutedEventArgs e) => contentFrame.Content = accountEditScreen;

            accountEditScreen.PhotoScreenButton.Click += (Object sender, RoutedEventArgs e) => contentFrame.Content = photoEditScreen;
            accountEditScreen.ProfileEditButton.Click += (Object sender, RoutedEventArgs e) => contentFrame.Content = profileEditScreen;

            photoEditScreen.ProfileEditButton.Click += (Object sender, RoutedEventArgs e) => contentFrame.Content = profileEditScreen;
            photoEditScreen.AccountScreenButton.Click += (Object sender, RoutedEventArgs e) => contentFrame.Content = accountEditScreen;

            matchingQuiz.ExitPage += SwitchToProfileScreen;

            matchingQuiz.ExitPage += (sender, e) => SwitchToProfileScreen(sender, e);

            SwitchToProfileScreen(this, EventArgs.Empty);
        }

        public void AddProfileDataToScreens()
        {
            if (MainWindow.profile != null)
            {
                profileEditScreen.BeschrijvingBox.Text = MainWindow.profile.Description;
                profileEditScreen.OpleidingBox.Text = MainWindow.profile.Degree;
                profileEditScreen.SchoolBox.Text = MainWindow.profile.School;
                profileEditScreen.WerkplekBox.Text = MainWindow.profile.WorkPlace;
                profileEditScreen.SetDiet(MainWindow.profile.Diet);
                profileEditScreen.InitializePage();

                accountEditScreen.BirthDatePicker.Text = MainWindow.profile.BirthDate.ToString();
                accountEditScreen.CountryBox.Text = MainWindow.profile.Country;
                accountEditScreen.CityBox.Text = MainWindow.profile.City;
                accountEditScreen.PostalCodeBox.Text = MainWindow.profile.PostalCode;
                accountEditScreen.SetGender(MainWindow.profile.Gender);
                accountEditScreen.SetPreference(MainWindow.profile.SexualPreference);
            }
        }

        public void AddProfileDataToScreens(object? sender, EventArgs e)
        {
            AddProfileDataToScreens();
        }
    }
}
