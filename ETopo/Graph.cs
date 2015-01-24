using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
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
        public double Buttom;
        public double top;
        private double? _splineX;
        private double? _splineY;
        private double _devX;
        private double _devY;
        private bool _listOnly;

        private List<Spline> _spline;
        public List<Piquet> PqList;
        private List<Piquet> _currPqList;
        public List<Trace> TrcList; 
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
            var dev = Math.Max(top - Buttom, right - left);
            _moveX = -left;
            _moveY = -Buttom;
            var screenW = dev*anT.Width/anT.Height;
            var screenH = dev;
            _devX = (float)screenW / anT.Width;
            _devY = (float)screenH / anT.Height;
            Glu.gluOrtho2D(0.0, screenW, 0.0, screenH);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            _spline = new List<Spline>();
            _currPqList = new List<Piquet>();
        }

        private static float Y(Spline spline0, Spline spline1, double t)
        {
            return
                (float)
                    (spline0.Y*(2*t*t*t - 3*t*t + 1) + spline0.Rb*(t*t*t - 2*t*t + t) + spline1.Y*(-2*t*t*t + 3*t*t) +
                     spline1.Ra*(t*t*t - t*t));
        }

        private static float Y(Spline spline0, Spline spline1, double t, double alpha)
        {
            return
                (float)
                    (spline0.Y*(2*t*t*t - 3*t*t + 1) + spline0.Rb*(t*t*t - 2*t*t + t) + spline1.Y*(-2*t*t*t + 3*t*t) +
                     spline1.Ra*(t*t*t - t*t));
        }

        private static Spline Rotate(double x, double y, double alpha)
        {
            return new Spline
            {
                X = x*Math.Cos(alpha) - y*Math.Sin(alpha),
                Y = x*Math.Sin(alpha) + y*Math.Cos(alpha)
            };
        }
        
        private static void GetVector(IList<Spline> lst)
        {
            for (var i = 1; i < lst.Count - 2; i++)
            {
                var g1 = (lst[i].Y - lst[i - 1].Y) * (1 + lst[i].Bias);
                var g2 = (lst[i + 1].Y - lst[i].Y) * (1 - lst[i].Bias);
                var g3 = g2 - g1;
                lst[i].Ra = (1 - lst[1].Tens) * (g1 + 0.5 * g3 * (1 + lst[i].Cont));
                lst[i].Rb = (1 - lst[1].Tens) * (g1 + 0.5 * g3 * (1 - lst[i].Cont));
            }
            lst[0].Ra = lst[1].Y - lst[0].Y;
            lst[0].Rb = (1.5 * (lst[1].Y - lst[0].Y) - 0.5 * lst[1].Ra) * (1 - lst[0].Tens);
            lst[lst.Count - 1].Rb = (1.5 * (lst[lst.Count - 1].Y - lst[lst.Count - 2].Y) - 0.5 * lst[lst.Count - 2].Ra) * (1 - lst[lst.Count - 1].Tens);
            lst[lst.Count - 1].Ra = lst[lst.Count - 1].Y - lst[lst.Count - 2].Y;
        }
        
        private void btBuild_Click(object sender, EventArgs e)
        {
            tVis.Enabled = true;
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

            var glScale = 1 / max;
            if (cbTrapez.Checked)
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
                    if (Math.Abs(trace.FromDown + trace.FromUp + trace.FromLeft + trace.FromRight) < 0.01)
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

            foreach (var piquet in PqList.Where(p => !_listOnly || lbPiq.Items.Contains(p.Name)))
            {
                Gl.glPointSize(10);
                if (_currPqList.Contains(piquet))
                    Gl.glColor3f(0.5f, 0.5f, 0.5f);
                else
                    Gl.glColor3f(0.0f, 1.0f, 0.0f);
                    
                Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2d((float) (piquet.X*_scale + _moveX), (float) (piquet.Y*_scale + _moveY));
                Gl.glEnd();
                Gl.glPointSize(1);
                Gl.glColor3f(0.0f, 0.0f, 0.0f);
                PrintText2D((float)(piquet.X*_scale+_moveX), (float)(piquet.Y*_scale+_moveY), piquet.Name);
            }
            Gl.glFlush();
            anT.Invalidate();
        }

        private void PrintText2D(float x, float y, string text)
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

        private void anT_Click(object sender, EventArgs e)
        {
            foreach (var piquet in PqList)
            {
                //if(piquet.X==e)
            }
        }

        private void tVis_Tick(object sender, EventArgs e)
        {
            DrawMap();
        }

        private void btZoomIn_Click(object sender, EventArgs e)
        {
            _scale *= 1.1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _scale /= 1.1;
        }

        private void anT_MouseClick(object sender, MouseEventArgs e)
        {
            // приводим к нужному нам формату, в соотвествии с настройками проекции 
            var lineX = (e.X*_devX - _moveX)/_scale;
            var lineY = ((anT.Height - e.Y)*_devY - _moveY)/_scale;
            if (!cbSpline.Checked)
            {
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
                    }
                }
            }
            else
            {
                
            }
            if (_splineX != null && _splineY != null)
            {
                _spline.Add(new Spline { Bias = 0, Cont = 0, Ra = 0, Rb = 0, Tens = 0, X = e.X, Y = e.Y });
            }
            _splineX = e.X;
            _splineY = e.Y;
        }

        private void anT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _move = true;
                _dX = e.X;
                _dY = e.Y;
            }
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
            }
            _dX = e.X;
            _dY = e.Y; 

        }

        private void anT_MouseUp(object sender, MouseEventArgs e)
        {
            _move = false;
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

        private void Graph_Resize(object sender, EventArgs e)
        {
            Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, anT.Width, anT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            var dev = Math.Max(top - Buttom, right - left);
            _moveX = -left;
            _moveY = -Buttom;
            var screenW = dev * anT.Width / anT.Height;
            var screenH = dev;
            _devX = (float)screenW / anT.Width;
            _devY = (float)screenH / anT.Height;
            Glu.gluOrtho2D(0.0, screenW, 0.0, screenH);
            //Glu.gluOrtho2D(left, right, buttom, top);
            //  Glu.gluPerspective(0, (float)anT.Width / anT.Height, 0, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);
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
                cbTrapez.Checked = true;
        }

        private void cbTrapez_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbTrapez.Checked)
                cbSpline.Checked = false;
        }

    }
}
