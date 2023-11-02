namespace Common.Models;

public class QuizItem
{
    public string Question { get; set; } = string.Empty;
    public List<string> Choices { get; set; } = default!;
    public int AnswerIndex { get; set; }
    public int Score { get; set; }
}
