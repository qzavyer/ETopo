using System.Collections.Generic;

namespace ETopo
{
    /// <summary>
    /// класс кольцовки
    /// </summary>
    public class Ring
    {
        public List<Piquet> Points { get; private set; }
        public double Length { get; set; }
        public Vector Offset { get; set; }

        public Ring(List<Piquet> points)
        {
            Points = points;
            Length = 0;
            Offset = new Vector();
        }
    }
}