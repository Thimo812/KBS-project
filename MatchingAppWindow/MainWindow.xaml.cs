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
using KBS_project.Enums;
using System.Security.Policy;

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
        private ProfileEditScreen ProfileEditScreen = new();
        private AccountEditScreen AccountEditScreen = new();
        private PhotoEditScreen PhotoEditScreen = new();

        public static Profile? profile;
        public static MatchingAppRepository repository;
        public MainWindow()
        {
            repository = new MatchingAppRepository();
            InsertDummyProfile();

            InitializeComponent();

            startScreen.RegisterButton.Click += (object sender, RoutedEventArgs e) => Content = registerScreen;
            registerScreen.CreateAccountButton.Click += registerAccount;

            InitizalizeScreens();
            AddProfileDataToScreens();

            Content = ProfileEditScreen;
        }

        private void registerAccount(Object? sender, EventArgs args)
        {
            profile = registerScreen.Profile;
            Content = navigation;
            repository.SaveProfile(profile);
        }

        public void SwitchToProfileScreen(Object sender, RoutedEventArgs e)
        {
            Content = ProfileEditScreen;
        }

        public void SwitchToAccountScreen(Object sender, RoutedEventArgs e)
        {
           Content = AccountEditScreen;
        }

        public void SwitchToPhotoScreen(Object sender, RoutedEventArgs e)
        {
           Content = PhotoEditScreen;
        }

        public void InitizalizeScreens()
        {
            ProfileEditScreen.PhotoScreenButton.Click += SwitchToPhotoScreen;
            ProfileEditScreen.AccountScreenButton.Click += SwitchToAccountScreen;
            
            AccountEditScreen.PhotoScreenButton.Click += SwitchToPhotoScreen;
            AccountEditScreen.ProfileEditButton.Click += SwitchToProfileScreen;

            PhotoEditScreen.ProfileEditButton.Click += SwitchToProfileScreen;
            PhotoEditScreen.AccountScreenButton.Click += SwitchToAccountScreen;
        }

        public void AddProfileDataToScreens()
        {
            if (profile != null)
            {
                ProfileEditScreen.BeschrijvingBox.Text = profile.Description;
                ProfileEditScreen.OpleidingBox.Text = profile.degree;
                ProfileEditScreen.SchoolBox.Text = profile.School;
                ProfileEditScreen.WerkplekBox.Text = profile.WorkPlace;
                ProfileEditScreen.DieetBox.Text = profile.Diet;

                AccountEditScreen.BirthDatePicker.Text = profile.BirthDate.ToString();
                AccountEditScreen.CountryBox.Text = profile.Country;
                AccountEditScreen.CityBox.Text = profile.City;
                AccountEditScreen.PostalCodeBox.Text = profile.PostalCode;
                AccountEditScreen.SetGender(profile.Gender);
                AccountEditScreen.setPreference(profile.SexualPreference);
            }
        }

        public void InsertDummyProfile()
        {
            profile = repository.GetProfile("Henk");
        }
    }
}
