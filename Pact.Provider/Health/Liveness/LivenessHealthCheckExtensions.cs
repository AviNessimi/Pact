using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pact.Provider.Health.Models;
using System.Net.Mime;

namespace Pact.Provider.Health.Liveness
{
    public static class LivenessHealthCheckExtensions
    {
        /// <summary>
        /// The liveness endpoint, often available via /health/live, 
        /// If the check does not return the expected response,
        /// it means that the process is unhealthy or dead and should be replaced as soon as possible.
        /// </summary>
        /// <param name="pattern">The URL pattern of the health checks endpoint.</param>
        /// <returns>returns the liveness of a microservice. </returns>
        public static IEndpointConventionBuilder MapLivenessHealthChecks(
            this IEndpointRouteBuilder endpoints, string pattern, string tag)
        {
            return endpoints.MapHealthChecks(pattern, new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains(tag),
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
                        new LivenessHealthResult(report.Status),
                        Formatting.None,
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
