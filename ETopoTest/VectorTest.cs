using ETopo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ETopoTest
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void TestVector()
        {
            var x = -0.001;
            var y = -1;
            var z = 0;
            var vect = new Vector(x, y, z);
        }
    }
}
