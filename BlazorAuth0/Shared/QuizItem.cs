namespace QuizManagerClientHosted.Shared;

public class QuizItem
{
    public string Question { get; set; } = default!;
    public List<string> Choices { get; set; } = default!;
    public int AnswerIndex { get; set; }
    public int Score { get; set; }
}
