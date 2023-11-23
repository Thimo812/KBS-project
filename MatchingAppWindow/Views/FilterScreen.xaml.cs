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

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for FilterScreen.xaml
    /// </summary>
    public partial class FilterScreen : Page
    {
        MatchingAppRepository repo = new MatchingAppRepository();

        private LocationFilter location;
        private int minimumAge;
        private int maximumAge;
        private List<Interest> includedHobbies = new();
        private List<Interest> excludedHobbies = new();
        private List<Diet> includedDiets = new();
        private List<Diet> excludedDiets = new();

        public FilterScreen()
        {
            InitializeComponent();
        }

        private void buttonExtendFilters_Click(object sender, RoutedEventArgs e)
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

        private void IncOrExcButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if ((string)button.Content == "wel")
            {
                button.Background = Brushes.Red;
                button.Content = "niet";
            }
            else if((string)button.Content == "niet")
            {
                button.Background = Brushes.Green;
                button.Content = "wel";
            }
        }

        private void LocationChecked(object sender, RoutedEventArgs e)
        {
            RadioButton senderLoc = (RadioButton)sender;

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

        private void HobbyChecked(object sender, RoutedEventArgs e)
        {
            CheckBox senderHobby = (CheckBox)sender;
            if(buttonHobby.Content == "wel")
            {
                if (senderHobby.Name == "Soccer")
                {
                    includedHobbies.Add(Interest.Soccer);
                }
                if (senderHobby.Name == "Gaming")
                {
                    includedHobbies.Add(Interest.Gaming);
                }
                if (senderHobby.Name == "Art")
                {
                    includedHobbies.Add(Interest.Art);
                }
            }
            else if(buttonHobby.Content == "niet")
            {
                if (senderHobby.Name == "Soccer")
                {
                    excludedHobbies.Add(Interest.Soccer);
                }
                if (senderHobby.Name == "Gaming")
                {
                    excludedHobbies.Add(Interest.Gaming);
                }
                if (senderHobby.Name == "Art")
                {
                    excludedHobbies.Add(Interest.Art);
                }
            }
        }

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

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(MinAge.Text, out minimumAge);
            int.TryParse(MaxAge.Text, out maximumAge);

            repo.GetProfiles(location, minimumAge, maximumAge, includedHobbies, excludedHobbies, includedDiets, excludedDiets);
        }
    }
}
