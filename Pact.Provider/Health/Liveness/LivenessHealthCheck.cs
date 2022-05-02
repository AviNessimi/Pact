using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pact.Provider.Health.Liveness
{
    public class LivenessHealthCheck : IHealthCheck
    {
        private const string HealthCheckName = "Ping";

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var message = $"{HealthCheckName} is healthy";
                return Task.FromResult(HealthCheckResult.Healthy(message));
            }
            catch (Exception ex)
            {
                var message = $"There is an error with {HealthCheckName} health check";
                return Task.FromResult(HealthCheckResult.Unhealthy(message, ex));
            }
        }
    }
}
