namespace ETopo
{
    /// <summary>
    /// класс редактируемой опорной точки сплайна
    /// </summary>
    public class EditPoint
    {
        public SplinePoint Point { get; set; }
        public string PointName { get; set; }

        public EditPoint(SplinePoint point, string name)
        {
            Point = point;
            PointName = name;
        }
    }

    /// <summary>
    /// класс редактируемой точки УГО
    /// </summary>
    public class EditCgnPoint
    {
        public Cgn Cgn { get; set; }
        public string PointName { get; set; }

        public EditCgnPoint(Cgn cgn, string name)
        {
            Cgn = cgn;
            PointName = name;
        }
    }
}
