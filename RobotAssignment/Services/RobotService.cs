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
            if(!IsRobotInRoom(room, robot))
            {
                throw new ArgumentException("The robot can not start from outside the room");
            }
            _room = room ?? throw new ArgumentNullException("room was null");
            _robot = robot ?? throw new ArgumentNullException("robot was null");
        }

        public bool IsRobotInRoom(Room room, Robot robot)
        {
            if(room.Width < robot.StartPositionX || room.Depth < robot.StartPositionY)
            {
                return false;
            }
            return true;
        }
    }
}
