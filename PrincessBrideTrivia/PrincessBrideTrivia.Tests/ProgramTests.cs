namespace PrincessBrideTrivia.Tests;



[TestClass]
public class ProgramTests
{
    [TestMethod]
    public void LoadQuestions_RetrievesQuestionsFromFile()
    {
        string filePath = Path.GetRandomFileName();
        try
        {
            // Arrange
            GenerateQuestionsFile(filePath, 2);

            // Act
            Question[] questions = Program.LoadQuestions(filePath);

            // Assert 
            Assert.HasCount(2, questions);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    [DataRow("1", true)]
    [DataRow("2", false)]
    public void DisplayResult_ReturnsTrueIfCorrect(string userGuess, bool expectedResult)
    {
        // Arrange
        Question question = new();
        question.CorrectAnswerIndex = "1";

        // Act
        bool displayResult = Program.DisplayResult(userGuess, question);

        // Assert
        Assert.AreEqual(expectedResult, displayResult);
    }

    [TestMethod]
    public void GetFilePath_ReturnsFileThatExists()
    {
        // Arrange

        // Act
        string filePath = Program.GetFilePath();

        // Assert
        Assert.IsTrue(File.Exists(filePath));
    }

    [TestMethod]
    [DataRow(1, 1, "100%")]
    [DataRow(5, 10, "50%")]
    [DataRow(1, 10, "10%")]
    [DataRow(0, 10, "0%")]
    public void GetPercentCorrect_ReturnsExpectedPercentage(int numberOfCorrectGuesses,
        int numberOfQuestions, string expectedString)
    {
        // Arrange

        // Act
        string percentage = Program.GetPercentCorrect(numberOfCorrectGuesses, numberOfQuestions);

        // Assert
        Assert.AreEqual(expectedString, percentage);
    }

    [TestMethod]
    public void DisplayScoreCard_PrintsEntries()
    {
        // Arrange
        Program.ScoreCard.Clear();
        Program.ScoreCard.Add($"{1,-15} | {2,15}");
        Program.ScoreCard.Add($"{1,-15} | {3,15}");

        using StringWriter sw = new();
        Console.SetOut(sw);

        // Act
        Program.DisplayScoreCard();

        // Assert
        string output = sw.ToString();
        StringAssert.Contains(output, "Score Card:");
        StringAssert.Contains(output, $"{1,-15} | {2,15}");
        StringAssert.Contains(output, $"{1,-15} | {3,15}");

        // Reset console
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }



    private static void GenerateQuestionsFile(string filePath, int numberOfQuestions)
    {
        for (int i = 0; i < numberOfQuestions; i++)
        {
            string[] lines =
            [
                "Question " + i + " this is the question text",
                "Answer 1",
                "Answer 2",
                "Answer 3",
                "2",
            ];
            File.AppendAllLines(filePath, lines);
        }
    }



    [TestMethod]
    public void DisplayScoreCard_DisplaysUserInputs()
    {
        // Arrange
        Program.ScoreCard.Clear();
            
        string user1 = "A";
        string user2 = "C";

        Program.ScoreCard.Add($"{user1,-15} | {"2",15}");
        Program.ScoreCard.Add($"{user2,-15} | {"1",15}");

        var originalOut = Console.Out;
        var sw = new System.IO.StringWriter();
        try
        {
            Console.SetOut(sw);

            // Act
            Program.DisplayScoreCard();

            // Assert 
            string output = sw.ToString();
            StringAssert.Contains(output,(user1));
            StringAssert.Contains(output,(user2));
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }



}
