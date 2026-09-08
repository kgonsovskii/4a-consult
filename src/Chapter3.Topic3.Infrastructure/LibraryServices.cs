using Chapter3.Topic3.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Chapter3.Topic3.Infrastructure;

public static class LibraryServices
{
    public static IServiceCollection AddLibrary(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<SqlScripts>();
        services.AddSingleton<Schema>();
        services.AddSingleton<IBookRepository>(sp =>
            new SqliteBookRepository(connectionString, sp.GetRequiredService<SqlScripts>()));
        return services;
    }
}
