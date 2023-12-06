using KBS_project;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MatchingAppWindow
{
    public partial class Matchingquiz : Page
    {
        public event EventHandler<EventArgs> ExitPage;

        private readonly AnswerManager answerManager = new AnswerManager();
        IMatchingAppRepository Repo {  get; }

        public Matchingquiz(IMatchingAppRepository repo)
        {
            Repo = repo;
            InitializeComponent();
            WireUpRadioButtons();
            var radioButtons = FindRadioButtons(this);
            Console.WriteLine($"Found {radioButtons.Count} RadioButtons.");
        }
        private void WireUpRadioButtons()
        {
            foreach (var radioButton in FindSameElementsUI<RadioButton>(this))
            {
                radioButton.Checked += RadioButton_Checked;
            }
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            List<RadioButton> radioButtons = FindSameElementsUI<RadioButton>(this);
            
            if (sender is RadioButton radioButton && radioButton.IsChecked == true)
            {
                string question = GetQuestion(radioButton);
                answerManager.StoreAnswer(question, radioButton.Content.ToString());
            }
        }



        public string GetSelectedAnswer(string question)
        {
            return answerManager.GetAnswer(question);
        }

        private string GetQuestion(RadioButton radioButton)
        {
            Dictionary<string, string> questions = new Dictionary<string, string>()
            {
                { "Question1", "Ben je meer een introvert of extrovert?"},
                { "Question2", "Wil je dat je partner introverted of extroverted is?"},
                { "Question3", "Hoe belangrijk is religie voor jou?" },
                { "Question4", "Hoe belangrijk is carrière voor jou?" },
                { "Question5", "Wat is jouw 'love language'?" },
                { "Question6", "Wat wil je dat je partners 'love language' is?" },
                { "Question7", "Hoe belangrijk is fysieke fitness voor jou?" },
                { "Question8", "Wat denk je over kinderen famillie plannen?" },
                { "Question9", "Ben je op zoek naar een serieuze relatie?" },
                { "Question10", "Ben je een ochtend of avond mens?" },
                { "Question11", "Heb je liever buitendoorse activiteiten, binnen activiteiten of allebij?" },
                { "Question12", "Wat is je religie?" },
                { "Question13", "Als je partner een huisdier heeft, hoe zou je dit vinden?" }
            };

            return questions[radioButton.GroupName];
        }

        private List<RadioButton> FindRadioButtons(DependencyObject parent)
        {
            var radioButtonsList = new List<RadioButton>();

            // Recursive method to traverse the visual tree
            void TraverseVisualTree(DependencyObject element)
            {
                int count = VisualTreeHelper.GetChildrenCount(element);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);

                    if (child is RadioButton radioButton)
                    {
                        // Print GroupName for debugging
                        var groupName = (string)radioButton.GetValue(RadioButton.GroupNameProperty);
                        Console.WriteLine($"RadioButton: Content = {radioButton.Content}, GroupName = {groupName}");

                        radioButtonsList.Add(radioButton);
                    }
                    else
                    {
                        // If the child is not a RadioButton, recursively traverse its children
                        TraverseVisualTree(child);
                    }
                }
            }

            TraverseVisualTree(parent);

            return radioButtonsList;
        }





        private void SaveAnswers_Click(object sender, RoutedEventArgs e)
        {

            var savedAnswers = FindRadioButtons(this)
                  .Where(radioButton => radioButton.IsChecked == true);


            var unansweredQuestions = FindRadioButtons(this)
                 .Where(radioButton => radioButton.IsChecked == false)
                 .Select(x => GetQuestion(x))
                 .Distinct()
                 .Except(savedAnswers.Select(x => GetQuestion(x)))
                 .ToList();

            if (unansweredQuestions.Count > 0)
            {
                MessageBox.Show($"Please answer the following questions:\n\n{string.Join("\n", unansweredQuestions)}");
            }
            else
            {
                var answerList = savedAnswers.Select(x => GetButtonIndex(x)).ToList();

                MainWindow.profile.QuizAnswers = answerList;
                Repo.SaveMatchingQuiz(answerList);
            }

            ExitPage?.Invoke(this, EventArgs.Empty);
        }
    



        private bool IsAnyRadioButtonChecked(GroupBox groupBox)
        {
            // Check if any RadioButton in the specified GroupBox is checked
            var radioButtons = FindRadioButtons(groupBox);

            // Filter radio buttons that belong to the same group as the first one
            var firstRadioButton = radioButtons.FirstOrDefault();
            if (firstRadioButton != null)
            {
                var groupName = (string)firstRadioButton.GetValue(RadioButton.GroupNameProperty);
                radioButtons = (List<RadioButton>)radioButtons.Where(rb => (string)rb.GetValue(RadioButton.GroupNameProperty) == groupName);
            }

            return radioButtons.Any(radioButton => radioButton.IsChecked == true);
        }


    

        public static List<T> FindSameElementsUI<T>(UIElement parent, int depth = 0) where T : UIElement
        {
            var sameElementsUI = new List<T>();

            if (parent is Panel panel)
            {
                sameElementsUI.AddRange(panel.Children.OfType<T>());

                foreach (var sameElement in panel.Children.OfType<FrameworkElement>())
                {
                    Console.WriteLine($"{new string(' ', depth * 2)}Encountered: {sameElement.GetType().Name}");
                    sameElementsUI.AddRange(FindSameElementsUI<T>(sameElement, depth + 1));
                }
            }

            return sameElementsUI;
        }

        public int GetButtonIndex(RadioButton button)
        {
            if (button.Parent is StackPanel stackPanel)
            {
                return stackPanel.Children.IndexOf(button);
            }
            throw new IndexOutOfRangeException();
        }


    }
}