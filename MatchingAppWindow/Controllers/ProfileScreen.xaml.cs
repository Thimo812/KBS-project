using KBS_project.Enums.FilterOptions;
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
using KBS_project;
using MatchingApp.DataAccess.SQL;
using System.Security.Policy;
using System.Data.SqlClient;
using System.Windows.Controls.Primitives;
using System.Net;
using System.Xml;

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for FilterScreen.xaml
    /// </summary>
    public partial class ProfileScreen : Page
    {

        //Creating all attributes
        MatchingAppRepository repo = new MatchingAppRepository();
        private LocationFilter location = LocationFilter.Global;
        private int minimumAge;
        private int maximumAge;
        private List<int> includedHobbies = new();
        private List<int> excludedHobbies = new();
        private List<Diet> includedDiets = new();
        private List<Diet> excludedDiets = new();

        private ProfileDetails profileDetails = new();
        public ProfileScreen()
        {
            InitializeComponent();

            var profileList = repo.GetProfiles(MainWindow.profile);

            resultBox.ItemsSource = profileList;

            profileDetailsFrame.Content = profileDetails;

            DataContext = this;

            int hobbyCount = repo.GetHobbies().Count;

            for (int i = 1; i < hobbyCount; i++)
            {
                CheckBox checkBox = new CheckBox();

                checkBox.Tag = i;
                checkBox.Style = this.FindResource("CustomCheckBoxStyle") as Style;
                checkBox.Content = InterestExtensions.GetString(i);
                checkBox.Height = 22;
                checkBox.IsThreeState = true;
                checkBox.Checked += HobbyChecked;
                checkBox.Unchecked += HobbyUnchecked;
                checkBox.Indeterminate += HobbyIndeterminate;
                checkBox.Foreground = Brushes.White;

                HobbyCheckBoxes.Children.Add(checkBox);
            }

            for (int i = 1; i < Enum.GetNames(typeof(Diet)).Length; i++)
            {
                CheckBox checkBox = new CheckBox();

                checkBox.Style = this.FindResource("CustomCheckBoxStyle") as Style;
                checkBox.Content = Enum.GetValues(typeof(Diet)).GetValue(i);
                checkBox.Height = 22;
                checkBox.IsThreeState = true;
                checkBox.Checked += DietChecked;
                checkBox.Unchecked += DietUnchecked;
                checkBox.Indeterminate += DietIndeterminate;
                checkBox.Foreground = Brushes.White;

                DietCheckBoxes.Children.Add(checkBox);
            }
        }

        private void LocationChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton senderLoc)
            {
                location = Enum.TryParse<LocationFilter>(senderLoc.Name, out var filter) ? filter : LocationFilter.Global;
                Filter();
            }
        }


        private void ExtendOrCollapseHobbies_Click(object sender, RoutedEventArgs e)
        {
            if ((string)ExtendOrCollapseHobbies.Content == "Hobby ▲")
            {
                ExtendOrCollapseHobbies.Content = "Hobby ▼";
                HobbyCheckBoxes.Visibility = Visibility.Collapsed;
            }
            else if((string)ExtendOrCollapseHobbies.Content == "Hobby ▼")
            {
                ExtendOrCollapseHobbies.Content = "Hobby ▲";
                HobbyCheckBoxes.Visibility = Visibility.Visible;
            }
        }

        private void HobbyChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Green;
            IncludeHobbys((int)checkBox.Tag);
            Filter();
        }

        private void HobbyUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.White;
            ClearUncheckedHobbies((int)checkBox.Tag);
            Filter();
        }

        private void HobbyIndeterminate(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Red;
            ExcludeHobbys((int)checkBox.Tag);
            Filter();
        }

        private void DietChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Green;
            IncludeDiets((Diet)checkBox.Content);
            Filter();
        }

        private void DietUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.White;
            ClearUncheckedDiets((Diet)checkBox.Content);
            Filter();
        }

        private void DietIndeterminate(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Red;
            ExcludeDiets((Diet)checkBox.Content);
            Filter();
        }

        private void ChangeAgeButton(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinAge.Text, out int minAge) && int.TryParse(MaxAge.Text, out int maxAge))
            {
                if (minAge >= 18 && minAge <= 200 && minAge <= maxAge)
                {
                    MinAge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
                    MinAge.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
                    minimumAge = minAge;
                }
                else
                {
                    MinAge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff6666");
                    MinAge.BorderBrush = Brushes.Red;
                }

                if (maxAge >= 18 && maxAge <= 200 && maxAge >= minAge)
                {
                    MaxAge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
                    MaxAge.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");
                    maximumAge = maxAge;
                }
                else
                {
                    MaxAge.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff6666");
                    MaxAge.BorderBrush = Brushes.Red; ;
                }
            }
            else
            {
                MinAge.Background = Brushes.PaleVioletRed;
                MaxAge.Background = Brushes.PaleVioletRed;
            }

            Filter();
        }

        //Save the filteroptions and show the matching profiles
        private void Filter()
        {
            List<string> results = repo.GetProfiles(MainWindow.profile, location, minimumAge, maximumAge, includedHobbies, excludedHobbies, includedDiets, excludedDiets, false, false);

            resultBox.ItemsSource = results;
        }

        private void ClearUncheckedHobbies(int item)
        {
            includedHobbies.Remove(item);
            excludedHobbies.Remove(item);
        }

        private void IncludeHobbys(int item)
        {
            includedHobbies.Add(item);
            excludedHobbies.Remove(item);
        }

        private void ExcludeHobbys(int item)
        {
            excludedHobbies.Add(item);
            includedHobbies.Remove(item);
        }

        private void ClearUncheckedDiets(Diet item)
        {
            includedDiets.Remove(item);
            excludedDiets.Remove(item);
        }

        private void IncludeDiets(Diet item)
        {
            includedDiets.Add(item);
            excludedDiets.Remove(item);
        }

        private void ExcludeDiets(Diet item)
        {
            excludedDiets.Add(item);
            includedDiets.Remove(item);
        }

        private void resultBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Get the currently selected item in the ListBox.
            if (resultBox.SelectedItem != null)
            {
                string curItem = resultBox.SelectedItem.ToString();
                profileDetails.SetProfile(curItem);
                profileDetails.Visibility = Visibility.Visible;
            }
        }
    }
}
