using System.Collections.Generic;

namespace ETopo
{
    public enum SplineType
    {
        Wall = 1,
        Precipice = 2
    }
    public enum SplineDirrection
    {
        Left = 0,
        Right = 1
    }

    public class Spline
    {
        public string Name { get; set; }
        public SplineType Type { get; set; }
        public List<SplinePoint> PointList { get; set; }
        public SplineDirrection Dirrection { get; set; }

        public Spline(List<SplinePoint> points)
        {
            Name = "";
            Type = SplineType.Wall;
            PointList = points;
            Dirrection = SplineDirrection.Left;
        }
        public Spline(SplineType type, List<SplinePoint> points, SplineDirrection dirrection)
        {
            Name = "";
            Type = type;
            PointList = points;
            Dirrection = dirrection;
        }
        public Spline(SplineType type, List<SplinePoint> points)
        {
            Name = "";
            Type = type;
            PointList = points;
            Dirrection = SplineDirrection.Left;
        }
    }

    public class SplinePoint
    {
        public Point Point { get; set; }
        public Point Ra { get; set; }
        public Point Rb { get; set; }

        public SplinePoint(float x, float y, List<SplinePoint> list)
        {
            // координаты текущей точки
            Point = new Point {X = x, Y = y};
            // если список пуст (нет предыдущих)
            // координаты точек интерполяции = координатам точки
            if (list.Count == 0)
            {
                Ra = new Point {X = 0, Y = 0};
                Rb = new Point {X = 0, Y = 0};
                return;
            }

            // если в списке есть точки, вычистяем точку интерполяции для текущей точки
            var prevPoint = list[list.Count - 1];

            Ra = new Point {X = (Point.X - prevPoint.Point.X)/2, Y = (Point.Y - prevPoint.Point.Y)/2};
            Rb = new Point {X = (Point.X - prevPoint.Point.X)/2, Y = (Point.Y - prevPoint.Point.Y)/2};

            // если в списке больше одной точки, вычисляем точки интерполяции для предыдущей
            if (list.Count == 1)
            {
                prevPoint.Ra = new Point {X = (Point.X - prevPoint.Point.X)/2, Y = (Point.Y - prevPoint.Point.Y)/2};
                prevPoint.Rb = new Point {X = (Point.X - prevPoint.Point.X)/2, Y = (Point.Y - prevPoint.Point.Y)/2};
                return;
            }
            var pprevPoint = list[list.Count - 2];
            prevPoint.Ra = new Point {X = (Point.X - pprevPoint.Point.X)/2, Y = (Point.Y - pprevPoint.Point.Y)/2};
            prevPoint.Rb = new Point {X = (Point.X - pprevPoint.Point.X)/2, Y = (Point.Y - pprevPoint.Point.Y)/2};
        }
    }
}
