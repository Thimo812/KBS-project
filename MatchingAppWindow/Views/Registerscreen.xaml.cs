using KBS_project;
using KBS_project.Enums;
using MatchingApp.DataAccess.SQL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class RegisterScreen : Page
    {
        public Profile? Profile {  get; set; }

        private MatchingAppRepository Repo {  get; set; }

        public ObservableCollection<BitmapImage> ImageList { get; set; } = new();

        public RegisterScreen(MatchingAppRepository repo)
        {
            Repo = repo;

            InitializeComponent();

            DataContext = this;

            imageBox.SelectionChanged += (object sender, SelectionChangedEventArgs e) => 
            { 
                if (imageBox.SelectedItem != null)
                {
                    DeletePhotoButton.IsEnabled = true;
                }
                else
                {
                    DeletePhotoButton.IsEnabled = false;
                }
            };

            imageBox.LostFocus += (object sender, RoutedEventArgs e) => imageBox.SelectedItem = null;
        }

        private void AddPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                //ImageList.Add(new Image() { ImagePath = openFileDialog.FileName });
                ImageList.Add(new BitmapImage(new Uri(openFileDialog.FileName)));
            }
        }

        private void DeletePhoto(object sender, RoutedEventArgs e)
        {
            ImageList.RemoveAt(imageBox.SelectedIndex);
        }

        private void RegisterAccount(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = RegistrationFieldsExtensions.Validate(UserNameInput.Text, RegistrationFields.UserName, Repo);
                string firstName = RegistrationFieldsExtensions.Validate(FirstNameInput.Text, RegistrationFields.FirstName, Repo);
                string infix = RegistrationFieldsExtensions.Validate(InfixInput.Text, RegistrationFields.Infix, Repo);
                string lastName = RegistrationFieldsExtensions.Validate(LastNameInput.Text, RegistrationFields.LastName, Repo);
                string city = RegistrationFieldsExtensions.Validate(CityInput.Text, RegistrationFields.City, Repo);
                string country = RegistrationFieldsExtensions.Validate(CountryInput.Text, RegistrationFields.Country, Repo);
                string postalCode = RegistrationFieldsExtensions.Validate(PostalCodeInput.Text, RegistrationFields.PostalCode, Repo);

                DateTime birthdate = RegistrationFieldsExtensions.Validate(BirthDatePicker.SelectedDate);

                List<bool?> genderRadioButtonValues = new List<bool?>() { MaleGender.IsChecked, FemaleGender.IsChecked, NonBinaryGender.IsChecked };
                Gender gender = RegistrationFieldsExtensions.ValidateGender(genderRadioButtonValues);

                List<bool?> sexRadioButtonValues = new List<bool?>() { MaleGender.IsChecked, FemaleGender.IsChecked, NonBinaryGender.IsChecked };
                SexualPreference sexuality = RegistrationFieldsExtensions.ValidateSexuality(sexRadioButtonValues);

                List<string> ImagePaths = ImageList.Select(x => x.UriSource.ToString()).ToList();

                Profile = new Profile(userName, firstName, infix, lastName, birthdate, gender, sexuality, city, postalCode, country, ImagePaths);
            } 
            catch (FieldEmptyException fee)
            {
                
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void DisplayFieldError(TextBox textBox)
        {
            textBox.Background = Brushes.Pink;
            textBox.BorderBrush = Brushes.Red;
        }

        private void DisplayFieldError(RadioButton[] radioButtons)
        {
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.Background = Brushes.Pink;
                radioButton.BorderBrush = Brushes.Red;

            }
        }

        private void DisplayFieldError(DatePicker datePicker)
        {
            datePicker.Background = Brushes.Pink;
            datePicker.BorderBrush = Brushes.Red;
        }
    }
}
