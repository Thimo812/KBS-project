using KBS_project;
using KBS_project.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ProfileEditScreen.xaml
    /// </summary>
    public partial class ProfileEditScreen : Page
    {

        public ObservableCollection<Interest> Interests { get; set; } = new();

        public ObservableCollection<Interest> AvailableInterests {  get; set; } = new();

        public ProfileEditScreen()
        {
            InitializeComponent();

            DataContext = this;
        }

        public void InitializePage()
        {
            foreach (var item in MainWindow.profile.Interests)
            {
                Interests.Add(item);
            }

            for (int i = 0;  i < InterestExtensions.count; i++)
            {
                if (!Interests.Contains((Interest)i))
                {
                    AvailableInterests.Add((Interest)i);
                }
            }
        }

        public void InterestSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hobbyPanel.SelectedIndex == -1)
            {
                return;
            }

            var selectedInterest = Interests[hobbyPanel.SelectedIndex];

            AvailableInterests.Add(selectedInterest);
            Interests.Remove(selectedInterest);

        }

        public void AvailableInterestSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (availableHobbyList.SelectedIndex == -1)
            {
                return;
            }

            var selectedInterest = AvailableInterests[availableHobbyList.SelectedIndex];

            Interests.Add(selectedInterest);
            AvailableInterests.Remove(selectedInterest);
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
                MainWindow.profile.Interests = Interests.ToList();

                MainWindow.repo.UpdateProfile(MainWindow.profile);
            }
        }

        public void SetDiet(Diet? diet)
        {
            switch(diet)
            {
                case Diet.Geen:
                    NoDiet.IsChecked = true;
                    break;
                case Diet.Vegetarisch:
                    VegieDiet.IsChecked = true;
                    break;
                case Diet.Veganistisch:
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
                return Diet.Geen;
            }
            else if ((bool)VegieDiet.IsChecked)
            {
                return Diet.Vegetarisch;
            }
            else if ((bool)VeganDiet.IsChecked)
            {
                return Diet.Veganistisch;
            }
            else if ((bool)KetoDiet.IsChecked)
            {
                return Diet.Keto;
            }
            return Diet.Anders;
        }
    }
}
