using OpenAI.Chat;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrincessBrideTrivia;

public class Program
{
    public static async Task Main(string[] args)
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);

        int numberCorrect = 0;
        int numberOfQuestions = questions.Length;
        bool runWhile = true;

        for (int i = 0; i < questions.Length; i++)
        {
            (bool result, _) = AskQuestion(questions[i]);
            if (result)
            {
                numberCorrect++;
            }
        }

        Console.WriteLine("Enter 'exit' to exit");

        while (runWhile)
        {
            try
            {
                Question q = await TriviaGenerator.GeneratePrincessBrideQuestionAsync(apiKey);

                (bool result, runWhile) = AskQuestion(q);

                if (runWhile)
                {
                    numberOfQuestions++;
                }
                
                if (result)
                {
                    numberCorrect++;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
        }

        Console.WriteLine("You got " + GetPercentCorrect(numberCorrect, numberOfQuestions) + " correct");
    }

    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        return ((float)numberCorrectAnswers / (float)numberOfQuestions * 100) + "%";
    }

    public static (bool, bool) AskQuestion(Question question)
    {
        DisplayQuestion(question);

        string userGuess = GetGuessFromUser();

        if (userGuess == "exit")
        {
            return (false, false);
        } else
        {
            return (DisplayResult(userGuess, question), true);
        }
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
        Console.WriteLine("Question: " + question.Text);
        for (int i = 0; i < question.Answers.Length; i++)
        {
            Console.WriteLine((i + 1) + ": " + question.Answers[i]);
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

public static class TriviaGenerator
{
    private const string Model = "gpt-4.1";

    /// <summary>
    /// Generates one Princess Bride multiple-choice question using the OpenAI API.
    /// </summary>
    public static async Task<Question> GeneratePrincessBrideQuestionAsync(string apiKey, int choices = 4)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key is required.", nameof(apiKey));

        if (choices is < 3 or > 6)
            throw new ArgumentOutOfRangeException(nameof(choices), "choices must be between 3 and 6.");

        var client = new ChatClient(model: Model, apiKey: apiKey);

        var system = """
            You generate trivia strictly about the 1987 film "The Princess Bride".
            Output ONLY a single JSON object with this exact C#-friendly shape:
            {
              "Text": string,                // the question text
              "Answers": string[],           // exactly N distinct options, concise, no markup
              "CorrectAnswerIndex": string   // "1"-based index of the correct answer, as a string
            }
            Rules:
            - The question must be unambiguous and answerable from the film (not the novel).
            - Answers must be short (max ~80 chars each) and mutually exclusive.
            - Make sure to switch up which index is correct.
            - Do not include explanations, hints, or extra keys.
            - Do not include code fences. Print raw JSON only.
        """;

        var user = $"""
            Create ONE multiple-choice question.
            Number of options: {choices}.
            Difficulty: hard.
        """;

        var completion = await client.CompleteChatAsync(
            new ChatMessage[]
            {
                new SystemChatMessage(system),
                new UserChatMessage(user)
            },
            new ChatCompletionOptions
            {
                // Mild creativity
                Temperature = (float)0.7
            });

        var message = completion.Value.Content[0];
        var json = message.Text?.Trim();

        if (string.IsNullOrWhiteSpace(json))
            throw new InvalidOperationException("Model returned empty content.");

        // Strict JSON parse
        var question = JsonSerializer.Deserialize<Question>(json, new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Disallow,
            AllowTrailingCommas = false,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.Strict
        });

        Validate(question, choices);
        return question!;
    }

    private static void Validate(Question q, int expectedChoices)
    {
        if (q is null) throw new InvalidOperationException("Failed to parse the model's JSON.");
        if (string.IsNullOrWhiteSpace(q.Text)) throw new InvalidOperationException("Question text missing.");
        if (q.Answers is null || q.Answers.Length != expectedChoices)
            throw new InvalidOperationException($"Expected {expectedChoices} answers, got {q?.Answers?.Length ?? 0}.");

        if (q.Answers.Any(string.IsNullOrWhiteSpace))
            throw new InvalidOperationException("One or more answers are empty.");

        if (!int.TryParse(q.CorrectAnswerIndex, out var idx))
            throw new InvalidOperationException("CorrectAnswerIndex must be a numeric string.");

        if (idx < 0 || idx >= q.Answers.Length)
            throw new InvalidOperationException("CorrectAnswerIndex out of range.");
    }
}