﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for MatchInfoScreen.xaml
    /// </summary>
    public partial class MatchInfoScreen : Window
    {

        List<string> questions = new List<string>()
        {
            { "Ben je meer een introvert of extrovert?"},
            { "Wil je dat je partner introverted of extroverted is?"},
            { "Hoe belangrijk is religie voor jou?" },
            { "Hoe belangrijk is carrière voor jou?" },
            { "Wat is jouw 'love language'?" },
            { "Wat wil je dat je partners 'love language' is?" },
            { "Hoe belangrijk is fysieke fitness voor jou?" },
            { "Wat denk je over kinderen famillie plannen?" },
            { "Ben je op zoek naar een serieuze relatie?" },
            { "Ben je een ochtend of avond mens?" },
            { "Heb je liever buitendoorse activiteiten, binnen activiteiten of allebij?" },
            { "Wat is je religie?" },
            { "Als je partner een huisdier heeft, hoe zou je dit vinden?" }
        };

        public MatchInfoScreen(string currentUser, string selectedUser)
        {
            InitializeComponent();

            //int i = 2;
            //foreach (string question in questions)
            //{
            //    System.Windows.Controls.Label label = new System.Windows.Controls.Label();

            //    label.FontSize = 16;
            //    label.Content = question;
            //    label.Foreground = new SolidColorBrush(Colors.White);
            //    label.Visibility = Visibility.Visible;
            //    label.Margin = new Thickness(3);

            //    Grid.SetColumn(label, 0);
            //    Grid.SetRow(label, i);
            //    i++;

            //    table.Children.Add(label);
            //}

            DataContext = selectedUser;
        }
    }
}