using System.Drawing;

namespace ETopo
{
    public enum DrawType
    {
        Lines = 0,
        Elipse = 1,
        Polygon = 2,
        Text = 3,
        Arrow = 4
    }

    public class PdfElement
    {
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }
        public Point[] Points { get; set; }
        public DrawType Type { get; set; }
        public string Text { get; set; }
    }
}
