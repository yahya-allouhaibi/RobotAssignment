using FluentValidation.Results;
using RobotAssignment.Models;

namespace RobotAssignment.Services
{
    public class RobotService : IRobotService
    {

        private Room? _room;
        private Robot? _robot;

        public string ReturnValidationErrorString(ValidationResult robotValidationResult, ValidationResult roomValidationResult)
        {
            string validationErrors = string.Join(Environment.NewLine,
                robotValidationResult.Errors.Concat(roomValidationResult.Errors));
            return validationErrors;
        }

        public void  InitializeRobot(Room room, Robot robot)
        {
            _room = room ?? throw new ArgumentNullException("room was null");
            _robot = robot ?? throw new ArgumentNullException("robot was null");
        }
    }
}
