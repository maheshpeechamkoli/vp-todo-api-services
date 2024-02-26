using System.ComponentModel.DataAnnotations;

namespace Todo.Contracts.Todo
{
    public record AddTodoRequest
    (
        [Required(ErrorMessage = "Task is required.")]
        [MinLength(10, ErrorMessage = "Tasks must be longer than 10 characters.")]
        string Task,
        [Required(ErrorMessage = "Date is required.")]
        DateTime Deadline
    );
}

