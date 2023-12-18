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
        MatchingAppRepository repo = new MatchingAppRepository();
        private string usr = MainWindow.profile.UserName;
        private int currentImage;
        public ProfileDetails()
        {
            InitializeComponent();

            DataContext = this;
            Visibility = Visibility.Collapsed;
            

            likebutton.Visibility = Visibility.Visible;
            dislikebutton.Visibility = Visibility.Collapsed;         
            
        }

        public List<string> GetProfileDetails()
        {
            updatebutton();
            return new List<string>
            {
                $"Woonplaats: {selectedProfile.City}",
                selectedProfile.School != null ? $"School: {selectedProfile.School}" : null,
                selectedProfile.Degree != null ? $"Opleiding: {selectedProfile.Degree}" : null,
                selectedProfile.WorkPlace != null ? $"Werkplaats: {selectedProfile.WorkPlace}" : null,
                selectedProfile.Diet != null ? $"Dieet: {selectedProfile.Diet}" : null,
                selectedProfile.Vaccinated != null ? $"Is {((bool)selectedProfile.Vaccinated ? string.Empty : "niet")} gevaccineerd" : null
            }.Where(detail => detail != null).ToList();

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

        public void LikeProfileEvent(object sender, RoutedEventArgs e)
        {
            repo.LikeProfile(usr, selectedProfile.UserName);
            updatebutton();
        }

        public void DislikeProfileEvent(object sender, RoutedEventArgs e)
        {
            repo.DislikeProfile(usr, selectedProfile.UserName);
            updatebutton();
        }

        public void updatebutton()
        {
            if (repo.CheckLikeStatus(usr, selectedProfile.UserName) == usr)
            {
                dislikebutton.Visibility = Visibility.Visible;
                likebutton.Visibility = Visibility.Collapsed;
            }
            else
            {
                likebutton.Visibility = Visibility.Visible;
                dislikebutton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
