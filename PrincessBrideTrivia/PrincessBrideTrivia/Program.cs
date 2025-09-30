namespace PrincessBrideTrivia;

public class Program
{
    public static void Main(string[] args)
    {
        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);

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



    public static bool DisplayResult(string userGuess, Question question)
{
    // Preserve old API for tests; parse the string and forward to the int version
    if (!int.TryParse((userGuess ?? string.Empty).Trim(), out int guess1Based))
    {
        Console.WriteLine("Please enter 1, 2, or 3:");
        return false;
    }
    return DisplayResult(guess1Based, question);
}


    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        if (numberOfQuestions <= 0) return "0%";
        // promote to double to avoid integer division, format as whole percent
        double pct = (double)numberCorrectAnswers / numberOfQuestions * 100.0;
        return $"{Math.Round(pct)}%";
    }

    public static bool AskQuestion(Question question)
    {
        DisplayQuestion(question);

        int userGuessIndex = GetGuessIndexFromUser();
        return DisplayResult(userGuessIndex, question);
    }

    public static string GetGuessFromUser()
    {
        // keep this method for compatibility, but trim the input
        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    private static int GetGuessIndexFromUser()
    {
        // read, parse as 1-based integer; keep asking until valid
        while (true)
        {
            string raw = GetGuessFromUser();
            if (int.TryParse(raw, out int guess) && guess is >= 1 and <= 3)
            {
                return guess; // 1-based
            }
            Console.WriteLine("Please enter 1, 2, or 3:");
        }
    }

    public static bool DisplayResult(int userGuessIndex1Based, Question question)
    {
        // CorrectAnswerIndex is coming from file; normalize/trim and parse
        if (!int.TryParse(question.CorrectAnswerIndex?.Trim(), out int correct1Based))
        {
            Console.WriteLine("Question data invalid.");
            return false;
        }

        if (userGuessIndex1Based == correct1Based)
        {
            Console.WriteLine("Correct");
            return true;
        }

        Console.WriteLine("Incorrect");
        return false;
    }

    public static void DisplayQuestion(Question question)
    {
        Console.WriteLine("Question: " + question.Text);
        for (int i = 0; i < question.Answers.Length; i++)
        {
            Console.WriteLine($"{i + 1}: {question.Answers[i]}");
        }
    }

    public static string GetFilePath()
    {
        // keep as-is; tests likely expect this exact name in the working directory
        return "Trivia.txt";
    }

    public static Question[] LoadQuestions(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        // Each question block is 5 lines: Q, A1, A2, A3, CorrectIndex
        int block = 5;
        if (lines.Length % block != 0)
            throw new InvalidOperationException("Trivia file is malformed.");

        Question[] questions = new Question[lines.Length / block];
        for (int i = 0; i < questions.Length; i++)
        {
            int lineIndex = i * block;

            string questionText = lines[lineIndex].Trim();

            string answer1 = lines[lineIndex + 1].Trim();
            string answer2 = lines[lineIndex + 2].Trim();
            string answer3 = lines[lineIndex + 3].Trim();

            string correctAnswerIndex = lines[lineIndex + 4].Trim();

            Question question = new()
            {
                Text = questionText,
                Answers = new[] { answer1, answer2, answer3 },
                // store the raw 1-based index string; we parse later
                CorrectAnswerIndex = correctAnswerIndex
            };

            // ✅ this was missing:
            questions[i] = question;
        }



        

        return questions;
    }
}
