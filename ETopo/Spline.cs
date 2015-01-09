using System.Drawing;
using System.Windows.Forms;

namespace ETopo
{
    public class Spline
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Tens { get; set; }
        public double Bias { get; set; }
        public double Cont { get; set; }
        public double Ra { get; set; }
        public double Rb { get; set; }
    }

    public class SplinePiont
    {
        public DPoint Point{get;set;}
        public DPoint Ra { get; set; }
        public DPoint Rb { get; set; }

        public SplinePiont(SplinePiont p1, SplinePiont p2,SplinePiont p3, double tens, double cont, double bias)
        {
            Point = p2.Point;
            if (p1==null)
            {
                Ra = new DPoint {X = p3.Point.X - p2.Point.X, Y = p3.Point.Y - p2.Point.Y};
                Rb = new DPoint
                {
                    X = (1.5*(p3.Point.X - p2.Point.X) - 0.5*p3.Ra.X)*(1 - tens),
                    Y = (1.5*(p3.Point.Y - p2.Point.Y) - 0.5*p3.Ra.Y)*(1 - tens)
                };
                return;
            }
            if (p3 == null)
            {
                Ra = new DPoint
                {
                    X = (1.5*(p2.Point.X - p1.Point.X) - 0.5*p2.Rb.X)*(1 - tens),
                    Y = (1.5*(p2.Point.Y - p1.Point.Y) - 0.5*p2.Rb.Y)*(1 - tens)
                };
                Rb = new DPoint {X = p2.Point.X - p2.Point.X, Y = p2.Point.Y - p2.Point.Y};
                return;
            }
            var g1 = new DPoint {X = (p2.Point.X - p1.Point.X)*(1 + bias), Y = (p2.Point.Y - p1.Point.Y)*(1 + bias)};
            var g2 = new DPoint {X = (p3.Point.X - p2.Point.X)*(1 + bias), Y = (p3.Point.Y - p2.Point.Y)*(1 - bias)};
            var g3 = new DPoint {X = g2.X - g1.X, Y = g2.Y - g1.Y};
            Ra = new DPoint
            {
                X = (g1.X + 0.5*g3.X*(1 + cont))*(1 - tens),
                Y = (g1.Y + 0.5*g3.Y*(1 + cont))*(1 - tens)
            };
            Rb = new DPoint
            {
                X = (g1.X + 0.5*g3.X*(1 - cont))*(1 - tens),
                Y = (g1.Y + 0.5*g3.Y*(1 - cont))*(1 - tens)
            };
        }
    }
}
