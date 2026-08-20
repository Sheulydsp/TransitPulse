using FluentValidation;

namespace TransitPulse.Application.Features.Authentication.Register;

public class RegisterCommandValidator
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(command => command.FullName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]")
            .WithMessage(
                "Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage(
                "Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage(
                "Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage(
                "Password must contain at least one special character.");
    }
}