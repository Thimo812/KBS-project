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
    /// Interaction logic for ProfileEditScreen.xaml
    /// </summary>
    public partial class ProfileEditScreen : Page
    {

        public ProfileEditScreen()
        {
            InitializeComponent();
        }

        private void SwitchToPhotoEditScreen(object sender, RoutedEventArgs e)
        {
        }

        private void SwitchToAccountEditScreen(object sender, RoutedEventArgs e)
        {
        }

        private void ConfirmChanges(object sender, RoutedEventArgs e)
        {
            if (MainWindow.profile != null)
            {
                MainWindow.profile.Description = BeschrijvingBox.Text;
                MainWindow.profile.Degree = OpleidingBox.Text;
                MainWindow.profile.School = SchoolBox.Text;
                MainWindow.profile.WorkPlace = WerkplekBox.Text;
                MainWindow.profile.Diet = GetDiet();

                MainWindow.repo.UpdateProfile(MainWindow.profile);
            }
        }

        public void SetDiet(Diet? diet)
        {
            switch(diet)
            {
                case Diet.None:
                    NoDiet.IsChecked = true;
                    break;
                case Diet.Vegetarian:
                    VegieDiet.IsChecked = true;
                    break;
                case Diet.Vegan:
                    VeganDiet.IsChecked = true;
                    break;
                case Diet.Keto:
                    KetoDiet.IsChecked = true;
                    break;
                default:
                    OtherDiet.IsChecked = true;
                    break;
            }
        }

        public Diet GetDiet()
        {
            if ((bool)NoDiet.IsChecked)
            {
                return Diet.None;
            }
            else if ((bool)VegieDiet.IsChecked)
            {
                return Diet.Vegetarian;
            }
            else if ((bool)VeganDiet.IsChecked)
            {
                return Diet.Vegan;
            }
            else if ((bool)KetoDiet.IsChecked)
            {
                return Diet.Keto;
            }
            return Diet.Other;
        }
    }
}
