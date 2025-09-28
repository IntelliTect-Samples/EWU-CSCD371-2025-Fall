using System.Diagnostics;

namespace PrincessBrideTrivia;

public class Program
{
    public static void Main(string[] args)
    {
        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);
        Console.WriteLine(questions.Length);
        int numberCorrect = 0;
        for (int i = 0; i < questions.Length; i++)
        {
            bool result = AskQuestion(questions[i]);
            if (result)
            {
                numberCorrect++;
            }
        }
        Console.WriteLine($"You got {GetPercentCorrect(numberCorrect, questions.Length)} correct");
    }

    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        if (numberOfQuestions == 0)
            return "N/A";

        double precent = (double)numberCorrectAnswers / numberOfQuestions * 100;

        return $"{precent:N0}%";
    }

    public static bool AskQuestion(Question question)
    {
        DisplayQuestion(question);

        string userGuess = GetGuessFromUser();
        return DisplayResult(userGuess, question);
    }

    public static string GetGuessFromUser()
    {
        return Console.ReadLine();
    }

    public static bool DisplayResult(string userGuess, Question question)
    {
        if (userGuess == question.CorrectAnswerIndex)
        {
            Console.WriteLine("Correct");
            return true;
        }

        Console.WriteLine("Incorrect");
        return false;
    }

    public static void DisplayQuestion(Question question)
    {
        Console.WriteLine($"Question: {question.Text}");
        for (int i = 0; i < question.Answers.Length; i++)
        {
            Console.WriteLine($"{(i + 1)} : { question.Answers[i]}");
        }
    }

    public static string GetFilePath()
    {
        return "Trivia.txt";
    }

    public static Question[] LoadQuestions(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        Question[] questions = new Question[lines.Length / 5];
        for (int i = 0; i < questions.Length; i++)
        {
            //These two variables could be moved into params to give more versatility in question setup
            int questionBlockSize = 5;

            // since we know that each question block contains both a question text and a correct answer, we can get the number
            // of questions expected by subtracting 2 from the question block size
            int numAnswerOptionsGiven = questionBlockSize - 2; 

            Question question = new Question();

            int lineIndex = i * questionBlockSize;
            string questionText = lines[lineIndex];

            question.Text = questionText;
            question.Answers = new string[numAnswerOptionsGiven];

            for (int j = 0; j < numAnswerOptionsGiven; j++)
            {
                question.Answers[j] = lines[lineIndex + j + 1]; // + 1 is included because index 0 of lines is the question text
            }

            question.CorrectAnswerIndex = lines[lineIndex + questionBlockSize - 1];
            questions[i] = question;

        }
        return questions;
    }
}
