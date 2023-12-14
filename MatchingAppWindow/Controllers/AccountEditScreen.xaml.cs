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
        public AccountEditScreen()
        {
            InitializeComponent();

            DataContext = this;

            imageBox.SelectionChanged += UpdateDeletePhotoButton;
            Images.CollectionChanged += UpdateAddPhotoButton;
        }

        public void InitializePage()
        {
            BirthDatePicker.Text = MainWindow.profile.BirthDate.ToString();
            CountryBox.Text = MainWindow.profile.Country;
            CityBox.Text = MainWindow.profile.City;
            PostalCodeBox.Text = MainWindow.profile.PostalCode;
            SetGender(MainWindow.profile.Gender);
            SetPreference(MainWindow.profile.SexualPreference);

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
                MainWindow.profile.Gender = GetGender();
                MainWindow.profile.SexualPreference = GetSexuality();
                MainWindow.profile.Images = imageData;

                MainWindow.repo.UpdateProfile(MainWindow.profile);
            }
        }

        public void SetGender(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    MaleGender.IsChecked = true;
                    break;
                case Gender.Female:
                    FemaleGender.IsChecked = true;
                    break;
                default: 
                    NonBinaryGender.IsChecked = true;
                    break;
            }
        }

        public void SetPreference(SexualPreference sp)
        {
            switch(sp)
            {
                case SexualPreference.Hetero:
                    HeteroSexuality.IsChecked = true;
                    break;
                case SexualPreference.Homoseksueel:
                    HomoSexuality.IsChecked = true;
                    break;
                default:
                    BiSexuality.IsChecked = true;
                    break;
            }
        }

        public Gender GetGender()
        {
            if ((bool)MaleGender.IsChecked)
            {
                return Gender.Male;
            }
            else if ((bool)FemaleGender.IsChecked)
            {
                return Gender.Female;
            }
            else if ((bool)NonBinaryGender.IsChecked)
            {
                return Gender.NonBinary;
            }
            return Gender.Male;
        }

        public SexualPreference GetSexuality()
        {
            if ((bool)HeteroSexuality.IsChecked)
            {
                return SexualPreference.Hetero;
            }
            else if ((bool)HomoSexuality.IsChecked)
            {
                return SexualPreference.Homoseksueel;
            }
            else if ((bool)BiSexuality.IsChecked)
            {
                return SexualPreference.Biseksueel;
            }
            return SexualPreference.Hetero;
        }

        private void FemaleSexuality_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
