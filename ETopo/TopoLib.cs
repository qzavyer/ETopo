using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ETopo
{
    public static class TopoLib
    {
        private static Piquet GetPiquet(Trace trace, Piquet start, bool back)
        {
            var piquet = new Piquet
            {
                X =
                    start.X +
                    trace.Tape*Math.Cos(trace.Clino*(back ? -1 : 1)*MathConst.Rad)*
                    Math.Sin((trace.Azimuth + (back ? 180 : 0))*MathConst.Rad),
                Y =
                    start.Y +
                    trace.Tape*Math.Cos(trace.Clino*(back ? -1 : 1)*MathConst.Rad)*
                    Math.Cos((trace.Azimuth + (back ? 180 : 0))*MathConst.Rad),
                Z = start.Z + trace.Tape*Math.Sin(trace.Clino*(back ? -1 : 1)*MathConst.Rad),
                Step = start.Step + 1
            };
            piquet.Distance = start.Distance + trace.Tape;
            return piquet;
        }

        public static void GetTrace(List<Trace> trcList, List<Piquet> pqList, Piquet start)
        {
            foreach (var trace in trcList.Where(t => t.From == start.Name))
            {
                var exists = false;
                foreach (var piquet in pqList.Where(p => p.Name == trace.To))
                {
                    if (piquet.Step > start.Step + 1)
                    {
                        var tmpPq = GetPiquet(trace, start, false);
                        piquet.X = tmpPq.X;
                        piquet.Y = tmpPq.Y;
                        piquet.Z = tmpPq.Z;
                        piquet.Step = tmpPq.Step;
                        piquet.Distance = tmpPq.Distance;
                        GetTrace(trcList, pqList, piquet);
                    }
                    exists = true;
                }
                if (exists) continue;
                var pq = GetPiquet(trace, start, false);
                pq.Name = trace.To;
                pq.Note = trace.Note;
                pqList.Add(pq);
                GetTrace(trcList, pqList, pq);
            }
            foreach (var trace in trcList.Where(t => t.To == start.Name))
            {
                var exists = false;
                foreach (var piquet in pqList.Where(p => p.Name == trace.From))
                {
                    if (piquet.Step > start.Step + 1)
                    {
                        var tmpPq = GetPiquet(trace, start, true);
                        piquet.X = tmpPq.X;
                        piquet.Y = tmpPq.Y;
                        piquet.Z = tmpPq.Z;
                        piquet.Step = tmpPq.Step;
                        piquet.Distance = tmpPq.Distance;
                        GetTrace(trcList, pqList, piquet);
                    }
                    exists = true;
                }
                if (exists) continue;
                var pq = GetPiquet(trace, start, true);
                pq.Name = trace.From;
                pq.Note = trace.Note;
                pqList.Add(pq);
                GetTrace(trcList, pqList, pq);
            }
        }

        public static List<Trace> GetString(List<Piquet> stringPqList, List<Trace> traceList, Piquet prevous, List<Piquet> piquetList)
        {
            var result = new List<Trace>();
            if (traceList == null || traceList.Count == 0) return new List<Trace>();
            foreach (var trace in traceList.Where(trc =>
                trc.From == prevous.Name && !stringPqList.Select(pq => pq.Name).Contains(trc.To)))
            {
                var exPiq = piquetList.Find(p => p.Name == trace.To);
                if (exPiq.Step <= prevous.Step) continue;
                var current = new Piquet(exPiq);
                stringPqList.Add(current);
                result.Add(trace);
                result.AddRange(GetString(stringPqList, traceList, current,piquetList));
            }
            foreach (var trace in traceList.Where(trc =>
                trc.To == prevous.Name && !stringPqList.Select(pq => pq.Name).Contains(trc.From)))
            {
                var exPiq = piquetList.Find(p => p.Name == trace.From);
                if (exPiq.Step <= prevous.Step) continue;
                var current = new Piquet(exPiq);
                stringPqList.Add(current);
                result.Add(trace);
                result.AddRange(GetString(stringPqList, traceList, current, piquetList));
            }
            return result;
        }

        public static List<Ring> GetAllRing(List<Trace> trcLst, Piquet start,List<Piquet> piquets )
        {
            var result = new List<Ring>();
            var pqTemp = new List<Piquet> {start};
            var pString = GetString(pqTemp, trcLst, start, piquets);
            foreach (var trace in trcLst.Where(trc => !pString.Contains(trc)))
            {
                var pqPair = pqTemp.Where(pq => pq.Name == trace.From || pq.Name == trace.To).ToList();
                if(pqPair.Count != 2) continue;
                var step = Math.Max(pqPair[0].Step, pqPair[1].Step);
                var pq1 = pqPair[0];
                var pq2 = pqPair[1];
                var lst1 = new List<Piquet>{pq1};
                var lst2 = new List<Piquet>{pq2};
                while (pq1 != pq2)
                {
                    if (pq1.Step == step)
                    {
                        Piquet pq = null;
                        foreach (var trace1 in trcLst.Where(t => t.To == pq1.Name))
                        {
                            pq = pqTemp.Find(p => p.Name == trace1.From && p.Step == step - 1);
                        }
                        if (pq == null)
                        {
                            foreach (var trace1 in trcLst.Where(t => t.From == pq1.Name))
                            {
                                pq = pqTemp.Find(p => p.Name == trace1.To && p.Step == step - 1);
                            }
                        }
                        if (pq == null) continue;
                        pq1 = pq;
                        lst1.Add(pq1);
                    }
                    if (pq2.Step == step)
                    {
                        Piquet pq = null;
                        foreach (var trace1 in trcLst.Where(t => t.To == pq2.Name))
                        {
                            pq = pqTemp.Find(p => p.Name == trace1.From && p.Step == step - 1);
                        }
                        if (pq == null)
                        {
                            foreach (var trace1 in trcLst.Where(t => t.From == pq2.Name))
                            {
                                pq = pqTemp.Find(p => p.Name == trace1.To && p.Step == step - 1);
                            }
                        }
                        if (pq == null)continue;
                        pq2 = pq;
                        if (pq == pq1) continue;
                        lst2.Add(pq2);
                    }
                    step--;
                }
                lst2.Reverse();
                lst1.AddRange(lst2);
                var ring = new Ring(lst1);
                ring.Length = GetRingLength(ring, trcLst);
                ring.Offset = GetRingOffset(ring, trcLst, piquets);

                var tgOff = ring.Offset.Length/ring.Length;

                var minStep = ring.Points[0].Step;
                var minInd = 0;
                var minDist = 0D;
                foreach (var point in ring.Points.Where(point => point.Step < minStep))
                {
                    minStep = point.Step;
                    minInd = ring.Points.IndexOf(point);
                    minDist = point.Distance;
                }

                foreach (var point in ring.Points)
                {
                    var dirrFi = ring.Points.IndexOf(point) < minInd ? 0 : 180;
                    var dirrTe = ring.Points.IndexOf(point) < minInd ? 1 : -1;
                    var vect = new Vector
                    {
                        Length = tgOff*(point.Distance-minDist),
                        Fi = ring.Offset.Fi + dirrFi,
                        Teta = ring.Offset.Teta*dirrTe
                    };
                    point.Offset = point.Offset + vect;
                }

                result.Add(ring);
            }
            return result;
        }

        public static double GetRingLength(Ring ring, List<Trace> trcLst)
        {
            var piqets = ring.Points;
            var length = 0D;
            for (var i = 0; i < piqets.Count; i++)
            {
                var j = (i + 1)%(piqets.Count);
                var pq1 = piqets[i];
                var pq2 = piqets[j];
                length += trcLst.Where(trc =>
                    trc.From == pq1.Name && trc.To == pq2.Name || trc.From == pq2.Name && trc.To == pq1.Name)
                    .Sum(trace => trace.Tape);
            }
            return length;
        }

        public static Vector GetRingOffset(Ring ring, List<Trace> trcLst, List<Piquet> piquets)
        {
            if (trcLst == null || piquets == null || ring == null || ring.Points.Count == 0) return null;
            var startName = ring.Points.First().Name;
            var endName = ring.Points.Last().Name;
            foreach (var trace in trcLst.Where(t=>t.From==startName && t.To==endName))
            {
                var start = piquets.Find(p => p.Name == startName);
                var end1 = piquets.Find(p => p.Name == endName);
                var end2 = GetPiquet(trace, start, false);
                var x = end1.X - end2.X;
                var y = end1.Y - end2.Y;
                var z = end1.Z - end2.Z;
                return new Vector(x, y, z);
            }
            foreach (var trace in trcLst.Where(t => t.From == endName && t.To == startName))
            {
                var start = piquets.Find(p => p.Name == endName);
                var end1 = piquets.Find(p => p.Name == startName);
                var end2 = GetPiquet(trace, start, false);
                var x = end1.X - end2.X;
                var y = end1.Y - end2.Y;
                var z = end1.Z - end2.Z;
                return new Vector(x, y, z);
            }
            return null;
        }

        public static void PiquetsCorrection(Ring ring, List<Trace> trcList, List<Piquet> pqList, Piquet start,Vector offset)
        {
            foreach (var trace in trcList.Where(t=>t.From==start.Name))
            {
                var currentPiquet = pqList.Find(p => p.Name == trace.To);
                if (currentPiquet.Step == start.Step + 1&&!ring.Points.Select(p=>p.Name).Contains(currentPiquet.Name))
                {
                    currentPiquet.Correct(offset);
                    PiquetsCorrection(ring, trcList, pqList, currentPiquet, offset);
                }
            }
            foreach (var trace in trcList.Where(t => t.To == start.Name))
            {
                var currentPiquet = pqList.Find(p => p.Name == trace.From);
                if (currentPiquet.Step == start.Step + 1 && !ring.Points.Select(p => p.Name).Contains(currentPiquet.Name))
                {
                    currentPiquet.Correct(offset);
                    PiquetsCorrection(ring, trcList, pqList, currentPiquet, offset);
                }
            }
        }

        public  static List<Piquet> GetTrace(List<Piquet> pqList,List<Trace> trcList, Piquet start, Piquet prev)
        {
            var lst1 = new List<Piquet>();
            foreach (var trace in trcList.Where(trace => trace.From == start.Name && trace.To != prev.Name))
            {
                foreach (var piquet in pqList)
                {
                    if (piquet.Name == trace.To && piquet.Step < start.Step)
                    {
                        lst1.Add(piquet);
                        lst1.AddRange(GetTrace(pqList, trcList, piquet, prev));
                    }
                }
            }
            foreach (var trace in trcList.Where(trace => trace.To == start.Name && trace.From != prev.Name))
            {
                foreach (var piquet in pqList)
                {
                    if (piquet.Name == trace.From && piquet.Step < start.Step)
                    {
                        lst1.Add(piquet);
                        lst1.AddRange(GetTrace(pqList, trcList, piquet, prev));
                    }
                }
            }
            return lst1;
        }

        public static List<Piquet> GetRing(List<Piquet> pqList, List<Trace> trcList, Piquet start, Piquet prev)
        {
            var lst1 = GetTrace(pqList, trcList, start, prev);
            lst1.Add(start);
            var lst2 = GetTrace(pqList, trcList, prev, start);
            lst2.Add(prev);
            var lst3 = new List<Piquet>();
            lst3.AddRange(lst1.Where(p => !lst2.Exists(m => m == p)));
            //lst3.AddRange(lst1);
            lst3.AddRange(lst2.Where(p => !lst1.Exists(m => m == p)));
            var max = 0;
            Piquet pq = null;
            foreach (
                var piquet in
                    from piquet in lst1
                    from piquet1 in lst2
                    where piquet.Name == piquet1.Name && piquet.Step > max
                    select piquet)
            {
                max = piquet.Step;
                pq = piquet;
            }
            lst3.Add(pq);
            return lst3;
        }

        public static double GetRingLength(List<Piquet> pqList, List<Trace> trcList, Piquet start, Piquet prev)
        {
            var lst1 = GetTrace(pqList, trcList, start, prev);
            lst1.Add(start);
            var lst2 = GetTrace(pqList, trcList, prev, start);
            lst2.Add(prev);
            var lst3 = new List<Piquet>();
            lst3.AddRange(lst1.Where(p => !lst2.Exists(m => m == p)));
            lst3.AddRange(lst2.Where(p => !lst1.Exists(m => m == p)));

            var max = 0;
            Piquet pq = null;
            foreach (Piquet piquet in lst1)
                foreach (Piquet piquet1 in lst2)
                {
                    if (piquet.Name == piquet1.Name && piquet.Step > max)
                    {
                        max = piquet.Step;
                        pq = piquet;
                    }
                }
            if(pq!=null)
                lst3.Add(pq);
            var lst4 = new List<Piquet>(lst3);

            double sum = 0;
            foreach (Trace trace in trcList)
            {
                double sum1 = 0;
                foreach (Piquet pqt in lst3)
                {
                    if (pqt.Name == trace.From)
                    {
                        double sum2 = 0;
                        foreach (Piquet pqt4 in lst4)
                        {
                            if (pqt4.Name == trace.To) sum2 += trace.Tape;
                        }
                        sum1 += sum2;
                    }
                }
                sum += sum1;
            }
            return sum;
        }

        public static double GetTraceLength(List<Piquet> pqList, List<Trace> trcList, Piquet start)
        {
            var len = 0.0;
            foreach (var trace in trcList.Where(t => t.From == start.Name))
            {
                foreach (var piquet in pqList.Where(piquet => piquet.Name == trace.To && piquet.Step < start.Step))
                {
                    len = trace.Tape;
                    len += GetTraceLength(pqList, trcList, piquet);
                }
            }
            foreach (var trace in trcList.Where(t => t.To == start.Name))
            {
                foreach (var piquet in pqList.Where(piquet => piquet.Name == trace.From && piquet.Step < start.Step))
                {
                    len = trace.Tape;
                    len += GetTraceLength(pqList, trcList, piquet);
                }
            }
            return len;
        }

        public static double CheckRing(List<Piquet> pqList, List<Trace> trcList, Piquet start, Piquet piquet,
            string prev, int step)
        {
            var dx = 0.0;
            var dy = 0.0;
            var dz = 0.0;

            var errX = 0.0;
            var errY = 0.0;
            var errZ = 0.0;
            foreach (var p in pqList.Where(p => p.Name == piquet.Name))
            {
                errX = p.X - piquet.X;
                errY = p.Y - piquet.Y;
                errZ = p.Z - piquet.Z;
                if (p.Step < piquet.Step)
                {
                    dx = (p.X - piquet.X)/(step - p.Step);
                    dy = (p.Y - piquet.Y)/(step - p.Step);
                    dz = (p.Z - piquet.Z)/(step - p.Step);
                    piquet.X += dx;
                    piquet.Y += dy;
                    piquet.Z += dz;
                    break;
                }


                if (p.Step > piquet.Step)
                {
                    dx = (piquet.X - p.X)/(p.Step - step);
                    dy = (piquet.Y - p.Y)/(p.Step - step);
                    dz = (piquet.Z - p.Z)/(p.Step - step);
                    p.X += dx;
                    p.Y += dy;
                    p.Z += dz;
                    break;
                }
            }

            foreach (var trace in trcList)
            {
                if (trace.From == start.Name)
                {
                    foreach (var pq in pqList.Where(pq => pq.Name == trace.To && pq.Step == step + 1))
                    {
                        pq.X += dx;
                        pq.Y += dy;
                        pq.Z += dz;
                        CheckRing(pqList, trcList, pq, piquet, prev, step + 1);
                    }
                }
                if (trace.To == start.Name)
                {
                    foreach (var pq in pqList.Where(pq => pq.Name == trace.From && pq.Step == step + 1))
                    {
                        pq.X += dx;
                        pq.Y += dy;
                        pq.Z += dz;
                        CheckRing(pqList, trcList, pq, piquet, prev, step + 1);
                    }
                }
            }
            Piquet curr = null;
            foreach (var pq in pqList.Where(pq => pq.Name == start.Name))
            {
                curr = pq;
            }
            Piquet prv = null;
            foreach (var pq in pqList.Where(pq => pq.Name == prev))
            {
                prv = pq;
            }
            var len = GetRingLength(pqList, trcList, curr, prv);
            var err = Math.Sqrt(errX*errX + errY*errY + errZ*errZ);
            return err/len;
        }

        public static void SetSteps(List<Piquet> pqList, List<Trace> trcList, Piquet start, int step)
        {
            var flag = false;
            foreach (var piquet in pqList.Where(piquet => piquet.Name == start.Name && piquet.Step > step))
            {
                piquet.Step = step;
                flag = true;
            }

            if (!flag) return;
            foreach (var trace in trcList)
            {
                if (trace.From == start.Name)
                {
                    var pq = new Piquet();
                    foreach (var piquet in pqList.Where(p => p.Name == trace.To))
                    {
                        pq = piquet;
                    }
                    SetSteps(pqList, trcList, pq, step + 1);
                }
                if (trace.To == start.Name)
                {
                    var pq = new Piquet();
                    foreach (var piquet in pqList.Where(p => p.Name == trace.From))
                    {
                        pq = piquet;
                    }
                    SetSteps(pqList, trcList, pq, step + 1);
                }
            }
        }

        /// <summary>
        /// Построение треугольника смещений
        /// </summary>
        /// <param type="List(Piquet)" name="pqringList">Список пикетов, входящих в кольцовку</param>
        /// <param type="List(Trace)" name="trcList">Список топосъемок</param>
        /// <param type="Piquet" name="pqCurr">Текущий пикет</param>
        /// <param type="Piquet" name="pqPrev">Предыдущий пикет</param>
        /// <param type="Piquet" name="pqStart">Начальный пикет</param>
        /// <param type="double" name="tg">Тангенс угла треугольника смещений</param>
        /// <return>Длина вектора смещения текущего пикета</return>
        /// <example>
        /// var offset = Edit(ring, trace, curr, prev, start, tg);
        /// </example>
        private static double Edit(List<Piquet> pqringList, List<Trace> trcList, Piquet pqCurr, Piquet pqPrev,
            Piquet pqStart, double tg)
        {
            var sum = 0.0;
            foreach (var trace in trcList)
            {
                foreach (var piquet in pqringList.Where(p => p.Name != pqPrev.Name))
                {
                    if (trace.From == pqPrev.Name && trace.To == piquet.Name && piquet.Name != pqStart.Name)
                    {
                        sum = tg*trace.Tape + Edit(pqringList, trcList, piquet, pqCurr, pqStart, tg);
                        piquet.Delta = sum;
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// Смещение гипотенузы треугольника смещений
        /// </summary>
        /// <param type="List(Piquet)" name="pqringList">Список пикетов, входящих в кольцовку</param>
        /// <example>
        /// RingOffset(ring);
        /// </example>
        private static void RingOffset(List<Piquet> pqringList)
        {
            var pq = pqringList[0];
            foreach (var piquet in pqringList)
            {
                if (pq.Step > piquet.Step)
                {
                    pq = piquet;
                }
            }
            foreach (var piquet in pqringList)
            {
                piquet.Delta -= pq.Delta;
            }
        }

        /// <summary>
        /// Получение направления смещения
        /// </summary>
        /// <param type="Piquet" name="piquet1">Пикет невязки с одного конца</param>
        /// <param type="Piquet" name="piquet2">Пикет невязки с другого конца</param>
        /// <return>Длину вектора смещения текущего пикета</return>
        /// <example>
        /// <code>
        /// var vect = GetVector(ring[0],ring[10]);
        /// </code>
        /// </example>
        private static Vector GetVector(Piquet piquet1, Piquet piquet2)
        {
            var y = piquet2.Y - piquet1.Y;
            var x = piquet2.X - piquet1.X;
            var z = piquet2.Z - piquet1.Z;
            var vector = new Vector
            {
                Length = Math.Sqrt(x*x + y*y + z*z),
                Fi = Math.Atan(y/x),
                Teta = Math.Atan(Math.Sqrt(x*x + y*y)/z)
            };
            return vector;
        }

        /// <summary>
        /// Изменение координат пикетов в существующем списке
        /// </summary>
        /// <param type="List(Piquet)" name="pqList">Список существующих пикетов</param>
        /// <param type="List(Piquet)" name="pqringList">Список пикетов, входящих в кольцовку</param>
        /// <param type="List(Trace)" name="trcList">Список топосъемок</param>
        /// <param type="Piquet" name="pqStart">Начальный пикет</param>
        /// <param type="double" name="vector">Угол вектора смещения</param>
        /// <param type="double" name="length">Величина смещения</param>
        /// <example>
        /// <code>
        /// EditPiquerCoords(pqList, ring, trace, start, vector, len);
        /// </code>
        /// </example>
        private static void EditPiquetCoords(List<Piquet> pqList, List<Piquet> pqringList, List<Trace> trcList,
            Piquet pqStart, Vector vector, double length)
        {
            pqStart.X -= Math.Sin(vector.Teta)*Math.Cos(vector.Fi)*length;
            pqStart.Y -= Math.Cos(vector.Teta)*Math.Sin(vector.Fi)*length;
            pqStart.Z -= Math.Cos(vector.Teta)*length;
            foreach (var piquet in pqList)
            {
                foreach (var pq in pqringList)
                    if (pq != piquet && piquet.Step > pq.Step)
                    {
                        foreach (var trace in trcList)
                        {
                            if (trace.From == pqStart.Name && trace.To == piquet.Name ||
                                trace.To == pqStart.Name && trace.From == piquet.Name)
                            {
                                EditPiquetCoords(pqList, pqringList, trcList, piquet, vector, length);
                            }
                        }
                    }
            }
        }
    }
}
