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
        string description;
        string hobbies;
        string education;
        string school;
        string employment;
        string diet;

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
            description = BeschrijvingBox.Text;
            hobbies = HobbyBox.Text;
            education = OpleidingBox.Text;
            school = SchoolBox.Text;
            employment  = WerkplekBox.Text;
            diet = DieetBox.Text;
        }
    }
}
