using KBS_project;
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
        private Profile? profile;
        string? description;
        string? hobbies;
        string? education;
        string? school;
        string? employment;
        string? diet;

        public ProfileEditScreen()
        {
            profile = MainWindow.profile;
            if (profile != null ) {
                BeschrijvingBox.Text = profile.Description;
                OpleidingBox.Text = profile.degree;
                SchoolBox.Text = profile.School;
                WerkplekBox.Text = profile.WorkPlace;
                DieetBox.Text = profile.Diet;
            }
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
            profile.Description = BeschrijvingBox.Text;
            profile.degree = OpleidingBox.Text;
            profile.School = SchoolBox.Text;
            profile.WorkPlace  = WerkplekBox.Text;
            profile.Diet = DieetBox.Text;

            MainWindow.profile = profile;

        }
    }
}
