using KBS_project;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.Management.Smo;
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
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Page
    {
        //     private readonly Login login = new Login();
        private readonly IMatchingAppRepository repository;
        private readonly LoginManager login;
        public event EventHandler loginSuccessful;

        public StartScreen()
        {
            InitializeComponent();
        }

        public StartScreen(IMatchingAppRepository repository)
        {
            InitializeComponent();

            this.repository = repository;
            this.login = new LoginManager(repository);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string username = text
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = userNameField.Text;

            if (login != null && login.ValidateLogin(enteredUsername))
            {
                MessageBox.Show("Login successful!");
                // Navigate to the next page or perform additional actions for a successful login
                loginSuccessful?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Invalid username. Please try again.");

            }
        }
    }
}
