using TransitPulse.Application.Exceptions;

namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

public class GenerateReliabilitySnapshotValidator
{
    public void Validate(GenerateReliabilitySnapshotCommand command)
    {
        if (command.RouteId == Guid.Empty)
        {
            throw new ValidationException(
                "RouteId is required.");
        }

        if (command.PeriodEnd <= command.PeriodStart)
        {
            throw new ValidationException(
                "PeriodEnd must be after PeriodStart.");
        }
    }

}
