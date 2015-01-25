using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

    public class SplinePoint
    {
        public Point Point { get; set; }
        public Point Ra { get; set; }
        public Point Rb { get; set; }

        public SplinePoint(SplinePoint p1, SplinePoint p2, SplinePoint p3, double tens, double cont, double bias)
        {
            Point = p2.Point;
            if (p1 == null)
            {
                Ra = new Point {X = p3.Point.X - p2.Point.X, Y = p3.Point.Y - p2.Point.Y};
                Rb = new Point
                {
                    X = (float) ((1.5*(p3.Point.X - p2.Point.X) - 0.5*p3.Ra.X)*(1 - tens)),
                    Y = (float) ((1.5*(p3.Point.Y - p2.Point.Y) - 0.5*p3.Ra.Y)*(1 - tens))
                };
                return;
            }
            if (p3 == null)
            {
                Ra = new Point
                {
                    X = (float) ((1.5*(p2.Point.X - p1.Point.X) - 0.5*p2.Rb.X)*(1 - tens)),
                    Y = (float) ((1.5*(p2.Point.Y - p1.Point.Y) - 0.5*p2.Rb.Y)*(1 - tens))
                };
                Rb = new Point {X = p2.Point.X - p2.Point.X, Y = p2.Point.Y - p2.Point.Y};
                return;
            }
            var g1 = new Point
            {
                X = (float) ((p2.Point.X - p1.Point.X)*(1 + bias)),
                Y = (float) ((p2.Point.Y - p1.Point.Y)*(1 + bias))
            };
            var g2 = new Point
            {
                X = (float) ((p3.Point.X - p2.Point.X)*(1 + bias)),
                Y = (float) ((p3.Point.Y - p2.Point.Y)*(1 - bias))
            };
            var g3 = new Point {X = g2.X - g1.X, Y = g2.Y - g1.Y};
            Ra = new Point
            {
                X = (float) ((g1.X + 0.5*g3.X*(1 + cont))*(1 - tens)),
                Y = (float) ((g1.Y + 0.5*g3.Y*(1 + cont))*(1 - tens))
            };
            Rb = new Point
            {
                X = (float) ((g1.X + 0.5*g3.X*(1 - cont))*(1 - tens)),
                Y = (float) ((g1.Y + 0.5*g3.Y*(1 - cont))*(1 - tens))
            };
        }

        public SplinePoint(float x, float y, List<SplinePoint> list)
        {
            // координаты текущей точки
            Point = new Point {X = x, Y = y};
            // если список пуст (нет предыдущих)
            // координаты точек интерполяции = координатам точки
            if (list.Count == 0)
            {
                Ra = new Point {X = x, Y = y};
                Rb = new Point
                {
                    X = x,
                    Y = y
                };
                return;
            }

            // если в списке есть точки, вычистяем точку интерполяции для текущей точки
            var prevPoint = list[list.Count - 1];

            Ra = new Point
            {
                X = (float) (1.5*(Point.X - prevPoint.Point.X) - 0.5*prevPoint.Rb.X),
                Y = (float) (1.5*(Point.Y - prevPoint.Point.Y) - 0.5*prevPoint.Rb.Y)
            };
            Rb = new Point {X = Point.X - prevPoint.Point.X, Y = Point.Y - prevPoint.Point.Y};

            // если в списке больше одной точки, вычисляем точки интерполяции для предыдущей
            if (list.Count == 1)
            {
                prevPoint.Ra = new Point
                {
                    X = (float)Point.X - prevPoint.Point.X,
                    Y = (float)Point.Y - prevPoint.Point.Y
                };
                Rb = new Point { X = Point.X - prevPoint.Point.X, Y = Point.Y - prevPoint.Point.Y };
                return;
            }
            var pprevPoint = list[list.Count - 2];



            var g1 = new Point
            {
                X = prevPoint.Point.X - pprevPoint.Point.X,
                Y = prevPoint.Point.Y - pprevPoint.Point.Y
            };
            var g2 = new Point
            {
                X = Point.X - prevPoint.Point.X,
                Y = Point.Y - prevPoint.Point.Y
            };
            var g3 = new Point {X = g2.X - g1.X, Y = g2.Y - g1.Y};
            Ra = new Point
            {
                X = (float) (g1.X + 0.5*g3.X),
                Y = (float) (g1.Y + 0.5*g3.Y)
            };
            Rb = new Point
            {
                X = (float) (g1.X + 0.5*g3.X),
                Y = (float) (g1.Y + 0.5*g3.Y)
            };
        }
    }
}
