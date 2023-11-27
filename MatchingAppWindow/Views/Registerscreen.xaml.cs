using KBS_project;
using KBS_project.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class RegisterScreen : Page
    {
        public Profile Profile {  get; set; }
        public RegisterScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ProfileImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void RegisterAccount(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = UserNameInput.Text.Equals(String.Empty) ? throw new FieldEmptyException() : UserNameInput.Text;
                string firstName = FirstNameInput.Text.Equals(String.Empty) ? throw new FieldEmptyException() : FirstNameInput.Text;
                string lastName = LastNameInput.Text.Equals(String.Empty) ? throw new FieldEmptyException() : LastNameInput.Text;
                DateTime birthDate = BirthDatePicker.SelectedDate.Equals(null) ? throw new FieldEmptyException() : (DateTime)BirthDatePicker.SelectedDate;
                Gender gender = GetGender();
                SexualPreference sexualPreference = getSexuality();
                string city = CityInput.Text.Equals(String.Empty) ? throw new FieldEmptyException() : CityInput.Text;
                string postalCode = PostalCodeInput.Text.Equals(String.Empty) ? throw new FieldEmptyException() : PostalCodeInput.Text;
                string country = CountryInput.Text.Equals(String.Empty) ? throw new FieldEmptyException() : CountryInput.Text;

                Profile = new(userName, firstName, String.Empty, lastName, birthDate, gender, sexualPreference, city, postalCode, country);
            } 
            catch (FieldEmptyException fee)
            {
                if (UserNameInput.InputBindings.Equals(String.Empty)) { DisplayTextBoxError(UserNameInput); }
                if (FirstNameInput.InputBindings.Equals(String.Empty)) DisplayTextBoxError(FirstNameInput);
                if (LastNameInput.InputBindings.Equals(String.Empty)) DisplayTextBoxError(LastNameInput);
                if (CityInput.InputBindings.Equals(String.Empty)) DisplayTextBoxError(CityInput);
                if (PostalCodeInput.InputBindings.Equals(String.Empty)) DisplayTextBoxError(PostalCodeInput);
                if (CountryInput.InputBindings.Equals(String.Empty)) DisplayTextBoxError(CountryInput);
                if (!(bool)MaleGender.IsChecked && !(bool)FemaleGender.IsChecked && !(bool)NonBinaryGender.IsChecked) DisplayRadioBoxError(new RadioButton[] { MaleGender, FemaleGender, NonBinaryGender});
                if (!(bool)MaleSexuality.IsChecked && !(bool)FemaleSexuality.IsChecked && !(bool)EveryoneSexuality.IsChecked) DisplayRadioBoxError(new RadioButton[] { MaleSexuality, FemaleSexuality, EveryoneSexuality });
                if (BirthDatePicker.SelectedDate.Equals(null)) DisplayDatePickerError(BirthDatePicker);
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

        private void DisplayTextBoxError(TextBox textBox)
        {
            textBox.Background = Brushes.Pink;
            textBox.BorderBrush = Brushes.Red;
        }

        private void DisplayRadioBoxError(RadioButton[] radioButtons)
        {
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.Background = Brushes.Pink;
                radioButton.BorderBrush = Brushes.Red;

            }
        }

        private void DisplayDatePickerError(DatePicker datePicker)
        {
            datePicker.Background = Brushes.Pink;
            datePicker.BorderBrush = Brushes.Red;
        }

        private Gender GetGender()
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
            throw new FieldEmptyException();
        }

        private SexualPreference getSexuality()
        {
            if ((bool)MaleSexuality.IsChecked)
            {
                return SexualPreference.Men;
            } 
            else if ((bool)FemaleSexuality.IsChecked)
            {
                return SexualPreference.Women;
            } 
            else if ((bool)EveryoneSexuality.IsChecked)
            {
                return SexualPreference.All;
            }
            throw new FieldEmptyException();
        }

    }
}
