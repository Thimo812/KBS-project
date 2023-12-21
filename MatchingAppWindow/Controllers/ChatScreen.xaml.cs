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

        private ProfileDetails ProfileDetails { get; set; } = new();

        private bool AutoScroll { get; set; } = true;

        public BackgroundWorker MessageChecker { get; private set; }

        public ChatScreen()
        {
            InitializeComponent();

            string userName = MainWindow.profile.UserName;
            List<string> contactNames = MainWindow.repo.GetContactNames(userName);

            Contacts.CollectionChanged += (sender, e) =>
            {
                noChatsLabel1.Visibility = Visibility.Hidden;
                noChatsLabel2.Visibility = Visibility.Hidden;
            };

            foreach (string contactName in contactNames)
            {
                BitmapImage contactImage = ImageConverter.ImageDataToBitmap(MainWindow.repo.GetProfileImageData(contactName));
                Contact contact = new Contact(contactName, contactImage);

                Contacts.Add(contact);
            }

            DataContext = this;

            messageControl.ItemsSource = Messages;
            contactList.ItemsSource = Contacts;
            detailFrame.Content = ProfileDetails;

            Loaded += StartChecking;
            Unloaded += StopChecking;

            sendButton.IsEnabledChanged += UpdateSendButtonImage;
        }

        private void SelectContact(object sender, SelectionChangedEventArgs e)
        {
            if (contactList.SelectedItem == null) return;

            SelectedContact = Contacts[contactList.SelectedIndex];

            ProfileDetails.SetProfile(SelectedContact.UserName);

            chatWindow.Visibility = Visibility.Visible;
            ProfileDetails.Visibility = Visibility.Visible;
        }

        private void CheckMessages(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (SelectedContact == null || MainWindow.profile == null) continue;

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

                Contacts = new(Contacts.OrderByDescending(x => MainWindow.repo.GetLatestTimeStamp(MainWindow.profile.UserName, x.UserName)));
                contactList.ItemsSource = Contacts;
            }
        }

        public void StopChecking(object sender, RoutedEventArgs e)
        {
            if (MessageChecker != null)
            {
                if (MessageChecker.IsBusy) MessageChecker.CancelAsync();
            }
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

        private void UpdateSendButtonImage(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sendButton.IsEnabled == false) sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIconUnabled.png", UriKind.Relative));

            else sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIcon.png", UriKind.Relative));
        }

        private void UpdateSendButton(object sender, TextChangedEventArgs e)
        {
            if (messageBox.Text.Length == 0) sendButton.IsEnabled = false;

            else sendButton.IsEnabled = true;
        }

        private void UpdateScrollViewer(Object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                if (messageScrollViewer.VerticalOffset == messageScrollViewer.ScrollableHeight)
                {
                    AutoScroll = true;
                }
                else
                {
                    AutoScroll = false;
                }
            }

            if (AutoScroll && e.ExtentHeightChange != 0)
            {
                messageScrollViewer.ScrollToVerticalOffset(messageScrollViewer.ExtentHeight);
            }
        }

        private void SendButtonFocus(object? sender, MouseEventArgs e)
        {
            if (messageBox.Text.Length == 0) return;
            sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIconFocus.png", UriKind.Relative));
        }

        private void SendButtonFocusLost(object? sender, MouseEventArgs e)
        {
            if (messageBox.Text.Length == 0) return;
            sendButton.Source = new BitmapImage(new Uri("/Views/SendMessageIcon.png", UriKind.Relative));
        }
    }
}
