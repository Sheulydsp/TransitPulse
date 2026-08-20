namespace TransitPulse.Application.Exceptions;

public class BadRequestException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public BadRequestException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors.ToList();
    }
}