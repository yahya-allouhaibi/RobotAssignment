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
        /// Enpoint to Initialize the robot and the room.
        /// </summary>
        /// <param name="roomWidth">Enter the rooms Width.</param>
        /// <param name="roomDepth">Enter the rooms depth.</param>
        /// <param name="startPositionX">Enter the robots X starting position.</param>
        /// <param name="startPositionY">Enter the robots Y starting position.</param>
        /// <param name="robotStartDirection">Enter the Direction the robot is facing (N: North, E: East, S: South, W: West)</param>
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
        /// Endpoint to execute the commands provided by the user.
        ///  Enter the commands (L: turn Left, R: turn Right, F: move Forward).
        ///  Always start by Initializing the robot before executing the commands,or no report will be returned.
        /// </summary>
        /// <param name="commands">Can only contain the letters L, R and F</param>
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
