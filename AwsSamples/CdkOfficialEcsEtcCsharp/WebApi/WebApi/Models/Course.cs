namespace WebApi.Models;

public class Course
{
    /// <summary>
    ///     科目ID。
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     科目名。
    /// </summary>
    public string Name { get; set; } = default!;
}
