using KBS_project;
using MatchingApp.DataAccess.SQL;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Animation;
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

        private int currentImage;
        public ProfileDetails()
        {
            InitializeComponent();

            DataContext = this;
            Visibility = Visibility.Collapsed;
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

        public void SetProfile(string profileName)
        {
            currentImage = 0;

            selectedProfile = MainWindow.repo.GetProfile(profileName);

            try
            {
                profileImage.Source = ImageConverter.ImageDataToBitmap(selectedProfile.Images[0]);
            }
            catch (ArgumentOutOfRangeException)
            {
                profileImage.Source = null;
            }

            nameLabel.Content = $"{selectedProfile.FirstName} {selectedProfile.LastName}, {selectedProfile.Age()}";
            descriptionBlock.Text = selectedProfile.Description;
            interestBlock.ItemsSource = selectedProfile.Interests;
            detailList.ItemsSource = GetProfileDetails();
        }

        public void PreviousImage(object sender, RoutedEventArgs e)
        {
            if (currentImage == 0) return;

            currentImage--;

            profileImage.Source = ImageConverter.ImageDataToBitmap(selectedProfile.Images[currentImage]);
        }

        public void NextImage(object sender, RoutedEventArgs e)
        {
            if (currentImage == selectedProfile.Images.Count - 1) return;

            currentImage++;

            profileImage.Source = ImageConverter.ImageDataToBitmap(selectedProfile.Images[currentImage]);
        }
    }
}
