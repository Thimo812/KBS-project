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

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for FilterScreen.xaml
    /// </summary>
    public partial class FilterScreen : Page
    {

        //Creating all attributes
        MatchingAppRepository repo = new MatchingAppRepository();
        private LocationFilter location = LocationFilter.Global;
        private int minimumAge;
        private int maximumAge;
        private List<Interest> includedHobbies = new();
        private List<Interest> excludedHobbies = new();
        private List<Diet> includedDiets = new();
        private List<Diet> excludedDiets = new();
        private string? resultString;

        public FilterScreen()
        {
            InitializeComponent();

            try
            {
                //Showing all profiles from the database on the screen
                foreach (Profile profile in repo.GetProfiles())
                {
                    resultString += profile.UserName + "\n";
                    filteredProfiles.Content = resultString;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Er kon geen verbinding worden gemaakt met de database");
            }
        }

        //Button to extend or collapse the filteroptions
        private void ButtonExpandFilters_Click(object sender, RoutedEventArgs e)
        {
            if (filterPanel.Visibility == Visibility.Collapsed)
            {
                filterPanel.Visibility = Visibility.Visible;
                filterButton.Content = ">";
            }
            else
            {
                filterPanel.Visibility = Visibility.Collapsed;
                filterButton.Content = "v";
            }
        }

        private void IncOrExcHobby_Click(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider button = (Slider)sender;
            if (button.Value == 1)
            {
                ExcludeHobbys();
                includedHobbies.Clear();
                button.Background = Brushes.Red;
            }
            else if (button.Value == 0)
            {
                IncludeHobbys();
                excludedHobbies.Clear();
                button.Background = Brushes.Green;
            }

            Filter();
        }

        //Button to include or exclude diets
        private void IncOrExcDiet_Click(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider button = (Slider)sender;
            if (button.Value == 1)
            {
                ExcludeDiets();
                includedDiets.Clear();
                button.Background = Brushes.Red;
            }
            else if (button.Value == 0)
            {
                IncludeDiets();
                excludedDiets.Clear();
                button.Background = Brushes.Green;
            }

            Filter();
        }

        //RadioButtons to filter on location
        private void LocationChecked(object sender, RoutedEventArgs e)
        {
            RadioButton senderLoc = (RadioButton)sender;

            if (senderLoc.Name == "Global")
            {
                location = LocationFilter.Global;
            }
            if (senderLoc.Name == "Country")
            {
                location = LocationFilter.Country;
            }
            if (senderLoc.Name == "City")
            {
                location = LocationFilter.City;
            }

            Filter();
        }

        //CheckBoxes to filter on hobbies
        private void HobbyChecked(object sender, RoutedEventArgs e)
        {
            if (buttonHobby.Value == 0)
            {
                IncludeHobbys();
            }
            else if (buttonHobby.Value == 1)
            {
                ExcludeHobbys();
            }

            Filter();
        }

        //CheckBoxes to filter on diet
        private void DietChecked(object sender, RoutedEventArgs e)
        {
            if (buttonDiet.Value == 0)
            {
                IncludeDiets();
            }
            else if (buttonDiet.Value == 1)
            {
                ExcludeDiets();
            }

            Filter();
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void ChangeAgeButton(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinAge.Text, out int minAge) && int.TryParse(MaxAge.Text, out int maxAge))
            {
                if (minAge >= 18 && minAge <= 200 && minAge <= maxAge)
                {
                    MinAge.Background = Brushes.White;
                    minimumAge = minAge;
                }
                else
                {
                    MinAge.Background = Brushes.PaleVioletRed;
                }

                if (maxAge >= 18 && maxAge <= 200 && maxAge >= minAge)
                {
                    MaxAge.Background = Brushes.White;
                    maximumAge = maxAge;
                }
                else
                {
                    MaxAge.Background = Brushes.PaleVioletRed;
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
            ClearUncheckedAttributes();

            List<string> results = repo.GetProfiles(location, minimumAge, maximumAge, includedHobbies, excludedHobbies, includedDiets, excludedDiets);

            resultString = string.Empty;

            foreach (string result in results)
            {
                resultString += result + "\n";
            }

            filteredProfiles.Content = resultString;
        }

        private void ClearUncheckedAttributes()
        {
            if (Reading.IsChecked == false)
            {
                includedHobbies.Remove(Interest.Reading);
                excludedHobbies.Remove(Interest.Reading);
            }
            if (Cycling.IsChecked == false)
            {
                includedHobbies.Remove(Interest.Cycling);
                excludedHobbies.Remove(Interest.Cycling);
            }
            if (Cooking.IsChecked == false)
            {
                includedHobbies.Remove(Interest.Cooking);
                excludedHobbies.Remove(Interest.Cooking);
            }
            if (Vegetarian.IsChecked == false)
            {
                includedDiets.Remove(Diet.Vegetarian);
                excludedDiets.Remove(Diet.Vegetarian);
            }
            if (Vegan.IsChecked == false)
            {
                includedDiets.Remove(Diet.Vegan);
                includedDiets.Remove(Diet.Vegan);
            }
        }

        private void IncludeDiets()
        {
            if (Vegetarian.IsChecked == true && !includedDiets.Contains(Diet.Vegetarian))
            {
                includedDiets.Add(Diet.Vegetarian);
            }
            if (Vegan.IsChecked == true && !includedDiets.Contains(Diet.Vegan))
            {
                includedDiets.Add(Diet.Vegan);
            }
        }

        private void ExcludeDiets()
        {
            if (Vegetarian.IsChecked == true && !excludedDiets.Contains(Diet.Vegetarian))
            {
                excludedDiets.Add(Diet.Vegetarian);
            }
            if (Vegan.IsChecked == true && !excludedDiets.Contains(Diet.Vegan))
            {
                excludedDiets.Add(Diet.Vegan);
            }
        }

        private void IncludeHobbys()
        {
            if (Reading.IsChecked == true && !includedHobbies.Contains(Interest.Reading))
            {
                includedHobbies.Add(Interest.Reading);
            }
            if (Cycling.IsChecked == true && !includedHobbies.Contains(Interest.Cycling))
            {
                includedHobbies.Add(Interest.Cycling);
            }
            if (Cooking.IsChecked == true && !includedHobbies.Contains(Interest.Cooking))
            {
                includedHobbies.Add(Interest.Cooking);
            }
        }

        private void ExcludeHobbys()
        {
            if (Reading.IsChecked == true && !excludedHobbies.Contains(Interest.Reading))
            {
                excludedHobbies.Add(Interest.Reading);
            }
            if (Cycling.IsChecked == true && !excludedHobbies.Contains(Interest.Cycling))
            {
                excludedHobbies.Add(Interest.Cycling);
            }
            if (Cooking.IsChecked == true && !excludedHobbies.Contains(Interest.Cooking))
            {
                excludedHobbies.Add(Interest.Cooking);
            }
        }
    }
}
