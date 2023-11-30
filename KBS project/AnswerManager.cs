using System.Collections.Generic;

public class AnswerManager
{
    private readonly Dictionary<string, string> selectedAnswers = new Dictionary<string, string>();

    public void StoreAnswer(string question, string answer)
    {
        selectedAnswers[question] = answer;
    }

    public string GetAnswer(string question)
    {
        return selectedAnswers.TryGetValue(question, out var answer) ? answer : null;
    }
}