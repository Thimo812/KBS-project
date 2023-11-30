using KBS_project;
using KBS_project.Enums;
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
    /// Interaction logic for AccountEditScreen.xaml
    /// </summary>
    public partial class AccountEditScreen : Page
    {
        public AccountEditScreen()
        {
            InitializeComponent();
        }

        private void SwitchToPhotoEditScreen(Object sender, RoutedEventArgs e)
        {
            
        }

        private void SwitchToProfileEditScreen(Object sender, RoutedEventArgs e)
        {
        }

        private void ConfirmChanges(object sender, RoutedEventArgs e)
        {
            if (MainWindow.profile != null)
            {
                MainWindow.profile.BirthDate = BirthDatePicker.DisplayDate;
                MainWindow.profile.Country = CountryBox.Text;
                MainWindow.profile.City = CityBox.Text;
                MainWindow.profile.PostalCode = PostalCodeBox.Text;
                MainWindow.profile.Gender = GetGender();
                MainWindow.profile.SexualPreference = getSexuality();

                MainWindow.repository.UpdateProfile(MainWindow.profile);
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

        public void setPreference(SexualPreference sp)
        {
            switch(sp)
            {
                case SexualPreference.Men:
                    MaleSexuality.IsChecked = true;
                    break;
                case SexualPreference.Women:
                    FemaleSexuality.IsChecked = true;
                    break;
                default:
                    EveryoneSexuality.IsChecked = true;
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

        public SexualPreference getSexuality()
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
            return SexualPreference.Men;
        }
    }
}
