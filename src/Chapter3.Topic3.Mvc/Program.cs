using Chapter3.Topic3.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Chapter3.Topic3.Mvc;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var testing = builder.Environment.IsEnvironment("Testing");
        var connectionString = Schema.Resolve(
            builder.Configuration.GetConnectionString("Library")!,
            builder.Environment.ContentRootPath);

        builder.Services.AddControllersWithViews(options =>
        {
            if (testing)
            {
                options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
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
        app.MapControllerRoute("default", "{controller=Books}/{action=Index}/{id?}");
        await app.RunAsync();
    }
}
