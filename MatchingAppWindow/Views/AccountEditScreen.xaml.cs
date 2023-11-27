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
        private Profile? profile;
        DateTime? birthdate;
        string? residence;
        Gender? gender;
        SexualPreference? sexualPreference;


        public AccountEditScreen()
        {
            profile = MainWindow.profile;
            if (profile != null)
            {
                BirthDatePicker.DisplayDate = profile.BirthDate;
                CityBox.Text = profile.City;
                SetGender(profile.Gender);
                setPreference(profile.SexualPreference);
            }
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
            birthdate = BirthDatePicker.DisplayDate;
            residence = CityBox.Text;
            gender = GetGender();
            sexualPreference = getSexuality();
        }

        private void SetGender(Gender gender)
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

        private void setPreference(SexualPreference sp)
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
            return Gender.Male;
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
            return SexualPreference.Men;
        }
    }
}
