using System.Text.RegularExpressions;

namespace PrincessBrideTrivia;

public class Program
{
    public static void Main(string[] args)
    {
        string choice;
        do
        {
            Console.Write("Princess Bride Trivia!!! Do you want to try hard mode?(y/n): ");
            choice = Console.ReadLine()?.Trim().ToLower();
            if (choice != "y" && choice != "n")
            {
                Console.WriteLine("Please enter 'y' for yes or 'n' for no.");
            }
        } while (choice != "y" && choice != "n");

        bool isHardMode = choice == "y";
        if (isHardMode)
        {
            Console.WriteLine("HARD MODE ACTIVATED!!!!!!!!!!!!");
        }

        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);

        int numberCorrect = 0;
        for (int i = 0; i < questions.Length; i++)
        {
            
            bool result = AskQuestion(questions[i], isHardMode);
            
            if (result)
            {
                numberCorrect++;
            }
        }
        Console.WriteLine("You got " + GetPercentCorrect(numberCorrect, questions.Length) + " correct");
    }

    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        return (100 * numberCorrectAnswers / numberOfQuestions) + "%";
    }

    //UPDATED THIS FUNCTION WITH PARAMETER isHard
    public static bool AskQuestion(Question question, bool isHard)
    {
        DisplayQuestion(question, isHard);

        string userGuess = GetGuessFromUser();
        if (isHard)
        {
            return DisplayHardResult(userGuess, question);
        }
        return DisplayResult(userGuess, question);
    }

    public static string GetGuessFromUser()
    {
        return Console.ReadLine();
    }

    //WE CANT CHANGE THIS METHOD BECAUSE IT IS RUNNING TESTS
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

    // Helper method that strips punctuation from user input
    public static string RemovePunctuation(string input)
    {
        if (input == null) return string.Empty;
        return Regex.Replace(input, @"[^\w\s]", "");
    }

    public static bool DisplayHardResult(string userGuess, Question question)
    {
        if (userGuess == null)
        {
            Console.WriteLine("Incorrect");
            return false;
        }
        string normalizedGuess = RemovePunctuation(userGuess).Trim();
        if (!int.TryParse(question.CorrectAnswerIndex, out int index) || index < 1 || index > question.Answers.Length)
        {
            Console.WriteLine("Incorrect");
            return false;
        }
        string normalizedAnswer = RemovePunctuation(question.Answers[index - 1]).Trim();

        if (String.Equals(normalizedGuess, normalizedAnswer, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Correct");
            return true;
        }

        Console.WriteLine("Incorrect");
        return false;
    }

    //UPDATED THIS FUNCTION WITH PARAMETER isHard
    public static void DisplayQuestion(Question question, bool isHard)
    {
        Console.WriteLine("Question: " + question.Text);
        if (!isHard)
        {
            for (int i = 0; i < question.Answers.Length; i++)
            {
                Console.WriteLine((i + 1) + ": " + question.Answers[i]);
            }
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
            int lineIndex = i * 5;
            string questionText = lines[lineIndex];

            string answer1 = lines[lineIndex + 1];
            string answer2 = lines[lineIndex + 2];
            string answer3 = lines[lineIndex + 3];

            string correctAnswerIndex = lines[lineIndex + 4];

            Question question = new();
            question.Text = questionText;
            question.Answers = new string[3];
            question.Answers[0] = answer1;
            question.Answers[1] = answer2;
            question.Answers[2] = answer3;
            question.CorrectAnswerIndex = correctAnswerIndex;
            questions[i] = question;
        }
        return questions;
    }
}
