using System;

namespace ETopo
{
    public class Piquet
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public string Note { get; set; }
        public int Step { get; set; }
        public double Delta { get; set; }
        public double Distance { get; set; }
        public Vector Offset { get; set; }

        public Piquet()
        {
            Name = "";
            X = 0;
            Y = 0;
            Note = "";
            Step = 0;
            Delta = 0;
            Distance = 0;
            Offset = new Vector();
        }
        public Piquet(Piquet piquet)
        {
            Name = piquet.Name;
            X = piquet.X;
            Y = piquet.Y;
            Note = piquet.Note;
            Step = piquet.Step;
            Delta = piquet.Delta;
            Distance = piquet.Distance;
            Offset = new Vector(piquet.Offset);
        }

        public void Correct(Vector offset)
        {
            var x = Math.Sin(offset.Fi*MathConst.Rad)*offset.Length;
            var y = Math.Cos(offset.Fi*MathConst.Rad)*offset.Length;
            var z = Math.Sin(offset.Teta*MathConst.Rad)*offset.Length;
            X += x;
            Y += y;
            Z += z;
        }
    }
}
