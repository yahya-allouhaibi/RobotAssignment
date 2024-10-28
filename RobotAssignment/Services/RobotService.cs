using FluentValidation.Results;
using RobotAssignment.Models;

namespace RobotAssignment.Services
{
    public class RobotService : IRobotService
    {
        public string ReturnValidationErrorString(ValidationResult robotValidationResult, ValidationResult roomValidationResult)
        {
            string validationErrors = string.Join(Environment.NewLine,
                robotValidationResult.Errors.Concat(roomValidationResult.Errors));
            return validationErrors;
        }

        public Task InitializeRobot(Room room, Robot robot)
        {
            throw new NotImplementedException();
        }
    }
}
