using KBS_project;
using KBS_project.Enums;
using KBS_project.Exceptions;
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
        public event EventHandler ExitPage;
        private MatchingAppRepository Repo {  get; set; }

        private List<Control> invalidFields;

        public ObservableCollection<string> ImageList { get; set; } = new();

        public RegisterScreen(MatchingAppRepository repo)
        {
            Repo = repo;

            InitializeComponent();

            DataContext = this;

            imageBox.SelectionChanged += UpdateDeletePhotoButton;
            ImageList.CollectionChanged += UpdateAddPhotoButton;
        }

        private void AddPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ImageList.Add(openFileDialog.FileName);
            }
        }

        private void DeletePhoto(object sender, RoutedEventArgs e)
        {
            ImageList.RemoveAt(imageBox.SelectedIndex);
            imageBox.SelectedItem = null;
        }

        private void RegisterAccount(object sender, RoutedEventArgs e)
        {
            ClearErrorFields();

            invalidFields = new List<Control>();

            string userName = CheckTextField(userNameInput, RegistrationFields.UserName);
            string firstName = CheckTextField(firstNameInput, RegistrationFields.FirstName);
            string infix = CheckTextField(infixInput, RegistrationFields.Infix);
            string lastName = CheckTextField(lastNameInput, RegistrationFields.LastName);
            string city = CheckTextField(cityInput, RegistrationFields.City);
            string country = CheckTextField(countryInput, RegistrationFields.Country);
            string postalCode = CheckTextField(postalCodeInput, RegistrationFields.PostalCode);
            Gender gender = new();
            SexualPreference sexualPreference = new();
            DateTime birthDate = new();

            try
            {
                gender = RegistrationFieldsExtensions.ValidateGender(new List<bool?>() { maleGender.IsChecked, femaleGender.IsChecked, nonBinaryGender.IsChecked });
            }
            catch (InvalidFieldException)
            {
                invalidFields.AddRange(new List<Control>() {maleGender, femaleGender, nonBinaryGender } );
            }

            try
            {
                sexualPreference = RegistrationFieldsExtensions.ValidateSexuality(new List<bool?>() { maleSexuality.IsChecked, femaleSexuality.IsChecked, everyoneSexuality.IsChecked });
            }
            catch (InvalidFieldException)
            {
                invalidFields.AddRange(new List<Control>() { maleSexuality, femaleSexuality, everyoneSexuality });
            }

            try
            {
                birthDate = RegistrationFieldsExtensions.Validate(birthDatePicker.SelectedDate);
            }
            catch (InvalidFieldException)
            {
                invalidFields.Add(birthDatePicker);
            }

            if (invalidFields.Count > 0 || ImageList.Count == 0)
            {
                ShowErrors();
                return;
            }

            MainWindow.profile = new Profile(userName, firstName, infix, lastName, birthDate, gender, sexualPreference, city, postalCode, country, ImageList.ToList());

            Repo.SaveProfile(MainWindow.profile);

            ExitPage?.Invoke(this, EventArgs.Empty);
        }

        private string CheckTextField(TextBox textBox, RegistrationFields field)
        {
            try
            {
                return RegistrationFieldsExtensions.Validate(textBox.Text, field, Repo);
            }
            catch (InvalidFieldException)
            {
                invalidFields.Add(textBox);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Er kon geen verbinding worden gemaakt met de database");
            }

            return String.Empty;

        }

        private void ClearErrorFields()
        {
            var fields = new List<Control>()
            {
                userNameInput,
                firstNameInput,
                lastNameInput,
                infixInput,
                cityInput,
                countryInput,
                postalCodeInput,
                birthDatePicker,
                maleGender,
                femaleGender,
                nonBinaryGender,
                maleSexuality,
                femaleSexuality,
                everyoneSexuality
            };

            foreach (Control field in fields)
            {
                field.Background = Brushes.White;
                field.BorderBrush = Brushes.Gray;
            }

            errorMessage.Visibility = Visibility.Hidden;
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
            inputField.Background = Brushes.Pink;
            inputField.BorderBrush = Brushes.Red;
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

        private void UpdateAddPhotoButton(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ImageList.Count >= 5)
            {
                AddPhotoButton.IsEnabled = false;
            }
            else
            {
                AddPhotoButton.IsEnabled = true;
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
    }
}
