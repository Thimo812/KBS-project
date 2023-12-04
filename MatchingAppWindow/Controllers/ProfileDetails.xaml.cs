﻿using KBS_project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for ProfileDetails.xaml
    /// </summary>
    public partial class ProfileDetails : Page
    {
        Profile selectedProfile;

        public ProfileDetails()
        {
            selectedProfile = 

            InitializeComponent();

            DataContext = this;

            nameLabel.Content = $"{selectedProfile.FirstName} {selectedProfile.LastName}, {selectedProfile.Age()}";
            descriptionBlock.Text = selectedProfile.Description;
            interestBlock.Text = GetInterestString();
            detailList.ItemsSource = GetProfileDetails();
        }

        public List<string> GetProfileDetails()
        {
            var result = new List<string>();

            result.Add($"Woonplaats: {selectedProfile.City}");
            if (selectedProfile.School != null) result.Add($"School: {selectedProfile.School}");
            if (selectedProfile.Degree != null) result.Add($"Opleiding: {selectedProfile.Degree}");
            if (selectedProfile.WorkPlace != null) result.Add($"Werkplaats: {selectedProfile.WorkPlace}");
            if (selectedProfile.Diet != null) result.Add($"Dieet: {selectedProfile.Diet}");
            if (selectedProfile.Vaccinated != null) result.Add($"Is {((bool)selectedProfile.Vaccinated ? string.Empty : "niet")} gevaccineerd");

            return result;
        }

        public string GetInterestString()
        {
            string result = string.Empty;

            foreach(var interest in selectedProfile.Interests)
            {
                if (result == string.Empty) result = $"{interest}";
                else result = result + ", " + interest;
            }

            return result;
        }
    }
}
