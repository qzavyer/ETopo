using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETopo
{
    public enum CgnType
    {
        Stone = 1,
        Water = 2,
        Stalactite = 3,
        Stalagmite = 4,
        Stalagnate = 5,
        Way = 6,
        Enter = 7
    }

    public class Cgn
    {
        public string Name { get; set; }
        public Point Point { get; set; }
        public CgnType Type { get; set; }
        public double Angle { get; set; }

        public Cgn()
        {
            Angle = 0D;
        }
    }
}
