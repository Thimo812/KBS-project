using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MatchingAppWindow
{
    public partial class Matchingquiz : Page
    {
        private readonly AnswerManager answerManager = new AnswerManager();

        public Matchingquiz()
        {
            InitializeComponent();
            WireUpRadioButtons();
        }

        private void WireUpRadioButtons()
        {
            foreach (var radioButton in FindVisualChildren<RadioButton>(this))
            {
                radioButton.Checked += RadioButton_Checked;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
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
            if (radioButton.Parent is StackPanel stackPanel && stackPanel.Children.Count > 0 &&
                stackPanel.Children[0] is TextBlock textBlock)
            {
                return textBlock.Text;
            }
            return string.Empty;
        }

        private IEnumerable<RadioButton> FindRadioButtons(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is RadioButton radioButton)
                {
                    yield return radioButton;
                }

                foreach (var nestedRadioButton in FindRadioButtons(child))
                {
                    yield return nestedRadioButton;
                }
            }
        }

        private void SaveAnswers_Click(object sender, RoutedEventArgs e)
        {
            var savedAnswers = FindRadioButtons(this)
                .Where(radioButton => radioButton.IsChecked == true)
                .Select(radioButton => $"{GetQuestion(radioButton)}: {radioButton.Content}");

            MessageBox.Show($"Saved Answers:\n\n{string.Join("\n", savedAnswers)}");
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T result)
                    {
                        yield return result;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}