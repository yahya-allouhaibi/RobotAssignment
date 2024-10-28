using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RobotAssignment.Controllers;
using RobotAssignment.Enums;
using RobotAssignment.Models;
using RobotAssignment.Services;

namespace RobotAssignment.Tests
{
    public class RobotControllerTests
    {
        private readonly Mock<IRobotService> _robotService;
        private readonly RobotController _controller;
        public RobotControllerTests()
        {
            _robotService = new Mock<IRobotService>();
            _controller = new RobotController(_robotService.Object);
        }

        [Fact]
        public void InitializeRobot_With_The_Valid_Values_Returns_OK()
        {
            //Arrange
            var roomWidth = 5;
            var roomDepth = 5;
            var startPositionX = 1;
            var startPositionY = 1;
            var direction = Direction.N;

            _robotService.Setup(service => service.InitializeRobot(It.IsAny<Room>(), It.IsAny<Robot>()));

            //Act
            var result = _controller.InitializeRobot(roomWidth, roomDepth, startPositionX, startPositionY, direction);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Robot initialized", okResult.Value);
        }

        [Fact]
        public void InitializeRobot_With_Negative_Values_Returns_BadRequest()
        {
            //Arrange
            var roomWidth = -5;
            var roomDepth = 5;
            var startPositionX = -1;
            var startPositionY = 1;
            var direction = Direction.N;

            _robotService.Setup(service => service.ReturnValidationErrorString(It.IsAny<ValidationResult>(), It.IsAny<ValidationResult>()))
                .Returns("Validation Errors");

            //Act
            var result = _controller.InitializeRobot(roomWidth, roomDepth, startPositionX, startPositionY, direction);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Validation Errors", badRequestResult.Value);
        }

        [Fact]
        public void InitializeRobot_With_Out_Of_Bound_Values_Returns_ArgumentException()
        {
            //Arrange
            var roomWidth = 5;
            var roomDepth = 5;
            var startPositionX = 8;
            var startPositionY = 1;
            var direction = Direction.N;

            _robotService.Setup(service => service.InitializeRobot(It.IsAny<Room>(), It.IsAny<Robot>()))
                .Throws(new ArgumentException("The robot can not start from outside the room"));

            //Act
            var result = _controller.InitializeRobot(roomWidth, roomDepth, startPositionX, startPositionY, direction);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The robot can not start from outside the room", badRequestResult.Value);
        }
    }
}