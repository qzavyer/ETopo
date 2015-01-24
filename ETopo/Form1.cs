using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Ionic.Zip;
using Tao.FreeGlut;

namespace ETopo
{
    public partial class Form1 : Form
    {
        private const double Rad = Math.PI/180;
        private List<Piquet> _piquetLst = new List<Piquet>();
        private List<Trace> _traceList = new List<Trace>();
        private string _name;
        private string _date;
        private List<string> _autor;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (odLoad.ShowDialog() != DialogResult.OK) return;
            var sum = 0.0;
            var err = 0.0;
            try
            {

                using (var zip = ZipFile.Read(odLoad.FileName))
                {
                    var fl = new MemoryStream();
                    var ent = zip["cave.xml"];
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
                                                                                          dgTopo.Rows[
                                                                                              dgTopo.RowCount - 2]
                                                                                              .Cells[5].Value;
                                        trc.FromLeft = Convert.ToDouble(reader.Value.Replace('.', ','));
                                        break;
                                    case "f_right":
                                        reader.Read();
                                        dgTopo.Rows[dgTopo.RowCount - 2].Cells[6].Value = reader.Value + "\\" +
                                                                                          dgTopo.Rows[
                                                                                              dgTopo.RowCount - 2]
                                                                                              .Cells[6].Value;
                                        trc.FromRight = Convert.ToDouble(reader.Value.Replace('.', ','));
                                        break;
                                    case "f_up":
                                        reader.Read();
                                        dgTopo.Rows[dgTopo.RowCount - 2].Cells[7].Value = reader.Value + "\\" +
                                                                                          dgTopo.Rows[
                                                                                              dgTopo.RowCount - 2]
                                                                                              .Cells[7].Value;
                                        trc.FromUp = Convert.ToDouble(reader.Value.Replace('.', ','));
                                        break;
                                    case "f_down":
                                        reader.Read();
                                        dgTopo.Rows[dgTopo.RowCount - 2].Cells[8].Value = reader.Value + "\\" +
                                                                                          dgTopo.Rows[
                                                                                              dgTopo.RowCount - 2]
                                                                                              .Cells[8].Value;
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
                        _piquetLst.Add(new Piquet {X = 0, Y = 0, Z = 0, Name = _traceList[0].From, Step = 0});

                        foreach (var trace in _traceList)
                        {
                            if (_piquetLst.Exists(p => p.Name == trace.From) &&
                                _piquetLst.Exists(p => p.Name == trace.To))
                            {
                                foreach (
                                    var pq in
                                        _piquetLst.Where(piquet => trace.From == piquet.Name)
                                            .Select(piquet => new Piquet
                                            {
                                                Name = trace.To,
                                                Note = trace.Note,
                                                X =
                                                    piquet.X +
                                                    trace.Tape*Math.Cos(trace.Clino*Rad)*
                                                    Math.Cos((90 - trace.Azimuth)*Rad),
                                                Y =
                                                    piquet.Y +
                                                    trace.Tape*Math.Cos(trace.Clino*Rad)*
                                                    Math.Sin((90 - trace.Azimuth)*Rad),
                                                Z = piquet.Z + trace.Tape*Math.Sin(trace.Clino*Rad),
                                                Step = piquet.Step + 1
                                            }))
                                {
                                    err += TopoLib.CheckRing(_piquetLst, _traceList, pq, pq, trace.From, pq.Step);
                                    TopoLib.SetSteps(_piquetLst, _traceList, pq, pq.Step);
                                }

                            }
                            else if (_piquetLst.Exists(p => p.Name == trace.From))
                            {
                                foreach (
                                    var pq in
                                        _piquetLst.Where(piquet => trace.From == piquet.Name)
                                            .Select(piquet => new Piquet
                                            {
                                                Name = trace.To,
                                                Note = trace.Note,
                                                X =
                                                    piquet.X +
                                                    trace.Tape*Math.Cos(trace.Clino*Rad)*
                                                    Math.Cos((90 - trace.Azimuth)*Rad),
                                                Y =
                                                    piquet.Y +
                                                    trace.Tape*Math.Cos(trace.Clino*Rad)*
                                                    Math.Sin((90 - trace.Azimuth)*Rad),
                                                Z = piquet.Z + trace.Tape*Math.Sin(trace.Clino*Rad),
                                                Step = piquet.Step + 1
                                            }))
                                {
                                    _piquetLst.Add(pq);
                                    break;
                                }
                            }
                            else
                                foreach (
                                    var pq in
                                        _piquetLst.Where(piquet => trace.To == piquet.Name).Select(piquet => new Piquet
                                        {
                                            Name = trace.From,
                                            Note = trace.Note,
                                            X =
                                                piquet.X +
                                                trace.Tape*Math.Cos(trace.Clino*Rad)*Math.Cos((90 - trace.Azimuth)*Rad),
                                            Y =
                                                piquet.Y +
                                                trace.Tape*Math.Cos(trace.Clino*Rad)*Math.Sin((90 - trace.Azimuth)*Rad),
                                            Z = piquet.Z + trace.Tape*Math.Sin(trace.Clino*Rad),
                                            Step = piquet.Step + 1
                                        }))
                                {
                                    _piquetLst.Add(pq);
                                    break;
                                }
                            sum += trace.Tape;
                        }
                    }
                }

            }
            catch (Exception)
            {

                //throw;
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
                MessageBox.Show(string.Format("Длина: {0}м. Глубина: {1}м. Неточность: {2}%", sum.ToString("N0"),
                    (max - min).ToString("N0"), (err*100).ToString("F3")));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            dgTopo.BackgroundImage = Properties.Resources.cell;
            
        }

        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fr = new Graph {PqList = _piquetLst, TrcList = _traceList};
            var d = Math.Max(_traceList.Max(t => t.Left), _traceList.Max(t => t.Right));
            fr.top = _piquetLst.Max(p => p.Y)+d;
            fr.Buttom = _piquetLst.Min(p => p.Y)-d;
            fr.left = _piquetLst.Min(p=>p.X)-d;
            fr.right = _piquetLst.Max(p=>p.X)+d;

