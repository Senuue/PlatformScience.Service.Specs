namespace PlatformScience.Service.Specs.Models
{
    public class Coordinates
    {
        public int X;
        public int Y;

        public int[] GetCoordinates()
        {
            return new int[2] { X, Y };
        }
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
