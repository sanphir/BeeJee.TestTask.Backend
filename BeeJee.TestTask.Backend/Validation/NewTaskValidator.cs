using BeeJee.TestTask.Backend.Dto;
using FluentValidation;

namespace BeeJee.TestTask.Backend.Validation
{
    public class NewTaskValidator : AbstractValidator<NewTaskDto>
    {
        public NewTaskValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage(Constants.ERROR_MSG_REQUIRED_PROP);
            RuleFor(x => x.Email).EmailAddress().WithMessage(Constants.ERROR_MSG_INVALID_EMAIL);
            RuleFor(x => x.Text).NotEmpty().WithMessage(Constants.ERROR_MSG_REQUIRED_PROP);
        }
    }
}
