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

        public Piquet()
        {
            Name = "";
            X = 0;
            Y = 0;
            Note = "";
            Step = 0;
            Delta = 0;
        }
        public Piquet(Piquet piquet)
        {
            Name = piquet.Name;
            X = piquet.X;
            Y = piquet.Y;
            Note = piquet.Note;
            Step = piquet.Step;
            Delta = piquet.Delta;
        }
    }
}
