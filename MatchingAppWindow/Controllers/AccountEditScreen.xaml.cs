using KBS_project;
using KBS_project.Enums;
using KBS_project.Exceptions;
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

        private List<Control> invalidFields;

        public event EventHandler ExitPage;

        public AccountEditScreen()
        {
            InitializeComponent();

            DataContext = this;

            imageBox.SelectionChanged += UpdateDeletePhotoButton;
            Images.CollectionChanged += UpdateAddPhotoButton;

            ExitPage += (sender, e) => ClearErrorFields();

            SizeChanged += (sender, e) =>
            {
                if (ActualWidth >= 1100) ProfilePreview.Visibility = Visibility.Visible;
                else ProfilePreview.Visibility = Visibility.Collapsed;
            };
        }

        public void InitializePage()
        {
            birthDatePicker.Text = MainWindow.profile.BirthDate.ToString();
            CountryBox.Text = MainWindow.profile.Country;
            CityBox.Text = MainWindow.profile.City;
            PostalCodeBox.Text = MainWindow.profile.PostalCode;
            genderBox.SelectedIndex = (int)MainWindow.profile.Gender;
            sexualPreferenceBox.SelectedIndex = (int)MainWindow.profile.SexualPreference;

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
            if (MainWindow.profile == null) return;

            invalidFields = new();

            if (Images.Count != 0)
            {
                List<byte[]> imageData = Images.Select(x => ImageConverter.BitmapImageToData(x)).ToList();
                MainWindow.profile.Images = imageData;
            }
            else 
            {
                invalidFields.Add(imageBox);
            }

            if (genderBox.SelectedIndex != -1)
            {
                MainWindow.profile.Gender = (Gender)genderBox.SelectedIndex;
            }
            else
            {
                invalidFields.Add(genderBox);
            }

            if (sexualPreferenceBox.SelectedIndex != -1)
            {
                MainWindow.profile.SexualPreference = (SexualPreference)sexualPreferenceBox.SelectedIndex;
            }
            else
            {
                invalidFields.Add(sexualPreferenceBox);
            }

            try
            {
                MainWindow.profile.BirthDate = RegistrationFieldsExtensions.Validate(birthDatePicker.SelectedDate);
            } 
            catch (InvalidFieldException)
            {
                invalidFields.Add(birthDatePicker);
            }

            MainWindow.profile.Country = CheckField(CountryBox, RegistrationFields.Country);
            MainWindow.profile.City = CheckField(CityBox, RegistrationFields.City);
            MainWindow.profile.PostalCode = CheckField(PostalCodeBox, RegistrationFields.PostalCode);

            if (invalidFields.Count > 0)
            {
                ShowErrors();
                return;
            }
            else ClearErrorFields();

            MainWindow.repo.UpdateProfile(MainWindow.profile);

            ExitPage?.Invoke(this, new EventArgs());
        }

        private string CheckField(TextBox textBox, RegistrationFields field)
        {
            try
            {
                return RegistrationFieldsExtensions.Validate(textBox.Text, field, MainWindow.repo);
            }
            catch (InvalidFieldException)
            {
                invalidFields.Add(textBox);
            }

            return String.Empty;
        }

        private void ShowErrors()
        {
            foreach (var field in invalidFields)
            {
                DisplayFieldError(field);
            }

            errorMessage.Visibility = Visibility.Visible;
        }

        private void DisplayFieldError(Control inputField)
        {
            inputField.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff6666");
            inputField.BorderBrush = Brushes.Red;
        }

        private void ClearErrorFields()
        {
            var fields = new List<Control>()
            {
                CityBox,
                CountryBox,
                PostalCodeBox,
                genderBox,
                sexualPreferenceBox,
                birthDatePicker,
            };

            foreach (Control field in fields)
            {
                field.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
                field.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
            }

            errorMessage.Visibility = Visibility.Hidden;
        }
    }
}
