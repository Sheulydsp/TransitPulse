using TransitPulse.Application.Common;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

/// <summary>
/// Calculates reliability metrics from route events.
/// </summary>
public interface IReliabilityCalculator
{
    ReliabilityMetrics Calculate(
        IEnumerable<RouteEvent> routeEvents);
}