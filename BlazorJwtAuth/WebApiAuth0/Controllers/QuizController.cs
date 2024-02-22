using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiAuth0.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class QuizController : ControllerBase
{
    [HttpGet]
    public List<QuizItem> Get()
    {
        return
        [
            new QuizItem
            {
                Question = "Which of the following is the name of a Leonardo da Vinci's masterpiece?",
                Choices = ["Sunflowers", "Mona Lisa", "The Kiss"],
                AnswerIndex = 1,
                Score = 3
            },
            new QuizItem
            {
                Question = "Which of the following novels was written by Miguel de Cervantes?",
                Choices =
                [
                    "The Ingenious Gentleman Don Quixote of La Mancia", "The Life of Gargantua and of Pantagruel",
                    "One Hundred Years of Solitude"
                ],
                AnswerIndex = 0,
                Score = 5
            }
        ];
    }
}
