using KBS_project;
using MatchingApp.DataAccess.SQL;
using MatchingAppWindow.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        List<int> you;
        List<int> other;

        public ObservableCollection<string> ProfileInfo { get; set; }

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
                selectedProfile.Vaccinated != null ? $"Is {((bool)selectedProfile.Vaccinated ? string.Empty : "niet")} gevaccineerd" : null,
            }.Where(detail => detail != null).ToList();

        }

        public void SetProfile(string profileName)
        {
            currentImage = 0;
            selectedProfile = MainWindow.repo.GetProfile(profileName);

            ProfileInfo = new(GetProfileDetails());

            try
            {
                profileImage.Source = ImageConverter.ImageDataToBitmap(selectedProfile.Images[0]);
                previousImage.Visibility = Visibility.Visible;
                nextImage.Visibility = Visibility.Visible;
            }
            catch (ArgumentOutOfRangeException)
            {
                profileImage.Source = null;
                previousImage.Visibility = Visibility.Hidden;
                nextImage.Visibility = Visibility.Hidden;
            }

            nameLabel.Content = $"{selectedProfile.FirstName} {selectedProfile.LastName}, {selectedProfile.Age()}";
            descriptionBlock.Text = selectedProfile.Description;
            interestBlock.ItemsSource = selectedProfile.Interests;
            detailList.ItemsSource = ProfileInfo;

            you = MainWindow.repo.GetMatchingQuiz(usr);
            other = MainWindow.repo.GetMatchingQuiz(selectedProfile.UserName);

            if (you.Count > 0 && other.Count > 0)
            {
                ViewAnswers.Visibility = Visibility.Visible;
                MatchingPercentage.Visibility = Visibility.Visible;

                SetMatchingPercentage();
            }
            else
            {
                ViewAnswers.Visibility = Visibility.Collapsed;
                MatchingPercentage.Visibility = Visibility.Collapsed;
            }
        }

        public void SetMatchingPercentage()
        {
            double matchingnumber = 0;

                for (int i = 0; i < 13; i++)
                {
                    int diff = Math.Abs(you[i] - other[i]);

                    if (i == 0 || i == 1 || i == 4 || i == 5)
                    {
                        matchingnumber += CalculateMatchingNumber(diff, 5, 3, 2, 1, 0);
                    }
                    else if (i == 2)
                    {
                        if (you[11] == other[11])
                        {
                            matchingnumber += 5;

                            if (you[2] == 1)
                                matchingnumber += 3;
                            else if (you[2] == 3)
                                matchingnumber += 5;
                        }
                        else
                        {
                            if (you[2] == 1)
                                matchingnumber -= 3;
                            else if (you[2] == 3)
                                matchingnumber -= 5;
                        }
                    }
                    else if (i == 3 || i == 6 || i == 7)
                    {
                        matchingnumber += CalculateMatchingNumber(diff, 5, 3, 0);
                    }
                    else if (i == 8)
                    {
                        matchingnumber += (you[8] == other[8]) ? 5 : 0;
                    }
                    else if (i == 9 || i == 10)
                    {
                        matchingnumber += CalculateMatchingNumber(diff, 5, 0, 3);
                    }
                    else if (i == 12)
                    {
                        matchingnumber += CalculateMatchingNumber(diff, 5, 3, 1, 0);
                    }
                }

                double matchingpercentage = (matchingnumber / 60) * 100;
                MatchingPercentage.Content = Math.Round(matchingpercentage) + "% match";
        }

        private double CalculateMatchingNumber(int diff, params int[] weights)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                if (diff == i)
                    return weights[i];
            }
            return 0;
        }

        public void NewChatRequest(object sender, RoutedEventArgs e)
        {
            MainWindow.repo.CreateMessageRequest(MainWindow.profile.UserName, selectedProfile.UserName);
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

        public void ViewAnswers_Click(object sender, RoutedEventArgs e)
        {
            MatchInfoScreen matchInfoScreen = new MatchInfoScreen(usr, selectedProfile.UserName);
            matchInfoScreen.Visibility = Visibility.Visible;
        }
    }
}
