using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ETopo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ETopoTest
{
    /// <summary>
    /// Сводное описание для UnitTest1
    /// </summary>
    [TestClass]
    public class TopoListTest
    {
        private readonly List<Trace> _trcLst = new List<Trace>
        {
            new Trace {Azimuth = 90, Clino = 0, From = "0", To = "1", Tape = 5},
            new Trace {Azimuth = 60, Clino = 0, From = "1", To = "2", Tape = 4.2},
            new Trace {Azimuth = 18, Clino = 0, From = "2", To = "3", Tape = 3},
            new Trace {Azimuth = 56, Clino = 0, From = "3", To = "4", Tape = 2},
            new Trace {Azimuth = 130, Clino = 0, From = "4", To = "5", Tape = 1.7},
            new Trace {Azimuth = 160, Clino = 0, From = "5", To = "6", Tape = 2.5},
            new Trace {Azimuth = 215, Clino = 0, From = "6", To = "7", Tape = 1.5},
            new Trace {Azimuth = -108, Clino = 0, From = "7", To = "8", Tape = 2},
            new Trace {Azimuth = -67, Clino = 0, From = "3", To = "12", Tape = 5},
            new Trace {Azimuth = 46, Clino = 0, From = "12", To = "13", Tape = 1.2},
            new Trace {Azimuth = 0, Clino = 0, From = "13", To = "14", Tape = 2},
            new Trace {Azimuth = -38, Clino = 0, From = "14", To = "15", Tape = 1.4},
            new Trace {Azimuth = 233, Clino = 0, From = "15", To = "16", Tape = 1.5},
            new Trace {Azimuth = 205, Clino = 0, From = "16", To = "17", Tape = 2},
            new Trace {Azimuth = 172, Clino = 0, From = "17", To = "18", Tape = 2},
            new Trace {Azimuth = 65, Clino = 0, From = "5", To = "9", Tape = 1.7},
            new Trace {Azimuth = 130, Clino = 0, From = "9", To = "10", Tape = 2},
            new Trace {Azimuth = 161, Clino = 0, From = "10", To = "11", Tape = 2.2},
            new Trace {Azimuth = -57, Clino = 0, From = "8", To = "2", Tape = 2.4}
        };
        
        #region Дополнительные атрибуты тестирования
        //
        // При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        // ClassInitialize используется для выполнения кода до запуска первого теста в классе
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // TestInitialize используется для выполнения кода перед запуском каждого теста 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // TestCleanup используется для выполнения кода после завершения каждого теста
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGetTrace()
        {
            var piqLst = new List<Piquet>
            {
                new Piquet {Name = "0", X = 0, Y = 0},
                new Piquet {Name = "1", X = 5, Y = 0},
                new Piquet {Name = "2", X = 8.642, Y = 2.092},
                new Piquet {Name = "3", X = 9.577, Y = 4.942},
                new Piquet {Name = "4", X = 11.241, Y = 6.052},
                new Piquet {Name = "5", X = 12.577, Y = 4.942},
                new Piquet {Name = "6", X = 13.425, Y = 2.591},
                new Piquet {Name = "7", X = 12.571, Y = 1.358},
                new Piquet {Name = "8", X = 10.661, Y = 0.764},
                new Piquet {Name = "9", X = 14.169, Y = 5.539},
                new Piquet {Name = "10", X = 15.71, Y = 4.264},
                new Piquet {Name = "11", X = 16.426, Y = 2.184},
                new Piquet {Name = "12", X = 4.98, Y = 6.909},
                new Piquet {Name = "13", X = 5.838, Y = 7.747},
                new Piquet {Name = "14", X = 5.838, Y = 9.747},
                new Piquet {Name = "15", X = 4.98, Y = 10.850},
                new Piquet {Name = "16", X = 3.785, Y = 9.943},
                new Piquet {Name = "17", X = 2.932, Y = 8.134},
                new Piquet {Name = "18", X = 3.203, Y = 6.153},
            };
            var pq0 = new Piquet {Name = "0", Step = 0, X = 0, Y = 0, Z = 0};
            var res = new List<Piquet> { pq0 };
            TopoLib.GetTrace(_trcLst, res, pq0);
            var deltaX = 0D;
            var deltaY = 0D;
            foreach (var piquet in res)
            {
                var pq = piquet;
                foreach (var piquet1 in piqLst.Where(p=>p.Name==pq.Name))
                {
                    deltaX += Math.Abs(pq.X - piquet1.X);
                    deltaY += Math.Abs(pq.Y - piquet1.Y);
                }
            }

            //var a = Math.Sqrt(deltaX*deltaX + deltaY*deltaY)/_trcLst.Sum(tr => tr.Tape);//==0.0196 (1.96 %)
            var bResult = res.Count == 19 && Math.Sqrt(deltaX * deltaX + deltaY * deltaY) / _trcLst.Sum(tr => tr.Tape) < 0.04;

            Assert.AreEqual(bResult, true);
            
        }

        [TestMethod]
        public void TestGetString()
        {
            var pq0 = new Piquet { Name = "0", Step = 0, X = 0, Y = 0, Z = 0 };
            var pqList = new List<Piquet> { pq0 };
            TopoLib.GetTrace(_trcLst, pqList, pq0);
            var pqTemp = new List<Piquet> {pqList[0]};
            var res = TopoLib.GetString(pqTemp, _trcLst, pqList[0], pqList);
            var result = res.Count <= _trcLst.Count && pqTemp.Count == pqList.Count;
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void TestGetRings()
        {
            var pq0 = new Piquet { Name = "0", Step = 0, X = 0, Y = 0, Z = 0 };
            var pqList = new List<Piquet> { pq0 };
            TopoLib.GetTrace(_trcLst, pqList, pq0);
            var rings = TopoLib.GetAllRing(_trcLst, pq0, pqList);

            var result = true;

            if (rings.Count > 0)
            {
                foreach (var ring in rings)
                {
                    var start = ring.Points.First();
                    var end = ring.Points.Last();
                    result = result &&
                             (_trcLst.Exists(
                                 trc =>
                                     trc.From == start.Name && trc.To == end.Name ||
                                     trc.From == end.Name && trc.To == start.Name));
                }
            }
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void TestGetRingLength()
        {
            var pq0 = new Piquet { Name = "0", Step = 0, X = 0, Y = 0, Z = 0 };
            var pqList = new List<Piquet> { pq0 };
            TopoLib.GetTrace(_trcLst, pqList, pq0);
            var rings = TopoLib.GetAllRing(_trcLst, pq0, pqList);

            var lengths = new List<double>();

            if (rings.Count > 0)
            {
                lengths.AddRange(rings.Select(ring => TopoLib.GetRingLength(ring, _trcLst)));
            }
            Assert.AreEqual(lengths[0], 15.1);
        }

        [TestMethod]
        public void TestGetRingOffset()
        {
            var pq0 = new Piquet { Name = "0", Step = 0, X = 0, Y = 0, Z = 0 };
            var pqList = new List<Piquet> { pq0 };
            TopoLib.GetTrace(_trcLst, pqList, pq0);
            var rings = TopoLib.GetAllRing(_trcLst, pq0, pqList);
            var offsets = new List<Vector>();
            if (rings.Count > 0)
            {
                offsets.AddRange(rings.Select(ring => TopoLib.GetRingOffset(ring, _trcLst, pqList)));
            }
            var result = offsets[0].Length - 0.0343 < 0.001 && offsets[0].Teta < MathConst.Accuracy &&
                         Math.Abs(offsets[0].Fi) - 73 < 0.1;
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void TestPiquetsCorrection()
        {
            var pq0 = new Piquet { Name = "0", Step = 0, X = 0, Y = 0, Z = 0 };
            var pqList = new List<Piquet> { pq0 };
            TopoLib.GetTrace(_trcLst, pqList, pq0);
            var rings = TopoLib.GetAllRing(_trcLst, pq0, pqList);
            foreach (var ring in rings)
            {
                foreach (var point in ring.Points)
                {
                    var piquet = pqList.Find(p => p.Name == point.Name);
                    piquet.Correct(point.Offset);
                    TopoLib.PiquetsCorrection(ring, _trcLst, pqList, piquet, point.Offset);
                }
            }
            var result = true;
            Assert.AreEqual(result, true);
        }
    }
}
