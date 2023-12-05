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
        public static MatchingAppRepository repo = new MatchingAppRepository();

        private StartScreen startScreen = new();
        private RegisterScreen registerScreen;
        private Matchingquiz matchingQuiz;
        private Navigation navigation = new();
        private FilterScreen filterScreen = new();

        public MainWindow()
        {
            repo = new MatchingAppRepository();

           // profile = repository.GetProfile("Thimo812");

            registerScreen = new(repo);
            matchingQuiz = new(repo);
            startScreen = new();

            InitializeComponent();

            startScreen.registerButton.Click += (object sender, RoutedEventArgs e) => Content = registerScreen;
         
            
            registerScreen.ExitPage += (object sender, EventArgs e) => Content = filterScreen;
            startScreen.loginSuccessful += SwitchToFilterScreen;

            Content = startScreen;
        }
        private void SwitchToFilterScreen(object? sender, EventArgs args)
        {
            Content = filterScreen;
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