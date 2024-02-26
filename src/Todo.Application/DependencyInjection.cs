namespace Todo.Application;

using Microsoft.Extensions.DependencyInjection;
using Todo.Application.Services.TodoService;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddScoped<ITodoService, TodoService>();

        return service;
    }

}