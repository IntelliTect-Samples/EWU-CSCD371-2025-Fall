namespace PrincessBrideTrivia.Tests;

[TestClass]
public class ProgramTests
{
    [TestMethod]
    public void DisplayResult_ReturnsTrueWhenCorrect()
    {
        var testQuestion = new Question { CorrectAnswerIndex = 1 };
        bool result = Program.DisplayResult(1, testQuestion);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DisplayResult_ReturnsFalseWhenIncorrect()
    {
        var testQuestion = new Question { CorrectAnswerIndex = 1 };
        bool result = Program.DisplayResult(2, testQuestion);
        Assert.IsFalse(result);
    }

    [TestMethod] // NOTE: user will see 1-based index, we convert to zero-based (input of 2 means index 1 is correct)
    public void GetGuessFromUser_AllowsValidInput()
    {
        using var sr = new StringReader("2\n");
        Console.SetIn(sr);

        int userGuess = Program.GetGuessFromUser();
        Assert.AreEqual(1, userGuess);
    }

    [TestMethod] // added test
    public void EnsureFileExists_ThrowsExceptionWhenFileDoesNotExist()
    {
        string wrongFile = "thisFileDoesNotExist.wrong";
        Assert.Throws<FileNotFoundException>(() =>
        {
            Program.EnsureFileExists(wrongFile);
        });
    }

    [TestMethod] // Note for testing: GetPercentCorrect % displays are rounded normally via Math.Round
    [DataRow(1, 1, "100%")]
    [DataRow(5, 10, "50%")]
    [DataRow(1, 10, "10%")]
    [DataRow(2, 7, "29%")]
    [DataRow(0, 10, "0%")]
    [DataRow(2, 0, "N/A")]
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
    public void ParseQuestionsFromFile_ThrowsWhenFileEndsWithOpenQuestionBlock()
    {
        string content = """
            QUESTION: question text
            CHOICE: choice 1
            CHOICE: choice 2
            """;

        string expectedMessage = "question block still open";
        AssertParseFailure(content, expectedMessage);
    }

    [TestMethod]
    [DataRow("QUESTION: question")]
    [DataRow("not valid entry")]
    public void ParseQuestionsFromFile_ThrowsWhenUnexpectedInQuestionBlock(string unexpectedData)
    {
        string content = $"""
            QUESTION: question
            CHOICE: choice
            {unexpectedData}
            """;

        string expectedMessage = "Unexpected data within question block";
        AssertParseFailure(content, expectedMessage);
    }

    [TestMethod]
    [DataRow("CHOICE:")]
    [DataRow("CORRECT:")]
    [DataRow("not valid entry")]
    public void ParseQuestionFromFile_ThrowsWhenUnexpectedOutOfQuestionBlock(string unexpectedData)
    {
        string content = $"""
            {unexpectedData} should not be here
            """;

        string expectedMessage = "Unexpected data outside of question block";
        AssertParseFailure(content, expectedMessage);
    }

    [TestMethod]
    [DataRow("-1")]
    [DataRow("3")]
    public void ParseQuestionsFromFile_ThrowsWhenCorrectIndexIsOutOfBounds(string correctIndex)
    {
        string content = $"""
            QUESTION: question text
            CHOICE: choice 1 
            CHOICE: choice 2
            CORRECT: {correctIndex}
            """;

        string expectedMessage = "correct index outside of bounds";
        AssertParseFailure(content, expectedMessage);
    }

    [TestMethod] // note: converts string text to int via int.TryParse()
    [DataRow("not an int")]
    [DataRow("1.25")]
    [DataRow("1A")]
    [DataRow("")]
    public void ParseQuestionsFromFile_ThrowsWhenCorrectIndexIsNotInt(string invalidIndex)
    {
        string content = $"""
            QUESTION: question text
            CHOICE: choice 1 
            CHOICE: choice 2
            CORRECT: {invalidIndex}
            """;

        string expectedMessage = "correct index not valid integer";
        AssertParseFailure(content, expectedMessage);
    }

    [TestMethod]
    public void ParseQuestionsFromFile_ThrowsWhenNotEnoughChoices()
    {
        string content = """
            QUESTION: question
            CHOICE: choice 1
            CORRECT: 1
            """;

        string expectedMessage = "not enough choices";
        AssertParseFailure(content, expectedMessage);
    }

    [TestMethod]
    public void ParseQuestionsFromFile_ThrowsWhenFileEndAndNoQuestions()
    {
        string content = """
            // this file has no question data but exists
            """;

        string expectedMessage = "No valid Questions parsed from file";
        AssertParseFailure(content, expectedMessage);
    }

    public static void AssertParseFailure(string content, string expectedMessage)
    {
        string path = WriteTempFile(content);

        try {
            var e = Assert.Throws<InvalidDataException>(() =>
            Program.ParseQuestionsFromFile(path));

            Assert.Contains(expectedMessage, e.Message);
        }
        finally { File.Delete(path); }
    }
    
    private static string WriteTempFile(string content)
    {
        string path = Path.GetTempFileName();
        File.WriteAllText(path, content);
        return path;
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

    /* TODO: Tests

    Program.EnsureFileExists
        [x] file does not exist => throws

    Program.ParseQuestionsFromFile
        [x] Unexpected Data outside of question block => throws
        [x] Unexpected Data inside of question block => throws
        [x] Ends parsing with question still open => throws
        [x] non int given as index => throws
        [x] Index out of bounds => throws
        [x] not enough choices when index given => throws
        [x] ends parsing with no question formed => throws

    Program.AskQuestion - handles asking question,
        returns bool for correct / incorrect

    Program.DisplayQuestion

    Program.GetGuessFromUser - handles validating input from user, 
        returns (int.TryParse(user's input) *- 1*) 
        * this also handles converting of index from 1 to 0 based

    Program.DisplayResult - return bool that displays to user if they were right or wrong

    Program.GetPercentCorrect
    */
}
