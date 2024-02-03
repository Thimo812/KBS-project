using KBS_project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MatchingAppWindow.Views
{
    /// <summary>
    /// Interaction logic for MatchInfoScreen.xaml
    /// </summary>
    public partial class MatchInfoScreen : Window
    {
        List<string> questions = new List<string>()
        {
            { "Ben je meer een introvert of extrovert?"},
            { "Wil je dat je partner introverted of extroverted is?"},
            { "Hoe belangrijk is religie voor jou?" },
            { "Hoe belangrijk is carrière voor jou?" },
            { "Wat is jouw 'love language'?" },
            { "Wat wil je dat je partners 'love language' is?" },
            { "Hoe belangrijk is fysieke fitness voor jou?" },
            { "Wat denk je over kinderen famillie plannen?" },
            { "Ben je op zoek naar een serieuze relatie?" },
            { "Ben je een ochtend of avond mens?" },
            { "Heb je liever buitendoorse activiteiten, binnen activiteiten of allebij?" },
            { "Wat is je religie?" },
            { "Als je partner een huisdier heeft, hoe zou je dit vinden?" }
        };

        List<int> answersCurrentUser;
        List<int> answersSelectedUser;
        Brush[] AnswerDifference;

        public MatchInfoScreen(string currentUser, string selectedUser, Brush[] answerDifference)
        {
            InitializeComponent();

            answersCurrentUser = MainWindow.repo.GetMatchingQuiz(currentUser);
            answersSelectedUser = MainWindow.repo.GetMatchingQuiz(selectedUser);
            AnswerDifference = answerDifference;

            setQuestions();
            setAnswersCurrentUser();
            setAnswersSelectedUser();

            DataContext = selectedUser;
        }

        private void setQuestions()
        {
            int i = 1;
            foreach (string question in questions)
            {
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();

                label.FontSize = 16;
                label.Content = question;
                label.Foreground = new SolidColorBrush(Colors.White);
                label.Visibility = Visibility.Visible;
                label.Margin = new Thickness(3);

                if (AnswerDifference[i-1] != null)
                {
                    SolidColorBrush originalBrush = (SolidColorBrush)AnswerDifference[i - 1];
                    SolidColorBrush answerColor = new SolidColorBrush(originalBrush.Color);
                    answerColor.Opacity = 0.3;
                    label.Background = answerColor;
                }

                Grid.SetColumn(label, 0);
                Grid.SetRow(label, i);

                table.Children.Add(label);

                Rectangle rectangle = new Rectangle();

                rectangle.Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#434E5B");

                Grid.SetColumnSpan(rectangle, 3);
                Grid.SetRow(rectangle, i);

                table.Children.Add(rectangle);

                i++;
            }
            MessageBox.Show($"{AnswerDifference.Length}");
        }

        private void setAnswersCurrentUser()
        {
            int i = 1;
            foreach (int answer in answersCurrentUser)
            {
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();

                label.FontSize = 14;
                label.Content = FormatEnumValue(GetEnumValueByIndex(answer, i));
                label.Foreground = new SolidColorBrush(Colors.White);
                label.Visibility = Visibility.Visible;
                label.Margin = new Thickness(3);

                Grid.SetColumn(label, 1);
                Grid.SetRow(label, i);

                table.Children.Add(label);

                i++;
            }
        }

        private void setAnswersSelectedUser()
        {
            int i = 1;
            foreach (int answer in answersSelectedUser)
            {
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();

                label.FontSize = 14;
                label.Content = FormatEnumValue(GetEnumValueByIndex(answer, i));
                label.Foreground = new SolidColorBrush(Colors.White);
                label.Visibility = Visibility.Visible;
                label.Margin = new Thickness(3);

                Grid.SetColumn(label, 2);
                Grid.SetRow(label, i);

                table.Children.Add(label);

                i++;
            }
        }

        private string FormatEnumValue(string enumValue)
        {
            return enumValue.Replace("_", " ");
        }

        private string GetEnumValueByIndex(int answer, int questionIndex)
        {
            switch (questionIndex)
            {
                case 1:
                case 2:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion1_2), answer);
                case 3:
                case 4:
                case 7:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion3_4_7), answer);
                case 5:
                case 6:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion5_6), answer);
                case 8:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion8), answer);
                case 9:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion9), answer);
                case 10:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion10), answer);
                case 11:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion11), answer);
                case 12:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion12), answer);
                case 13:
                    return Enum.GetName(typeof(MatchingquizAnswers.AnswerQuestion13), answer);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
