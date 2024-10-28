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
        public void InitializeRobot_With_Valid_Values_Returns_OK()
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

        [Fact]
        public void ExecuteCommands_With_Valid_Values_Returns_OK()
        {
            //Arrange
            var commands = "FFLR";
            var newPosition = "Report: 1 2 W";

            _robotService.Setup(service => service.ValidateCommands(commands))
                .Returns(true);
            _robotService.Setup(service => service.ExcecuteCommands(commands))
                .Returns(newPosition);

            //Act
            var result = _controller.ExecuteCommands(commands);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(newPosition, okResult.Value);
        }

        [Fact]
        public void ExecuteCommands_With_Empty_String_Returns_BadRequest()
        {
            //Arrange
            var commands = "";

            //Act
            var result = _controller.ExecuteCommands(commands);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The commands can not be null or empty", badRequestResult.Value);
        }

        [Fact]
        public void ExecuteCommands_With_Invalid_Input_Returns_BadRequest()
        {
            //Arrange
            var commands = "FFGLLR";

            _robotService.Setup(service => service.ValidateCommands(commands))
                .Throws(new ArgumentException("The commands can not contain characters other than L, R and F"));

            //Act
            var result = _controller.ExecuteCommands(commands);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The commands can not contain characters other than L, R and F", badRequestResult.Value);
        }

        [Fact]
        public void ExecuteCommands_Returns_BadRequest_When_Robot_Moves_Out_Of_Bounds()
        {
            //Arrange
            var commands = "FFFFFFFFFFFF";

            _robotService.Setup(service => service.ValidateCommands(commands))
                .Returns(true);
            _robotService.Setup(service => service.ExcecuteCommands(commands))
                .Throws(new ArgumentOutOfRangeException(null, "The robot walked outside the room bounds."));

            //Act
            var result = _controller.ExecuteCommands(commands);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The robot walked outside the room bounds.", badRequestResult.Value);
        }
    }
}