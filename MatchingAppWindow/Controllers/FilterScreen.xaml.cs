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
        private string resultString;

        private ProfileDetails profileDetails = new();

        public FilterScreen()
        {
            InitializeComponent();

            profileDetailsFrame.Content = profileDetails;

            DataContext = this;

            try
            {
                //Showing all profiles from the database on the screen
                foreach (Profile profile in repo.GetProfiles())
                {
                    resultString += profile.UserName + "\n";
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

        //Button to include or exclude hobbies
        private void IncOrExcHobby_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if ((string)button.Content == "wel")
            {
                excludedHobbies.Clear();
                button.Background = Brushes.Red;
                button.Content = "niet";
            }
            else if((string)button.Content == "niet")
            {
                includedHobbies.Clear();
                button.Background = Brushes.Green;
                button.Content = "wel";
            }
        }

        //Button to include or exclude diets
        private void IncOrExcDiet_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if ((string)button.Content == "wel")
            {
                excludedDiets.Clear();
                button.Background = Brushes.Red;
                button.Content = "niet";
            }
            else if ((string)button.Content == "niet")
            {
                includedDiets.Clear();
                button.Background = Brushes.Green;
                button.Content = "wel";
            }
        }

        //CheckBoxes to filter on location
        private void LocationChecked(object sender, RoutedEventArgs e)
        {
            CheckBox senderLoc = (CheckBox)sender;

            if(senderLoc.Name == "Global")
            {
                location = LocationFilter.Global;
            }
            if(senderLoc.Name == "Country")
            {
                location = LocationFilter.Country;
            }
            if(senderLoc.Name == "City")
            {
                location = LocationFilter.City;
            }
        }

        //CheckBoxes to filter on hobbies
        private void HobbyChecked(object sender, RoutedEventArgs e)
        {
            CheckBox senderHobby = (CheckBox)sender;
            if ((string)buttonHobby.Content == "wel")
            {
                if (senderHobby.Name == "Reading")
                {
                    includedHobbies.Add(Interest.Reading);
                }
                if (senderHobby.Name == "Cycling")
                {
                    includedHobbies.Add(Interest.Cycling);
                }
                if (senderHobby.Name == "Cooking")
                {
                    includedHobbies.Add(Interest.Cooking);
                }
            }
            else if((string)buttonHobby.Content == "niet")
            {
                if (senderHobby.Name == "Reading")
                {
                    excludedHobbies.Add(Interest.Reading);
                }
                if (senderHobby.Name == "Cycling")
                {
                    excludedHobbies.Add(Interest.Cycling);
                }
                if (senderHobby.Name == "Cooking")
                {
                    excludedHobbies.Add(Interest.Cooking);
                }
            }
        }

        //CheckBoxes to filter on diet
        private void DietChecked(object sender, RoutedEventArgs e)
        {
            CheckBox senderDiet = (CheckBox)sender;

            if((string)buttonDiet.Content == "wel")
            {
                if (senderDiet.Name == "Vegetarian")
                {
                    includedDiets.Add(Diet.Vegetarian);
                }
                if (senderDiet.Name == "Vegan")
                {
                    includedDiets.Add(Diet.Vegan);
                }
            }
            else if ((string)buttonDiet.Content == "niet")
            {
                if (senderDiet.Name == "Vegetarian")
                {
                    excludedDiets.Add(Diet.Vegetarian);
                }
                if (senderDiet.Name == "Vegan")
                {
                    excludedDiets.Add(Diet.Vegan);
                }
            }
        }

        

        //Button to save the filteroptions and show the matching profiles
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(MinAge.Text, out minimumAge);
            int.TryParse(MaxAge.Text, out maximumAge);

            List<string> results = repo.GetProfiles(location, minimumAge, maximumAge, includedHobbies, excludedHobbies, includedDiets, excludedDiets);

            resultString = string.Empty;

            foreach(string result in results)
            {
                resultString += result + "\n";
            }

            resultBox.ItemsSource = results;

            location = 0;
            minimumAge = 18;
            maximumAge = 200;
            includedHobbies.Clear();
            excludedHobbies.Clear();
            includedDiets.Clear();
            excludedDiets.Clear();
        }
    }
}