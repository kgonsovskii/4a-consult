using Chapter3.Topic3.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chapter3.Topic3.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLibrary(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<SqlScripts>();
        services.AddSingleton<Schema>();
        services.AddDbContext<LibraryContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<StoredProcedures>();
        services.AddScoped<ISqliteBookRepository, SqliteBookRepository>();
        services.AddScoped<IBookRepository>(sp =>
            sp.GetRequiredService<ISqliteBookRepository>());
        return services;
    }
}
