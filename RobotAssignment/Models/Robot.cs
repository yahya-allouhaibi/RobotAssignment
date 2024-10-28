using RobotAssignment.Enums;

namespace RobotAssignment.Models
{
    public class Robot
    {
        public int StartPositionX { get; set; }
        public int StartPositionY { get; set; }
        public Direction Direction { get; set; }

        public Robot(int startPositionX, int starPositionY, Direction direction)
        {
            StartPositionX = startPositionX;
            StartPositionY = starPositionY;
            Direction = direction;
        }
    }
}
