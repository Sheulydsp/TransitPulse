namespace TransitPulse.Application.Common
{
    public record ReliabilityMetrics(
        double AverageDelay,
        double CancellationRate,
        double OnTimePercentage,
        double Score);
}