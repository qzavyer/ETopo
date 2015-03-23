using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ETopo.Properties;
using Ionic.Zip;
using Tao.FreeGlut;

namespace ETopo
{
    public partial class FrMain : Form
    {
        public SurveyData _surData = new SurveyData();
        private List<Piquet> _piquetLst = new List<Piquet>();
        private List<Trace> _traceList = new List<Trace>();
        private List<Spline> _splineList = new List<Spline>();
        private List<Cgn> _cgnList = new List<Cgn>();
        private string _name;
        private string _date;
        private List<string> _autor;

        public FrMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            dgTopo.BackgroundImage = Resources.cell;
        }

        private void LoadMenu_Click(object sender, EventArgs e)
        {
            if (odLoad.ShowDialog() != DialogResult.OK) return;
            try
            {
                dgTopo.Rows.Clear();
                _traceList = new List<Trace>();
                _piquetLst=new List<Piquet>();
                using (var zip = ZipFile.Read(odLoad.FileName))
                {
                    using (var fl = new MemoryStream())
                    {
                        var ent = zip["trace.xml"];
                        ent.Extract(fl);
                        fl.Position = 0;
                        using (var reader = XmlReader.Create(fl))
                        {

                            var trc = new Trace();
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    switch (reader.Name)
                                    {
                                        case "team":
                                            _surData.Team = new List<string>();
                                            break;
                                        case "name":
                                            reader.Read();
                                            _surData.Team.Add(reader.Value);
                                            break;
                                        case "date":
                                            reader.Read();
                                            try
                                            {
                                                _surData.Date = Convert.ToDateTime(reader.Value);
                                            }
                                            catch
                                            {
                                                _surData.Date = new DateTime();
                                            }
                                            break;
                                        case "title":
                                            reader.Read();
                                            _surData.Name = reader.Value;
                                            break;
                                        case "segment":
                                            dgTopo.Rows.Add();
                                            trc = new Trace();
                                            break;
                                        case "from":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[0].Value = reader.Value;
                                            trc.From = reader.Value;
                                            break;
                                        case "to":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[1].Value = reader.Value;
                                            trc.To = reader.Value;
                                            break;
                                        case "tape":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[2].Value = reader.Value;
                                            trc.Tape = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "compass":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[3].Value = reader.Value;
                                            trc.Azimuth = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "clino":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[4].Value = reader.Value;
                                            trc.Clino = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "left":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[5].Value = reader.Value;
                                            trc.Left = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "right":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[6].Value = reader.Value;
                                            trc.Right = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "up":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[7].Value = reader.Value;
                                            trc.Up = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "down":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[8].Value = reader.Value;
                                            trc.Down = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "f_left":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[5].Value = reader.Value + "\\" +
                                                dgTopo.Rows[dgTopo.RowCount - 2].Cells[5].Value;
                                            trc.FromLeft = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "f_right":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[6].Value = reader.Value + "\\" +
                                                dgTopo.Rows[dgTopo.RowCount - 2].Cells[6].Value;
                                            trc.FromRight = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "f_up":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[7].Value = reader.Value + "\\" +
                                                dgTopo.Rows[dgTopo.RowCount - 2].Cells[7].Value;
                                            trc.FromUp = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "f_down":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[8].Value = reader.Value + "\\" +
                                                dgTopo.Rows[dgTopo.RowCount - 2].Cells[8].Value;
                                            trc.FromDown = Convert.ToDouble(reader.Value.Replace('.', ','));
                                            break;
                                        case "note":
                                            reader.Read();
                                            dgTopo.Rows[dgTopo.RowCount - 2].Cells[9].Value = reader.Value;
                                            trc.Note = reader.Value;
                                            break;
                                    }

                                }
                                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "segment")
                                {
                                    _traceList.Add(trc);
                                }
                            } //while(reader.Read())

                        }
                    }
                    using (var fl = new MemoryStream())
                    {
                        var pqEnt = zip["piquet.xml"];
                        pqEnt.Extract(fl);
                        fl.Position = 0;
                        using (var pqReader = XmlReader.Create(fl))
                        {

                            var piquet = new Piquet();
                            while (pqReader.Read())
                            {
                                if (pqReader.NodeType == XmlNodeType.Element)
                                {
                                    switch (pqReader.Name)
                                    {
                                        case "piquet":
                                            if (pqReader.IsEmptyElement) break;
                                            piquet = new Piquet();
                                            break;
                                        case "name":
                                            if (pqReader.IsEmptyElement) break;
                                            pqReader.Read();
                                            piquet.Name = pqReader.Value;
                                            break;
                                        case "note":
                                            if(pqReader.IsEmptyElement) break;
                                            pqReader.Read();
                                            piquet.Note = pqReader.Value;
                                            break;
                                        case "x":
                                            if (pqReader.IsEmptyElement) break;
                                            pqReader.Read();
                                            piquet.X = Convert.ToDouble(pqReader.Value.Replace('.', ','));
                                            break;
                                        case "y":
                                            if (pqReader.IsEmptyElement) break;
                                            pqReader.Read();
                                            piquet.Y = Convert.ToDouble(pqReader.Value.Replace('.', ','));
                                            break;
                                        case "z":
                                            if (pqReader.IsEmptyElement) break;
                                            pqReader.Read();
                                            piquet.Z = Convert.ToDouble(pqReader.Value.Replace('.', ','));
                                            break;
                                        case "step":
                                            pqReader.Read();
                                            piquet.Step = Convert.ToInt32(pqReader.Value);
                                            break;
                                    }

                                }
                                else if (pqReader.NodeType == XmlNodeType.EndElement && pqReader.Name == "piquet")
                                {
                                    _piquetLst.Add(piquet);
                                }
                            } //while(reader.Read())

                        }
                    }

