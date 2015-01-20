namespace ETopo
{
    partial class Graph
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tVis = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btZoomIn = new System.Windows.Forms.Button();
            this.btBuild = new System.Windows.Forms.Button();
            this.anT = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.lbPiq = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tVis
            // 
            this.tVis.Interval = 25;
            this.tVis.Tick += new System.EventHandler(this.tVis_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbPiq);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(941, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 518);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btZoomIn);
            this.panel2.Controls.Add(this.btBuild);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(941, 37);
            this.panel2.TabIndex = 9;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(185, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 25);
            this.button1.TabIndex = 10;
            this.button1.Text = "-";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btZoomIn
            // 
            this.btZoomIn.Location = new System.Drawing.Point(123, 3);
            this.btZoomIn.Name = "btZoomIn";
            this.btZoomIn.Size = new System.Drawing.Size(25, 25);
            this.btZoomIn.TabIndex = 9;
            this.btZoomIn.Text = "+";
            this.btZoomIn.UseVisualStyleBackColor = true;
            this.btZoomIn.Click += new System.EventHandler(this.btZoomIn_Click);
            // 
            // btBuild
            // 
            this.btBuild.Location = new System.Drawing.Point(12, 3);
            this.btBuild.Name = "btBuild";
            this.btBuild.Size = new System.Drawing.Size(105, 25);
            this.btBuild.TabIndex = 8;
            this.btBuild.Text = "Построить";
            this.btBuild.UseVisualStyleBackColor = true;
            this.btBuild.Click += new System.EventHandler(this.btBuild_Click);
            // 
            // anT
            // 
            this.anT.AccumBits = ((byte)(0));
            this.anT.AutoCheckErrors = false;
            this.anT.AutoFinish = false;
            this.anT.AutoMakeCurrent = true;
            this.anT.AutoSwapBuffers = true;
            this.anT.BackColor = System.Drawing.Color.Black;
            this.anT.ColorBits = ((byte)(32));
            this.anT.DepthBits = ((byte)(16));
            this.anT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.anT.Location = new System.Drawing.Point(0, 37);
            this.anT.Name = "anT";
            this.anT.Size = new System.Drawing.Size(941, 481);
            this.anT.StencilBits = ((byte)(0));
            this.anT.TabIndex = 10;
            this.anT.Load += new System.EventHandler(this.anT_Load);
            this.anT.Click += new System.EventHandler(this.anT_Click);
            this.anT.MouseClick += new System.Windows.Forms.MouseEventHandler(this.anT_MouseClick);
            this.anT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.anT_MouseDown);
            this.anT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.anT_MouseMove);
            this.anT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.anT_MouseUp);
            this.anT.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.anT_MouseWheel);
            // 
            // lbPiq
            // 
            this.lbPiq.FormattingEnabled = true;
            this.lbPiq.ItemHeight = 16;
            this.lbPiq.Location = new System.Drawing.Point(6, 12);
            this.lbPiq.Name = "lbPiq";
            this.lbPiq.Size = new System.Drawing.Size(182, 164);
            this.lbPiq.TabIndex = 0;
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 518);
            this.Controls.Add(this.anT);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Graph";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Graph_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tVis;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btZoomIn;
        private System.Windows.Forms.Button btBuild;
        public Tao.Platform.Windows.SimpleOpenGlControl anT;
        private System.Windows.Forms.ListBox lbPiq;
    }
}