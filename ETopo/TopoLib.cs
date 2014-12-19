using System;
using System.Collections.Generic;
using System.Linq;

namespace ETopo
{
    internal static class TopoLib
    {
        public static List<Piquet> GetTrace(List<Piquet> pqList, List<Trace> trcList, Piquet start, Piquet prev)
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

        /// <summary>
        /// Направление вектора сплайна
        /// </summary>
        /// <param type="List(Piquet)" name="lst">Список точек сплайна</param>
        private static void GetVector(List<Spline> lst)
        {
            for (var i = 1; i < lst.Count - 2; i++)
            {
                var g1 = (lst[i].Y - lst[i - 1].Y)*(1 + lst[i].Bias);
                var g2 = (lst[i + 1].Y - lst[i].Y)*(1 - lst[i].Bias);
                var g3 = g2 - g1;
                lst[i].Ra = (1 - lst[1].Tens)*(g1 + 0.5*g3*(1 + lst[i].Cont));
                lst[i].Rb = (1 - lst[1].Tens)*(g1 + 0.5*g3*(1 - lst[i].Cont));
            }
            lst[0].Ra = lst[1].Y - lst[0].Y;
            lst[0].Rb = (1.5*(lst[1].Y - lst[0].Y) - 0.5*lst[1].Ra)*(1 - lst[0].Tens);
            lst[lst.Count - 1].Rb = (1.5*(lst[lst.Count - 1].Y - lst[lst.Count - 2].Y) - 0.5*lst[lst.Count - 2].Ra)*
                                    (1 - lst[lst.Count - 1].Tens);
            lst[lst.Count - 1].Ra = lst[lst.Count - 1].Y - lst[lst.Count - 2].Y;
        }

    }
}
