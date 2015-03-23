using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ETopo.Properties;
using Ionic.Zip;
using iTextSharp.text.pdf;
using Tao.FreeGlut;
using Tao.OpenGl;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html;
using Image = System.Drawing.Image;

namespace ETopo
{
    public partial class Graph : Form
    {
        public SurveyData SurData;
        private double _scale = 1;
        private double _moveX;
        private double _moveY;
        private bool _move;
        private double _maxX = 0;
        private double _maxY = 0;
        private double _minX = 0;
        private double _minY = 0;
        private double _dX;
        private double _dY;
        public double left;
        public double right;
        public double bottom;
        public double top;
        //private double? _splineX;
        //private double? _splineY;
        private double _devX;
        private double _devY;
        //private bool _listOnly;
        private EditPoint _editPoint;
        private EditCgnPoint _editCgnPoint;

        public List<Spline> SplList;
        public List<Piquet> PqList;
        public List<Trace> TrcList;
        public List<Cgn> CgnList;
        private List<SplinePoint> _curSpline;
        private List<Piquet> _currPqList;

        public Graph()
        {
            InitializeComponent();
            anT.InitializeContexts();
        }

        private void Graph_Load(object sender, EventArgs e)
        {
            Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, anT.Width, anT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            var dev = Math.Max(top - bottom, right - left);
            _moveX = -left;
            _moveY = -bottom;
            var screenW = dev*anT.Width/anT.Height;
            var screenH = dev;
            _devX = (float) screenW/anT.Width;
            _devY = (float) screenH/anT.Height;
            Glu.gluOrtho2D(0.0, screenW, 0.0, screenH);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            //Gl.glEnable(Gl.GL_DEPTH_TEST);

            //SplList = new List<Spline>();
            _curSpline = new List<SplinePoint>();
            _currPqList = new List<Piquet>();
            //CgnList = new List<Cgn>();
            _editPoint = null;

            tVis.Enabled = true;
        }

        private static Point GetSplinePoint(SplinePoint spline0, SplinePoint spline1, double t)
        {
            return new Point
            {
                X = (float)
                    (spline0.Point.X*(2*t*t*t - 3*t*t + 1) + spline0.Rb.X*(t*t*t - 2*t*t + t) +
                     spline1.Point.X*(-2*t*t*t + 3*t*t) + spline1.Ra.X*(t*t*t - t*t)),
                Y =
                    (float)
                        (spline0.Point.Y*(2*t*t*t - 3*t*t + 1) + spline0.Rb.Y*(t*t*t - 2*t*t + t) +
                         spline1.Point.Y*(-2*t*t*t + 3*t*t) + spline1.Ra.Y*(t*t*t - t*t))
            };
        }

        private static Point GetDerSplinePoint(SplinePoint spline0, SplinePoint spline1, double t)
        {
            return new Point
            {
                X = (float)
                    (spline0.Point.X*(6*t*t - 6*t) + spline0.Rb.X*(3*t*t - 4*t + 1) +
                     spline1.Point.X*(-6*t*t + 6*t) + spline1.Ra.X*(3*t*t - 2*t)),
                Y =
                    (float)
                        (spline0.Point.Y*(6*t*t - 6*t) + spline0.Rb.Y*(3*t*t - 4*t + 1) +
                         spline1.Point.Y*(-6*t*t + 6*t) + spline1.Ra.Y*(3*t*t - 2*t))
            };
        }

        private static Point Rotate(double x, double y, double alpha)
        {
            return new Point
            {
                X = (float) (x*Math.Cos(alpha) - y*Math.Sin(alpha)),
                Y = (float) (x*Math.Sin(alpha) + y*Math.Cos(alpha))
            };
        }

        private void AddCgn(CgnType type, string prefix, float x, float y)
        {
            var numbs = new List<int>();
            foreach (var item in listBox1.Items)
            {
                if (item.ToString().Contains(prefix))
                {
                    numbs.Add(Convert.ToInt32(item.ToString().Replace(prefix, "")));
                }
            }
            var numb = numbs.Any() ? (numbs.Max() + 1) : 1;
            var cgn = new Cgn
            {
                Point = new Point {X = x, Y = y},
                Type = type,
                Name = prefix + numb
            };
            CgnList.Add(cgn);
            ReloadNames();
            return;
        }

