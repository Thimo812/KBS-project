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
        private Navigation navigation = new();
        private RegisterScreen registerScreen = new();

        public MainWindow()
        {
            InitializeComponent();

            InitScreen();
        }

        public void SwitchToLikesMachesScreen(object? sender, EventArgs e)
        {
            if (likesMachesScreen == null) likesMachesScreen = new();
            Content = likesMachesScreen;
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

        private void InitScreen()
        {
            startScreen.LoginSuccessful += (sender, e) =>
            {
                navigation.InitScreens();
                Content = navigation;
            };

            registerScreen.loginButton.Click += (sender, e) => Content = startScreen;

            startScreen.registerButton.Click += (sender, e) => Content = registerScreen;

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;

            Content = startScreen;
        }
    }
}