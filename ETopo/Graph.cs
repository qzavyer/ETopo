using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace ETopo
{
    public partial class Graph : Form
    {
        private double _depth = 100;
        private double _left = 0;
        private double _top = 0;
        private double _scale = 1;
        private double _moveX = 0;
        private double _moveY = 0;
        private bool _move = false;
        private double dX = 0;
        private double dY = 0;
        public double left = 0.0;
        public double right = 0.0;
        public double buttom = 0.0;
        public double top = 0.0;

        public List<Spline> _spline;
        public List<Piquet> PqList;
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
            Glu.gluOrtho2D(left, right, buttom, top);
            //  Glu.gluPerspective(0, (float)anT.Width / anT.Height, 0, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            _spline = new List<Spline>();
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
            return (float)(spline0.Y * (2 * t * t * t - 3 * t * t + 1) + spline0.Rb * (t * t * t - 2 * t * t + t) + spline1.Y * (-2 * t * t * t + 3 * t * t) +
                   spline1.Ra * (t * t * t - t * t));
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
            _left = 0;
            _top = 0;
            _depth = 0;
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
                }
                var trace2 = trace;
                foreach (var piquet in PqList.Where(piquet => piquet.Name == trace2.To))
                {
                    x1 = piquet.X;
                    y1 = piquet.Y;
                }
                var alpha = (x1 - x0) < 0.000001 ? Math.PI/2*(y1 > y0 ? 1 : -1) : Math.Atan((y1 - y0)/(x1 - x0));
                var path = new GraphicsPath();
                var point0 = new Point();
                var point1 = new Point();

                var point2 = new Point
                {
                    X = Convert.ToInt32(x1 - Rotate(trace.Left, 0, alpha + Math.PI / 2).X),
                    Y = Convert.ToInt32(y1 - Rotate(trace.Left, 0, alpha + Math.PI / 2).Y)
                };
                var point3 = new Point
                {
                    X = Convert.ToInt32(x1 + Rotate(trace.Right, 0, alpha + Math.PI / 2).X),
                    Y = Convert.ToInt32(y1 + Rotate(trace.Right, 0, alpha + Math.PI / 2).Y)
                };
                if (Math.Abs(trace.FromDown + trace.FromUp + trace.FromLeft + trace.FromRight) < 0.01)
                {
                    var trace3 = trace;
                    foreach (var trc in TrcList.Where(trc => trc.To == trace3.From))
                    {
                        point0.X = Convert.ToInt32(x0 - Rotate(trc.Left, 0, alpha + Math.PI / 2).X);
                        point0.Y = Convert.ToInt32(y0 - Rotate(trc.Left, 0, alpha + Math.PI / 2).Y);
                        point1.X = Convert.ToInt32(x0 + Rotate(trc.Right, 0, alpha + Math.PI / 2).X);
                        point1.Y = Convert.ToInt32(y0 + Rotate(trc.Right, 0, alpha + Math.PI / 2).Y);
                    }
                }
                else
                {
                    point0.X = Convert.ToInt32(x0 - Rotate(trace.FromLeft, 0, alpha + Math.PI / 2).X);
                    point0.Y = Convert.ToInt32(y0 - Rotate(trace.FromLeft, 0, alpha + Math.PI / 2).Y);
                    point1.X = Convert.ToInt32(x0 + Rotate(trace.FromRight, 0, alpha + Math.PI / 2).X);
                    point1.Y = Convert.ToInt32(y0 + Rotate(trace.FromRight, 0, alpha + Math.PI / 2).Y);
                }
                path.AddLine(point0, point1);
                path.AddLine(point1, point3);
                path.AddLine(point3, point2);
                path.AddLine(point2, point0);
                Gl.glLineWidth(1);
                Gl.glColor3f(0.5f, 1f, 1f);
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glVertex2d(point0.X * _scale + _moveX, point0.Y * _scale + _moveY);
                Gl.glVertex2d(point1.X * _scale + _moveX, point1.Y * _scale + _moveY);
                Gl.glVertex2d(point3.X * _scale + _moveX, point3.Y * _scale + _moveY);
                Gl.glVertex2d(point2.X * _scale + _moveX, point2.Y * _scale + _moveY);
                // Gl.glVertex2d(0, 0);
                Gl.glEnd();
            }
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
        //    Gl.glPopMatrix();
            Gl.glFlush();
            anT.Invalidate();
        }
        
        private void anT_Click(object sender, EventArgs e)
        {
           
            DrawMap();
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

        private void DrawSpline()
        {
            
        }

        private void FillCave()
        {
            var t = Glu.gluNewTess();
            Glu.gluTessProperty(t, Glu.GLU_TESS_WINDING_RULE, Glu.GLU_TESS_WINDING_ODD);
            Glu.gluTessProperty(t, Glu.GLU_TESS_BOUNDARY_ONLY, Gl.GL_FALSE);
            Glu.gluTessProperty(t, Glu.GLU_TESS_TOLERANCE, 0);

            Glu.gluTessCallback(t, Glu.GLU_TESS_BEGIN,new Glu.TessBeginCallback(Gl.glBegin));
            Glu.gluTessCallback(t, Glu.GLU_TESS_END, Gl.glEnd);
            Glu.gluTessCallback(t, Glu.GLU_TESS_VERTEX, new Glu.TessVertexCallback(Gl.glVertex2dv));
            
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glColor3f(0.5f, 0.5f, 1f);
            var max = 0.0;
            foreach (var piquet in PqList)
            {
                max = Math.Max(max, piquet.X);
                max = Math.Max(max, piquet.Y);
            }

            var glScale = 1 / max;
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
                }
                var trace2 = trace;
                foreach (var piquet in PqList.Where(piquet => piquet.Name == trace2.To))
                {
                    x1 = piquet.X;
                    y1 = piquet.Y;
                }
                var alpha = (x1 - x0) < 0.000001 ? Math.PI / 2 * (y1 > y0 ? 1 : -1) : Math.Atan((y1 - y0) / (x1 - x0));
                //var alpha = Math.Atan((y1 - y0) / (x1 - x0));
                var path = new GraphicsPath();
                var point0 = new Point();
                var point1 = new Point();

                var point2 = new Point
                {
                    X = Convert.ToInt32(x1 - Rotate(trace.Left, 0, alpha + Math.PI / 2).X),
                    Y = Convert.ToInt32(y1 - Rotate(trace.Left, 0, alpha + Math.PI / 2).Y)
                };
                var point3 = new Point
                {
                    X = Convert.ToInt32(x1 + Rotate(trace.Right, 0, alpha + Math.PI / 2).X),
                    Y = Convert.ToInt32(y1 + Rotate(trace.Right, 0, alpha + Math.PI / 2).Y)
                };
                if (Math.Abs(trace.FromDown + trace.FromUp + trace.FromLeft + trace.FromRight) < 0.01)
                {
                    var trace3 = trace;
                    foreach (var trc in TrcList.Where(trc => trc.To == trace3.From))
                    {
                        point0.X = Convert.ToInt32(x0 - Rotate(trc.Left, 0, alpha + Math.PI / 2).X);
                        point0.Y = Convert.ToInt32(y0 - Rotate(trc.Left, 0, alpha + Math.PI / 2).Y);
                        point1.X = Convert.ToInt32(x0 + Rotate(trc.Right, 0, alpha + Math.PI / 2).X);
                        point1.Y = Convert.ToInt32(y0 + Rotate(trc.Right, 0, alpha + Math.PI / 2).Y);
                    }
                }
                else
                {
                    point0.X = Convert.ToInt32(x0 - Rotate(trace.FromLeft, 0, alpha + Math.PI / 2).X);
                    point0.Y = Convert.ToInt32(y0 - Rotate(trace.FromLeft, 0, alpha + Math.PI / 2).Y);
                    point1.X = Convert.ToInt32(x0 + Rotate(trace.FromRight, 0, alpha + Math.PI / 2).X);
                    point1.Y = Convert.ToInt32(y0 + Rotate(trace.FromRight, 0, alpha + Math.PI / 2).Y);
                }
                path.AddLine(point0, point1);
                path.AddLine(point1, point3);
                path.AddLine(point3, point2);
                path.AddLine(point2, point0);

                Glu.gluTessBeginPolygon(t, IntPtr.Zero); // начинаем тесселяцию
                Glu.gluTessBeginContour(t); // начинаем описание очередного кривого полигона
                Glu.gluTessVertex(t, new[] { point0.X * _scale + _moveX, point0.Y * _scale + _moveY, 0 }, new[] { point0.X * _scale + _moveX, point0.Y * _scale + _moveY, 0 });
                Glu.gluTessVertex(t, new[] { point1.X * _scale + _moveX, point1.Y * _scale + _moveY, 0 }, new[] { point1.X * _scale + _moveX, point1.Y * _scale + _moveY, 0 });
                Glu.gluTessVertex(t, new[] { point3.X * _scale + _moveX, point3.Y * _scale + _moveY, 0 }, new[] { point3.X * _scale + _moveX, point3.Y * _scale + _moveY, 0 });
                Glu.gluTessVertex(t, new[] { point2.X * _scale + _moveX, point2.Y * _scale + _moveY, 0 }, new[] { point2.X * _scale + _moveX, point2.Y * _scale + _moveY, 0 });
                Glu.gluTessEndContour(t); // сообщаем глу, что контур закончен
                Glu.gluTessEndPolygon(t); // сообщаем, что ввод данных закончен
            }
            //    Gl.glPopMatrix();
            Gl.glFlush();
            Glu.gluDeleteTess(t);
            anT.Invalidate();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FillCave();
        }

        private void anT_Load(object sender, EventArgs e)
        {

        }

        private void anT_MouseClick(object sender, MouseEventArgs e)
        {
            _spline.Add(new Spline { Bias = 0, Cont = 0, Ra = 0, Rb = 0, Tens = 0, X = e.X, Y = e.Y });
        }

        private void anT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _move = true;
                dX = e.X;
                dY = e.Y;
            }
        }

        private void anT_MouseMove(object sender, MouseEventArgs e)
        {
            if (_move)
            {
                if (e.X > dX)
                {
                    _moveX += 0.2f;
                }
                if (e.X < dX)
                {
                    _moveX -= 0.2f;
                }
                if (e.Y > dY)
                {
                    _moveY -= 0.2f;
                }
                if (e.Y < dY)
                {
                    _moveY += 0.2f;
                }
            }
            dX = e.X;
            dY = e.Y; 

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

    }
}
