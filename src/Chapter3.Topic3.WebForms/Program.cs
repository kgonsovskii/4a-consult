using Chapter3.Topic3.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Chapter3.Topic3.WebForms;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var testing = builder.Environment.IsEnvironment("Testing");
        var connectionString = Schema.Resolve(
            builder.Configuration.GetConnectionString("Library")!,
            builder.Environment.ContentRootPath);

        builder.Services.AddRazorPages(options =>
        {
            if (testing)
            {
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            }
        });
        builder.Services.AddLibrary(connectionString);

        var app = builder.Build();
        if (!testing)
        {
            await app.Services.GetRequiredService<Schema>().ApplyAsync(connectionString);
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.MapRazorPages();
        await app.RunAsync();
    }
}
