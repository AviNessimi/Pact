using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pact.Provider.ContractTests.Middleware;

namespace Pact.Provider.ContractTests
{
    public class TestStartup
    {
        private Startup _proxy;

        public TestStartup(IConfiguration configuration)
        {
            _proxy = new Startup(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _proxy.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ProviderStateMiddleware>();
            _proxy.Configure(app, env);
        }
    }
}