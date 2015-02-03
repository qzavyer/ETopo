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
}
