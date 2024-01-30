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

            SetMatchingPercentage();
        }

        public void SetMatchingPercentage()
        {
            List<int> you = MainWindow.repo.GetMatchingQuiz(usr);
            List<int> other = MainWindow.repo.GetMatchingQuiz(selectedProfile.UserName);

            double matchingnumber = 0;

            if (you.Count > 0 && other.Count > 0) { 
                for (int i = 0; i < 13; i++)
                {
                    //question 1
                    if (i == 0)
                    {
                        if (you[0] == other[1])
                        {
                            matchingnumber += 5;
                        }
                        if ((you[0] == (other[1] + 1)) || (you[0] == (other[1] - 1)))
                        {
                            matchingnumber += 3;
                        }
                        if ((you[0] == (other[1] + 2)) || (you[0] == (other[1] - 2)))
                        {
                            matchingnumber += 2;
                        }
                        if ((you[0] == (other[1] + 3)) || (you[0] == (other[1] - 3)))
                        {
                            matchingnumber += 1;
                        }
                        if ((you[0] == (other[1] + 4)) || (you[0] == (other[1] - 4)))
                        {
                            matchingnumber += 0;
                        }
                    }

                    //question 2
                    if(i == 1)
                    {
                        if (you[1] == other[0])
                        {
                            matchingnumber += 5;
                        }
                        if ((you[1] == (other[0] + 1)) || (you[1] == (other[0] - 1)))
                        {
                            matchingnumber += 3;
                        }
                        if ((you[1] == (other[0] + 2)) || (you[1] == (other[0] - 2)))
                        {
                            matchingnumber += 2;
                        }
                        if ((you[1] == (other[0] + 3)) || (you[1] == (other[0] - 3)))
                        {
                            matchingnumber += 1;
                        }
                        if ((you[1] == (other[0] + 4)) || (you[1] == (other[0] - 4)))
                        {
                            matchingnumber += 0;
                        }
                    }

                    //question 3 and 12
                    if (i == 2)
                    {
                        if (you[11] == other[11])
                        {
                            matchingnumber += 5;
                            if (you[2] == 0)
                            {
                                matchingnumber += 0;
                            }
                            if (you[2] == 1)
                            {
                                matchingnumber += 3;
                            }
                            if (you[2] == 3)
                            {
                                matchingnumber += 5;
                            }
                        }
                        else
                        {
                            if (you[2] == 1)
                            {
                                matchingnumber -= 3;
                            }
                            if (you[2] == 3)
                            {
                                matchingnumber -= 5;
                            }
                        }
                    }

                    //question 4, 7, 8
                    if (i == 3 || i == 6 || i == 7)
                    {
                        if (you[i] == other[i])
                        {
                            matchingnumber += 5;
                        }
                        if ((you[i] == (other[i] + 1)) || (you[i] == (other[i] - 1)))
                        {
                            matchingnumber += 3;
                        }
                        if ((you[i] == (other[i] + 2)) || (you[i] == (other[i] - 2)))
                        {
                            matchingnumber += 0;
                        }
                    }

                    //question 5 and 6
                    if (i == 4)
                    {
                        if (you[4] == other[5])
                        {
                            matchingnumber += 5;
                        }
                        if ((you[4] == (other[5] + 1)) || (you[4] == (other[5] - 1)))
                        {
                            matchingnumber += 3;
                        }
                        if ((you[4] == (other[5] + 2)) || (you[4] == (other[5] - 2)))
                        {
                            matchingnumber += 2;
                        }
                        if ((you[4] == (other[5] + 3)) || (you[4] == (other[5] - 3)))
                        {
                            matchingnumber += 1;
                        }
                        if ((you[4] == (other[5] + 4)) || (you[4] == (other[5] - 4)))
                        {
                            matchingnumber += 0;
                        }
                    }

                    if (i == 5)
                    {
                        if (you[5] == other[4])
                        {
                            matchingnumber += 5;
                        }
                        if ((you[5] == (other[4] + 1)) || (you[5] == (other[4] - 1)))
                        {
                            matchingnumber += 3;
                        }
                        if ((you[5] == (other[4] + 2)) || (you[5] == (other[4] - 2)))
                        {
                            matchingnumber += 2;
                        }
                        if ((you[5] == (other[4] + 3)) || (you[5] == (other[4] - 3)))
                        {
                            matchingnumber += 1;
                        }
                        if ((you[5] == (other[4] + 4)) || (you[5] == (other[4] - 4)))
                        {
                            matchingnumber += 0;
                        }
                    }

                    //question 9
                    if (i == 8)
                    {
                        if (you[8] == (other[8]))
                        {
                            matchingnumber += 5;
                        }
                        else
                        {
                            matchingnumber += 0;
                        }
                    }

                    //question 10 and 11
                    if (i == 9 || i == 10)
                    {
                        if (you[i] == other[i])
                        {
                            matchingnumber += 5;
                        }
                        if ((you[i] == (other[i] + 1)) || (you[i] == (other[i] - 1)))
                        {
                            matchingnumber += 0;
                        }
                        if ((you[i] == (other[i] + 2)) || (you[i] == (other[i] - 2)))
                        {
                            matchingnumber += 3;
                        }
                    }

                    //question 13
                    if (i == 12)
                    {
                        if (you[i] == other[i])
                        {
                            matchingnumber += 5;
                        }
                        if ((you[i] == (other[i] + 1)) || (you[i] == (other[i] - 1)))
                        {
                            matchingnumber += 3;
                        }
                        if ((you[i] == (other[i] + 2)) || (you[i] == (other[i] - 2)))
                        {
                            matchingnumber += 1;
                        }
                        if ((you[i] == (other[i] + 2)) || (you[i] == (other[i] - 2)))
                        {
                            matchingnumber += 0;
                        }
                    }

                    double matchingpercentage = (matchingnumber / 60) * 100;
                    MatchingPercentage.Content = Math.Round(matchingpercentage) + "% match";
                }
            }
            else
            {
                MatchingPercentage.Content = "geen matchingpercentage";
            }
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
            List<int> you = MainWindow.repo.GetMatchingQuiz(usr);
            List<int> other = MainWindow.repo.GetMatchingQuiz(selectedProfile.UserName);

            MatchInfoScreen matchInfoScreen = new MatchInfoScreen(usr, selectedProfile.UserName);
            matchInfoScreen.Visibility = Visibility.Visible;
        }
    }
}
