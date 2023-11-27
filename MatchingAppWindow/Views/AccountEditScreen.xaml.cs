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
        DateTime birthdate;
        string residence;
        Gender gender;
        SexualPreference sexualPreference;


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
            birthdate = BirthDatePicker.DisplayDate;
            residence = CityBox.Text;
            gender = GetGender();
            sexualPreference = getSexuality();
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
