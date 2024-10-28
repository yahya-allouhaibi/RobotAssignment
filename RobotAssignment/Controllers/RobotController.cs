using Microsoft.AspNetCore.Mvc;
using RobotAssignment.Enums;
using RobotAssignment.Models;
using RobotAssignment.Services;
using RobotAssignment.Validators;

namespace RobotAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotController : Controller
    {
        private readonly IRobotService _robotService;

        public RobotController(IRobotService robotService)
        {
            _robotService = robotService;
        }

        [HttpPost("InitializeRobot")]
        public IActionResult InitializeRobot(int roomWidth, int roomDepth, int startPositionX, int startPositionY, Direction robotStartDirection)
        {
            var robotValidator = new RobotValidator();
            var roomValidator = new RoomValidator();

            var room = new Room(roomWidth, roomDepth);
            var robot = new Robot(startPositionX, startPositionY, robotStartDirection);

            var robotValidationResult = robotValidator.Validate(robot);
            var roomValidationResult = roomValidator.Validate(room);

            if (!robotValidationResult.IsValid || !roomValidationResult.IsValid)
            {
                string validationErrors = _robotService.ReturnValidationErrorString(robotValidationResult, roomValidationResult);
                return BadRequest(validationErrors);
            }

            try
            {
                _robotService.InitializeRobot(room, robot);
                return Ok("Robot initialized");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ExecuteCommands")]
        public IActionResult ExecuteCommands(string commands)
        {
            try
            {
                if (String.IsNullOrEmpty(commands))
                {
                    return BadRequest("The commands can not be null or empty");
                }
                _robotService.ValidateCommands(commands);
                var newPosition = _robotService.ExcecuteCommands(commands);

                return Ok(newPosition);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
