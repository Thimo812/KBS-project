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

        private void ConfirmChanges(object sender, RoutedEventArgs e)
        {
            if (MainWindow.profile != null)
            {
                MainWindow.profile.BirthDate = BirthDatePicker.DisplayDate;
                MainWindow.profile.Country = CountryBox.Text;
                MainWindow.profile.City = CityBox.Text;
                MainWindow.profile.PostalCode = PostalCodeBox.Text;
                MainWindow.profile.Gender = GetGender();
                MainWindow.profile.SexualPreference = GetSexuality();

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

        public void SetPreference(SexualPreference sp)
        {
            switch(sp)
            {
                case SexualPreference.Hetero:
                    HeteroSexuality.IsChecked = true;
                    break;
                case SexualPreference.Homoseksueel:
                    HomoSexuality.IsChecked = true;
                    break;
                default:
                    BiSexuality.IsChecked = true;
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

        public SexualPreference GetSexuality()
        {
            if ((bool)HeteroSexuality.IsChecked)
            {
                return SexualPreference.Hetero;
            }
            else if ((bool)HomoSexuality.IsChecked)
            {
                return SexualPreference.Homoseksueel;
            }
            else if ((bool)BiSexuality.IsChecked)
            {
                return SexualPreference.Biseksueel;
            }
            return SexualPreference.Hetero;
        }

        private void FemaleSexuality_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
