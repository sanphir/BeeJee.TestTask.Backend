using BeeJee.TestTask.Backend.Dto;
using BeeJee.TestTask.Backend.Extensions;
using FluentValidation;

namespace BeeJee.TestTask.Backend.Validation
{
    public class NewTaskValidator : AbstractValidator<NewTaskDto>
    {
        public NewTaskValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage(Constants.ERROR_MSG_REQUIRED_PROP);
            RuleFor(x => x.Email).NotEmpty().WithMessage(Constants.ERROR_MSG_REQUIRED_PROP)
                .Must(x => x?.IsEmail() ?? false).WithMessage(Constants.ERROR_MSG_INVALID_EMAIL);
            RuleFor(x => x.Text).NotEmpty().WithMessage(Constants.ERROR_MSG_REQUIRED_PROP);
        }
    }
}
