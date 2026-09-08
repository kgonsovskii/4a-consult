using Chapter3.Topic3.Domain;
using Chapter3.Topic3.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var testing = builder.Environment.IsEnvironment("Testing");
var connectionString = builder.Configuration.GetConnectionString("Library")!;

builder.Services.AddRazorPages(options =>
{
    if (testing)
    {
        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    }
});
builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(connectionString));
builder.Services.AddSingleton<IBookRepository, PostgresBookRepository>();

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
