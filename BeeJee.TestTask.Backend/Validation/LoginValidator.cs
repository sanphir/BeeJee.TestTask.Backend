using BeeJee.TestTask.Backend.Dto;
using FluentValidation;

namespace BeeJee.TestTask.Backend.Validation
{
    public class LoginValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage(Constants.ERROR_MSG_REQUIRED_PROP);
            RuleFor(x => x.Password).NotEmpty().WithMessage(Constants.ERROR_MSG_REQUIRED_PROP);
        }
    }
}
