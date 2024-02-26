namespace Todo.Application.Services.TodoService;

using Todo.Contracts.Todo;
using Todo.Domain.Entities;

public interface ITodoService
{
    Task AddTodoAsync(AddTodoRequest request);
    Task<IEnumerable<Todo>> GetAllTodosAsync();
    Task UpdateTodoAsync(UpdateTodoRequest todo);
    Task MarkTodoAsDoneAsync(Guid id, bool isDone);
    Task DeleteTodoAsync(Guid id);
}