            fr.ShowDialog();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            var fr = (Form1)sender;
            dgTopo.Width = fr.Width - 32;
            dgTopo.Height = fr.Height - 97;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
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
                    Left = Convert.ToDouble(row.Cells["ClLeft"].Value.ToString().Replace('.', ',')),
                    Right = Convert.ToDouble(row.Cells["ClRight"].Value.ToString().Replace('.', ',')),
                    Up = Convert.ToDouble(row.Cells["ClUp"].Value.ToString().Replace('.', ',')),
                    Down = Convert.ToDouble(row.Cells["ClDown"].Value.ToString().Replace('.', ',')),
                    Note = row.Cells["ClNote"].Value == null ? "" : row.Cells["ClNote"].Value.ToString()
                };
                _traceList.Add(trace);
            }
            var err = 0.0;
            var sum = 0.0;
            _piquetLst.Add(new Piquet
            {
                Delta = 0D,
                Name = _traceList[0].From,
                X = 0D,
                Y = 0D,
                Z = 0D,
                Step = 0,
                Note = ""
            });
            foreach (var trace in _traceList)
            {
                if (_piquetLst.Exists(p => p.Name == trace.From) &&
                    _piquetLst.Exists(p => p.Name == trace.To))
                {
                    foreach (
                        var pq in
                            _piquetLst.Where(piquet => trace.From == piquet.Name)
                                .Select(piquet => new Piquet
                                {
                                    Name = trace.To,
                                    Note = trace.Note,
                                    X =
                                        piquet.X +
                                        trace.Tape * Math.Cos(trace.Clino * Rad) *
                                        Math.Cos((90 - trace.Azimuth) * Rad),
                                    Y =
                                        piquet.Y +
                                        trace.Tape * Math.Cos(trace.Clino * Rad) *
                                        Math.Sin((90 - trace.Azimuth) * Rad),
                                    Z = piquet.Z + trace.Tape * Math.Sin(trace.Clino * Rad),
                                    Step = piquet.Step + 1
                                }))
                    {
                        err += TopoLib.CheckRing(_piquetLst, _traceList, pq, pq, trace.From, pq.Step);
                        TopoLib.SetSteps(_piquetLst, _traceList, pq, pq.Step);
                    }

                }
                else if (_piquetLst.Exists(p => p.Name == trace.From))
                {
                    foreach (
                        var pq in
                            _piquetLst.Where(piquet => trace.From == piquet.Name)
                                .Select(piquet => new Piquet
                                {
                                    Name = trace.To,
                                    Note = trace.Note,
                                    X =
                                        piquet.X +
                                        trace.Tape * Math.Cos(trace.Clino * Rad) *
                                        Math.Cos((90 - trace.Azimuth) * Rad),
                                    Y =
                                        piquet.Y +
                                        trace.Tape * Math.Cos(trace.Clino * Rad) *
                                        Math.Sin((90 - trace.Azimuth) * Rad),
                                    Z = piquet.Z + trace.Tape * Math.Sin(trace.Clino * Rad),
                                    Step = piquet.Step + 1
                                }))
                    {
                        _piquetLst.Add(pq);
                        break;
                    }
                }
                else
                    foreach (
                        var pq in
                            _piquetLst.Where(piquet => trace.To == piquet.Name).Select(piquet => new Piquet
                            {
                                Name = trace.From,
                                Note = trace.Note,
                                X =
                                    piquet.X +
                                    trace.Tape * Math.Cos(trace.Clino * Rad) * Math.Cos((90 - trace.Azimuth) * Rad),
                                Y =
                                    piquet.Y +
                                    trace.Tape * Math.Cos(trace.Clino * Rad) * Math.Sin((90 - trace.Azimuth) * Rad),
                                Z = piquet.Z + trace.Tape * Math.Sin(trace.Clino * Rad),
                                Step = piquet.Step + 1
                            }))
                    {
                        _piquetLst.Add(pq);
                        break;
                    }
                sum += trace.Tape;
            }
            var min = 0.0;
            var max = 0.0;
            foreach (var piquet in _piquetLst)
            {
                min = Math.Min(min, piquet.Z);
                max = Math.Max(max, piquet.Z);
            }
            MessageBox.Show(string.Format("Длина: {0}м. Глубина: {1}м. Неточность: {2}%", sum.ToString("N0"),
                (max - min).ToString("N0"), (err * 100).ToString("F3")));
            using (var fl = new FileStream(sdSave.FileName, FileMode.Create))
            {
                
                using (var zipp = new ZipOutputStream(fl))
                {
                    zipp.PutNextEntry("cave.xml");
                    using (var wr = new MemoryStream())
                    {
                        var writer = XmlWriter.Create(wr);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("cave");
                        writer.WriteElementString("title", _name);
                        writer.WriteStartElement("survey");
                        writer.WriteElementString("date", _date);
                        writer.WriteStartElement("team");
                        foreach (var item in _autor)
                        {
                            writer.WriteElementString("name", item);    
                        }
                        writer.WriteEndElement();
                        foreach (var trace in _traceList)
                        {
                            writer.WriteStartElement("segment");
                            writer.WriteElementString("from", trace.From);
                            writer.WriteElementString("to", trace.To);
                            writer.WriteElementString("tape", trace.Tape.ToString());
                            writer.WriteElementString("compass", trace.Azimuth.ToString());
                            writer.WriteElementString("clino", trace.Clino.ToString());
                            writer.WriteElementString("left", trace.Left.ToString());
                            writer.WriteElementString("right", trace.Right.ToString());
                            writer.WriteElementString("up", trace.Up.ToString());
                            writer.WriteElementString("down", trace.Down.ToString());
                            if (trace.FromLeft > 0)
                                writer.WriteElementString("f_left", trace.FromLeft.ToString());
                            if (trace.FromRight > 0)
                                writer.WriteElementString("f_right", trace.FromRight.ToString());
                            if (trace.FromUp > 0)
                                writer.WriteElementString("f_up", trace.FromUp.ToString());
                            if (trace.FromDown > 0)
                                writer.WriteElementString("f_down", trace.FromDown.ToString());
                            if (!string.IsNullOrEmpty(trace.Note))
                                writer.WriteElementString("note", trace.Note);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        zipp.Write(wr.ToArray(), 0, (int)wr.Length);
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
                            writer.WriteElementString("x", piquet.X.ToString());
                            writer.WriteElementString("y", piquet.Y.ToString());
                            writer.WriteElementString("z", piquet.Z.ToString());
                            writer.WriteElementString("a", piquet.X.ToString());
                            writer.WriteElementString("b", piquet.Y.ToString());
                            writer.WriteElementString("c", piquet.Z.ToString());
                            writer.WriteElementString("step", piquet.Step.ToString());
                            writer.WriteElementString("note", piquet.Note);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                        zipp.Write(wr.ToArray(), 0, (int)wr.Length);
                    }
                }
            }

            MessageBox.Show(@"Сохранено");
        }

        private void mData_Click(object sender, EventArgs e)
        {
            var fr = new FrTopoData();
            fr.ShowDialog();
            _name = fr.name;
            _date = fr.date;
            _autor = fr.autor;
        }
    }
}
