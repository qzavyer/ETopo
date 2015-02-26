using System;
using System.Collections.Generic;
using System.Linq;

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
                Step = start.Step + 1,
                Distance = start.Distance + trace.Tape
            };
            return piquet;
        }

        public static void GetTrace(List<Trace> trcList, List<Piquet> pqList, Piquet start)
        {
            foreach (var trace in trcList.Where(t => t.From == start.Name))
            {
                var exists = false;
                var trace1 = trace;
                foreach (var piquet in pqList.Where(p => p.Name == trace1.To))
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
                var trace1 = trace;
                foreach (var piquet in pqList.Where(p => p.Name == trace1.From))
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

        public static List<Ring> GetAllRing(List<Trace> trcLst, List<Piquet> piquets, Piquet start)
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
    }
}
