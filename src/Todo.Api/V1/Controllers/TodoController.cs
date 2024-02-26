namespace Todo.Api.V1.Controllers;

using Microsoft.AspNetCore.Mvc;
using Todo.Application.Services.TodoService;
using Todo.Contracts.Todo;


[ApiController]
[Route("api/v{version:apiVersion}/todo")]
[ApiVersion("1.0")]
public class TodoController(ITodoService todoService) : ControllerBase
{
    private readonly ITodoService _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));

    [HttpPost("add")]
    public async Task<IActionResult> AddTodoAsync(AddTodoRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _todoService.AddTodoAsync(request);
        return StatusCode(StatusCodes.Status201Created, new { message = "Task added successfully." });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllTodosAsync()
    {
        var todos = await _todoService.GetAllTodosAsync();
        return Ok(todos);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTodoAsync(UpdateTodoRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _todoService.UpdateTodoAsync(request);
        return Ok(new { message = "Task updated successfully." });
    }

    [HttpPatch("mark-as-done/{id}/{isDone}")]
    public async Task<IActionResult> MarkTodoAsDoneAsync(Guid id, bool isDone)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }
        await _todoService.MarkTodoAsDoneAsync(id, isDone);
        return Ok(new { message = isDone ? "Task marked as done." : "Task marked as not done." });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTodoAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid ID.");
        }

        await _todoService.DeleteTodoAsync(id);
        return Ok(new { message = "Task deleted successfully." });
    }

}