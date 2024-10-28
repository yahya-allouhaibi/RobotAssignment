using FluentValidation;
using RobotAssignment.Models;

namespace RobotAssignment.Validators
{
    public class RobotValidator: AbstractValidator<Robot>
    {
        public RobotValidator()
        {
            RuleFor(r => r.StartPositionX)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} can not be negative")
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .NotNull().WithMessage("{PropertyName} can not be null");
            RuleFor(r => r.StartPositionY)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} can not be negative")
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .NotNull().WithMessage("{PropertyName} can not be null");
            RuleFor(r => r.Direction)
                .IsInEnum<Robot, Enums.Direction>().WithMessage("{PropertyName} must be N, E, S or W")
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .NotNull().WithMessage("{PropertyName} can not be null");
        }
    }
}
