using KBS_project;
using KBS_project.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for ChatBox.xaml
    /// </summary>
    public partial class ChatScreen : Page
    {

        public static int chatRefreshDelay = 100;
        private ObservableCollection<Contact> Contacts { get; set; } = new();

        private ObservableCollection<Message> Messages { get; set; } = new();

        private Contact SelectedContact { get; set; }

        public BackgroundWorker MessageChecker { get; private set; }

        private int maxMessageBoxWidth = 400;

        public ChatScreen()
        {
            InitializeComponent();

            DataContext = this;

            messageControl.ItemsSource = Messages;
            contactList.ItemsSource = Contacts;

            Loaded += StartChecking;
            Unloaded += StopChecking;

            UpdateSendButton(this, null);
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
        }

        private void SelectContact(object sender, SelectionChangedEventArgs e)
        {
            if (contactList.SelectedItem == null) return;

            SelectedContact = Contacts[contactList.SelectedIndex];

            chatWindow.Visibility = Visibility.Visible;
        }

        private void CheckMessages(object sender, DoWorkEventArgs e)
        {

            while (true)
            {
                if (SelectedContact == null) continue;

                DateTime localLatestMessage = Messages.Count == 0 ? DateTime.MinValue : Messages[Messages.Count - 1].TimeStamp;
                DateTime? LatestMessage = MainWindow.repo.GetLatestTimeStamp(MainWindow.profile.UserName, SelectedContact.UserName);

                if (((DateTime)LatestMessage).CompareTo(localLatestMessage) != 0) MessageChecker.ReportProgress(0, GetMessages());

                Thread.Sleep(chatRefreshDelay);
            }
        }

        private void UpdateMessages(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is List<Message> messages)
            {
                Messages.Clear();

                foreach (Message message in messages)
                {
                    Messages.Add(message);
                }
            }
        }

        public void StopChecking(object sender, RoutedEventArgs e)
        {
            if (MessageChecker.IsBusy) MessageChecker.CancelAsync();
        }

        private void StartChecking(object sender, RoutedEventArgs e)
        {
            MessageChecker = new();
            MessageChecker.DoWork += CheckMessages;
            MessageChecker.ProgressChanged += UpdateMessages;
            MessageChecker.WorkerSupportsCancellation = true;
            MessageChecker.WorkerReportsProgress = true;

            MessageChecker.RunWorkerAsync();
        }

        private List<Message> GetMessages()
        {
            return MainWindow.repo.GetMessages(MainWindow.profile.UserName, SelectedContact.UserName);
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (messageBox.Text.Length == 0) return;
            Message message = new(DateTime.UtcNow, messageBox.Text, true);
            MainWindow.repo.SendMessage(message, MainWindow.profile.UserName, SelectedContact.UserName);
            messageBox.Text = string.Empty;
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
                sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIconUnabled.png", UriKind.Relative));
            }
            else
            {
                sendButton.IsEnabled = true;
                sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIcon.png", UriKind.Relative));
            }
        }

        private void SendButtonFocus(object? sender, MouseEventArgs e)
        {
            sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIconFocus.png", UriKind.Relative));
        }

        private void SendButtonFocusLost(object? sender, MouseEventArgs e)
        {
            sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIcon.png", UriKind.Relative));
        }
    }
}
