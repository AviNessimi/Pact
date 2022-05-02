using Pact.Provider.Health;
using Pact.Provider.Health.Liveness;
using Pact.Provider.Health.Readiness;
using Pact.Provider.Repositories;

namespace Pact.Provider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();

            services.Configure<MyOptions>(Configuration.GetSection(nameof(MyOptions)));
            services.AddControllers()
                        .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = false);

            services.AddHealthChecks()
                .AddCheck<ReadinessHealthCheck>("Version Health Check", tags: new[] { "readiness" })
                .AddCheck<LivenessHealthCheck>("Apm Activity Health Check", tags: new[] { "liveness" });
                

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseEndpoints(
                e =>
                {
                    e.MapDependenciesHealthChecks("/health");
                    e.MapLivenessHealthChecks("/health/live", "liveness");
                    e.MapReadinessHealthChecks("/health/ready", "readiness");

                    e.MapControllers();
                });
        }
    }
}