        private void DrowTrapez()
        {
            foreach (
                var trace in
                    TrcList)
            {
                var x0 = 0.0;
                var y0 = 0.0;
                var x1 = 0.0;
                var y1 = 0.0;
                var trace1 = trace;
                foreach (var piquet in PqList.Where(piquet => piquet.Name == trace1.From))
                {
                    x0 = piquet.X;
                    y0 = piquet.Y;
                }
                var trace2 = trace;
                foreach (var piquet in PqList.Where(piquet => piquet.Name == trace2.To))
                {
                    x1 = piquet.X;
                    y1 = piquet.Y;
                }
                var alpha = Math.Abs(x1 - x0) < 0.000001 ? Math.PI/2*(y1 > y0 ? 1 : -1) : Math.Atan((y1 - y0)/(x1 - x0));
                var point0 = new Point();
                var point1 = new Point();
                var point2 = new Point
                {
                    X = (float) (x1 + Rotate(trace.Left, 0, alpha + Math.PI/2).X),
                    Y = (float) (y1 + Rotate(trace.Left, 0, alpha + Math.PI/2).Y)
                };
                var point3 = new Point
                {
                    X = (float) (x1 - Rotate(trace.Right, 0, alpha + Math.PI/2).X),
                    Y = (float) (y1 - Rotate(trace.Right, 0, alpha + Math.PI/2).Y)
                };
                if (Math.Abs(trace.FromDown + trace.FromUp + trace.FromLeft + trace.FromRight) < MathConst.Accuracy)
                {
                    var trace3 = trace;
                    foreach (var trc in TrcList.Where(trc => trc.To == trace3.From))
                    {
                        point0.X = (float) (x0 + Rotate(trc.Left, 0, alpha + Math.PI/2).X);
                        point0.Y = (float) (y0 + Rotate(trc.Left, 0, alpha + Math.PI/2).Y);
                        point1.X = (float) (x0 - Rotate(trc.Right, 0, alpha + Math.PI/2).X);
                        point1.Y = (float) (y0 - Rotate(trc.Right, 0, alpha + Math.PI/2).Y);
                    }
                }
                else
                {
                    point0.X = (float) (x0 + Rotate(trace.FromLeft, 0, alpha + Math.PI/2).X);
                    point0.Y = (float) (y0 + Rotate(trace.FromLeft, 0, alpha + Math.PI/2).Y);
                    point1.X = (float) (x0 - Rotate(trace.FromRight, 0, alpha + Math.PI/2).X);
                    point1.Y = (float) (y0 - Rotate(trace.FromRight, 0, alpha + Math.PI/2).Y);
                }

                if (point0.X < _minX) _minX = point0.X;
                if (point0.Y < _minY) _minY = point0.Y;
                if (point0.X > _maxX) _maxX = point0.X;
                if (point0.Y > _maxY) _maxY = point0.Y;
                if (point1.X < _minX) _minX = point1.X;
                if (point1.Y < _minY) _minY = point1.Y;
                if (point1.X > _maxX) _maxX = point1.X;
                if (point1.Y > _maxY) _maxY = point1.Y;
                if (point2.X < _minX) _minX = point2.X;
                if (point2.Y < _minY) _minY = point2.Y;
                if (point2.X > _maxX) _maxX = point2.X;
                if (point2.Y > _maxY) _maxY = point2.Y;
                if (point3.X < _minX) _minX = point3.X;
                if (point3.Y < _minY) _minY = point3.Y;
                if (point3.X > _maxX) _maxX = point3.X;
                if (point3.Y > _maxY) _maxY = point3.Y;

                Gl.glLineWidth(1);
                Gl.glColor3f(0.5f, 1f, 1f);
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glVertex2d(point0.X*_scale + _moveX, point0.Y*_scale + _moveY);
                Gl.glVertex2d(point1.X*_scale + _moveX, point1.Y*_scale + _moveY);
                Gl.glVertex2d(point3.X*_scale + _moveX, point3.Y*_scale + _moveY);
                Gl.glVertex2d(point2.X*_scale + _moveX, point2.Y*_scale + _moveY);
                Gl.glEnd();
            }
        }

        private void DrowTraces()
        {
            foreach (var trace in TrcList)
            {
                var x0 = 0.0;
                var y0 = 0.0;
                var x1 = 0.0;
                var y1 = 0.0;
                var trace1 = trace;
                foreach (var piquet in PqList.Where(piquet => piquet.Name == trace1.From))
                {
                    x0 = piquet.X;
                    y0 = piquet.Y;
                    if (x0 < _minX) _minX = x0;
                    if (y0 < _minY) _minY = y0;
                    if (x0 > _maxX) _maxX = x0;
                    if (y0 > _maxY) _maxY = y0;
                }
                var trace2 = trace;
                foreach (var piquet in PqList.Where(piquet => piquet.Name == trace2.To))
                {
                    x1 = piquet.X;
                    y1 = piquet.Y;
                    if (x1 < _minX) _minX = x1;
                    if (y1 < _minY) _minY = y1;
                    if (x1 > _maxX) _maxX = x1;
                    if (y1 > _maxY) _maxY = y1;
                }
                Gl.glLineWidth(2);
                Gl.glColor3f(1f, 0, 0);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d(x0*_scale + _moveX, y0*_scale + _moveY);
                Gl.glVertex2d(x1*_scale + _moveX, y1*_scale + _moveY);
                Gl.glEnd();
            }
        }

        private void DrowPiquets()
        {
            foreach (var piquet in PqList)
            {
                Gl.glPointSize(5);
                if (_currPqList.Contains(piquet))
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.0f, 1.0f, 0.0f);

                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2d((float) (piquet.X*_scale + _moveX), (float) (piquet.Y*_scale + _moveY));
                Gl.glEnd();
                Gl.glPointSize(1);
                Gl.glColor3f(0.0f, 0.0f, 0.0f);
                PrintText2D((float) (piquet.X*_scale + _moveX), (float) (piquet.Y*_scale + _moveY), piquet.Name, 12);
            }
        }

        private void DrowWalls()
        {
            foreach (var spline in SplList.Where(s => s.Type == SplineType.Wall))
            {
                if (ReferenceEquals(spline.Name, listBox1.SelectedItem))
                {
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                }
                else
                {
                    Gl.glColor3f(0.0f, 0.0f, 0.0f);
                }
                Gl.glBegin(Gl.GL_LINE_STRIP);
                for (var i = 0; i < spline.PointList.Count - 1; i++)
                {
                    double t = 0;
                    while (t < 1)
                    {
                        var point = GetSplinePoint(spline.PointList[i], spline.PointList[i + 1], t);
                        if (point.X < _minX) _minX = point.X;
                        if (point.Y < _minY) _minY = point.Y;
                        if (point.X > _maxX) _maxX = point.X;
                        if (point.Y > _maxY) _maxY = point.Y;
                        Gl.glVertex2d(point.X*_scale + _moveX, point.Y*_scale + _moveY);
                        t += 0.1;
                    }
                }
                Gl.glEnd();
            }
            Gl.glPointSize(5);
            Gl.glBegin(Gl.GL_POINTS);
            foreach (var spline in SplList.Where(s => s.Type == SplineType.Wall))
            {
                foreach (var point in spline.PointList)
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                    Gl.glVertex2d((float) (point.Point.X*_scale + _moveX), (float) (point.Point.Y*_scale + _moveY));
                    Gl.glColor3f(0.0f, 0.0f, 1.0f);
                    Gl.glVertex2d(
                        (float) ((point.Point.X + point.Ra.X/3)*_scale + _moveX),
                        (float) ((point.Point.Y + point.Ra.Y/3)*_scale + _moveY));
                    Gl.glVertex2d(
                        (float) ((point.Point.X - point.Rb.X/3)*_scale + _moveX),
                        (float) ((point.Point.Y - point.Rb.Y/3)*_scale + _moveY));
                }
            }
            Gl.glEnd();

