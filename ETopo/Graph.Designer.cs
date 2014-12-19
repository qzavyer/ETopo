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
            this.anT = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.btBuild = new System.Windows.Forms.Button();
            this.tVis = new System.Windows.Forms.Timer(this.components);
            this.btZoomIn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.anT.Location = new System.Drawing.Point(12, 43);
            this.anT.Name = "anT";
            this.anT.Size = new System.Drawing.Size(807, 463);
            this.anT.StencilBits = ((byte)(0));
            this.anT.TabIndex = 3;
            this.anT.Load += new System.EventHandler(this.anT_Load);
            this.anT.Click += new System.EventHandler(this.anT_Click);
            this.anT.MouseClick += new System.Windows.Forms.MouseEventHandler(this.anT_MouseClick);
            this.anT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.anT_MouseDown);
            this.anT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.anT_MouseMove);
            this.anT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.anT_MouseUp);
            this.anT.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.anT_MouseWheel);
            // 
            // btBuild
            // 
            this.btBuild.Location = new System.Drawing.Point(12, 12);
            this.btBuild.Name = "btBuild";
            this.btBuild.Size = new System.Drawing.Size(105, 25);
            this.btBuild.TabIndex = 4;
            this.btBuild.Text = "Построить";
            this.btBuild.UseVisualStyleBackColor = true;
            this.btBuild.Click += new System.EventHandler(this.btBuild_Click);
            // 
            // tVis
            // 
            this.tVis.Interval = 25;
            this.tVis.Tick += new System.EventHandler(this.tVis_Tick);
            // 
            // btZoomIn
            // 
            this.btZoomIn.Location = new System.Drawing.Point(123, 12);
            this.btZoomIn.Name = "btZoomIn";
            this.btZoomIn.Size = new System.Drawing.Size(25, 25);
            this.btZoomIn.TabIndex = 5;
            this.btZoomIn.Text = "+";
            this.btZoomIn.UseVisualStyleBackColor = true;
            this.btZoomIn.Click += new System.EventHandler(this.btZoomIn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 25);
            this.button1.TabIndex = 6;
            this.button1.Text = "-";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(185, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 518);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btZoomIn);
            this.Controls.Add(this.btBuild);
            this.Controls.Add(this.anT);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Graph";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Graph_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public Tao.Platform.Windows.SimpleOpenGlControl anT;
        private System.Windows.Forms.Button btBuild;
        private System.Windows.Forms.Timer tVis;
        private System.Windows.Forms.Button btZoomIn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}