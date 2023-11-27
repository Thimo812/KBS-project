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

namespace MatchingAppWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var startScreen = new StartScreen();

            var matchingQuiz = new Matchingquiz();

            startScreen.RegisterButton.Click += SwitchToRegisterScreen;
            startScreen.LoginButton.Click += SwitchToFilterScreen;

            Content = matchingQuiz;
        }

        private void SwitchToRegisterScreen(Object? sender, EventArgs args)
        {
            Content = new RegisterScreen();
        }
        private void SwitchToFilterScreen(Object? sender, EventArgs args)
        {
            Content = new FilterScreen();
        }


    }
}
