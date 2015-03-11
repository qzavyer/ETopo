using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace ETopo
{
    public partial class Graph : Form
    {
        private double _scale = 1;
        private double _moveX;
        private double _moveY;
        private bool _move;
        private double _dX;
        private double _dY;
        public double left;
        public double right;
        public double bottom;
        public double top;
        private double? _splineX;
        private double? _splineY;
        private double _devX;
        private double _devY;
        private bool _listOnly;
        private EditPoint _editPoint;

        private List<Spline> _spline;
        private List<SplinePoint> _curSpline;
        public List<Piquet> PqList;
        private List<Piquet> _currPqList;
        public List<Trace> TrcList;
        private List<Cgn> _cgnList; 
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
            _devX = (float)screenW / anT.Width;
            _devY = (float)screenH / anT.Height;
            Glu.gluOrtho2D(0.0, screenW, 0.0, screenH);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            //Gl.glEnable(Gl.GL_DEPTH_TEST);

            _spline = new List<Spline>();
            _curSpline = new List<SplinePoint>();
            _currPqList = new List<Piquet>();
            _cgnList = new List<Cgn>();
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
                X = (float)(x*Math.Cos(alpha) - y*Math.Sin(alpha)),
                Y = (float)(x*Math.Sin(alpha) + y*Math.Cos(alpha))
            };
        }

        private void DrowTrapez()
        {
            foreach (
                    var trace in
                        TrcList.Where(t => !_listOnly || lbPiq.Items.Contains(t.From) && lbPiq.Items.Contains(t.To)))
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
                var alpha = Math.Abs(x1 - x0) < 0.000001 ? Math.PI / 2 * (y1 > y0 ? 1 : -1) : Math.Atan((y1 - y0) / (x1 - x0));
                var point0 = new Point();
                var point1 = new Point();
                var point2 = new Point
                {
                    X = (float)(x1 + Rotate(trace.Left, 0, alpha + Math.PI / 2).X),
                    Y = (float)(y1 + Rotate(trace.Left, 0, alpha + Math.PI / 2).Y)
                };
                var point3 = new Point
                {
                    X = (float)(x1 - Rotate(trace.Right, 0, alpha + Math.PI / 2).X),
                    Y = (float)(y1 - Rotate(trace.Right, 0, alpha + Math.PI / 2).Y)
                };
                if (Math.Abs(trace.FromDown + trace.FromUp + trace.FromLeft + trace.FromRight) < 0.01)
                {
                    var trace3 = trace;
                    foreach (var trc in TrcList.Where(trc => trc.To == trace3.From))
                    {
                        point0.X = (float)(x0 + Rotate(trc.Left, 0, alpha + Math.PI / 2).X);
                        point0.Y = (float)(y0 + Rotate(trc.Left, 0, alpha + Math.PI / 2).Y);
                        point1.X = (float)(x0 - Rotate(trc.Right, 0, alpha + Math.PI / 2).X);
                        point1.Y = (float)(y0 - Rotate(trc.Right, 0, alpha + Math.PI / 2).Y);
                    }
                }
                else
                {
                    point0.X = (float)(x0 + Rotate(trace.FromLeft, 0, alpha + Math.PI / 2).X);
                    point0.Y = (float)(y0 + Rotate(trace.FromLeft, 0, alpha + Math.PI / 2).Y);
                    point1.X = (float)(x0 - Rotate(trace.FromRight, 0, alpha + Math.PI / 2).X);
                    point1.Y = (float)(y0 - Rotate(trace.FromRight, 0, alpha + Math.PI / 2).Y);
                }
                Gl.glLineWidth(1);
                Gl.glColor3f(0.5f, 1f, 1f);
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glVertex2d(point0.X * _scale + _moveX, point0.Y * _scale + _moveY);
                Gl.glVertex2d(point1.X * _scale + _moveX, point1.Y * _scale + _moveY);
                Gl.glVertex2d(point3.X * _scale + _moveX, point3.Y * _scale + _moveY);
                Gl.glVertex2d(point2.X * _scale + _moveX, point2.Y * _scale + _moveY);
                Gl.glEnd();
            }
        }

        private void DrowTraces()
        {
            foreach (var trace in TrcList.Where(t => !_listOnly || lbPiq.Items.Contains(t.From) && lbPiq.Items.Contains(t.To)))
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
                Gl.glLineWidth(2);
                Gl.glColor3f(1f, 0, 0);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d(x0 * _scale + _moveX, y0 * _scale + _moveY);
                Gl.glVertex2d(x1 * _scale + _moveX, y1 * _scale + _moveY);
                Gl.glEnd();
            }
        }

        private void DrowPiquets()
        {
            foreach (var piquet in PqList.Where(p => !_listOnly || lbPiq.Items.Contains(p.Name)))
            {
                Gl.glPointSize(10);
                if (_currPqList.Contains(piquet))
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.0f, 1.0f, 0.0f);

                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2d((float)(piquet.X * _scale + _moveX), (float)(piquet.Y * _scale + _moveY));
                Gl.glEnd();
                Gl.glPointSize(1);
                Gl.glColor3f(0.0f, 0.0f, 0.0f);
                PrintText2D((float)(piquet.X * _scale + _moveX), (float)(piquet.Y * _scale + _moveY), piquet.Name);
            }
        }

        private void DrowWalls()
        {
            foreach (var spline in _spline.Where(s=>s.Type==SplineType.Wall))
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
                        Gl.glVertex2d(point.X * _scale + _moveX, point.Y * _scale + _moveY);
                        t += 0.1;
                    }
                }
                Gl.glEnd();
            }
            Gl.glPointSize(5);
            Gl.glBegin(Gl.GL_POINTS);
            foreach (var spline in _spline.Where(s=>s.Type==SplineType.Wall))
            {
                foreach (var point in spline.PointList)
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                    Gl.glVertex2d((float)(point.Point.X * _scale + _moveX), (float)(point.Point.Y * _scale + _moveY));
                    Gl.glColor3f(0.0f, 0.0f, 1.0f);
                    Gl.glVertex2d(
                        (float)((point.Point.X + point.Ra.X / 3) * _scale + _moveX),
                        (float)((point.Point.Y + point.Ra.Y / 3) * _scale + _moveY));
                    Gl.glVertex2d(
                        (float)((point.Point.X - point.Rb.X / 3) * _scale + _moveX),
                        (float)((point.Point.Y - point.Rb.Y / 3) * _scale + _moveY));
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
                    Gl.glVertex2d(point.X * _scale + _moveX, point.Y * _scale + _moveY);
                    t += 0.1;
                }
            }
            Gl.glEnd();

            Gl.glPointSize(5);
            Gl.glBegin(Gl.GL_POINTS);
            foreach (var point in _curSpline)
            {
                Gl.glColor3f(0.6f, 0.6f, 0.0f);
                Gl.glVertex2d((float)(point.Point.X * _scale + _moveX), (float)(point.Point.Y * _scale + _moveY));
                Gl.glColor3f(0.3f, 0.3f, 0.0f);
                Gl.glVertex2d(
                    (float)((point.Point.X + point.Ra.X / 3) * _scale + _moveX),
                    (float)((point.Point.Y + point.Ra.Y / 3) * _scale + _moveY));
                Gl.glVertex2d(
                    (float)((point.Point.X - point.Rb.X / 3) * _scale + _moveX),
                    (float)((point.Point.Y - point.Rb.Y / 3) * _scale + _moveY));
            }

            Gl.glEnd();
        }

        private void DrowPrecipice()
        {
            foreach (var spline in _spline.Where(s => s.Type == SplineType.Precipice))
            {
                if (ReferenceEquals(spline.Name, listBox1.SelectedItem))
                {
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                }
                else
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                }
                Gl.glBegin(Gl.GL_LINE_STRIP);
                for (var i = 0; i < spline.PointList.Count - 1; i++)
                {
                    double t = 0;
                    while (t < 1)
                    {
                        var point = GetSplinePoint(spline.PointList[i], spline.PointList[i + 1], t);
                        Gl.glVertex2d(point.X * _scale + _moveX, point.Y * _scale + _moveY);
                        t += 0.1;
                    }
                }
                Gl.glEnd();
            }

            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            foreach (var spline in _spline.Where(s => s.Type == SplineType.Precipice))
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
            foreach (var spline in _spline.Where(s => s.Type == SplineType.Precipice))
            {
                foreach (var point in spline.PointList)
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                    Gl.glVertex2d((float)(point.Point.X * _scale + _moveX), (float)(point.Point.Y * _scale + _moveY));
                    Gl.glColor3f(0.0f, 0.0f, 1.0f);
                    Gl.glVertex2d(
                        (float)((point.Point.X + point.Ra.X / 3) * _scale + _moveX),
                        (float)((point.Point.Y + point.Ra.Y / 3) * _scale + _moveY));
                    Gl.glVertex2d(
                        (float)((point.Point.X - point.Rb.X / 3) * _scale + _moveX),
                        (float)((point.Point.Y - point.Rb.Y / 3) * _scale + _moveY));
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
                    Gl.glVertex2d(point.X * _scale + _moveX, point.Y * _scale + _moveY);
                    t += 0.1;
                }
            }
            Gl.glEnd();

            Gl.glPointSize(5);
            Gl.glBegin(Gl.GL_POINTS);
            foreach (var point in _curSpline)
            {
                Gl.glColor3f(0.6f, 0.6f, 0.0f);
                Gl.glVertex2d((float)(point.Point.X * _scale + _moveX), (float)(point.Point.Y * _scale + _moveY));
                Gl.glColor3f(0.3f, 0.3f, 0.0f);
                Gl.glVertex2d(
                    (float)((point.Point.X + point.Ra.X / 3) * _scale + _moveX),
                    (float)((point.Point.Y + point.Ra.Y / 3) * _scale + _moveY));
                Gl.glVertex2d(
                    (float)((point.Point.X - point.Rb.X / 3) * _scale + _moveX),
                    (float)((point.Point.Y - point.Rb.Y / 3) * _scale + _moveY));
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

            foreach (var stone in _cgnList.Where(c => c.Type == CgnType.Stone))
            {
                if (ReferenceEquals(stone.Name, listBox1.SelectedItem))
                {
                    Gl.glColor3f(1.0f, 0.5f, 0.5f);
                }
                else
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                }
             
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
            
            Gl.glFlush();
            anT.Invalidate();
        }

        private static void PrintText2D(float x, float y, string text)
        {

            // устанавливаем позицию вывода растровых символов 
            // в переданных координатах x и y. 
            Gl.glRasterPos2f(x, y);

            // в цикле foreach перебираем значения из массива text, 
            // который содержит значение строки для визуализации 
            foreach (var liter in text)
            {
                // визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
                Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, liter);
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
                if (rbAddWall.Checked && _editPoint == null)
                {
                    var s = new SplinePoint((float) lineX, (float) lineY, _curSpline);
                    _curSpline.Add(s);
                    return;
                }
            }
            if (cbCGN.Checked)
            {
                if (rbAddPrecipice.Checked && _editPoint == null)
                {
                    var precip = new SplinePoint((float) lineX, (float) lineY, _curSpline);
                    _curSpline.Add(precip);
                    return;
                }
                if (rbStone.Checked)
                {
                    var numbs = new List<int>();
                    foreach (var item in listBox1.Items)
                    {
                        if (item.ToString().Contains("Камни"))
                        {
                            numbs.Add(Convert.ToInt32(item.ToString().Replace("Камни", "")));
                        }
                    }
                    var numb = numbs.Any() ? (numbs.Max() + 1) : 1;
                    var cgn = new Cgn
                    {
                        Point = new Point {X = (float) lineX, Y = (float) lineY},
                        Type = CgnType.Stone,
                        Name = "Камни" + numb
                    };
                    _cgnList.Add(cgn);
                    ReloadNames();
                    return;
                }
            }
            foreach (
                var piquet in
                    PqList.Where(piquet => Math.Abs(piquet.X - lineX) < 0.5 && Math.Abs(piquet.Y - lineY) < 0.5)
                        .Where(piquet => !_currPqList.Contains(piquet)))
            {
                lbPiq.Items.Clear();
                _currPqList.Add(piquet);
                _currPqList.Sort(
                    (item1, item2) => String.Compare(item1.Name, item2.Name, StringComparison.OrdinalIgnoreCase));
                foreach (var pq in _currPqList)
                {
                    lbPiq.Items.Add(pq.Name);
                    return;
                }
            }

            foreach (var stone in _cgnList)
            {
                if ((lineX - stone.Point.X)*(lineX - stone.Point.X) + (lineY - stone.Point.Y)*(lineY - stone.Point.Y) <
                    (0.75*0.75))
                {
                    listBox1.SelectedItem = stone.Name;
                    return;
                }
            }

            listBox1.SelectedItem = null;
            /*
            if (_splineX != null && _splineY != null)
            {
                // _spline.Add(new Spline { Bias = 0, Cont = 0, Ra = 0, Rb = 0, Tens = 0, X = e.X, Y = e.Y });
            }
            _splineX = e.X;
            _splineY = e.Y;*/
        }

        private void anT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var lineX = (e.X*_devX - _moveX)/_scale;
            var lineY = ((anT.Height - e.Y)*_devY - _moveY)/_scale;
            foreach (var splinePoint in _spline.SelectMany(spline => spline.PointList))
            {
                if (Math.Abs(lineX - splinePoint.Point.X) < 0.3 &&
                    Math.Abs(lineY - splinePoint.Point.Y) < 0.3)
                {
                    _editPoint = new EditPoint(splinePoint, "point");
                    return;
                }
                if (Math.Abs(lineX - (splinePoint.Point.X + splinePoint.Ra.X/3)) < 0.3 &&
                    Math.Abs(lineY - (splinePoint.Point.Y + splinePoint.Ra.Y/3)) < 0.3)
                {
                    _editPoint = new EditPoint(splinePoint, "rb");
                    return;
                }
                if (Math.Abs(lineX - (splinePoint.Point.X - splinePoint.Rb.X/3)) < 0.3 &&
                    Math.Abs(lineY - (splinePoint.Point.Y - splinePoint.Rb.Y/3)) < 0.3)
                {
                    _editPoint = new EditPoint(splinePoint, "ra");
                    return;
                }
            }
            foreach (var splinePoint in _curSpline)
            {
                if (Math.Abs(lineX - splinePoint.Point.X) < 0.3 &&
                    Math.Abs(lineY - splinePoint.Point.Y) < 0.3)
                {
                    _editPoint = new EditPoint(splinePoint, "point");
                    return;
                }
                if (Math.Abs(lineX - (splinePoint.Point.X + splinePoint.Ra.X / 3)) < 0.3 &&
                    Math.Abs(lineY - (splinePoint.Point.Y + splinePoint.Ra.Y / 3)) < 0.3)
                {
                    _editPoint = new EditPoint(splinePoint, "rb");
                    return;
                }
                if (Math.Abs(lineX - (splinePoint.Point.X - splinePoint.Rb.X / 3)) < 0.3 &&
                    Math.Abs(lineY - (splinePoint.Point.Y - splinePoint.Rb.Y / 3)) < 0.3)
                {
                    _editPoint = new EditPoint(splinePoint, "ra");
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
            if (_editPoint!=null)
            {
                var lineX = (e.X*_devX - _moveX)/_scale;
                var lineY = ((anT.Height - e.Y)*_devY - _moveY)/_scale;
                switch (_editPoint.PointName)
                {
                    case "point":
                        _editPoint.Point.Point.X = (float)lineX;
                        _editPoint.Point.Point.Y = (float)lineY;
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

        }

        private void anT_MouseUp(object sender, MouseEventArgs e)
        {
            _move = false;
            _editPoint = null;
        }

        private void anT_MouseWheel(object sender, MouseEventArgs e)
        {
            // изменение координаты Z в зависимости от направления вращения
            if (e.Delta > 0)
            {
                _scale+=0.1;
                return;
            }
            if (e.Delta == 0) return;
            _scale-=0.1;
        }

        private void anT_KeyPress(object sender, KeyPressEventArgs e)
        {
            
           /* if (e.KeyChar == (char) Keys.Delete)
            {
                var name = listBox1.SelectedItem;
                _spline.RemoveAll(s => ReferenceEquals(s.Name, name));
                _cgnList.RemoveAll(c => ReferenceEquals(c.Name, name));
                ReloadNames();
                return;
            }
            if ((e.KeyChar != (char) Keys.Enter && e.KeyChar != (char) Keys.Escape) || (!cbSpline.Checked&&!cbCGN.Checked)) return;
            if (e.KeyChar == (char) Keys.Enter)
            {
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
                    _spline.Add(spline);
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
                    _spline.Add(spline);
                }


                listBox1.Items.Clear();
                foreach (var spline1 in _spline.Where(s => s.Type == SplineType.Wall))
                {
                    listBox1.Items.Add(spline1.Name);
                }
                foreach (var spline1 in _spline.Where(s => s.Type == SplineType.Precipice))
                {
                    listBox1.Items.Add(spline1.Name);
                }
            }
            _curSpline = new List<SplinePoint>();*/
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
            var screenW = dev * anT.Width / anT.Height;
            var screenH = dev;
            _devX = (float)screenW / anT.Width;
            _devY = (float)screenH / anT.Height;
            Glu.gluOrtho2D(0.0, screenW, 0.0, screenH);
            //Glu.gluOrtho2D(left, right, buttom, top);
            //  Glu.gluPerspective(0, (float)anT.Width / anT.Height, 0, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            //Gl.glEnable(Gl.GL_DEPTH_TEST);
        }

        private void btClrLst_Click(object sender, EventArgs e)
        {
            lbPiq.Items.Clear();
            _currPqList.Clear();
        }

        private void btRemList_Click(object sender, EventArgs e)
        {
            _currPqList.RemoveAll(pq => pq.Name == (string)lbPiq.SelectedItem);
            _currPqList.Sort((item1, item2) => String.Compare(item1.Name, item2.Name, StringComparison.OrdinalIgnoreCase));
            lbPiq.Items.Clear();
            foreach (var piquet in _currPqList)
            {
                lbPiq.Items.Add(piquet.Name);
            }
        }

        private void btAddPiqList_Click(object sender, EventArgs e)
        {
            var addPq = PqList.Where(pq => pq.Name == tbAddPqName.Text).ToList();
            if (addPq.Count > 0 && !lbPiq.Items.Contains(tbAddPqName.Text))
            {
                lbPiq.Items.Clear();
                _currPqList.AddRange(addPq);
                _currPqList.Sort(
                    (item1, item2) => String.Compare(item1.Name, item2.Name, StringComparison.OrdinalIgnoreCase));
                foreach (var piquet in _currPqList)
                {
                    lbPiq.Items.Add(piquet.Name);
                }
            }
            tbAddPqName.Text = "";
        }

        private void btListFilt_Click(object sender, EventArgs e)
        {
            _listOnly = !_listOnly;
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
            rbAddWall.Checked = cbSpline.Checked;
            rbAddWay.Checked = false;
            rbAddPrecipice.Enabled = cbCGN.Checked;
            rbStone.Enabled = cbCGN.Checked;
        }

        private void cbTrapez_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbTrapez.Checked)
                cbSpline.Checked = false;
        }
        
        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar != 13) return;
            
            
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                var name = listBox1.SelectedItem;
                _spline.RemoveAll(s => ReferenceEquals(s.Name, name));
                _cgnList.RemoveAll(c => ReferenceEquals(c.Name, name));
                ReloadNames();
            }
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Text = e.Delta.ToString();
        }

        private void ReloadNames()
        {
            listBox1.Items.Clear();
            foreach (var spline1 in _spline.Where(s => s.Type == SplineType.Wall))
            {
                listBox1.Items.Add(spline1.Name);
            }
            foreach (var spline1 in _spline.Where(s => s.Type == SplineType.Precipice))
            {
                listBox1.Items.Add(spline1.Name);
            }
            foreach (var stone in _cgnList.Where(s => s.Type == CgnType.Stone))
            {
                listBox1.Items.Add(stone.Name);
            }
        }

        private void cbCGN_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCGN.Checked)
            {
                cbSpline.Checked = false;
            }
            rbAddPrecipice.Enabled = cbCGN.Checked;
            rbStone.Enabled = cbCGN.Checked;
            rbAddPrecipice.Checked = cbCGN.Checked;
            rbStone.Checked = false;
            rbAddWall.Enabled = cbSpline.Checked;
            rbAddWay.Enabled = cbSpline.Checked;
        }

        private void anT_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Delete:
                    var name = listBox1.SelectedItem;
                    _spline.RemoveAll(s => ReferenceEquals(s.Name, name));
                    _cgnList.RemoveAll(c => ReferenceEquals(c.Name, name));
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
                        var spline = new Spline(SplineType.Wall, _curSpline) { Name = "Стена" + numb };
                        _spline.Add(spline);
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
                        var spline = new Spline(SplineType.Precipice, _curSpline) { Name = "Обрыв" + numb };
                        _spline.Add(spline);
                    }


                    listBox1.Items.Clear();
                    foreach (var spline1 in _spline.Where(s => s.Type == SplineType.Wall))
                    {
                        listBox1.Items.Add(spline1.Name);
                    }
                    foreach (var spline1 in _spline.Where(s => s.Type == SplineType.Precipice))
                    {
                        listBox1.Items.Add(spline1.Name);
                    }
                    _curSpline = new List<SplinePoint>();
                    break;
                case Keys.Escape:
                    _curSpline = new List<SplinePoint>();
                    break;
            }
        }

    }
}
