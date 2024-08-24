namespace BlazorServerAuth0.Data;

public class QuizItem
{
    public string Question { get; set; } = string.Empty;
    public List<string> Choices { get; set; } = [];
    public int AnswerIndex { get; set; }
    public int Score { get; set; }
}
