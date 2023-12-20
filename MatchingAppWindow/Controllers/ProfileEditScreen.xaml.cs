using KBS_project;
using KBS_project.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for ProfileEditScreen.xaml
    /// </summary>
    public partial class ProfileEditScreen : Page
    {

        public ObservableCollection<Interest> Interests { get; set; } = new();

        public ObservableCollection<Interest> AvailableInterests {  get; set; } = new();

        private ProfileDetails ProfilePreview { get; set; } = new();

        public ProfileEditScreen()
        {
            InitializeComponent();

            DataContext = this;
        }

        public void InitializePage()
        {
            descriptionBox.Text = MainWindow.profile.Description;
            degreeBox.Text = MainWindow.profile.Degree;
            schoolBox.Text = MainWindow.profile.School;
            workPlaceBox.Text = MainWindow.profile.WorkPlace;
            if (MainWindow.profile.Diet != null) dietBox.SelectedIndex = (int)MainWindow.profile.Diet;

            Interests.Clear();
            AvailableInterests.Clear();

            foreach (var item in MainWindow.profile.Interests)
            {
                Interests.Add(item);
            }

            for (int i = 0;  i < InterestExtensions.count; i++)
            {
                if (!Interests.Contains((Interest)i))
                {
                    AvailableInterests.Add((Interest)i);
                }
            }
            hobbyBox.ItemsSource = Interests;
            availableHobbyBox.ItemsSource = AvailableInterests;

            ProfilePreview.SetProfile(MainWindow.profile.UserName);
            profilePreviewFrame.Content = ProfilePreview;
            ProfilePreview.likebutton.Visibility = Visibility.Hidden;
            ProfilePreview.chatrequest.Visibility = Visibility.Hidden;

            SizeChanged += (sender, e) =>
            {
                if (ActualWidth >= 1100) ProfilePreview.Visibility = Visibility.Visible;
                else ProfilePreview.Visibility = Visibility.Collapsed;
            };

            LinkProfilePreview();
        }

        private void InitializePage(object sender, RoutedEventArgs e)
        {
            InitializePage();
        } 

        private void LinkProfilePreview()
        {
            descriptionBox.TextChanged += (sender, e) => ProfilePreview.descriptionBlock.Text = descriptionBox.Text;
            schoolBox.TextChanged += (sender, e) => ProfilePreview.ProfileInfo[1] = $"School: {schoolBox.Text}";
            degreeBox.TextChanged += (sender, e) => ProfilePreview.ProfileInfo[2] = $"Opleiding: {degreeBox.Text}";
            workPlaceBox.TextChanged += (sender, e) => ProfilePreview.ProfileInfo[3] = $"Werkplek: {workPlaceBox.Text}";
            Interests.CollectionChanged += (sender, e) => ProfilePreview.interestBlock.ItemsSource = Interests;
        }

        public void InterestSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hobbyBox.SelectedIndex == -1) return;

            var selectedInterest = Interests[hobbyBox.SelectedIndex];

            AvailableInterests.Add(selectedInterest);
            Interests.Remove(selectedInterest);
        }

        public void AvailableInterestSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (availableHobbyBox.SelectedIndex == -1)
            {
                return;
            }

            var selectedInterest = AvailableInterests[availableHobbyBox.SelectedIndex];

            Interests.Add(selectedInterest);
            AvailableInterests.Remove(selectedInterest);
        }

        private void SwitchToPhotoEditScreen(object sender, RoutedEventArgs e)
        {
        }

        private void SwitchToAccountEditScreen(object sender, RoutedEventArgs e)
        {
        }

        private void ConfirmChanges(object sender, RoutedEventArgs e)
        {
            if (MainWindow.profile != null)
            {
                MainWindow.profile.Description = descriptionBox.Text;
                MainWindow.profile.Degree = degreeBox.Text;
                MainWindow.profile.School = schoolBox.Text;
                MainWindow.profile.WorkPlace = workPlaceBox.Text;
                MainWindow.profile.Diet = (Diet) dietBox.SelectedIndex;
                MainWindow.profile.Interests = Interests.ToList();

                MainWindow.repo.UpdateProfile(MainWindow.profile);
            }
        }
    }
}
