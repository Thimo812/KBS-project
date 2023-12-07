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
            AvailableInterests.Add(Interests[hobbyPanel.SelectedIndex]);
            Interests.RemoveAt(hobbyPanel.SelectedIndex);
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
