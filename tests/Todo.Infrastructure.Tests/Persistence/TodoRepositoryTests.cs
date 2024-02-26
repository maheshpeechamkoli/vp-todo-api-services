namespace Todo.Infrastructure.Tests.Persistence;

using AutoFixture;
using Todo.Infrastructure.Persistence;
using Todo.Domain.Entities;

public class TodoRepositoryTests
{
    private readonly IFixture _fixture;
    private readonly TodoRepository _repository;

    public TodoRepositoryTests()
    {
        _fixture = new Fixture();
        _repository = new TodoRepository();
    }

    [Fact]
    public async Task AddTodoAsync_AddsTodoToList()
    {
        // Arrange
        var todo = new Todo { Id = Guid.NewGuid(), Task = "New Task", Deadline = DateTime.UtcNow.AddDays(1), IsDone = false };

        // Act
        await _repository.AddTodoAsync(todo);

        // Assert
        var addedTodo = (await _repository.GetAllTodosAsync()).FirstOrDefault(t => t.Id == todo.Id);
        Assert.NotNull(addedTodo);
        Assert.Equal("New Task", addedTodo.Task);
    }

    [Fact]
    public async Task AddTodoAsync_AddsMultipleTodosToList()
    {
        // Arrange
        var todo1 = new Todo { Id = Guid.NewGuid(), Task = "Testing Task 1", Deadline = DateTime.UtcNow.AddDays(1), IsDone = false };
        var todo2 = new Todo { Id = Guid.NewGuid(), Task = "Testing Task 2", Deadline = DateTime.UtcNow.AddDays(2), IsDone = false };

        // Act
        await _repository.AddTodoAsync(todo1);
        await _repository.AddTodoAsync(todo2);

        // Assert
        var allTodos = await _repository.GetAllTodosAsync();
        Assert.Contains(todo1, allTodos);
        Assert.Contains(todo2, allTodos);
    }

    [Fact]
    public async Task UpdateTodoAsync_UpdatesExistingTodoSuccessfully()
    {
        // Arrange
        var originalTodo = new Todo { Id = Guid.NewGuid(), Task = "Original Task", Deadline = DateTime.UtcNow.AddDays(1), IsDone = false };
        await _repository.AddTodoAsync(originalTodo);

        var updatedTodo = new Todo { Id = originalTodo.Id, Task = "Updated Task", Deadline = originalTodo.Deadline, IsDone = !originalTodo.IsDone };

        // Act
        await _repository.UpdateTodoAsync(updatedTodo);

        // Assert
        var allTodos = await _repository.GetAllTodosAsync();
        var todoAfterUpdate = allTodos.FirstOrDefault(t => t.Id == originalTodo.Id);

        Assert.NotNull(todoAfterUpdate);
        Assert.Equal("Updated Task", todoAfterUpdate.Task);
        Assert.Equal(updatedTodo.IsDone, todoAfterUpdate.IsDone);
    }

    [Fact]
    public async Task MarkTodoAsync_MarksExistingTodoAsDone()
    {
        // Arrange
        var todo = new Todo { Id = Guid.NewGuid(), Task = "Task to be done", Deadline = DateTime.UtcNow.AddDays(1), IsDone = false };
        await _repository.AddTodoAsync(todo);

        // Act
        await _repository.MarkTodoAsync(todo.Id, true);

        // Assert
        var allTodos = await _repository.GetAllTodosAsync();
        var markedTodo = allTodos.FirstOrDefault(t => t.Id == todo.Id);
        Assert.NotNull(markedTodo);
        Assert.True(markedTodo.IsDone);
    }

    [Fact]
    public async Task DeleteTodoAsync_RemovesExistingTodo()
    {
        // Arrange
        var todo = new Todo { Id = Guid.NewGuid(), Task = "Task to delete", Deadline = DateTime.UtcNow.AddDays(1), IsDone = false };
        await _repository.AddTodoAsync(todo);

        // Act
        await _repository.DeleteTodoAsync(todo.Id);

        // Assert
        var allTodos = await _repository.GetAllTodosAsync();
        var deletedTodo = allTodos.FirstOrDefault(t => t.Id == todo.Id);
        Assert.Null(deletedTodo);
    }
}
