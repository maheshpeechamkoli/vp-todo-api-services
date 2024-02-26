
namespace Todo.Api.Tests.V1.Controllers;

using Moq;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.V1.Controllers;
using Todo.Application.Services.TodoService;
using Microsoft.AspNetCore.Http;
using Todo.Contracts.Todo;
using Newtonsoft.Json.Linq;

public partial class TodoControllerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ITodoService> _todoServiceMock;
    private readonly TodoController _sut;

    public TodoControllerTests()
    {
        _fixture = new Fixture();
        _todoServiceMock = _fixture.Freeze<Mock<ITodoService>>();
        _sut = new TodoController(_todoServiceMock.Object);
    }

    [Fact]
    public async Task AddTodoAsync_ValidRequest_ReturnsOkWith201StatusCode()
    {
        // Arrange
        var validRequest = new AddTodoRequest("Task Name", new DateTime(2023, 2, 5));

        // Mock the service call
        _todoServiceMock.Setup(x => x.AddTodoAsync(It.IsAny<AddTodoRequest>()))
                        .Verifiable();

        // Act
        var result = await _sut.AddTodoAsync(validRequest);

        // Assert
        _todoServiceMock.Verify(x => x.AddTodoAsync(It.IsAny<AddTodoRequest>()), Times.Once);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);

        var responseValue = JObject.FromObject(statusCodeResult.Value!);
        var message = responseValue["message"]?.ToString();

        Assert.NotNull(message);
        Assert.Equal("Task added successfully.", message);
    }

    [Fact]
    public async Task AddTodoAsync_ReturnsBadRequest_WhenTaskNameIsTooShort()
    {
        // Arrange
        var request = new AddTodoRequest("Task", new DateTime(2023, 2, 5));

        // Simulate model state error
        _sut.ModelState.AddModelError("Task", "Tasks must be longer than 10 characters.");

        // Act
        var result = await _sut.AddTodoAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);

        var serializableError = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(serializableError.ContainsKey("Task"));
        if (serializableError["Task"] is IEnumerable<string> errorMessages)
            Assert.Contains("Tasks must be longer than 10 characters.", errorMessages);
    }

    [Fact]
    public async Task UpdateTodoAsync_ReturnsOk_WhenModelStateIsValid()
    {
        // Arrange
        var requestId = new Guid("67f64594-c983-4c26-9931-805afc38ec74");
        var request = new UpdateTodoRequest(requestId, "New TaskFor Support", new DateTime(2023, 2, 5), false);

        _todoServiceMock.Setup(service => service.UpdateTodoAsync(It.IsAny<UpdateTodoRequest>()))
                   .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.UpdateTodoAsync(request);

        // Assert
        _todoServiceMock.Verify(service => service.UpdateTodoAsync(It.Is<UpdateTodoRequest>(req => req == request)), Times.Once);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

        var responseValue = JObject.FromObject(okObjectResult.Value!);
        var message = responseValue["message"]?.ToString();

        Assert.NotNull(message);
        Assert.Equal("Task updated successfully.", message);
    }

    [Fact]
    public async Task UpdateTodoAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var requestId = new Guid("67f64594-c983-4c26-9931-805afc38ec74");
        var request = new UpdateTodoRequest(requestId, "Short", new DateTime(2023, 2, 5), false);

        // Simulate an invalid model state
        _sut.ModelState.AddModelError("Task", "Tasks must be longer than 10 characters.");

        // Act
        var result = await _sut.UpdateTodoAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);

        var serializableError = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(serializableError.ContainsKey("Task"));
        if (serializableError["Task"] is IEnumerable<string> errorMessages)
            Assert.Contains("Tasks must be longer than 10 characters.", errorMessages);
    }

    [Fact]
    public async Task MarkTodoAsDoneAsync_ReturnsOk_WhenIdIsValid()
    {
        // Arrange
        var validId = Guid.NewGuid();
        var isDone = true;

        _todoServiceMock.Setup(service => service.MarkTodoAsDoneAsync(validId, isDone))
                   .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.MarkTodoAsDoneAsync(validId, isDone);

        // Assert
        _todoServiceMock.Verify(service => service.MarkTodoAsDoneAsync(validId, isDone), Times.Once);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

        var responseValue = JObject.FromObject(okObjectResult.Value!);
        var actualMessage = responseValue["message"]?.ToString();

        var expectedMessage = "Task marked as done.";
        Assert.NotNull(actualMessage);
        Assert.Equal(expectedMessage, actualMessage);
    }

    [Fact]
    public async Task MarkTodoAsDoneAsync_ReturnsBadRequest_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.Empty;
        var isDone = true;

        // Act
        var result = await _sut.MarkTodoAsDoneAsync(invalidId, isDone);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid ID.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteTodoAsync_ReturnsOk_WhenIdIsValid()
    {
        // Arrange
        var validId = Guid.NewGuid();

        _todoServiceMock.Setup(service => service.DeleteTodoAsync(validId))
                   .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteTodoAsync(validId);

        // Assert
        _todoServiceMock.Verify(service => service.DeleteTodoAsync(validId), Times.Once);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

        var responseValue = JObject.FromObject(okObjectResult.Value!);
        var message = responseValue["message"]?.ToString();

        Assert.NotNull(message);
        Assert.Equal("Task deleted successfully.", message);
    }

}
