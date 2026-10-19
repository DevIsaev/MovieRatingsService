using Domain.ValueObject.Validators;
using FluentValidation;

namespace MovieRatingsService.WebHost.Validators.Base
{
    public class UsernamePresentationValidator : AbstractValidator<string>
    {
        public UsernamePresentationValidator()
        {
            RuleFor(username => username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(UsernameValidator.MinLength)
                .MaximumLength(UsernameValidator.MaxLength);
        }
    }
}
