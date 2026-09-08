extern alias MvcHost;

using Chapter3.Topic3.Domain;
using Chapter3.Topic3.Tests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Chapter3.Topic3.Tests.Mvc;

public sealed class MvcFactory : WebApplicationFactory<MvcHost::Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IBookRepository>();
            services.AddSingleton<IBookRepository>(new InMemoryBookRepository(SampleBook.Oblomov()));
        });
    }
}
