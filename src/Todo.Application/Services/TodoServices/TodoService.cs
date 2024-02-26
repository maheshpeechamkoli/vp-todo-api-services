namespace Todo.Application.Services.TodoService;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Todo.Application.Interfaces.Persistence;
using Todo.Contracts.Todo;
using Todo.Domain.Entities;
public class TodoService(ITodoRepository todoRepository, ILogger<TodoService> logger) : ITodoService
{
    public readonly ITodoRepository _todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    public async Task AddTodoAsync(AddTodoRequest request)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var todo = new Todo
            {
                Task = request.Task,
                Deadline = request.Deadline
            };
            await _todoRepository.AddTodoAsync(todo);
        }
        catch (Exception exception)
        {
            _logger.LogError("Error AddTodoAsync: {Exception}", exception);
            throw;
        }
    }

    public async Task<IEnumerable<Todo>> GetAllTodosAsync()
    {
        try
        {
            return await _todoRepository.GetAllTodosAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError("Error GetAllTodosAsync: {Exception}", exception);
            throw;
        }
    }

    public async Task UpdateTodoAsync(UpdateTodoRequest request)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request);

            var todo = new Todo
            {
                Id = request.Id,
                Task = request.Task,
                Deadline = request.Deadline,
                IsDone = request.IsDone,
            };
            await _todoRepository.UpdateTodoAsync(todo);
        }
        catch (Exception exception)
        {
            _logger.LogError("Error UpdateTodoAsync: {Exception}", exception);
            throw;
        }

    }


    public async Task MarkTodoAsDoneAsync(Guid id, bool isDone)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            await _todoRepository.MarkTodoAsync(id, isDone);
        }
        catch (Exception exception)
        {
            _logger.LogError("Error MarkTodoAsDoneAsync: {Exception}", exception);
            throw;
        }

    }

    public async Task DeleteTodoAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            await _todoRepository.DeleteTodoAsync(id);
        }
        catch (Exception exception)
        {
            _logger.LogError("Error DeleteTodoAsync: {Exception}", exception);
            throw;
        }
    }

}