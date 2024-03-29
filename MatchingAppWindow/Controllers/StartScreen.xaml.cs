﻿using KBS_project;
using MatchingApp.DataAccess.SQL;
using Renci.SshNet.Common;
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
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Page
    {
        public event EventHandler<EventArgs> LoginSuccessful;

        public StartScreen()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }
        private void ShowErrors() 
        {

            userNameField.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff6666");
            userNameField.BorderBrush = Brushes.Red;
            errorMessage.Visibility = Visibility.Visible;
        }

        private void ClearErrors()
        {
            userNameField.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
            userNameField.BorderBrush = Brushes.Transparent;
            errorMessage.Visibility = Visibility.Hidden;
        }

        private void TryLogin(object sender, RoutedEventArgs e)
        {
            string enteredUserName = userNameField.Text;

            if (MainWindow.repo.ValidateUserName(enteredUserName))
            {
                MainWindow.profile = MainWindow.repo.GetProfile(enteredUserName);
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
                ClearErrors();
            }
            else
            {
                ShowErrors();
            }
        }

        private void NameFieldKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TryLogin(this, new RoutedEventArgs());
            }
        }        

    }
}
