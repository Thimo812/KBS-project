using KBS_project;
using MatchingApp.DataAccess.SQL;
using MatchingAppWindow.Views;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Renci.SshNet;

namespace MatchingAppWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Profile? profile;
        public static MatchingAppRepository repo = new MatchingAppRepository();
        private static SshClient sshClient;
        private static ForwardedPortLocal tunnel;

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

            Content = startScreen;
        }


        private void SwitchToRegisterScreen(Object? sender, EventArgs args)
        {
            Content = registerScreen;
        }


        public void SwitchToFilterScreen(object? sender, EventArgs e)
        {
            if (filterScreen == null) filterScreen = new();
            filterScreen.matchingQuizButton.Click += (object sender, RoutedEventArgs e) => Content = matchingQuiz;
            Content = filterScreen;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // SSH-verbinding instellen
            string host = "145.44.233.161";
            string username = "student";
            string password = "D1t1sEenSqlServertju";
            int localPort = 1433;
            int remotePort = 1433;


            sshClient = new SshClient(host, username, password);
            sshClient.Connect();

            tunnel = new ForwardedPortLocal("127.0.0.1", (uint)localPort, "127.0.0.1", (uint)remotePort);
            sshClient.AddForwardedPort(tunnel);
            tunnel.Start();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            // Controleren of de SSH-verbinding open is voordat we proberen deze te sluiten
            if (sshClient != null && sshClient.IsConnected)
            {
                // Stop de tunnel
                tunnel.Stop();

                // SSH-verbinding sluiten
                sshClient.Disconnect();
                sshClient.Dispose();
            }
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

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;

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