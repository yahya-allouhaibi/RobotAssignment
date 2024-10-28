using FluentValidation.Results;
using RobotAssignment.Models;

namespace RobotAssignment.Services
{
    public interface IRobotService
    {
        string ReturnValidationErrorString(ValidationResult robotValidationResult, ValidationResult roomValidationResult);
        bool ValidateCommands(string commands);
        void InitializeRobot(Room room, Robot robot);
        string ExcecuteCommands(string commands);
    }
}
