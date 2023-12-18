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
using System.IO;
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

        private List<Control> invalidFields;

        public ObservableCollection<string> ImageList { get; set; } = new();

        public RegisterScreen()
        {
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

            string userName = CheckField(userNameInput, RegistrationFields.UserName);
            string firstName = CheckField(firstNameInput, RegistrationFields.FirstName);
            string infix = CheckField(infixInput, RegistrationFields.Infix);
            string lastName = CheckField(lastNameInput, RegistrationFields.LastName);
            string city = CheckField(cityInput, RegistrationFields.City);
            string country = CheckField(countryInput, RegistrationFields.Country);
            string postalCode = CheckField(postalCodeInput, RegistrationFields.PostalCode);
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
                sexualPreference = RegistrationFieldsExtensions.ValidateSexuality(new List<bool?>() { heteroSexual.IsChecked, homoSexual.IsChecked, biSexual.IsChecked });
            }
            catch (InvalidFieldException)
            {
                invalidFields.AddRange(new List<Control>() { heteroSexual, homoSexual, biSexual });
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

            List <byte[]> imageDataList = new List<byte[]>();

            foreach (var image in ImageList)
            {
                imageDataList.Add(File.ReadAllBytes(image));
            }

            MainWindow.profile = new Profile(userName, firstName, infix, lastName, birthDate, gender, sexualPreference, city, postalCode, country, imageDataList);

            MainWindow.repo.SaveProfile(MainWindow.profile);

            ExitPage?.Invoke(this, EventArgs.Empty);
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
                heteroSexual,
                homoSexual,
                biSexual
            };

            foreach (Control field in fields)
            {
                field.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
                field.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
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
            inputField.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff6666");
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
