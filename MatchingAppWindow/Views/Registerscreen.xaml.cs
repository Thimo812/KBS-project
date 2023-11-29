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

        private MatchingAppRepository Repo {  get; set; }

        private List<Control> invalidFields;

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
        }

        private void AddPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ImageList.Add(new BitmapImage(new Uri(openFileDialog.FileName)));
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

            string userName = CheckTextField(UserNameInput, RegistrationFields.UserName);
            string firstName = CheckTextField(FirstNameInput, RegistrationFields.FirstName);
            string infix = CheckTextField(InfixInput, RegistrationFields.Infix);
            string lastName = CheckTextField(LastNameInput, RegistrationFields.LastName);
            string city = CheckTextField(CityInput, RegistrationFields.City);
            string country = CheckTextField(CountryInput, RegistrationFields.Country);
            string postalCode = CheckTextField(PostalCodeInput, RegistrationFields.PostalCode);
            Gender gender = new();
            SexualPreference sexualPreference = new();
            DateTime birthDate = new();

            try
            {
                gender = RegistrationFieldsExtensions.ValidateGender(new List<bool?>() { MaleGender.IsChecked, FemaleGender.IsChecked, NonBinaryGender.IsChecked });
            }
            catch (InvalidFieldException)
            {
                invalidFields.AddRange(new List<Control>() { MaleGender, FemaleGender, NonBinaryGender } );
            }

            try
            {
                sexualPreference = RegistrationFieldsExtensions.ValidateSexuality(new List<bool?>() { MaleSexuality.IsChecked, FemaleSexuality.IsChecked, EveryoneSexuality.IsChecked });
            }
            catch (InvalidFieldException)
            {
                invalidFields.AddRange(new List<Control>() { MaleSexuality, FemaleSexuality, EveryoneSexuality });
            }

            try
            {
                birthDate = RegistrationFieldsExtensions.Validate(BirthDatePicker.SelectedDate);
            }
            catch (InvalidFieldException)
            {
                invalidFields.Add(BirthDatePicker);
            }

            if (invalidFields.Count > 0)
            {
                ShowErrors();
                return;
            }

            List<string> imagePaths = ImageList.Select(x => x.UriSource.ToString()).ToList();

            MainWindow.profile = new Profile(userName, firstName, infix, lastName, birthDate, gender, sexualPreference, city, postalCode, country, imagePaths);

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
                UserNameInput,
                FirstNameInput,
                LastNameInput,
                InfixInput,
                CityInput,
                CountryInput,
                PostalCodeInput,
                BirthDatePicker,
                MaleGender,
                FemaleGender,
                NonBinaryGender,
                MaleSexuality,
                FemaleSexuality,
                EveryoneSexuality
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
        
        public event EventHandler ExitPage;

    }
}
