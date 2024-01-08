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

        public ObservableCollection<string> Interests { get; set; } = new();

        public ObservableCollection<string> AvailableInterests {  get; set; } = new();

        private ProfileDetails ProfilePreview { get; set; } = new();

        public ProfileEditScreen()
        {
            InitializeComponent();

            DataContext = this;

            SizeChanged += (sender, e) =>
            {
                if (ActualWidth >= 1100) ProfilePreview.Visibility = Visibility.Visible;
                else ProfilePreview.Visibility = Visibility.Collapsed;
            };

            hobbyBox.ItemsSource = Interests;
            availableHobbyBox.ItemsSource = AvailableInterests;
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
                Interests.Add(InterestExtensions.GetString((int)item));
            }

            for (int i = 0;  i < InterestExtensions.count; i++)
            {
                string interestString = InterestExtensions.GetString(i);
                if (!Interests.Contains(interestString)) AvailableInterests.Add(interestString);
            }

            ProfilePreview.SetProfile(MainWindow.profile.UserName);
            profilePreviewFrame.Content = ProfilePreview;
            ProfilePreview.likebutton.Visibility = Visibility.Hidden;
            ProfilePreview.chatrequest.Visibility = Visibility.Hidden;



            LinkProfilePreview();
        }

        private void InitializePage(object sender, RoutedEventArgs e)
        {
            InitializePage();
        } 

        private void LinkProfilePreview()
        {
            descriptionBox.TextChanged += (sender, e) => ProfilePreview.descriptionBlock.Text = descriptionBox.Text;
            schoolBox.TextChanged += UpdateProfileInfo;
            degreeBox.TextChanged += UpdateProfileInfo;
            workPlaceBox.TextChanged += UpdateProfileInfo;
            dietBox.SelectionChanged += UpdateProfileInfo;
            Interests.CollectionChanged += (sender, e) => ProfilePreview.interestBlock.ItemsSource = Interests;
        }

        private void UpdateProfileInfo(object sender, TextChangedEventArgs e) => UpdateProfileInfo();

        private void UpdateProfileInfo(object sender, SelectionChangedEventArgs e) => UpdateProfileInfo();

        private void UpdateProfileInfo()
        {
            ProfilePreview.ProfileInfo.Clear();
            ProfilePreview.ProfileInfo.Add($"Woonplaats: {MainWindow.profile.City}");
            ProfilePreview.ProfileInfo.Add(schoolBox == null || schoolBox.Text == string.Empty ? null : $"School: {schoolBox.Text}");
            ProfilePreview.ProfileInfo.Add(degreeBox == null || degreeBox.Text == string.Empty ? null : $"Opleiding: {degreeBox.Text}");
            ProfilePreview.ProfileInfo.Add(workPlaceBox == null || workPlaceBox.Text == string.Empty ? null : $"Werkplek: {workPlaceBox.Text}");
            ProfilePreview.ProfileInfo.Add(dietBox.SelectedIndex == -1 ? null : $"Dieet: {(Diet)dietBox.SelectedIndex}");
            ProfilePreview.ProfileInfo.Add(MainWindow.profile.Vaccinated != null ? $"Is {((bool)MainWindow.profile.Vaccinated ? string.Empty : "niet")} gevaccineerd" : null);
        }

        public void InterestSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hobbyBox.SelectedIndex == -1) return;

            var selectedInterest = Interests[hobbyBox.SelectedIndex];

            AvailableInterests.Add(selectedInterest);
            Interests.Remove(selectedInterest);

            AvailableInterests = new(AvailableInterests.OrderBy(x => x));
            availableHobbyBox.ItemsSource = AvailableInterests;
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
                MainWindow.profile.Interests = Interests.Select(x => InterestExtensions.getEnumFromString(x)).ToList();

                MainWindow.repo.UpdateProfile(MainWindow.profile);
            }
        }
    }
}
