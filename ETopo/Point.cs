namespace ETopo
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
