namespace Todo.Infrastructure.Persistence;

using System.Collections.Generic;
using Todo.Application.Interfaces.Persistence;
using Todo.Domain.Entities;

public class TodoRepository : ITodoRepository
{
    // TODO : We can use DbContext class
    private static readonly IList<Todo> _todos = [];

    public Task AddTodoAsync(Todo todo)
    {
        ArgumentNullException.ThrowIfNull(todo);
        _todos.Add(todo);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Todo>> GetAllTodosAsync()
    {
        var sortedTodos = _todos.OrderBy(todo => todo.Deadline).ToList();
        return Task.FromResult<IEnumerable<Todo>>(sortedTodos);
    }

    public Task UpdateTodoAsync(Todo updatedTodo)
    {
        var todo = _todos.FirstOrDefault(todo => todo.Id == updatedTodo.Id);
        if (todo != null)
        {
            todo.Task = updatedTodo.Task;
            todo.Deadline = updatedTodo.Deadline;
            todo.IsDone = updatedTodo.IsDone;
        }
        else
        {
            throw new InvalidOperationException("Task not found for update.");
        }

        return Task.CompletedTask;
    }

    public Task MarkTodoAsync(Guid id, bool isDone)
    {
        var todo = _todos.FirstOrDefault(todo => todo.Id == id);
        if (todo != null)
        {
            todo.IsDone = isDone;
        }
        else
        {
            throw new InvalidOperationException("Task not found for update.");
        }
        return Task.CompletedTask;
    }

    public Task DeleteTodoAsync(Guid id)
    {
        var todo = _todos.FirstOrDefault(todo => todo.Id == id);
        if (todo != null)
        {
            _todos.Remove(todo);
        }
        else
        {
            throw new InvalidOperationException("Task not found for deletion.");
        }

        return Task.CompletedTask;
    }
}
