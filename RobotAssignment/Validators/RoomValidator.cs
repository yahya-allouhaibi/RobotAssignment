using FluentValidation;
using RobotAssignment.Models;

namespace RobotAssignment.Validators
{
    public class RoomValidator:AbstractValidator<Room>
    {
        public RoomValidator()
        {
            RuleFor(r => r.Width)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} can not be negative")
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .NotNull().WithMessage("{PropertyName} can not be null");
            RuleFor(r => r.Depth)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} can not be negative")
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .NotNull().WithMessage("{PropertyName} can not be null");
        }
    }
}
