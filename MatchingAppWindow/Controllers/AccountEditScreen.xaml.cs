using KBS_project;
using KBS_project.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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
    /// Interaction logic for AccountEditScreen.xaml
    /// </summary>
    public partial class AccountEditScreen : Page
    {
        public ObservableCollection<BitmapImage> Images { get; set; } = new();
        private ProfileDetails ProfilePreview { get; set; } = new();
        public AccountEditScreen()
        {
            InitializeComponent();

            DataContext = this;

            imageBox.SelectionChanged += UpdateDeletePhotoButton;
            Images.CollectionChanged += UpdateAddPhotoButton;

            SizeChanged += (sender, e) =>
            {
                if (ActualWidth >= 1100) ProfilePreview.Visibility = Visibility.Visible;
                else ProfilePreview.Visibility = Visibility.Collapsed;
            };
        }

        public void InitializePage()
        {
            BirthDatePicker.Text = MainWindow.profile.BirthDate.ToString();
            CountryBox.Text = MainWindow.profile.Country;
            CityBox.Text = MainWindow.profile.City;
            PostalCodeBox.Text = MainWindow.profile.PostalCode;
            genderBox.SelectedIndex = (int)MainWindow.profile.Gender;
            sexuelPreferenceBox.SelectedIndex = (int)MainWindow.profile.SexualPreference;

            ProfilePreview.SetProfile(MainWindow.profile.UserName);
            profilePreviewFrame.Content = ProfilePreview;
            ProfilePreview.likebutton.Visibility = Visibility.Hidden;
            ProfilePreview.chatrequest.Visibility = Visibility.Hidden;

            Images.Clear();
            foreach (byte[] imageData in MainWindow.profile.Images)
            {
                Images.Add(ImageConverter.ImageDataToBitmap(imageData));
            }
        }

        private void AddPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));

                Images.Add(bitmapImage);

            }
        }

        private void DeletePhoto(object sender, RoutedEventArgs e)
        {
            try
            {
                Images.RemoveAt(imageBox.SelectedIndex);
                imageBox.SelectedItem = null;
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        private void UpdateDeletePhotoButton(object sender, SelectionChangedEventArgs e)
        {
            if (imageBox.SelectedItem != null)
            {
                deletePhotoButton.IsEnabled = true;
            }
            else
            {
                deletePhotoButton.IsEnabled = false;
            }
        }

        private void UpdateAddPhotoButton(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (Images.Count >= 5)
            {
                addPhotoButton.IsEnabled = false;
            }
            else
            {
                addPhotoButton.IsEnabled = true;
            }
        }

        private void ConfirmChanges(object sender, RoutedEventArgs e)
        {
            if (MainWindow.profile != null)
            {
                List<byte[]> imageData = Images.Select(x => ImageConverter.BitmapImageToData(x)).ToList();

                MainWindow.profile.BirthDate = BirthDatePicker.DisplayDate;
                MainWindow.profile.Country = CountryBox.Text;
                MainWindow.profile.City = CityBox.Text;
                MainWindow.profile.PostalCode = PostalCodeBox.Text;
                MainWindow.profile.Gender = (Gender)genderBox.SelectedIndex;
                MainWindow.profile.SexualPreference = (SexualPreference)sexuelPreferenceBox.SelectedIndex;
                MainWindow.profile.Images = imageData;

                MainWindow.repo.UpdateProfile(MainWindow.profile);
            }
        }

       

       
    }
}
