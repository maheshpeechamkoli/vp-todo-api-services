namespace Todo.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Todo
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [StringLength(10, ErrorMessage = "Task must be 10 characters or fewer.")]
    public string? Task { get; set; }
    [Required]
    public DateTime? Deadline { get; set; }
    public bool IsDone { get; set; } = false;
}
