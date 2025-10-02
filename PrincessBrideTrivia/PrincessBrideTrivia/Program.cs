namespace PrincessBrideTrivia;

public class Program
{
    public static async Task Main()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);

        int numberCorrect = 0;
        int numberOfQuestions = questions.Length;
        bool runWhile = true;

        for (int i = 0; i < questions.Length; i++)
        {
            (bool result, bool quitProgram) = AskQuestion(questions[i]);
            if (quitProgram)
            {
                runWhile = false;
                break;
            }
            if (result)
            {
                numberCorrect++;
            }
        }

        Console.WriteLine("Enter 'quit' to exit");

        while (runWhile)
        {
            try
            {
                Question q = await TriviaGenerator.GeneratePrincessBrideQuestionAsync(apiKey);

                (bool result, bool quitProgram) = AskQuestion(q);

                if (quitProgram)
                {
                    runWhile = false;
                }
                else
                {
                    numberOfQuestions++;
                    if (result)
                    {
                        numberCorrect++;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
        }

        Console.WriteLine($"You got {GetPercentCorrect(numberCorrect, numberOfQuestions)} correct");
    }

    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        double roundedPercent = Math.Round((float)numberCorrectAnswers / (float)numberOfQuestions * 100, 2);
        return $"{roundedPercent}%";
    }

    // 🔹 Updated AskQuestion version
    public static (bool answeredCorrectly, bool quitProgram) AskQuestion(Question question)
    {
        DisplayQuestion(question);
        string userGuess = GetGuessFromUser();

        if (userGuess == "quit")
        {
            return (false, true);
        }

        bool isCorrect = DisplayResult(userGuess, question);
        return (isCorrect, false);
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
            Console.WriteLine($"{i + 1}: {question.Answers[i]}");
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

            Question question = new() { Text = lines[lineIndex], Answers = new string[3], CorrectAnswerIndex = lines[lineIndex + 4] };
            question.Answers[0] = lines[lineIndex + 1];
            question.Answers[1] = lines[lineIndex + 2];
            question.Answers[2] = lines[lineIndex + 3];

            questions[i] = question;
        }

        return questions;
    }
}
