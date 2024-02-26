namespace Todo.Application.Tests.Services;

using Moq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Todo.Application.Services.TodoService;
using Todo.Contracts.Todo;
using Todo.Application.Interfaces.Persistence;
using Todo.Domain.Entities;

public partial class TodoServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ITodoRepository> _repositoryMock;
    private readonly Mock<ILogger<TodoService>> _loggerMock;
    private readonly TodoService _sut;

    public TodoServiceTests()
    {
        _fixture = new Fixture();
        _repositoryMock = _fixture.Freeze<Mock<ITodoRepository>>();
        _loggerMock = _fixture.Freeze<Mock<ILogger<TodoService>>>();
        _sut = new TodoService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddTodoAsync_CallsRepositoryWithCorrectTodo()
    {
        // Arrange
        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        var request = new AddTodoRequest("TestTaskForToday", DateTime.UtcNow.AddDays(1));

        // Act
        await service.AddTodoAsync(request);

        // Assert
        _repositoryMock.Verify(repo => repo.AddTodoAsync(It.Is<Todo>(t => t.Task == request.Task && t.Deadline == request.Deadline)), Times.Once);
    }

    [Fact]
    public async Task AddTodoAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddTodoAsync(null!));
    }

    [Fact]
    public async Task GetAllTodosAsync_ReturnsTodosFromRepository()
    {
        // Arrange
        var expectedTodos = new List<Todo>
        {
            new Todo
            {
                Id = Guid.NewGuid(),
                Task = "Task 1",
                Deadline = DateTime.UtcNow.AddDays(1),
                IsDone = false
            },
            new Todo
            {
                Id = Guid.NewGuid(),
                Task = "Task 2",
                Deadline = DateTime.UtcNow.AddDays(2),
                IsDone = true
            }
        };

        _repositoryMock.Setup(repo => repo.GetAllTodosAsync()).ReturnsAsync(expectedTodos);

        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        // Act
        var todos = await service.GetAllTodosAsync();

        // Assert
        Assert.Equal(expectedTodos, todos);
    }

    [Fact]
    public async Task UpdateTodoAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateTodoAsync(null!));
    }

    [Fact]
    public async Task UpdateTodoAsync_SuccessfullyUpdatesTodo_WhenRequestIsValid()
    {
        // Arrange
        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);
        var request = new UpdateTodoRequest(
            Guid.NewGuid(),
            "Updated Task",
            DateTime.UtcNow.AddDays(1),
            true
        );

        // Act
        await service.UpdateTodoAsync(request);

        // Assert
        _repositoryMock.Verify(repo => repo.UpdateTodoAsync(It.Is<Todo>(t =>
            t.Id == request.Id &&
            t.Task == request.Task &&
            t.Deadline == request.Deadline &&
            t.IsDone == request.IsDone)), Times.Once);
    }

    [Fact]
    public async Task MarkTodoAsDoneAsync_SuccessfullyMarksTodo_WhenIdIsValid()
    {
        // Arrange
        var validId = Guid.NewGuid();
        var isDone = true;

        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        // Act
        await service.MarkTodoAsDoneAsync(validId, isDone);

        // Assert
        _repositoryMock.Verify(repo => repo.MarkTodoAsync(validId, isDone), Times.Once);
    }

    [Fact]
    public async Task MarkTodoAsDoneAsync_ThrowsArgumentNullException_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.Empty;
        var isDone = true;

        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.MarkTodoAsDoneAsync(invalidId, isDone));
    }

    [Fact]
    public async Task DeleteTodoAsync_SuccessfullyDeletesTodo_WhenIdIsValid()
    {
        // Arrange
        var validId = Guid.NewGuid();

        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        // Act
        await service.DeleteTodoAsync(validId);

        // Assert
        _repositoryMock.Verify(repo => repo.DeleteTodoAsync(validId), Times.Once);
    }

    [Fact]
    public async Task DeleteTodoAsync_ThrowsArgumentNullException_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.Empty;

        var service = new TodoService(_repositoryMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.DeleteTodoAsync(invalidId));
    }
}
