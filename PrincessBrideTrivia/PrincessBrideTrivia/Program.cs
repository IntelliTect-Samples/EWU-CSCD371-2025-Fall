using System.Diagnostics;
using System.Linq;

namespace PrincessBrideTrivia;

public class Program
{
    public enum ResponseValidation
    {
        Accept,
        Reject,
        Invalid
    }

    private const int QuestionBlockSize = 5;

    private const int AnswerOptionsGiven = QuestionBlockSize - 2;
    // since we know that each question block contains both a question text and a correct answer, we can get the number
    // of questions expected by subtracting 2 from the question block size

    public static void Main(string[] args)
    {
        const int maxAttempts = 3;

        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);
        int numberCorrect = 0;
        int[] attemptScores = new int[maxAttempts];
        for (int attemptIndex = 0; attemptIndex < maxAttempts; attemptIndex++)
        {
            foreach (Question question in questions)
            {
                bool result = AskQuestion(question);
                if (result)
                {
                    numberCorrect++;
                }
            }
            attemptScores[attemptIndex] = numberCorrect;
            Console.WriteLine($"You got {GetPercentCorrect(numberCorrect, questions.Length)} correct");
            numberCorrect = 0;
            if (attemptIndex != 2)
            {
                Console.WriteLine("Do you want to make another attempt?");
                Console.WriteLine("1: Yes");
                Console.WriteLine("2: No");
                ResponseValidation responseValidation = ResponseValidation.Invalid;
                while (responseValidation == ResponseValidation.Invalid) 
                { 
                    responseValidation = AcceptRetryQuiz(Console.ReadLine());
                }
                if (responseValidation == ResponseValidation.Reject)
                {
                    break;
                }
                else if(responseValidation == ResponseValidation.Accept)
                {
                    Random random = new Random();
                    questions = questions.OrderBy(x => random.Next()).ToArray();
                    foreach (Question question in questions)
                    {
                        question.RandomizeOrder(random.Next());
                    }
                }
            }
        }
        Console.WriteLine($"Your final score is {GetPercentCorrect(attemptScores.Max(), questions.Length)}");
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
        while(true)
        {
            if(int.TryParse(Console.ReadLine(), out int guess) && guess > 0 && guess <= AnswerOptionsGiven)
            {
                return guess.ToString();
            }
            Console.WriteLine($"Please enter a number between 1 and {AnswerOptionsGiven}.");
        }
    }

    public static ResponseValidation AcceptRetryQuiz(string userInput)
    {
        if (userInput == "1")
        {
            return ResponseValidation.Accept;
        }
        else if (userInput == "2")
        {
            return ResponseValidation.Reject;
        }
        else
        {
            return ResponseValidation.Invalid;
        }
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
            Question question = new Question();

            int lineIndex = i * QuestionBlockSize;
            string questionText = lines[lineIndex];

            question.Text = questionText;
            question.Answers = new string[AnswerOptionsGiven];

            for (int j = 0; j < AnswerOptionsGiven; j++)
            {
                question.Answers[j] = lines[lineIndex + j + 1]; // + 1 is included because index 0 of lines is the question text
            }

            question.CorrectAnswerIndex = lines[lineIndex + QuestionBlockSize - 1];
            questions[i] = question;

        }
        return questions;
    }
}
