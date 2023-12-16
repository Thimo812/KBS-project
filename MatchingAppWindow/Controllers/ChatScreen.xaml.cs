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
using MatchingApp.DataAccess.SQL;

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class ChatScreen : Page
    {
        public static MatchingAppRepository repo = new MatchingAppRepository();
        public ChatScreen()
        {
            InitializeComponent();

            UserList.ItemsSource = repo.GetProfiles();
            UserList.SelectedIndex = 0;
        }

        private void SendButtonFocus(object? sender, MouseEventArgs e)
        {
            SendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIconFocus.png", UriKind.Relative));
        }

        private void SendButtonFocusLost(object? sender, MouseEventArgs e)
        {
            SendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIcon.png", UriKind.Relative));
        }

        private void SendButtonClick(object? sender, MouseEventArgs e)
        {
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(10, 10, 0, 10)));

            Label textMessage= new();
            textMessage.Content = MessageField.Text;
            textMessage.BorderBrush = Brushes.PaleVioletRed;
            textMessage.BorderThickness = new(2);
            textMessage.Height = 30;
            textMessage.Width = 150;
            textMessage.Margin = new(20);
            textMessage.HorizontalAlignment = HorizontalAlignment.Right;
            textMessage.Resources.Add(typeof(Border), borderStyle);

            ChatHistoryStackPanel.Children.Add(textMessage);
        }

        private void UserListSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            CurrentChatUserName.Content = UserList.SelectedItem as string;
        }
    }
}
