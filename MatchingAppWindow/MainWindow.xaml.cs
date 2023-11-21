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

        bool filterVisible = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!filterVisible)
            {
                filterPanel.Visibility = Visibility.Visible;
                filterVisible = true;
            }
            else
            {
                filterPanel.Visibility = Visibility.Collapsed;
                filterVisible = false;
            }
        }
    }
}