            Gl.glColor3f(0.8f, 1.0f, 0.0f);
            Gl.glBegin(Gl.GL_LINES);
            for (var i = 0; i < _curSpline.Count - 1; i++)
            {
                double t = 0;
                while (t < 1)
                {
                    var point = GetSplinePoint(_curSpline[i], _curSpline[i + 1], t);
                    Gl.glVertex2d(point.X*_scale + _moveX, point.Y*_scale + _moveY);
                    t += 0.1;
                }
            }
            Gl.glEnd();

            Gl.glPointSize(5);
            Gl.glBegin(Gl.GL_POINTS);
            foreach (var point in _curSpline)
            {
                Gl.glColor3f(0.6f, 0.6f, 0.0f);
                Gl.glVertex2d((float) (point.Point.X*_scale + _moveX), (float) (point.Point.Y*_scale + _moveY));
                Gl.glColor3f(0.3f, 0.3f, 0.0f);
                Gl.glVertex2d(
                    (float) ((point.Point.X + point.Ra.X/3)*_scale + _moveX),
                    (float) ((point.Point.Y + point.Ra.Y/3)*_scale + _moveY));
                Gl.glVertex2d(
                    (float) ((point.Point.X - point.Rb.X/3)*_scale + _moveX),
                    (float) ((point.Point.Y - point.Rb.Y/3)*_scale + _moveY));
            }

