namespace TransitPulse.API.Contracts;

public record ErrorResponse(
    string Code,
    IReadOnlyList<string> Errors);
