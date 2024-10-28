using FluentValidation.Results;
using RobotAssignment.Enums;
using RobotAssignment.Models;
using System.Text.RegularExpressions;

namespace RobotAssignment.Services
{
    public class RobotService : IRobotService
    {

        private Room? _room;
        private Robot? _robot;
        private string _report;

        /// <summary>
        /// Forms a string of validations error created by fluent validation and returns it.
        /// </summary>
        /// <param name="robotValidationResult"></param>
        /// <param name="roomValidationResult"></param>
        /// <returns></returns>
        public string ReturnValidationErrorString(ValidationResult robotValidationResult, ValidationResult roomValidationResult)
        {
            string validationErrors = string.Join(Environment.NewLine,
                robotValidationResult.Errors.Concat(roomValidationResult.Errors));
            return validationErrors;
        }

        public bool ValidateCommands(string commands)
        {
            bool containsWrongCharacters = Regex.IsMatch(commands.ToUpper(), @"[^LRF]");
            if (containsWrongCharacters)
            {
                throw new ArgumentException("The commands can not contain characters other than L, R and F");
            }
            return true;
        }

        /// <summary>
        /// Initializes the robot if the values of the room and the robot are not null, and if the robots start position is valid
        /// </summary>
        /// <param name="room"></param>
        /// <param name="robot"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void  InitializeRobot(Room room, Robot robot)
        {
            if(!IsRobotInRoom(room, robot))
            {
                throw new ArgumentException("The robot can not start from outside the room");
            }
            _room = room ?? throw new ArgumentNullException("room was null");
            _robot = robot ?? throw new ArgumentNullException("robot was null");
        }

        /// <summary>
        /// Checks if the Robot is in the Room in the start position
        /// </summary>
        /// <param name="room"></param>
        /// <param name="robot"></param>
        /// <returns></returns>
        public bool IsRobotInRoom(Room room, Robot robot)
        {
            if(room.Width < robot.StartPositionX || room.Depth < robot.StartPositionY)
            {
                return false;
            }
            return true;
        }

        public string ExcecuteCommands(string commands)
        {
            if(_robot is not null && _room is not null)
            {
                foreach (var command in commands.ToUpper())
                {
                    MoveRobot(command, _robot.Direction);
                }

                if(_robot.StartPositionY > _room.Depth || _robot.StartPositionX > _room.Width || _robot.StartPositionX < 0 || _robot.StartPositionY < 0)
                {
                    throw new ArgumentOutOfRangeException("The robot walked outside the room bounds.");
                }
                _report = $"Report: {_robot.StartPositionX} {_robot.StartPositionY} {_robot.Direction.ToString()}";
            }
            return _report;
        }

        private void MoveRobot(char turnDirection, Direction currentDirection)
        {
            if (turnDirection == 'L' && _robot is not null)
            {
                switch (currentDirection)
                {
                    case Direction.N: _robot.Direction = Direction.W; break;
                    case Direction.E: _robot.Direction = Direction.N; break;
                    case Direction.S: _robot.Direction = Direction.E; break;
                    case Direction.W: _robot.Direction = Direction.S; break;
                }
            }
            else if (turnDirection == 'R' && _robot is not null)
            {
                switch (currentDirection)
                {
                    case Direction.N: _robot.Direction = Direction.E; break;
                    case Direction.E: _robot.Direction = Direction.S; break;
                    case Direction.S: _robot.Direction = Direction.W; break;
                    case Direction.W: _robot.Direction = Direction.N; break;
                }
            }
            else if (turnDirection == 'F' && _robot is not null)
            {
                switch (currentDirection)
                {
                    case Direction.N: _robot.StartPositionY++; break;
                    case Direction.E: _robot.StartPositionX++; break;
                    case Direction.S: _robot.StartPositionY--; break;
                    case Direction.W: _robot.StartPositionX--; break;
                }
            }
        }
    }
}
