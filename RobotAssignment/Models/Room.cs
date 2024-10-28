namespace RobotAssignment.Models
{
    public class Room
    {
        public int Width { get; set; }
        public int Depth { get; set; }

        public Room(int width, int depth)
        {
            Width = width;
            Depth = depth;
        }
    }

}
