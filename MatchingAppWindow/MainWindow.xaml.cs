using KBS_project.Enums;
using KBS_project;
using MatchingApp.DataAccess.SQL;
using MatchingAppWindow.Views;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public static Profile? profile;
        public static MatchingAppRepository repo = new MatchingAppRepository();

        private StartScreen startScreen = new();
        private RegisterScreen registerScreen = new();
        private Matchingquiz matchingQuiz = new();
        private Navigation navigation = new();
        private FilterScreen filterScreen;
        private ProfileEditScreen ProfileEditScreen = new();
        private AccountEditScreen AccountEditScreen = new();
        private PhotoEditScreen PhotoEditScreen = new();

        public MainWindow()
        {
            InitializeComponent();

            InitScreens();

            Content = startScreen;
        }


        private void SwitchToRegisterScreen(Object? sender, EventArgs args)
        {
            Content = registerScreen;
        }

        public static BitmapImage ImageToBitmapImage(Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public void SwitchToFilterScreen(object? sender, EventArgs e)
        {
            if (filterScreen == null) filterScreen = new();
            filterScreen.matchingQuizButton.Click += (object sender, RoutedEventArgs e) => Content = matchingQuiz;
            Content = filterScreen;
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

        public void InitScreens()
        {
            ProfileEditScreen.PhotoScreenButton.Click += SwitchToPhotoScreen;
            ProfileEditScreen.AccountScreenButton.Click += SwitchToAccountScreen;
            
            AccountEditScreen.PhotoScreenButton.Click += SwitchToPhotoScreen;
            AccountEditScreen.ProfileEditButton.Click += SwitchToProfileScreen;

            PhotoEditScreen.ProfileEditButton.Click += SwitchToProfileScreen;
            PhotoEditScreen.AccountScreenButton.Click += SwitchToAccountScreen;

            startScreen.registerButton.Click += (object sender, RoutedEventArgs e) => Content = registerScreen;
            startScreen.loginButton.Click += (object sender, RoutedEventArgs e) => Content = filterScreen;

            registerScreen.ExitPage += (object sender, EventArgs e) => Content = filterScreen;
            startScreen.registerButton.Click += SwitchToRegisterScreen;

            startScreen.registerButton.Click += (object sender, RoutedEventArgs e) => Content = registerScreen;
            startScreen.LoginSuccessful += SwitchToFilterScreen;

            registerScreen.ExitPage += SwitchToFilterScreen;

            AddProfileDataToScreens();
        }

        public void AddProfileDataToScreens()
        {
            if (profile != null)
            {
                ProfileEditScreen.BeschrijvingBox.Text = profile.Description;
                ProfileEditScreen.OpleidingBox.Text = profile.degree;
                ProfileEditScreen.SchoolBox.Text = profile.School;
                ProfileEditScreen.WerkplekBox.Text = profile.WorkPlace;
                ProfileEditScreen.SetDiet(profile.Diet);

                AccountEditScreen.BirthDatePicker.Text = profile.BirthDate.ToString();
                AccountEditScreen.CountryBox.Text = profile.Country;
                AccountEditScreen.CityBox.Text = profile.City;
                AccountEditScreen.PostalCodeBox.Text = profile.PostalCode;
                AccountEditScreen.SetGender(profile.Gender);
                AccountEditScreen.SetPreference(profile.SexualPreference);
            }
        }

        public void InsertDummyProfile()
        {
            profile = repository.GetProfile("Henk");
        }
    }
}