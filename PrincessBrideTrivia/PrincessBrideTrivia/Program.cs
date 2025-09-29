namespace PrincessBrideTrivia;

public class Program
{
    public static void Main(string[] args)
    {
        const string TriviaQuestionsFilePath = "Trivia.txt";
        
        try
        {
            EnsureFileExists(TriviaQuestionsFilePath);
        }
        catch (FileNotFoundException e)
        {
            Console.Error.WriteLine($"Critical Error: {e.Message}");
            Environment.Exit(1);
        }

        Question[] questions = null;
        try
        {
            questions = ParseQuestionsFromFile(TriviaQuestionsFilePath);
        }
        catch (InvalidDataException e)
        {
            Console.Error.WriteLine($"Critical Error: {e.Message}");
            Environment.Exit(1);
        }

        int numberCorrect = 0;
        foreach (Question question in questions)
        {
            if (AskQuestion(question)) numberCorrect++;
            
        }
        Console.WriteLine("You got " + GetPercentCorrect(numberCorrect, questions.Length) + " correct");
    }

    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        if (numberOfQuestions == 0) return "N/A";
        double percent = (double)numberCorrectAnswers / numberOfQuestions * 100;
        return Math.Round(percent) + "%";
    }

    public static bool AskQuestion(Question question)
    {
        DisplayQuestion(question);

        int userGuess = GetGuessFromUser();
        return DisplayResult(userGuess, question);
    }

    public static int GetGuessFromUser()
    {
        while (true)
        {
            Console.Write("Your answer: ");
            string input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out int guess) && guess >= 1)
                return guess - 1;

            Console.WriteLine("Invalid input. Enter the number for your guess: ");
        }
    }

    public static bool DisplayResult(int userGuess, Question question)
    {
        bool isCorrect = userGuess == question.CorrectAnswerIndex;
        Console.WriteLine(isCorrect ? "Correct" : "Incorrect");
        return isCorrect;
    }

    public static void DisplayQuestion(Question question)
    {
        Console.WriteLine("Question: " + question.Text);
        for (int i = 0; i < question.Answers.Count; i++)
        {
            Console.WriteLine((i + 1) + ": " + question.Answers[i]);
        }
    }

    public static void EnsureFileExists(string filePath) =>
        _ = File.Exists(filePath) ? true : throw new FileNotFoundException($"Error 404: File '{filePath}' Not Found", filePath);

    public static Question[] ParseQuestionsFromFile(string filePath)
    {
        const string CommentPrefix = "//";
        const string QuestionTag = "QUESTION:";
        const string ChoiceTag = "CHOICE:";
        const string CorrectIndexTag = "CORRECT:";

        var lines = File.ReadAllLines(filePath);
        var questions = new List<Question>();
        Question current = null;
        int lineNumber = 0;

        foreach (var rawLine in lines)
        {
            lineNumber++;
            string line = rawLine.Trim();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(CommentPrefix))
                continue;

            if (current == null)
            {
                if (TryParseTag(line, QuestionTag, out var questionText))
                {
                    current = new Question
                    {
                        Text = questionText,
                        Answers = []
                    };
                }
                else
                {
                    ThrowParseError(lineNumber, "Unexpected data outside of question block");
                }
            }
            else
            {
                if (TryParseTag(line, ChoiceTag, out var choiceText))
                {
                    current.Answers.Add(choiceText);
                }
                else if (TryParseTag(line, CorrectIndexTag, out var indexText))
                {
                    if (current.Answers.Count < 2)
                        ThrowParseError(lineNumber, "Unexpected data, not enough choices for closing a question");

                    if (!int.TryParse(indexText, out int index))
                        ThrowParseError(lineNumber, "Unexpected data, correct index not valid integer");

                    if (index < 1 || index > current.Answers.Count)
                        ThrowParseError(lineNumber, "Unexpected data, correct index outside of bounds");

                    current.CorrectAnswerIndex = index - 1;
                    questions.Add(current);
                    current = null;
                }
                else
                {
                    ThrowParseError(lineNumber, "Unexpected data within question block");
                }
            }
        }
        if (current != null)
            ThrowParseError(lineNumber, "File ended with question block still open");

        if (questions.Count == 0)
            ThrowParseError(lineNumber, "No valid Questions parsed from file");

        return [.. questions];
    }

    private static bool TryParseTag(string line, string tag, out string value)
    {
        if (line.StartsWith(tag, StringComparison.OrdinalIgnoreCase))
        {
            value = line[tag.Length..].Trim();
            return true;
        }
        value = null;
        return false;
    }

    private static void ThrowParseError(int lineNumber, string message) => 
        throw new InvalidDataException($"Program.ParseQuestionsFromFile: Line {lineNumber}: {message}");
}