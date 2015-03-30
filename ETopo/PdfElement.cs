using System.Drawing;

namespace ETopo
{
    /// <summary>
    /// типы PDF-элементов
    /// </summary>
    public enum DrawType
    {
        Lines = 0,
        Elipse = 1,
        Polygon = 2,
        Text = 3,
        Arrow = 4
    }

    /// <summary>
    /// элемент экспорта в PDF
    /// </summary>
    public class PdfElement
    {
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }
        public Point[] Points { get; set; }
        public DrawType Type { get; set; }
        public string Text { get; set; }
    }
}
