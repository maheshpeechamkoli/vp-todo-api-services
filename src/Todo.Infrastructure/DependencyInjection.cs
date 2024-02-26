namespace Todo.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Todo.Application.Interfaces.Persistence;
using Todo.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddScoped<ITodoRepository, TodoRepository>();

        return service;
    }

}