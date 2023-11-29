using KBS_project;
using KBS_project.Enums;
using MatchingApp.DataAccess.SQL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
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
                    deletePhotoButton.IsEnabled = true;
                }
                else
                {
                    deletePhotoButton.IsEnabled = false;
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
                string userName = RegistrationFieldsExtensions.Validate(userNameInput.Text, RegistrationFields.UserName, Repo);
                string firstName = RegistrationFieldsExtensions.Validate(firstNameInput.Text, RegistrationFields.FirstName, Repo);
                string infix = RegistrationFieldsExtensions.Validate(infixInput.Text, RegistrationFields.Infix, Repo);
                string lastName = RegistrationFieldsExtensions.Validate(lastNameInput.Text, RegistrationFields.LastName, Repo);
                string city = RegistrationFieldsExtensions.Validate(cityInput.Text, RegistrationFields.City, Repo);
                string country = RegistrationFieldsExtensions.Validate(countryInput.Text, RegistrationFields.Country, Repo);
                string postalCode = RegistrationFieldsExtensions.Validate(postalCodeInput.Text, RegistrationFields.PostalCode, Repo);

                DateTime birthdate = RegistrationFieldsExtensions.Validate(birthDatePicker.SelectedDate);

                List<bool?> genderRadioButtonValues = new List<bool?>() { maleGender.IsChecked, femaleGender.IsChecked, nonBinaryGender.IsChecked };
                Gender gender = RegistrationFieldsExtensions.ValidateGender(genderRadioButtonValues);

                List<bool?> sexRadioButtonValues = new List<bool?>() { maleGender.IsChecked, femaleGender.IsChecked, nonBinaryGender.IsChecked };
                SexualPreference sexuality = RegistrationFieldsExtensions.ValidateSexuality(sexRadioButtonValues);

                List<string> imagePaths = ImageList.Select(x => x.UriSource.ToString()).ToList();

                Profile = new Profile(userName, firstName, infix, lastName, birthdate, gender, sexuality, city, postalCode, country, imagePaths);

                ClearErrorFields();

                exitPage?.Invoke(this, EventArgs.Empty);
            }
            catch (AggregateException aggEx)
            {
                foreach (var exception in aggEx.InnerExceptions)
                {
                    if (exception is FieldEmptyException fee)
                    {
                        AddValidationError(fee);
                    }

                    ShowErrors();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Er kon geen verbinding worden gemaakt met de database");
            }
        }

        private List<Control> validationErrors = new List<Control>();

        private void AddValidationError(FieldEmptyException exception)
        {
            switch (exception.Field)
            {
                case RegistrationFields.UserName:
                    validationErrors.Add(userNameInput);
                    break;
                case RegistrationFields.FirstName:
                    validationErrors.Add(firstNameInput);
                    break;
                case RegistrationFields.LastName:
                    validationErrors.Add(lastNameInput);
                    break;
                case RegistrationFields.Infix:
                    validationErrors.Add(infixInput);
                    break;
                case RegistrationFields.City:
                    validationErrors.Add(cityInput);
                    break;
                case RegistrationFields.Country:
                    validationErrors.Add(countryInput);
                    break;
                case RegistrationFields.PostalCode:
                    validationErrors.Add(postalCodeInput);
                    break;
                case RegistrationFields.Gender:
                    validationErrors.AddRange(new List<RadioButton>() { maleGender, femaleGender, nonBinaryGender });
                    break;
                case RegistrationFields.Sexuality:
                    validationErrors.AddRange(new List<RadioButton>() { maleSexuality, femaleSexuality, everyoneSexuality });
                    break;

            }

        }

        private void ClearErrorFields()
        {

            foreach (Control field in validationErrors)
            {
                field.Background = null;
                field.BorderBrush = null;
            }
        }

        private void ShowErrors()
        {
            foreach (var field in validationErrors)
            {
                DisplayFieldError(field);
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

        private void DisplayFieldError(Control inputField)
        {
            inputField.Background = Brushes.Pink;
            inputField.BorderBrush = Brushes.Red;
        }

        public event EventHandler exitPage;
    }
}
