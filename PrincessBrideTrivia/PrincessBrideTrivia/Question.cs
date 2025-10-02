namespace PrincessBrideTrivia;

public class Question
{
    public string Text { get; set; }
    public string[] Answers { get; set; }
    public string CorrectAnswerIndex { get; set; }
    public void RandomizeAnswerOrder(int randomSeed)
    {
        string lastCorrectAnswer = Answers[int.Parse(CorrectAnswerIndex) - 1];

        Random random = new Random(randomSeed);
        Answers = Answers.OrderBy(x => random.Next()).ToArray();

        for(int index = 0; index < Answers.Length; index++)
        {
            if(string.Equals(lastCorrectAnswer, Answers[index]))
            {
                CorrectAnswerIndex = (index + 1).ToString();
                return;
            }
        }
    }
}