                    using (var fl = new MemoryStream())
                    {
                        var splEnt = zip["spline.xml"];
                        try
                        {
                            splEnt.Extract(fl);
                            fl.Position = 0;
                            using (var splReader = XmlReader.Create(fl))
                            {

                                var spline = new Spline();
                                while (splReader.Read())
                                {
                                    if (splReader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (splReader.Name)
                                        {
                                            case "spline":
                                                if (splReader.IsEmptyElement) break;
                                                spline = new Spline();
                                                break;
                                            case "name":
                                                if (splReader.IsEmptyElement) break;
                                                splReader.Read();
                                                spline.Name = splReader.Value;
                                                break;
                                            case "type":
                                                if (splReader.IsEmptyElement) break;
                                                splReader.Read();
                                                spline.Type =
                                                    (SplineType) Enum.Parse(typeof (SplineType), splReader.Value);
                                                break;
                                            case "dirrection":
                                                if (splReader.IsEmptyElement) break;
                                                splReader.Read();
                                                spline.Dirrection =
                                                    (SplineDirrection)
                                                        Enum.Parse(typeof (SplineDirrection), splReader.Value);
                                                break;
                                            case "points":
                                                while (splReader.Read()&&(splReader.NodeType != XmlNodeType.EndElement ||
                                                       splReader.Name != "points"))
                                                {
                                                    var tX = 0D;
                                                    var tY = 0D;
                                                    var tRaX = 0D;
                                                    var tRaY = 0D;
                                                    var tRbX = 0D;
                                                    var tRbY = 0D;
                                                    while (splReader.Read()&&(splReader.NodeType != XmlNodeType.EndElement ||
                                                           splReader.Name != "point"))
                                                    {
                                                        if (splReader.NodeType == XmlNodeType.Element)
                                                        {
                                                            switch (splReader.Name)
                                                            {
                                                                case "x":
                                                                    if (splReader.IsEmptyElement) break;
                                                                    splReader.Read();
                                                                    tX =
                                                                        Convert.ToDouble(splReader.Value.Replace('.',
                                                                            ','));
                                                                    break;
                                                                case "y":
                                                                    if (splReader.IsEmptyElement) break;
                                                                    splReader.Read();
                                                                    tY =
                                                                        Convert.ToDouble(splReader.Value.Replace('.',
                                                                            ','));
                                                                    break;
                                                                case "ra_x":
                                                                    if (splReader.IsEmptyElement) break;
                                                                    splReader.Read();
                                                                    tRaX =
                                                                        Convert.ToDouble(splReader.Value.Replace('.',
                                                                            ','));
                                                                    break;
                                                                case "ra_y":
                                                                    if (splReader.IsEmptyElement) break;
                                                                    splReader.Read();
                                                                    tRaY =
                                                                        Convert.ToDouble(splReader.Value.Replace('.',
                                                                            ','));
                                                                    break;
                                                                case "rb_x":
                                                                    if (splReader.IsEmptyElement) break;
                                                                    splReader.Read();
                                                                    tRbX =
                                                                        Convert.ToDouble(splReader.Value.Replace('.',
                                                                            ','));
                                                                    break;
                                                                case "rb_y":
                                                                    if (splReader.IsEmptyElement) break;
                                                                    splReader.Read();
                                                                    tRbY =
                                                                        Convert.ToDouble(splReader.Value.Replace('.',
                                                                            ','));
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    spline.PointList.Add(new SplinePoint((float) tX, (float) tY,
                                                        (float) tRaX,
                                                        (float) tRaY, (float) tRbX, (float) tRbY));
                                                }
                                                break;
                                        }

                                    }
                                    else if (splReader.NodeType == XmlNodeType.EndElement && splReader.Name == "spline")
                                    {
                                        _splineList.Add(spline);
                                    }
                                } //while(reader.Read())

                            }
                        }
                        catch
                        {
                        }
                    }

                    using (var fl = new MemoryStream())
                    {
                        var splEnt = zip["cgn.xml"];
                        try
                        {
                            splEnt.Extract(fl);
                            fl.Position = 0;
                            using (var cgnReader = XmlReader.Create(fl))
                            {
                                var x = 0D;
                                var y = 0D;
                                var changex = false;
                                var changey = false;
                                var cgn = new Cgn();
                                while (cgnReader.Read())
                                {
                                    if (cgnReader.NodeType == XmlNodeType.Element)
                                    {
                                        switch (cgnReader.Name)
                                        {
                                            case "cgn":
                                                if (cgnReader.IsEmptyElement) break;
                                                cgn = new Cgn();
                                                break;
                                            case "name":
                                                if (cgnReader.IsEmptyElement) break;
                                                cgnReader.Read();
                                                cgn.Name = cgnReader.Value;
                                                break;
                                            case "type":
                                                if (cgnReader.IsEmptyElement) break;
                                                cgnReader.Read();
                                                cgn.Type = (CgnType) Enum.Parse(typeof (CgnType), cgnReader.Value);
                                                break;
                                            case "x":
                                                if (cgnReader.IsEmptyElement) break;
                                                cgnReader.Read();
                                                x = Convert.ToDouble(cgnReader.Value.Replace('.', ','));
                                                changex = true;
                                                if(changex&&changey)
                                                    cgn.Point = new Point((float)x, (float)y);
                                                break;
                                            case "y":
                                                if (cgnReader.IsEmptyElement) break;
                                                cgnReader.Read();
                                                y = Convert.ToDouble(cgnReader.Value.Replace('.', ','));
                                                changey = true;
                                                if (changex && changey)
                                                    cgn.Point = new Point((float)x, (float)y);
                                                break;
                                            case "angle":
                                                if (cgnReader.IsEmptyElement) break;
                                                cgnReader.Read();
                                                cgn.Angle = Convert.ToDouble(cgnReader.Value.Replace('.', ','));
                                                break;
                                        }

                                    }
                                    else if (cgnReader.NodeType == XmlNodeType.EndElement && cgnReader.Name == "cgn")
                                    {
                                        _cgnList.Add(cgn);
                                        x = 0D;
                                        y = 0D;
                                        changex = false;
                                        changey = false;
                                    }
                                } //while(reader.Read())

                            }
                        }
                        catch
                        {
                        }

                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show(Resources.FileReadError,Resources.ETopo,MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                var min = 0.0;
                var max = 0.0;
                foreach (var piquet in _piquetLst)
                {
                    min = Math.Min(min, piquet.Z);
                    max = Math.Max(max, piquet.Z);
                }
            }
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            if (sdSave.ShowDialog() != DialogResult.OK) return;
            _traceList = new List<Trace>();
            _piquetLst = new List<Piquet>();
            foreach (DataGridViewRow row in dgTopo.Rows)
            {
                if (row.Cells[0].Value == null) continue;
                var trace = new Trace
                {
                    From = row.Cells["ClFrom"].Value.ToString(),
                    To = row.Cells["ClTo"].Value.ToString(),
                    Tape = Convert.ToDouble(row.Cells["ClLen"].Value.ToString().Replace('.', ',')),
                    Azimuth = Convert.ToDouble(row.Cells["ClAz"].Value.ToString().Replace('.', ',')),
                    Clino = Convert.ToDouble(row.Cells["ClClino"].Value.ToString().Replace('.', ',')),
                    Note = row.Cells["ClNote"].Value == null ? "" : row.Cells["ClNote"].Value.ToString()
                };
                var down = row.Cells["ClDown"].Value.ToString().Replace('.', ',');
                var dVals = down.Split('\\');
                if (dVals.Count() == 2)
                {
                    trace.FromDown = Convert.ToDouble(dVals[0]);
                    trace.Down = Convert.ToDouble(dVals[1]);
                }
                else
                {
                    trace.Down = Convert.ToDouble(down);
                }
                var up = row.Cells["ClUp"].Value.ToString().Replace('.', ',');
                var uVals = up.Split('\\');
                if (uVals.Count() == 2)
                {
                    trace.FromUp = Convert.ToDouble(uVals[0]);
                    trace.Up = Convert.ToDouble(uVals[1]);
                }
                else
                {
                    trace.Up = Convert.ToDouble(up);
                }
                var left = row.Cells["ClLeft"].Value.ToString().Replace('.', ',');
                var lVals = left.Split('\\');
                if (lVals.Count() == 2)
                {
                    trace.FromLeft = Convert.ToDouble(lVals[0]);
                    trace.Left = Convert.ToDouble(lVals[1]);
                }
                else
                {
                    trace.Left = Convert.ToDouble(left);
                }
                var right = row.Cells["ClRight"].Value.ToString().Replace('.', ',');
                var rVals = right.Split('\\');
                if (rVals.Count() == 2)
                {
                    trace.FromRight = Convert.ToDouble(rVals[0]);
                    trace.Right = Convert.ToDouble(rVals[1]);
                }
                else
                {
                    trace.Right = Convert.ToDouble(right);
                }
                _traceList.Add(trace);
            }
            var start = new Piquet
            {
                Delta = 0D,
                Name = _traceList[0].From,
                X = 0D,
                Y = 0D,
                Z = 0D,
                Step = 0,
                Note = ""
            };
            _piquetLst.Add(start);
            TopoLib.GetTrace(_traceList, _piquetLst, start);
            var min = 0.0;
            var max = 0.0;
            foreach (var piquet in _piquetLst)
            {
                min = Math.Min(min, piquet.Z);
                max = Math.Max(max, piquet.Z);
            }

            var rings = TopoLib.GetAllRing(_traceList, _piquetLst, start);

            MessageBox.Show(string.Format("Длина: {0:N0}м. Глубина: {1:N0}м. Неточность: {2:F3}%",
                _traceList.Sum(t => t.Tape), max - min, rings.Count > 0
                    ? (rings.Sum(r => r.Offset.Length)/rings.Sum(r => r.Length)*100)
                    : 0));

            foreach (var ring in rings)
            {
                foreach (var point in ring.Points)
                {
                    TopoLib.PiquetsCorrection(ring, _traceList, _piquetLst, point, point.Offset);
                }
            }
            using (var fl = new FileStream(sdSave.FileName, FileMode.Create))
            {
                using (var zipp = new ZipOutputStream(fl))
                {
                    zipp.PutNextEntry("trace.xml");
                    using (var wr = new MemoryStream())
                    {
                        var writer = XmlWriter.Create(wr);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("cave");
                        writer.WriteElementString("title", _surData.Name);
                        writer.WriteStartElement("survey");
                        writer.WriteElementString("date", _surData.Date==null?"":_surData.Date.Value.ToString("dd.MM.yyyy"));
                        writer.WriteStartElement("team");
                        if(_surData.Team!=null)
                            foreach (var item in _surData.Team)
                            {
                                writer.WriteElementString("name", item);
                            }
                        writer.WriteEndElement();
                        foreach (var trace in _traceList)
                        {
                            writer.WriteStartElement("segment");
                            writer.WriteElementString("from", trace.From);
                            writer.WriteElementString("to", trace.To);
                            writer.WriteElementString("tape", trace.Tape.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("compass", trace.Azimuth.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("clino", trace.Clino.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("left", trace.Left.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("right", trace.Right.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("up", trace.Up.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("down", trace.Down.ToString(CultureInfo.InvariantCulture));
                            if (trace.FromLeft > 0)
                                writer.WriteElementString("f_left",
                                    trace.FromLeft.ToString(CultureInfo.InvariantCulture));
                            if (trace.FromRight > 0)
                                writer.WriteElementString("f_right",
                                    trace.FromRight.ToString(CultureInfo.InvariantCulture));
                            if (trace.FromUp > 0)
                                writer.WriteElementString("f_up", trace.FromUp.ToString(CultureInfo.InvariantCulture));
                            if (trace.FromDown > 0)
                                writer.WriteElementString("f_down",
                                    trace.FromDown.ToString(CultureInfo.InvariantCulture));
                            if (!string.IsNullOrEmpty(trace.Note))
                                writer.WriteElementString("note", trace.Note);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        zipp.Write(wr.ToArray(), 0, (int) wr.Length);
                    }
                    zipp.PutNextEntry("piquet.xml");
                    using (var wr = new MemoryStream())
                    {
                        var writer = XmlWriter.Create(wr);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("cave");
                        foreach (var piquet in _piquetLst)
                        {
                            writer.WriteStartElement("piquet");
                            writer.WriteElementString("name", piquet.Name);
                            writer.WriteElementString("x", piquet.X.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("y", piquet.Y.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("z", piquet.Z.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("step", piquet.Step.ToString(CultureInfo.InvariantCulture));
                            writer.WriteElementString("note", piquet.Note);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        zipp.Write(wr.ToArray(), 0, (int) wr.Length);
                    }

                    zipp.PutNextEntry("spline.xml");
                    using (var wr = new MemoryStream())
                    {
                        var writer = XmlWriter.Create(wr);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("cave");
                        foreach (var spline in _splineList)
                        {
                            writer.WriteStartElement("spline");
                            writer.WriteElementString("name", spline.Name);
                            writer.WriteElementString("type", spline.Type.ToString());
                            writer.WriteElementString("dirrection", spline.Dirrection.ToString());
                            writer.WriteStartElement("points");
                            foreach (var point in spline.PointList)
                            {
                                writer.WriteStartElement("point");
                                writer.WriteElementString("x", point.Point.X.ToString());
                                writer.WriteElementString("y", point.Point.Y.ToString());
                                writer.WriteElementString("ra_x", point.Ra.X.ToString());
                                writer.WriteElementString("ra_y", point.Ra.Y.ToString());
                                writer.WriteElementString("rb_x", point.Rb.X.ToString());
                                writer.WriteElementString("rb_y", point.Rb.Y.ToString());
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        zipp.Write(wr.ToArray(), 0, (int) wr.Length);
                    }

                    zipp.PutNextEntry("cgn.xml");
                    using (var wr = new MemoryStream())
                    {
                        var writer = XmlWriter.Create(wr);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("cave");
                        foreach (var cgn in _cgnList)
                        {
                            writer.WriteStartElement("cgn");
                            writer.WriteElementString("name", cgn.Name);
                            writer.WriteElementString("type", cgn.Type.ToString());
                            writer.WriteElementString("x", cgn.Point.X.ToString());
                            writer.WriteElementString("y", cgn.Point.Y.ToString());
                            writer.WriteElementString("angle", cgn.Angle.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        zipp.Write(wr.ToArray(), 0, (int) wr.Length);
                    }
                }
            }
            MessageBox.Show(Resources.Saved);
        }
        
        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_piquetLst==null||_piquetLst.Count==0||_traceList==null||_traceList.Count==0) return;
            var fr = new Graph
            {
                PqList = _piquetLst,
                TrcList = _traceList,
                SplList = _splineList,
                CgnList = _cgnList
            };
            var d = Math.Max(_traceList.Max(t => t.Left), _traceList.Max(t => t.Right));
            fr.top = _piquetLst.Max(p => p.Y)+d;
            fr.bottom = _piquetLst.Min(p => p.Y)-d;
            fr.left = _piquetLst.Min(p=>p.X)-d;
            fr.right = _piquetLst.Max(p=>p.X)+d;
            fr.ShowDialog();
        }

        private void mData_Click(object sender, EventArgs e)
        {
            var fr = new FrTopoData
            {
                name = _surData.Name,
                date = _surData.Date == null ? DateTime.Now : _surData.Date.Value,
                autor = _surData.Team
            };
            fr.ShowDialog();
            _surData.Name = fr.name;
            _surData.Date = fr.date;
            _surData.Team = fr.autor;
        }

        private void ExitMenu_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
