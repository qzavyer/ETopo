namespace ETopo
{
    /// <summary>
    /// типы УГО
    /// </summary>
    public enum CgnType
    {
        Stone = 1,
        Water = 2,
        Stalactite = 3,
        Stalagmite = 4,
        Stalagnate = 5,
        Way = 6,
        Enter = 7
    }

    /// <summary>
    /// класс УГО
    /// </summary>
    public class Cgn
    {
        public string Name { get; set; }
        public Point Point { get; set; }
        public CgnType Type { get; set; }
        public double Angle { get; set; }

        public Cgn()
        {
            Angle = 0D;
        }
    }
}
