namespace Todo.Application.Interfaces.Persistence;

using Todo.Domain.Entities;

public interface ITodoRepository
{
    Task AddTodoAsync(Todo todo);
    Task<IEnumerable<Todo>> GetAllTodosAsync();
    Task UpdateTodoAsync(Todo todo);
    Task MarkTodoAsync(Guid id, bool isDone);
    Task DeleteTodoAsync(Guid id);
}