using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Pact.Provider.Health.Readiness
{
    public class ReadinessHealthCheck : IHealthCheck
    {
        private readonly MyOptions _snapshotOptions;

        public ReadinessHealthCheck(IOptionsSnapshot<MyOptions> snapshotOptionsAccessor)
        {
            _snapshotOptions = snapshotOptionsAccessor.Value;
        }


        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                    CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                _ = _snapshotOptions?.Version ??
                    throw new ArgumentNullException(nameof(_snapshotOptions.Version));

                var message = $"Version is healthy: {_snapshotOptions?.Version}";
                return Task.FromResult(HealthCheckResult.Healthy(message));
            }
            catch (Exception ex)
            {
                var message = "There is an error with version health check";
                return Task.FromResult(HealthCheckResult.Unhealthy(message, ex));
            }
        }
    }
}
