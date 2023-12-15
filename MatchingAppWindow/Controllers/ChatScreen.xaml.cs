using KBS_project;
using KBS_project.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for ChatBox.xaml
    /// </summary>
    public partial class ChatScreen : Page
    {
        ObservableCollection<Contact> Contacts { get; set; } = new();

        ObservableCollection<Message> Messages { get; set; } = new();

        Contact SelectedContact { get; set; }

        public ChatScreen()
        {
            InitializeComponent();

            DataContext = this;

            messageControl.ItemsSource = Messages;
        }

        public void InitializePage()
        {
            string userName = MainWindow.profile.UserName;
            List<string> contactNames = MainWindow.repo.GetContactNames(userName);

            foreach(string contactName in contactNames)
            {
                BitmapImage contactImage = ImageConverter.ImageDataToBitmap(MainWindow.repo.GetProfileImageData(contactName));
                Contact contact = new Contact(contactName, contactImage);

                Contacts.Add(contact);
            }

            contactList.ItemsSource = Contacts;
        }

        private void SelectContact(object sender, SelectionChangedEventArgs e)
        {
            if (contactList.SelectedItem == null) return;

            SelectedContact = Contacts[contactList.SelectedIndex];

            chatWindow.Visibility = Visibility.Visible;

            LoadMessages();
        }

        private void LoadMessages()
        {
            List<Message> MessageList = MainWindow.repo.GetMessages(MainWindow.profile.UserName, SelectedContact.UserName);

            Messages.Clear();

            foreach(Message message in  MessageList)
            {
                Messages.Add(message);
            }
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (messageBox.Text.Length == 0) return;
            Message message = new(DateTime.Now, messageBox.Text, true);
            MainWindow.repo.SendMessage(message, MainWindow.profile.UserName, SelectedContact.UserName);
            messageBox.Text = string.Empty;

            LoadMessages();
        }

        private void SendMessage(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage(sender, new RoutedEventArgs());
            }
        }

        private void UpdateSendButton(object sender, TextChangedEventArgs e)
        {
            if (messageBox.Text.Length == 0)
            {
                sendButton.IsEnabled = false;
            }
            else
            {
                sendButton.IsEnabled = true;
            }
        }

        private class Contact
        {
            public string UserName { get; set; }
            public BitmapImage ProfileImage { get; set; }

            public Contact(string userName, BitmapImage profileImage)
            {
                UserName = userName;
                ProfileImage = profileImage;
            }
        }
    }
}
