using KBS_project;
using MatchingApp.DataAccess.SQL;
using MatchingAppWindow.Views;
using System;
using System.Windows;


namespace MatchingAppWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Profile? profile;
        public static MatchingAppRepository repo = new MatchingAppRepository();

        private StartScreen startScreen = new();
        private RegisterScreen registerScreen = new();
        private Matchingquiz matchingQuiz = new();
        private Navigation navigation = new();
        private FilterScreen filterScreen;
        private ProfileEditScreen profileEditScreen = new();
        private AccountEditScreen accountEditScreen = new();
        private PhotoEditScreen photoEditScreen = new();

        public MainWindow()
        {
            InitializeComponent();

            InitScreens();
        }

        public void SwitchToFilterScreen(object? sender, EventArgs e)
        {
            if (filterScreen == null) filterScreen = new();
            filterScreen.matchingQuizButton.Click += (object sender, RoutedEventArgs e) => Content = matchingQuiz;
            Content = filterScreen;
        }

        public void InitScreens()
        {
            profileEditScreen.PhotoScreenButton.Click += (Object sender, RoutedEventArgs e) => Content = photoEditScreen;
            profileEditScreen.AccountScreenButton.Click += (Object sender, RoutedEventArgs e) => Content = accountEditScreen;
            
            accountEditScreen.PhotoScreenButton.Click += (Object sender, RoutedEventArgs e) => Content = photoEditScreen;
            accountEditScreen.ProfileEditButton.Click += (Object sender, RoutedEventArgs e) => Content = profileEditScreen;

            photoEditScreen.ProfileEditButton.Click += (Object sender, RoutedEventArgs e) => Content = profileEditScreen;
            photoEditScreen.AccountScreenButton.Click += (Object sender, RoutedEventArgs e) => Content = accountEditScreen;

            registerScreen.ExitPage += (object? sender, EventArgs e) => Content = filterScreen;
            startScreen.registerButton.Click += (Object sender, RoutedEventArgs e) => Content = registerScreen;

            startScreen.LoginSuccessful += SwitchToFilterScreen;
            startScreen.LoginSuccessful += AddProfileDataToScreens;

            registerScreen.ExitPage += SwitchToFilterScreen;

            Content = startScreen;
        }

        public void AddProfileDataToScreens()
        {
            if (profile != null)
            {
                profileEditScreen.BeschrijvingBox.Text = profile.Description;
                profileEditScreen.OpleidingBox.Text = profile.Degree;
                profileEditScreen.SchoolBox.Text = profile.School;
                profileEditScreen.WerkplekBox.Text = profile.WorkPlace;
                profileEditScreen.SetDiet(profile.Diet);

                accountEditScreen.BirthDatePicker.Text = profile.BirthDate.ToString();
                accountEditScreen.CountryBox.Text = profile.Country;
                accountEditScreen.CityBox.Text = profile.City;
                accountEditScreen.PostalCodeBox.Text = profile.PostalCode;
                accountEditScreen.SetGender(profile.Gender);
                accountEditScreen.SetPreference(profile.SexualPreference);
            }
        }

        public void AddProfileDataToScreens(object? sender, EventArgs e)
        {
            AddProfileDataToScreens();
        }
    }
}