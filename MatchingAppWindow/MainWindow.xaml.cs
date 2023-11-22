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
using MatchingAppWindow;

namespace MatchingAppWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(filterPanel.Visibility == Visibility.Collapsed)
            {
                filterPanel.Visibility = Visibility.Visible;
            }
            else
            {
                filterPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if((string)button.Content == "wel")
            {
                button.Background = Brushes.Red;
                button.Content = "niet";
            }
            else
            {
                button.Background = Brushes.Green;
                button.Content = "wel";
            }
        }
    }
}
