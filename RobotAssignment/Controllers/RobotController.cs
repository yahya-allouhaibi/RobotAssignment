using Microsoft.AspNetCore.Mvc;
using RobotAssignment.Enums;
using RobotAssignment.Models;

namespace RobotAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotController : Controller
    {
        [HttpPost("InitializeRobot")]
        public IActionResult InitializeRobot(int roomWidth, int roomDepth, int startPositionX, int startPositionY, Direction robotStartDirection)
        {
            var room = new Room(roomWidth, roomDepth);
            var robot = new Robot(startPositionX, startPositionY, robotStartDirection);
            return Ok(robot);
        }
    }
}
