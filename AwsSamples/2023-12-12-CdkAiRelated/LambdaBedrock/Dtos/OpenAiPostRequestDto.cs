using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LambdaBedrock.Dtos;

public class OpenAiPostRequestDto
{
    [Required]
    [DefaultValue("富士山の高さは何メートルですか？")]
    public string Prompt { get; set; } = default!;
}
