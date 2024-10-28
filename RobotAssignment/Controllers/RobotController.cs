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

        /// <summary>
        /// Enpoint to Initialize the robot.
        /// </summary>
        /// <param name="roomWidth"></param>
        /// <param name="roomDepth"></param>
        /// <param name="startPositionX"></param>
        /// <param name="startPositionY"></param>
        /// <param name="robotStartDirection"></param>
        /// <returns> "Robot initialized" if the initialization was successful. </returns>
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

        /// <summary>
        /// Enpoint to execute the commands provided by the user.
        /// </summary>
        /// <param name="commands"></param>
        /// <returns> The new position of the robot if the commands were executed successfully. </returns>
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
