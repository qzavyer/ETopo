namespace ETopo
{
    class Quad
    {
        public DPoint TopLeft { get; set; }
        public DPoint TopRight { get; set; }
        public DPoint BottomRight { get; set; }
        public DPoint BottomLeft { get; set; }
    }

    class DPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
