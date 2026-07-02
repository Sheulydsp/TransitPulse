using TransitPulse.Application.Common;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Infrastructure.Services;

public class ReliabilityCalculator : IReliabilityCalculator
{
    public ReliabilityMetrics Calculate(
   IEnumerable<RouteEvent> routeEvents)
    {
        var events = routeEvents.ToList();
        if (!events.Any())
        {
            throw new InvalidOperationException(
                "Cannot calculate reliability metrics from an empty event collection.");
        }

        var completedTrips =
            events.Where(e => !e.IsCancelled)
                  .ToList();

        var cancelledTrips =
            events.Count(e => e.IsCancelled);

        var averageDelay =
            completedTrips.Any()
                ? completedTrips.Average(
                    e => Math.Max(0, e.DelayMinutes))
                : 0;

        var cancellationRate =
            (double)cancelledTrips
            / events.Count
            * 100;

        var onTimeTrips =
            completedTrips.Count(
                e => e.DelayMinutes <= 3);

        var onTimePercentage =
            completedTrips.Any()
                ? (double)onTimeTrips
                    / completedTrips.Count
                    * 100
                : 0;

        var score = (0.6 * onTimePercentage) + (0.4 * (100 - cancellationRate));

        return new ReliabilityMetrics(
            averageDelay,
            cancellationRate,
            onTimePercentage,
            score);
    }


}
