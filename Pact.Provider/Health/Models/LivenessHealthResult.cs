using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pact.Provider.Health.Models
{
    public class LivenessHealthResult
    {
        public LivenessHealthResult(HealthStatus status)
        {
            Status = status == HealthStatus.Healthy ?
                "available" :
                "unavailable";
        }

        public string Status { get; }
    }
}
