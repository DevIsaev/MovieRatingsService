using FluentValidation;
using MovieRatingsService.WebHost.Requests.User;
using MovieRatingsService.WebHost.Validators.Base;

namespace MovieRatingsService.WebHost.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(user => user.Username)
                .SetValidator(new UsernamePresentationValidator());
        }
    }
}
