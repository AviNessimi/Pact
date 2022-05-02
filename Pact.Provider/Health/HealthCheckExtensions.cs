using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pact.Provider.Health.Models;
using System.Net.Mime;

namespace Pact.Provider.Health
{
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// The readiness endpoint, often available via /health/ready, 
        /// Readiness signals that the app is running normally but isn’t ready to receive requests just yet.
        /// </summary>
        /// <param name="pattern">The URL pattern of the health checks endpoint.</param>
        /// <returns>returns the readiness state to accept incoming requests from the gateway or the upstream proxy.</returns>
        public static IEndpointConventionBuilder MapDependenciesHealthChecks(
            this IEndpointRouteBuilder endpoints, string pattern)
        {
            return endpoints.MapHealthChecks(pattern, new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonConvert.SerializeObject(
                        new HealthResult
                        {
                            Status = report.Status.ToString(),
                            Duration = report.TotalDuration,
                            Info = report.Entries.Select(e => new HealthInfo
                            {
                                Key = e.Key,
                                Description = e.Value.Description,
                                Duration = e.Value.Duration,
                                Status = Enum.GetName(typeof(HealthStatus), e.Value.Status),
                                Error = e.Value.Exception?.Message
                            }).ToList()
                        }, Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });
        }
    }
}
