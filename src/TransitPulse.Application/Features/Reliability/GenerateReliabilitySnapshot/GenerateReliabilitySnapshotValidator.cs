using FluentValidation;

namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

public class GenerateReliabilitySnapshotValidator
    : AbstractValidator<GenerateReliabilitySnapshotCommand>
{
    public GenerateReliabilitySnapshotValidator()
    {
        RuleFor(x => x.RouteId)
            .NotEmpty()
            .WithMessage("RouteId is required.");

        RuleFor(x => x)
            .Must(x => x.PeriodEnd > x.PeriodStart)
            .WithMessage(
                "PeriodEnd must be after PeriodStart.");
    }
}