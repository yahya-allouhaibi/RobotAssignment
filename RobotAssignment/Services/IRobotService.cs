﻿using FluentValidation.Results;
using RobotAssignment.Models;

namespace RobotAssignment.Services
{
    public interface IRobotService
    {
        string ReturnValidationErrorString(ValidationResult robotValidationResult, ValidationResult roomValidationResult);
        Task InitializeRobot(Room room, Robot robot);
    }
}