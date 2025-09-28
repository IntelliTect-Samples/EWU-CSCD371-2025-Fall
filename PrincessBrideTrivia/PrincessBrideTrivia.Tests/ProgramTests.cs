using static PrincessBrideTrivia.Program;

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
    [DataRow("1", ResponseValidation.Accept)]
    [DataRow("2", ResponseValidation.Reject)]
    [DataRow("foo", ResponseValidation.Invalid)]
    [DataRow("11", ResponseValidation.Invalid)]
    [DataRow("1 1", ResponseValidation.Invalid)]
    public void AcceptRetryQuiz_ReturnsExpectedResponse(string testUserInput, ResponseValidation desiredOutput)
    {
        // Arrange

        // Act

        // Assert
        Assert.AreEqual(AcceptRetryQuiz(testUserInput), desiredOutput);
    }
    [TestMethod]
    [DataRow(69)]
    [DataRow(5924502)]
    [DataRow(-500)]
    public void RandomizeOrder_ResponseMaintainsContinuity(int seed)
    {
        //Collect a range of possible answers
        for (int i = 0; i < 1000; i++)
        {
            // Arrange
            Question question = new Question();
            question.CorrectAnswerIndex = "1";
            string expectedCorrectAnswer = "1";
            question.Answers = new string[] { "1", "2", "3" };
            // Act
            question.RandomizeAnswerOrder(seed + i);
            // Assert
            if(!string.Equals(expectedCorrectAnswer, question.Answers[int.Parse(question.CorrectAnswerIndex) - 1]))
            {
                Assert.Fail();
            }
        }
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
}
