namespace ETopo
{
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
