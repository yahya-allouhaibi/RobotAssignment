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
        private int _newPositionX;
        private int _newPositionY;
        private Direction _newDirection;
        private string? _report;

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

        /// <summary>
        /// A method to validate the commands input. The input should only contain the letters L, R and F.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
            SetInitialRobotValues();
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

        /// <summary>
        /// Executes the commands from the user input.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns> A report of the new position of the robot</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string ExcecuteCommands(string commands)
        {
            if(_robot is not null && _room is not null)
            {
                foreach (var command in commands.ToUpper())
                {
                    MoveRobot(command, _newDirection);
                }

                if(_newPositionY > _room.Depth || _newPositionX > _room.Width || _newPositionX < 0 || _newPositionY < 0)
                {
                    SetInitialRobotValues();
                    throw new ArgumentOutOfRangeException("The robot walked outside the room bounds.");
                }
                SetNewRobotValues();
                _report = $"Report: {_newPositionX} {_newPositionY} {_newDirection}";
            }
            return _report ?? "No report available";
        }

        /// <summary>
        /// Set the initial values of the robot when the robot is initialized and if the robot moves out of bound.
        /// </summary>
        private void SetInitialRobotValues()
        {
            if (_robot is not null)
            {
                _newPositionX = _robot.StartPositionX;
                _newPositionY = _robot.StartPositionY;
                _newDirection = _robot.Direction;
            }
        }
        
        /// <summary>
        /// Set the new values of the robot when the commands is executed succesfully.
        /// </summary>
        private void SetNewRobotValues()
        {
            if (_robot is not null)
            {
                _robot.StartPositionX = _newPositionX;
                _robot.StartPositionY = _newPositionY;
                _robot.Direction = _newDirection;
            }
        }

        /// <summary>
        /// Moves the robot depending in the input of the user.
        /// </summary>
        /// <param name="turnDirection"></param>
        /// <param name="currentDirection"></param>
        private void MoveRobot(char turnDirection, Direction currentDirection)
        {
            if (turnDirection == 'L' && _robot is not null)
            {
                TurnLeft(currentDirection);
            }
            else if (turnDirection == 'R' && _robot is not null)
            {
                TurnRight(currentDirection);
            }
            else if (turnDirection == 'F' && _robot is not null)
            {
                MoveForward(currentDirection);
            }
        }

        /// <summary>
        /// Sets the new Direction of the robot when it turns left.
        /// </summary>
        /// <param name="currentDirection"></param>
        private void TurnLeft(Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.N: _newDirection = Direction.W; break;
                case Direction.E: _newDirection = Direction.N; break;
                case Direction.S: _newDirection = Direction.E; break;
                case Direction.W: _newDirection = Direction.S; break;
            }
        }

        /// <summary>
        /// Sets the new Direction of the robot when it turns right.
        /// </summary>
        /// <param name="currentDirection"></param>
        private void TurnRight(Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.N: _newDirection = Direction.E; break;
                case Direction.E: _newDirection = Direction.S; break;
                case Direction.S: _newDirection = Direction.W; break;
                case Direction.W: _newDirection = Direction.N; break;
            }
        }

        /// <summary>
        /// Sets the new X or Y position of the robot when it moves forward.
        /// </summary>
        /// <param name="currentDirection"></param>
        private void MoveForward(Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.N: _newPositionY++; break;
                case Direction.E: _newPositionX++; break;
                case Direction.S: _newPositionY--; break;
                case Direction.W: _newPositionX--; break;
            }
        }
    }
}
