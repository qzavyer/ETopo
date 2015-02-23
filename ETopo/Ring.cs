using System.Collections.Generic;

namespace ETopo
{
    public class Ring
    {
        public List<Piquet> Points { get; set; }
        public double Length { get; set; }
        public Vector Offset { get; set; }

        public Ring()
        {
            Points = new List<Piquet>();
            Length = 0;
            Offset = new Vector();
        }

        public Ring(List<Piquet> points)
        {
            Points = points;
            Length = 0;
            Offset = new Vector();
        }
    }
}