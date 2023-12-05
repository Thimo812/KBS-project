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

namespace MatchingAppWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Profile profile;
        private MatchingAppRepository repository;

        private StartScreen startScreen = new();
        private RegisterScreen registerScreen;
        private Matchingquiz matchingQuiz;
        private Navigation navigation = new();
        private FilterScreen filterScreen = new();

        public MainWindow()
        {
            repository = new MatchingAppRepository();

            profile = repository.GetProfile("Thimo812");

            registerScreen = new(repository);
            matchingQuiz = new(repository);

            InitializeComponent();

            startScreen.registerButton.Click += (object sender, RoutedEventArgs e) => Content = registerScreen;
            startScreen.loginButton.Click += (object sender, RoutedEventArgs e) => Content = filterScreen;

            registerScreen.exitPage += (object sender, EventArgs e) => Content = filterScreen;
            startScreen.registerButton.Click += SwitchToRegisterScreen;

            Content = matchingQuiz;
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

      


    }
}
