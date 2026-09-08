using Chapter3.Topic3.Domain;
using Chapter3.Topic3.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
builder.Services.AddSingleton<IBookRepository>(_ => new SqliteBookRepository(connectionString));

if (!testing)
{
    await Schema.ApplyAsync(connectionString);
}

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();

public partial class Program;
