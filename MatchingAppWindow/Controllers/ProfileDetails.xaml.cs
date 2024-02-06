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

        List<int> AnswersCurrentUser;
        List<int> AnswersSelectedUser;
        Brush[] AnswerDifference = new Brush[13];

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

            AnswersCurrentUser = MainWindow.repo.GetMatchingQuiz(usr);
            AnswersSelectedUser = MainWindow.repo.GetMatchingQuiz(selectedProfile.UserName);

            if (AnswersCurrentUser.Count > 0 && AnswersSelectedUser.Count > 0)
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
            double total = 0;

            for (int i = 0; i < 13; i++)
            {
                int diff = Math.Abs(AnswersCurrentUser[i] - AnswersSelectedUser[i]);

                if (i == 0 && AnswerFilledIn(5, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(AnswersCurrentUser[i] - AnswersSelectedUser[1], 5, 3, 2, 1, 0);
                    GetColorCode(i, AnswersCurrentUser[i] - AnswersSelectedUser[1], 5);
                    total += 5;
                }
                else if (i == 1 && AnswerFilledIn(5, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(AnswersCurrentUser[i] - AnswersSelectedUser[0], 5, 3, 2, 1, 0);
                    GetColorCode(i, AnswersCurrentUser[i] - AnswersSelectedUser[0], 5);
                    total += 5;
                }
                else if (i == 4 && AnswerFilledIn(5, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(AnswersCurrentUser[i] - AnswersSelectedUser[5], 5, 3, 2, 1, 0);
                    GetColorCode(i, AnswersCurrentUser[i] - AnswersSelectedUser[5], 5);
                    total += 5;
                }
                else if (i == 5 && AnswerFilledIn(5, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(AnswersCurrentUser[i] - AnswersSelectedUser[4], 5, 3, 2, 1, 0);
                    GetColorCode(i, AnswersCurrentUser[i] - AnswersSelectedUser[4], 5);
                    total += 5;
                }
                else if (i == 2 && AnswerFilledIn(3, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    total += 5;
                    if (AnswersCurrentUser[11] == AnswersSelectedUser[11] && AnswerFilledIn(4, AnswersCurrentUser[11], AnswersSelectedUser[11]))
                    {
                        matchingnumber += 5;
                        GetColorCode(11, 0, 1);
                        total += 5;

                        if (AnswersCurrentUser[2] == 1)
                            matchingnumber += 3;
                        else if (AnswersCurrentUser[2] == 3)
                            matchingnumber += 5;
                    }
                    else if (AnswerFilledIn(4, AnswersCurrentUser[11], AnswersSelectedUser[11]))
                    {
                        GetColorCode(11, 1, 1);
                        total += 5;

                        if (AnswersCurrentUser[2] == 1)
                            matchingnumber -= 3;
                        else if (AnswersCurrentUser[2] == 3)
                            matchingnumber -= 5;
                    }
                }
                else if ((i == 3 || i == 6 || i == 7) && AnswerFilledIn(3, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(diff, 5, 3, 0);
                    GetColorCode(i, diff, 3);
                    total += 5;
                }
                else if (i == 8 && AnswerFilledIn(2, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(diff, 5, 0);
                    GetColorCode(i, diff, 2);
                    total += 5;
                }
                else if ((i == 9 || i == 10) && AnswerFilledIn(3, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(diff, 5, 0, 3);
                    GetColorCode(i, diff, 3);
                    total += 5;
                }
                else if (i == 12 && AnswerFilledIn(4, AnswersCurrentUser[i], AnswersSelectedUser[i]))
                {
                    matchingnumber += CalculateMatchingNumber(diff, 5, 3, 1, 0);
                    GetColorCode(i, diff, 4);
                    total += 5;
                }
            }

            double matchingpercentage = (matchingnumber / total) * 100;
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

        private bool AnswerFilledIn(int optionAmount, int answerCurrentUser, int answerSelectedUser)
        {
            if (answerCurrentUser < optionAmount && answerSelectedUser < optionAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetColorCode(int id, int diff, int answerAmount)
        {
            AnswerDifference[id] = (diff == 0) ? Brushes.Green : (diff == answerAmount) ? Brushes.Red : Brushes.Yellow;
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
            MatchInfoScreen matchInfoScreen = new MatchInfoScreen(usr, selectedProfile.UserName, AnswerDifference);
            matchInfoScreen.Visibility = Visibility.Visible;
        }
    }
}