            Gl.glEnd();
        }

        private void DrowPrecipice()
        {
            foreach (var spline in SplList.Where(s => s.Type == SplineType.Precipice))
            {
                Gl.glColor3f(ReferenceEquals(spline.Name, listBox1.SelectedItem) ? 1.0f : 0.5f, 0.5f, 0.5f);
                Gl.glBegin(Gl.GL_LINE_STRIP);
                for (var i = 0; i < spline.PointList.Count - 1; i++)
                {
                    double t = 0;
                    while (t < 1)
                    {
                        var point = GetSplinePoint(spline.PointList[i], spline.PointList[i + 1], t);
                        if (point.X < _minX) _minX = point.X;
                        if (point.Y < _minY) _minY = point.Y;
                        if (point.X > _maxX) _maxX = point.X;
                        if (point.Y > _maxY) _maxY = point.Y;
                        Gl.glVertex2d(point.X*_scale + _moveX, point.Y*_scale + _moveY);
                        t += 0.1;
                    }
                }
                Gl.glEnd();
            }

            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            foreach (var spline in SplList.Where(s => s.Type == SplineType.Precipice))
            {


                for (var i = 0; i < spline.PointList.Count - 1; i++)
                {
                    double t = 0;
                    var p1 = spline.PointList[i].Point;
                    var p2 = spline.PointList[i + 1].Point;
                    var len = Math.Sqrt((p1.X - p2.X)*(p1.X - p2.X) + (p1.Y - p2.Y)*(p1.Y - p2.Y))*2;
                    while (t < 1)
                    {
                        var point = GetSplinePoint(spline.PointList[i], spline.PointList[i + 1], t);
                        var derPoint = GetDerSplinePoint(spline.PointList[i], spline.PointList[i + 1], t);

                        var p = new Point
                        {
                            X = derPoint.X/(float) Math.Sqrt(derPoint.X*derPoint.X + derPoint.Y*derPoint.Y),
                            Y = derPoint.Y/(float) Math.Sqrt(derPoint.X*derPoint.X + derPoint.Y*derPoint.Y)
                        };
                        p = Rotate(p.X, p.Y, 90);
                        Gl.glBegin(Gl.GL_LINE_STRIP);
                        Gl.glVertex2d(point.X*_scale + _moveX, point.Y*_scale + _moveY);
                        Gl.glVertex2d((point.X + p.X/5)*_scale + _moveX, (point.Y + p.Y/5)*_scale + _moveY);
                        Gl.glEnd();
                        t += 1/len;
                    }
                }
            }

            Gl.glPointSize(5);
            Gl.glBegin(Gl.GL_POINTS);
            foreach (var spline in SplList.Where(s => s.Type == SplineType.Precipice))
            {
                foreach (var point in spline.PointList)
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                    Gl.glVertex2d((float) (point.Point.X*_scale + _moveX), (float) (point.Point.Y*_scale + _moveY));
                    Gl.glColor3f(0.0f, 0.0f, 1.0f);
                    Gl.glVertex2d(
                        (float) ((point.Point.X + point.Ra.X/3)*_scale + _moveX),
                        (float) ((point.Point.Y + point.Ra.Y/3)*_scale + _moveY));
                    Gl.glVertex2d(
                        (float) ((point.Point.X - point.Rb.X/3)*_scale + _moveX),
                        (float) ((point.Point.Y - point.Rb.Y/3)*_scale + _moveY));
                }
            }
            Gl.glEnd();

            Gl.glColor3f(0.8f, 1.0f, 0.0f);
            Gl.glBegin(Gl.GL_LINES);
            for (var i = 0; i < _curSpline.Count - 1; i++)
            {
                double t = 0;
                while (t < 1)
                {
                    var point = GetSplinePoint(_curSpline[i], _curSpline[i + 1], t);
                    Gl.glVertex2d(point.X*_scale + _moveX, point.Y*_scale + _moveY);
                    t += 0.1;
                }
            }
            Gl.glEnd();

            Gl.glPointSize(5);
            Gl.glBegin(Gl.GL_POINTS);
            foreach (var point in _curSpline)
            {
                Gl.glColor3f(0.6f, 0.6f, 0.0f);
                Gl.glVertex2d((float) (point.Point.X*_scale + _moveX), (float) (point.Point.Y*_scale + _moveY));
                Gl.glColor3f(0.3f, 0.3f, 0.0f);
                Gl.glVertex2d(
                    (float) ((point.Point.X + point.Ra.X/3)*_scale + _moveX),
                    (float) ((point.Point.Y + point.Ra.Y/3)*_scale + _moveY));
                Gl.glVertex2d(
                    (float) ((point.Point.X - point.Rb.X/3)*_scale + _moveX),
                    (float) ((point.Point.Y - point.Rb.Y/3)*_scale + _moveY));
            }

            Gl.glEnd();
        }

        private void DrowStones()
        {
            // радиус описаной вокруг камней окружности
            const float rad = 0.75f;
            // сдлина стороны камня
            const float len = 0.6f;

            Gl.glPointSize(10);

            foreach (var stone in CgnList.Where(c => c.Type == CgnType.Stone))
            {
                Gl.glColor3f(ReferenceEquals(stone.Name, listBox1.SelectedItem) ? 1.0f : 0.5f, 0.5f, 0.5f);

                var x1 = stone.Point.X;
                var y1 = stone.Point.Y + rad;
                var x2 = x1 + Math.Cos(-60*MathConst.Rad)*len;
                var y2 = y1 + Math.Sin(-60*MathConst.Rad)*len;
                var x3 = x1 + Math.Cos(-120*MathConst.Rad)*len;
                var y3 = y1 + Math.Sin(-120*MathConst.Rad)*len;

                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();

                x1 = (float) (stone.Point.X + Math.Cos(-30*MathConst.Rad)*rad);
                y1 = (float) (stone.Point.Y + Math.Sin(-30*MathConst.Rad)*rad);
                x2 = x1 - len;
                y2 = y1;
                x3 = x2 + Math.Cos(60*MathConst.Rad)*len;
                y3 = y2 + Math.Sin(60*MathConst.Rad)*len;
                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();

                x1 = (float) (stone.Point.X + Math.Cos(-150*MathConst.Rad)*rad);
                y1 = (float) (stone.Point.Y + Math.Sin(-150*MathConst.Rad)*rad);
                x2 = x1 + len;
                y2 = y1;
                x3 = x1 + Math.Cos(60*MathConst.Rad)*len;
                y3 = y1 + Math.Sin(60*MathConst.Rad)*len;
                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();
            }
        }

        private void DrowWaters()
        {
            // малый радиус элипса
            const float ry = 0.3f;
            // большой радиус элипса
            const float rx = 0.5f;

            Gl.glPointSize(10);

            foreach (var water in CgnList.Where(c => c.Type == CgnType.Water))
            {
                if (ReferenceEquals(water.Name, listBox1.SelectedItem))
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.0f, 0.5f, 1.0f);
                var x0 = water.Point.X;
                var y0 = water.Point.Y;
                Gl.glBegin(Gl.GL_POLYGON);
                for (int i = 0; i < 360; i += 10)
                {
                    var x = x0 + rx*Math.Cos(i*MathConst.Rad);
                    var y = y0 + ry*Math.Sin(i*MathConst.Rad);
                    Gl.glVertex2d((x*_scale + _moveX), (y*_scale + _moveY));
                }
                Gl.glEnd();
            }
        }

        private void DrowStalagmites()
        {
            // радиус описаной вокруг символа окружности
            const float rad = 0.5f;

            Gl.glPointSize(10);

            foreach (var stalagmite in CgnList.Where(c => c.Type == CgnType.Stalagmite))
            {
                if (ReferenceEquals(stalagmite.Name, listBox1.SelectedItem))
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.2f, 0.2f, 0.2f);
                var x1 = stalagmite.Point.X;
                var y1 = stalagmite.Point.Y + rad;
                var x2 = stalagmite.Point.X + Math.Cos(-30*MathConst.Rad)*rad;
                var y2 = stalagmite.Point.Y + Math.Sin(-30*MathConst.Rad)*rad;
                var x3 = stalagmite.Point.X + Math.Cos(-150*MathConst.Rad)*rad;
                var y3 = stalagmite.Point.Y + Math.Sin(-150*MathConst.Rad)*rad;

                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();
            }
        }

        private void DrowStalactites()
        {
            // радиус описаной вокруг символа окружности
            const float rad = 0.5f;

            Gl.glPointSize(10);

            foreach (var stalactite in CgnList.Where(c => c.Type == CgnType.Stalactite))
            {
                if (ReferenceEquals(stalactite.Name, listBox1.SelectedItem))
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.2f, 0.2f, 0.2f);
                var x1 = stalactite.Point.X;
                var y1 = stalactite.Point.Y - rad;
                var x2 = stalactite.Point.X + Math.Cos(30*MathConst.Rad)*rad;
                var y2 = stalactite.Point.Y + Math.Sin(30*MathConst.Rad)*rad;
                var x3 = stalactite.Point.X + Math.Cos(150*MathConst.Rad)*rad;
                var y3 = stalactite.Point.Y + Math.Sin(150*MathConst.Rad)*rad;

                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();
            }
        }

        private void DrowStalagnates()
        {
            // радиус описаной вокруг символа окружности
            const float rad = 0.6f;

            //Gl.glPointSize(10);

            foreach (var stalagnate in CgnList.Where(c => c.Type == CgnType.Stalagnate))
            {
                if (ReferenceEquals(stalagnate.Name, listBox1.SelectedItem))
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.2f, 0.2f, 0.2f);
                var x1 = stalagnate.Point.X;
                var y1 = stalagnate.Point.Y;
                var x2 = stalagnate.Point.X + Math.Cos(60*MathConst.Rad)*rad;
                var y2 = stalagnate.Point.Y + Math.Sin(60*MathConst.Rad)*rad;
                var x3 = stalagnate.Point.X + Math.Cos(120*MathConst.Rad)*rad;
                var y3 = stalagnate.Point.Y + Math.Sin(120*MathConst.Rad)*rad;

                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();

                x2 = stalagnate.Point.X + Math.Cos(-60*MathConst.Rad)*rad;
                y2 = stalagnate.Point.Y + Math.Sin(-60*MathConst.Rad)*rad;
                x3 = stalagnate.Point.X + Math.Cos(-120*MathConst.Rad)*rad;
                y3 = stalagnate.Point.Y + Math.Sin(-120*MathConst.Rad)*rad;
                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();
            }
        }

        private void DrowWays()
        {
            foreach (var way in CgnList.Where(c => c.Type == CgnType.Way))
            {
                if (ReferenceEquals(way.Name, listBox1.SelectedItem))
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.0f, 0.0f, 0.0f);
                if (way.Point.X < _minX) _minX = way.Point.X;
                if (way.Point.Y < _minY) _minY = way.Point.Y;
                if (way.Point.X > _maxX) _maxX = way.Point.X;
                if (way.Point.Y > _maxY) _maxY = way.Point.Y;
                PrintText2D((float) (way.Point.X*_scale + _moveX), (float) (way.Point.Y*_scale + _moveY), "?", 18);
            }
        }

        private void DrowEnters()
        {
            // длина стрелки
            var arrow = 0.7;
            // длина указателя
            var pointer = 1.5;

            foreach (var enter in CgnList.Where(c => c.Type == CgnType.Enter))
            {
                if (ReferenceEquals(enter.Name, listBox1.SelectedItem))
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.0f, 0.0f, 0.0f);
                var x1 = enter.Point.X;
                var y1 = enter.Point.Y;
                var x2 = enter.Point.X + Math.Cos((enter.Angle - 90 - 60)*MathConst.Rad)*arrow;
                var y2 = enter.Point.Y + Math.Sin((enter.Angle - 90 - 60)*MathConst.Rad)*arrow;
                var x3 = enter.Point.X + Math.Cos((enter.Angle - 90 - 120)*MathConst.Rad)*arrow;
                var y3 = enter.Point.Y + Math.Sin((enter.Angle - 90 - 120)*MathConst.Rad)*arrow;

                var x4 = enter.Point.X - Math.Cos(enter.Angle*MathConst.Rad)*pointer;
                var y4 = enter.Point.Y - Math.Sin(enter.Angle*MathConst.Rad)*pointer;

                Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x2*_scale + _moveX), (float) (y2*_scale + _moveY));
                Gl.glVertex2d((float) (x3*_scale + _moveX), (float) (y3*_scale + _moveY));
                Gl.glEnd();

                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glVertex2d((float) (x4*_scale + _moveX), (float) (y4*_scale + _moveY));
                Gl.glEnd();

                Gl.glPointSize(5);
                Gl.glColor3f(0.5f, 0.5f, 0.5f);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2d((float) (x1*_scale + _moveX), (float) (y1*_scale + _moveY));
                Gl.glEnd();
                Gl.glColor3f(1.0f, 0.7f, 0.0f);
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2d((float) (x4*_scale + _moveX), (float) (y4*_scale + _moveY));
                Gl.glEnd();
                Gl.glPointSize(1);
            }
        }

        private void DrawMap()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glColor3f(0.5f, 1f, 1f);
            var max = 0.0;
            foreach (var piquet in PqList)
            {
                max = Math.Max(max, piquet.X);
                max = Math.Max(max, piquet.Y);
            }

            if (cbTrapez.Checked)
            {
                DrowTrapez();
            }
            DrowTraces();
            DrowPiquets();
            DrowWalls();
            DrowPrecipice();
            DrowStones();
            DrowWaters();
            DrowStalactites();
            DrowStalagmites();
            DrowStalagnates();
            DrowWays();
            DrowEnters();

            Gl.glFlush();
            anT.Invalidate();
        }

        private static void PrintText2D(float x, float y, string text, int fontSize = 9)
        {

            // устанавливаем позицию вывода растровых символов 
            // в переданных координатах x и y. 
            Gl.glRasterPos2f(x, y);

            IntPtr font;
            switch (fontSize)
            {
                case 8:
                    font = Glut.GLUT_BITMAP_8_BY_13;
                    break;
                case 9:
                    font = Glut.GLUT_BITMAP_9_BY_15;
                    break;
                case 10:
                    font = Glut.GLUT_BITMAP_HELVETICA_10;
                    break;
                case 12:
                    font = Glut.GLUT_BITMAP_HELVETICA_12;
                    break;
                case 18:
                    font = Glut.GLUT_BITMAP_HELVETICA_18;
                    break;
                case 24:
                    font = Glut.GLUT_BITMAP_TIMES_ROMAN_24;
                    break;
                default:
                    font = Glut.GLUT_BITMAP_TIMES_ROMAN_10;
                    break;
            }

            // в цикле foreach перебираем значения из массива text, 
            // который содержит значение строки для визуализации 
            foreach (var liter in text)
            {
                // визуализируем символ с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
                Glut.glutBitmapCharacter(font, liter);
            }
        }

        private void tVis_Tick(object sender, EventArgs e)
        {
            DrawMap();
        }

        private void anT_MouseClick(object sender, MouseEventArgs e)
        {
            // приводим к нужному нам формату, в соотвествии с настройками проекции 
            var lineX = (e.X*_devX - _moveX)/_scale;
            var lineY = ((anT.Height - e.Y)*_devY - _moveY)/_scale;
            if (cbSpline.Checked)
            {
                if (rbAddWall.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    var s = new SplinePoint((float) lineX, (float) lineY, _curSpline);
                    _curSpline.Add(s);
                    return;
                }
                if (rbAddEnter.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    AddCgn(CgnType.Enter, "Вход", (float) lineX, (float) lineY);
                    return;
                }
                if (rbAddWay.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    AddCgn(CgnType.Way, "Ход", (float) lineX, (float) lineY);
                    return;
                }
            }
            if (cbCGN.Checked)
            {
                if (rbAddPrecipice.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    var precip = new SplinePoint((float) lineX, (float) lineY, _curSpline);
                    _curSpline.Add(precip);
                    return;
                }
                if (rbAddStone.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    AddCgn(CgnType.Stone, "Камни", (float) lineX, (float) lineY);
                    return;
                }
                if (rbAddWater.Checked && _editPoint == null)
                {
                    AddCgn(CgnType.Water, "Лужи", (float) lineX, (float) lineY);
                    return;
                }
                if (rbAddStalactite.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    AddCgn(CgnType.Stalactite, "Сталактит", (float) lineX, (float) lineY);
                    return;
                }
                if (rbAddStalagmite.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    AddCgn(CgnType.Stalagmite, "Сталагмит", (float) lineX, (float) lineY);
                    return;
                }
                if (rbAddStalagnate.Checked && _editPoint == null && _editCgnPoint == null)
                {
                    AddCgn(CgnType.Stalagnate, "Сталагнат", (float) lineX, (float) lineY);
                    return;
                }

            }

            foreach (var stone in CgnList)
            {
                if ((lineX - stone.Point.X)*(lineX - stone.Point.X) + (lineY - stone.Point.Y)*(lineY - stone.Point.Y) <
                    (0.75*0.75))
                {
                    listBox1.SelectedItem = stone.Name;
                    return;
                }
            }

            listBox1.SelectedItem = null;
        }

        private void anT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var lineX = (e.X*_devX - _moveX)/_scale;
            var lineY = ((anT.Height - e.Y)*_devY - _moveY)/_scale;
            var deltaX = 5*_devX/_scale;
            var deltaY = 5*_devY/_scale;
            foreach (var splinePoint in SplList.SelectMany(spline => spline.PointList))
            {
                if (Math.Abs(splinePoint.Point.X - lineX) < deltaX &&
                    Math.Abs(splinePoint.Point.Y - lineY) < deltaY)
                {
                    _editPoint = new EditPoint(splinePoint, "point");
                    return;
                }
                if (Math.Abs((splinePoint.Point.X + splinePoint.Ra.X/3) - lineX) < deltaX &&
                    Math.Abs((splinePoint.Point.Y + splinePoint.Ra.Y/3) - lineY) < deltaY)

                {
                    _editPoint = new EditPoint(splinePoint, "rb");
                    return;
                }
                if (Math.Abs((splinePoint.Point.X - splinePoint.Rb.X/3) - lineX) < deltaX &&
                    Math.Abs((splinePoint.Point.Y - splinePoint.Rb.Y/3) - lineY) < deltaY)
                {
                    _editPoint = new EditPoint(splinePoint, "ra");
                    return;
                }
            }
            foreach (var splinePoint in _curSpline)
            {
                if (Math.Abs(splinePoint.Point.X - lineX) < deltaX && Math.Abs(splinePoint.Point.Y - lineY) < deltaY)
                {
                    _editPoint = new EditPoint(splinePoint, "point");
                    return;
                }
                if (Math.Abs(splinePoint.Point.X + splinePoint.Ra.X/3 - lineX) < deltaX &&
                    Math.Abs(splinePoint.Point.Y + splinePoint.Ra.Y/3 - lineY) < deltaY)
                {
                    _editPoint = new EditPoint(splinePoint, "rb");
                    return;
                }
                if (Math.Abs(splinePoint.Point.X - splinePoint.Rb.X/3 - lineX) < deltaX &&
                    Math.Abs(splinePoint.Point.Y - splinePoint.Rb.Y/3 - lineY) < deltaY)
                {
                    _editPoint = new EditPoint(splinePoint, "ra");
                    return;
                }
            }
            foreach (var cgn in CgnList)
            {
                if (Math.Abs(cgn.Point.X - lineX) < deltaX &&
                    Math.Abs(cgn.Point.Y - lineY) < deltaY)
                {
                    _editCgnPoint = new EditCgnPoint(cgn, "cgn");
                    return;
                }
            }
            foreach (var enter in CgnList.Where(r => r.Type == CgnType.Enter))
            {
                var pX = enter.Point.X - Math.Cos(enter.Angle*MathConst.Rad)*1.5;
                var pY = enter.Point.Y - Math.Sin(enter.Angle*MathConst.Rad)*1.5;
                if (Math.Abs(pX - lineX) < deltaX &&
                    Math.Abs(pY - lineY) < deltaY)
                {
                    _editCgnPoint = new EditCgnPoint(enter, "enter");
                    return;
                }
            }
            _move = true;
            _dX = e.X;
            _dY = e.Y;
        }

        private void anT_MouseMove(object sender, MouseEventArgs e)
        {
            if (_move)
            {
                if (e.X > _dX)
                {
                    _moveX += 0.2f;
                }
                if (e.X < _dX)
                {
                    _moveX -= 0.2f;
                }
                if (e.Y > _dY)
                {
                    _moveY -= 0.2f;
                }
                if (e.Y < _dY)
                {
                    _moveY += 0.2f;
                }
                _dX = e.X;
                _dY = e.Y;
            }
            if (_editPoint != null)
            {
                var lineX = (e.X*_devX - _moveX)/_scale;
                var lineY = ((anT.Height - e.Y)*_devY - _moveY)/_scale;
                switch (_editPoint.PointName)
                {
                    case "point":
                        _editPoint.Point.Point.X = (float) lineX;
                        _editPoint.Point.Point.Y = (float) lineY;
                        break;
                    case "ra":
                        _editPoint.Point.Ra.X = (float) (_editPoint.Point.Point.X - lineX)*3;
                        _editPoint.Point.Ra.Y = (float) (_editPoint.Point.Point.Y - lineY)*3;
                        _editPoint.Point.Rb.X = (float) (_editPoint.Point.Point.X - lineX)*3;
                        _editPoint.Point.Rb.Y = (float) (_editPoint.Point.Point.Y - lineY)*3;
                        break;
                    case "rb":
                        _editPoint.Point.Ra.X = (float) (lineX - _editPoint.Point.Point.X)*3;
                        _editPoint.Point.Ra.Y = (float) (lineY - _editPoint.Point.Point.Y)*3;
                        _editPoint.Point.Rb.X = (float) (lineX - _editPoint.Point.Point.X)*3;
                        _editPoint.Point.Rb.Y = (float) (lineY - _editPoint.Point.Point.Y)*3;
                        break;
                }
            }
            if (_editCgnPoint != null)
            {
                var lineX = (e.X*_devX - _moveX)/_scale;
                var lineY = ((anT.Height - e.Y)*_devY - _moveY)/_scale;
                switch (_editCgnPoint.PointName)
                {
                    case "cgn":
                        _editCgnPoint.Cgn.Point.X = (float) lineX;
                        _editCgnPoint.Cgn.Point.Y = (float) lineY;
                        break;
                    case "enter":
                        var x0 = _editCgnPoint.Cgn.Point.X;
                        var y0 = _editCgnPoint.Cgn.Point.Y;
                        if (Math.Abs(x0 - lineX) < MathConst.Accuracy)
                        {
                            _editCgnPoint.Cgn.Angle = 90*(y0 - lineY > 0 ? 1 : -1);
                        }
                        else
                        {
                            _editCgnPoint.Cgn.Angle = Math.Atan((y0 - lineY)/(x0 - lineX))/MathConst.Rad +
                                                      (x0 - lineX > 0 ? 0 : 180);
                        }
                        break;
                }
            }
        }

        private void anT_MouseUp(object sender, MouseEventArgs e)
        {
            _move = false;
            _editPoint = null;
            _editCgnPoint = null;
        }

        private void anT_MouseWheel(object sender, MouseEventArgs e)
        {
            // изменение координаты Z в зависимости от направления вращения
            if (e.Delta > 0)
            {
                _scale += 0.05;
                return;
            }
            if (e.Delta == 0) return;
            _scale -= 0.05;
            if (_scale < 0) _scale = 0;
        }

        private void Graph_Resize(object sender, EventArgs e)
        {
            Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, anT.Width, anT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            var dev = Math.Max(top - bottom, right - left);
            _moveX = -left;
            _moveY = -bottom;
            var screenW = dev*anT.Width/anT.Height;
            var screenH = dev;
            _devX = (float) screenW/anT.Width;
            _devY = (float) screenH/anT.Height;
            Glu.gluOrtho2D(0.0, screenW, 0.0, screenH);
            //Glu.gluOrtho2D(left, right, buttom, top);
            //  Glu.gluPerspective(0, (float)anT.Width / anT.Height, 0, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            //Gl.glEnable(Gl.GL_DEPTH_TEST);
        }

        private void cbSpline_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSpline.Checked)
            {
                cbTrapez.Checked = true;
                cbCGN.Checked = false;
            }
            rbAddWall.Enabled = cbSpline.Checked;
            rbAddWay.Enabled = cbSpline.Checked;
            rbAddEnter.Enabled = cbSpline.Checked;
            rbAddPrecipice.Enabled = cbCGN.Checked;
            rbAddStone.Enabled = cbCGN.Checked;
            rbAddWater.Enabled = cbCGN.Checked;
            rbAddStalactite.Enabled = cbCGN.Checked;
            rbAddStalagmite.Enabled = cbCGN.Checked;
            rbAddStalagnate.Enabled = cbCGN.Checked;
            rbAddWall.Checked = false;
            rbAddWay.Checked = false;
            rbAddEnter.Checked = false;
            rbAddPrecipice.Checked = false;
            rbAddStone.Checked = false;
            rbAddWater.Checked = false;
            rbAddStalactite.Checked = false;
            rbAddStalagmite.Checked = false;
            rbAddStalagnate.Checked = false;
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                var name = listBox1.SelectedItem;
                SplList.RemoveAll(s => ReferenceEquals(s.Name, name));
                CgnList.RemoveAll(c => ReferenceEquals(c.Name, name));
                ReloadNames();
            }
        }

        private void ReloadNames()
        {
            listBox1.Items.Clear();
            foreach (var wall in SplList.Where(s => s.Type == SplineType.Wall))
            {
                listBox1.Items.Add(wall.Name);
            }
            foreach (var way in CgnList.Where(s => s.Type == CgnType.Way))
            {
                listBox1.Items.Add(way.Name);
            }
            foreach (var enter in CgnList.Where(s => s.Type == CgnType.Enter))
            {
                listBox1.Items.Add(enter.Name);
            }
            foreach (var precipice in SplList.Where(s => s.Type == SplineType.Precipice))
            {
                listBox1.Items.Add(precipice.Name);
            }
            foreach (var stone in CgnList.Where(s => s.Type == CgnType.Stone))
            {
                listBox1.Items.Add(stone.Name);
            }
            foreach (var water in CgnList.Where(s => s.Type == CgnType.Water))
            {
                listBox1.Items.Add(water.Name);
            }
            foreach (var stalactite in CgnList.Where(s => s.Type == CgnType.Stalactite))
            {
                listBox1.Items.Add(stalactite.Name);
            }
            foreach (var stalagmite in CgnList.Where(s => s.Type == CgnType.Stalagmite))
            {
                listBox1.Items.Add(stalagmite.Name);
            }
            foreach (var stalagnate in CgnList.Where(s => s.Type == CgnType.Stalagnate))
            {
                listBox1.Items.Add(stalagnate.Name);
            }
        }

        private void cbCGN_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCGN.Checked)
            {
                cbSpline.Checked = false;
            }
            rbAddPrecipice.Enabled = cbCGN.Checked;
            rbAddStone.Enabled = cbCGN.Checked;
            rbAddWater.Enabled = cbCGN.Checked;
            rbAddStalactite.Enabled = cbCGN.Checked;
            rbAddStalagmite.Enabled = cbCGN.Checked;
            rbAddStalagnate.Enabled = cbCGN.Checked;
            rbAddWall.Enabled = cbSpline.Checked;
            rbAddWay.Enabled = cbSpline.Checked;
            rbAddEnter.Checked = cbSpline.Checked;
            rbAddWall.Checked = false;
            rbAddWay.Checked = false;
            rbAddEnter.Checked = false;
            rbAddPrecipice.Checked = false;
            rbAddStone.Checked = false;
            rbAddWater.Checked = false;
            rbAddStalactite.Checked = false;
            rbAddStalagmite.Checked = false;
            rbAddStalagnate.Checked = false;
        }

        private void anT_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Delete:
                    var name = listBox1.SelectedItem;
                    SplList.RemoveAll(s => ReferenceEquals(s.Name, name));
                    CgnList.RemoveAll(c => ReferenceEquals(c.Name, name));
                    ReloadNames();
                    break;
                case Keys.Enter:
                    if (rbAddWall.Checked)
                    {
                        var numbs = new List<int>();
                        foreach (var item in listBox1.Items)
                        {
                            if (item.ToString().Contains("Стена"))
                            {
                                numbs.Add(Convert.ToInt32(item.ToString().Replace("Стена", "")));
                            }
                        }
                        var numb = numbs.Any() ? (numbs.Max() + 1) : 1;
                        var spline = new Spline(SplineType.Wall, _curSpline) {Name = "Стена" + numb};
                        SplList.Add(spline);
                    }
                    if (rbAddPrecipice.Checked)
                    {
                        var numbs = new List<int>();
                        foreach (var item in listBox1.Items)
                        {
                            if (item.ToString().Contains("Обрыв"))
                            {
                                numbs.Add(Convert.ToInt32(item.ToString().Replace("Обрыв", "")));
                            }
                        }
                        var numb = numbs.Any() ? (numbs.Max() + 1) : 1;
                        var spline = new Spline(SplineType.Precipice, _curSpline) {Name = "Обрыв" + numb};
                        SplList.Add(spline);
                    }

                    ReloadNames();
                    _curSpline = new List<SplinePoint>();
                    break;
                case Keys.Escape:
                    _curSpline = new List<SplinePoint>();
                    break;
            }
        }

        private void ConvertToBitmap()
        {
            var bmpOut = new Bitmap(500, 500);
            var graph = Graphics.FromImage(bmpOut);
            graph.Clear(System.Drawing.Color.White);
            var p = new Pen(System.Drawing.Color.Black);
            foreach (var spline in SplList)
            {
                var pLst = new List<System.Drawing.Point>();
                pLst.Add(new System.Drawing.Point
                {
                    X = (int) (spline.PointList[0].Point.X*10),
                    Y = /*picture.Height */ 500 - (int) (spline.PointList[0].Point.Y*10)
                });
                for (var i = 0; i < spline.PointList.Count - 1; i++)
                {
                    double t = 0;
                    while (t < 1)
                    {
                        var point = GetSplinePoint(spline.PointList[i], spline.PointList[i + 1], t);
                        pLst.Add(new System.Drawing.Point((int) (point.X*10), /*picture.Height*/500 - (int) (point.Y*10)));
                        t += 0.1;
                    }
                }
                graph.DrawLines(p, pLst.ToArray());
            }

            var imgStream = new MemoryStream();
            //picture.Image.Save(imgStream, ImageFormat.Jpeg);
            //bmp.Save(imgStream, ImageFormat.Jpeg);
            //imgStream.Position = 0;
            bmpOut.Save(imgStream, ImageFormat.Jpeg);
            imgStream.Position = 0;
            //bmpOut.Save("file.jpg");
            var pdf = new MemoryStream();
            var document = new Document(PageSize.A4);

            document.SetMargins(40f, 20f, 5f, 5f);
            var wrPdf = PdfWriter.GetInstance(document, pdf);
            wrPdf.CloseStream = false;
            document.Open();
            var img = iTextSharp.text.Image.GetInstance(imgStream);
            document.Add(img);
            document.Close();
            pdf.Position = 0;
            var fileStream = File.Create("file.pdf");
            pdf.CopyTo(fileStream);
            fileStream.Close();
            pdf.Close();
            imgStream.Close();
            graph.Dispose();
        }

        private void mSave_Click(object sender, EventArgs e)
        {
            if (sdSave.ShowDialog() != DialogResult.OK) return;
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
                        writer.WriteElementString("title", SurData==null?"":SurData.Name);
                        writer.WriteStartElement("survey");
                        writer.WriteElementString("date", SurData == null ? "" : SurData.Date.ToString());
                        writer.WriteStartElement("team");
                        if(SurData!=null && SurData.Team!=null)
                        foreach (var item in SurData.Team)
                        {
                            writer.WriteElementString("name", item);
                        }
                        writer.WriteEndElement();
                        foreach (var trace in TrcList)
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
                        foreach (var piquet in PqList)
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
                        foreach (var spline in SplList)
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
                        foreach (var cgn in CgnList)
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
    }
}
