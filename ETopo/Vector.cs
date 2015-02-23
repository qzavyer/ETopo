using System;

namespace ETopo
{
    public class Vector
    {
        
        /// <summary>
        /// Длина вектрора
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Азимут
        /// </summary>
        public double Fi { get; set; }
        /// <summary>
        /// Клин
        /// </summary>
        public double Teta { get; set; }

        public Vector()
        {
            Length = 0;
            Fi = 0;
            Teta = 0;
        }

        public Vector(double x, double y, double z)
        {
            Length = Math.Sqrt(x*x + y*y + z*z);
            Fi = 90 -
                 (Math.Abs(x) < MathConst.Accuracy ? y < 0 ? -90 : 90 : Math.Atan(y/x)/MathConst.Rad + (x < 0 ? 180 : 0));
            Teta = (Math.Abs(x) + Math.Abs(y)) < MathConst.Accuracy
                ? 90
                : Math.Atan(z/Math.Sqrt(x*x + y*y))/MathConst.Rad;
        }

        public Vector(Vector vector)
        {
            Length = vector.Length;
            Fi = vector.Fi;
            Teta = vector.Teta;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            var x1 = Math.Sin(v1.Fi*MathConst.Rad)*v1.Length;
            var y1 = Math.Cos(v1.Fi*MathConst.Rad)*v1.Length;
            var z1 = Math.Sin(v1.Teta*MathConst.Rad)*v1.Length;
            var x2 = Math.Sin(v2.Fi*MathConst.Rad)*v2.Length;
            var y2 = Math.Cos(v2.Fi*MathConst.Rad)*v2.Length;
            var z2 = Math.Sin(v2.Teta*MathConst.Rad)*v2.Length;
            var x = x1 + x2;
            var y = y1 + y2;
            var z = z1 + z2;
            return new Vector(x, y, z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            var x1 = Math.Cos(v1.Fi) * v1.Length;
            var y1 = Math.Sin(v1.Fi) * v1.Length;
            var z1 = Math.Sin(v1.Teta) * v1.Length;
            var x2 = Math.Cos(v2.Fi) * v2.Length;
            var y2 = Math.Sin(v2.Fi) * v2.Length;
            var z2 = Math.Sin(v2.Teta) * v2.Length;
            var x = x1 - x2;
            var y = y1 - y2;
            var z = z1 - z2;
            return new Vector(x, y, z);
        }
    }
}
