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
            catch (SqlException)
            {
                MessageBox.Show("Er kon geen verbinding worden gemaakt met de database");
            }
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

        private void HobbyClicked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Green;
            IncludeHobbys(checkBox.Name);
            Filter();
        }

        private void HobbyUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.White;
            ClearUncheckedAttributes(checkBox.Name);
            Filter();
        }

        private void HobbyIndeterminate(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Red;
            ExcludeHobbys(checkBox.Name);
            Filter();
        }

        private void DietChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Green;
            IncludeDiets(checkBox.Name);
            Filter();
        }

        private void DietUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.White;
            ClearUncheckedAttributes(checkBox.Name);
            Filter();
        }

        private void DietIndeterminate(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Background = Brushes.Red;
            ExcludeDiets(checkBox.Name);
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
            //ClearUncheckedAttributes();

            List<string> results = repo.GetProfiles(location, minimumAge, maximumAge, includedHobbies, excludedHobbies, includedDiets, excludedDiets);

            resultString = string.Empty;

            foreach (string result in results)
            {
                resultString += result + "\n";
            }

            filteredProfiles.Content = resultString;
        }

        private void ClearUncheckedAttributes(string item)
        {
            if (item == "Reading")
            {
                includedHobbies.Remove(Interest.Reading);
                excludedHobbies.Remove(Interest.Reading);
            }
            if (item == "Cycling")
            {
                includedHobbies.Remove(Interest.Cycling);
                excludedHobbies.Remove(Interest.Cycling);
            }
            if (item == "Cooking")
            {
                includedHobbies.Remove(Interest.Cooking);
                excludedHobbies.Remove(Interest.Cooking);
            }
            if (item == "Vegetarian")
            {
                includedDiets.Remove(Diet.Vegetarian);
                excludedDiets.Remove(Diet.Vegetarian);
            }
            if (item == "Vegan")
            {
                includedDiets.Remove(Diet.Vegan);
                includedDiets.Remove(Diet.Vegan);
            }
        }

        private void IncludeDiets(string item)
        {
            if (item == "Vegetarian" && !includedDiets.Contains(Diet.Vegetarian))
            {
                includedDiets.Add(Diet.Vegetarian);
                excludedDiets.Remove(Diet.Vegetarian);
            }
            if (item == "Vegan" && !includedDiets.Contains(Diet.Vegan))
            {
                includedDiets.Add(Diet.Vegan);
                excludedDiets.Remove(Diet.Vegan);
            }
        }

        private void ExcludeDiets(string item)
        {
            if (item == "Vegetarian" && !excludedDiets.Contains(Diet.Vegetarian))
            {
                excludedDiets.Add(Diet.Vegetarian);
                includedDiets.Remove(Diet.Vegetarian);
            }
            if (item == "Vegan" && !excludedDiets.Contains(Diet.Vegetarian))
            {
                excludedDiets.Add(Diet.Vegan);
                includedDiets.Remove(Diet.Vegan);
            }
        }

        private void IncludeHobbys(string item)
        {
            if (item == "Reading" && !includedHobbies.Contains(Interest.Reading))
            {
                includedHobbies.Add(Interest.Reading);
                excludedHobbies.Remove(Interest.Reading);
            }
            if (item == "Cycling" && !includedHobbies.Contains(Interest.Cycling))
            {
                includedHobbies.Add(Interest.Cycling);
                excludedHobbies.Remove(Interest.Cycling);
            }
            if (item == "Cooking" && !includedHobbies.Contains(Interest.Cooking))
            {
                includedHobbies.Add(Interest.Cooking);
                excludedHobbies.Remove(Interest.Cooking);
            }
        }

        private void ExcludeHobbys(string item)
        {
            if (item == "Reading" && !excludedHobbies.Contains(Interest.Reading))
            {
                excludedHobbies.Add(Interest.Reading);
                includedHobbies.Remove(Interest.Reading);
            }
            if (item == "Cycling" && !excludedHobbies.Contains(Interest.Cycling))
            {
                excludedHobbies.Add(Interest.Cycling);
                includedHobbies.Remove(Interest.Cycling);
            }
            if (item == "Cooking" && !excludedHobbies.Contains(Interest.Cooking))
            {
                excludedHobbies.Add(Interest.Cooking);
                includedHobbies.Remove(Interest.Cooking);
            }
        }
    }
}
