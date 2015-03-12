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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graph));
            this.tVis = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbStone = new System.Windows.Forms.RadioButton();
            this.cbCGN = new System.Windows.Forms.CheckBox();
            this.rbAddWay = new System.Windows.Forms.RadioButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.rbAddPrecipice = new System.Windows.Forms.RadioButton();
            this.rbAddWall = new System.Windows.Forms.RadioButton();
            this.cbSpline = new System.Windows.Forms.CheckBox();
            this.btListFilt = new System.Windows.Forms.Button();
            this.btClrLst = new System.Windows.Forms.Button();
            this.btRemList = new System.Windows.Forms.Button();
            this.btAddPiqList = new System.Windows.Forms.Button();
            this.tbAddPqName = new System.Windows.Forms.TextBox();
            this.cbTrapez = new System.Windows.Forms.CheckBox();
            this.lbPiq = new System.Windows.Forms.ListBox();
            this.anT = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tVis
            // 
            this.tVis.Interval = 25;
            this.tVis.Tick += new System.EventHandler(this.tVis_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbStone);
            this.panel1.Controls.Add(this.cbCGN);
            this.panel1.Controls.Add(this.rbAddWay);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.rbAddPrecipice);
            this.panel1.Controls.Add(this.rbAddWall);
            this.panel1.Controls.Add(this.cbSpline);
            this.panel1.Controls.Add(this.btListFilt);
            this.panel1.Controls.Add(this.btClrLst);
            this.panel1.Controls.Add(this.btRemList);
            this.panel1.Controls.Add(this.btAddPiqList);
            this.panel1.Controls.Add(this.tbAddPqName);
            this.panel1.Controls.Add(this.cbTrapez);
            this.panel1.Controls.Add(this.lbPiq);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(893, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 629);
            this.panel1.TabIndex = 8;
            // 
            // rbStone
            // 
            this.rbStone.AutoSize = true;
            this.rbStone.Enabled = false;
            this.rbStone.Location = new System.Drawing.Point(22, 509);
            this.rbStone.Name = "rbStone";
            this.rbStone.Size = new System.Drawing.Size(71, 21);
            this.rbStone.TabIndex = 12;
            this.rbStone.TabStop = true;
            this.rbStone.Text = "Камни";
            this.rbStone.UseVisualStyleBackColor = true;
            // 
            // cbCGN
            // 
            this.cbCGN.AutoSize = true;
            this.cbCGN.Location = new System.Drawing.Point(6, 455);
            this.cbCGN.Name = "cbCGN";
            this.cbCGN.Size = new System.Drawing.Size(58, 21);
            this.cbCGN.TabIndex = 11;
            this.cbCGN.Text = "УГО";
            this.cbCGN.UseVisualStyleBackColor = true;
            this.cbCGN.CheckedChanged += new System.EventHandler(this.cbCGN_CheckedChanged);
            // 
            // rbAddWay
            // 
            this.rbAddWay.AutoSize = true;
            this.rbAddWay.Enabled = false;
            this.rbAddWay.Location = new System.Drawing.Point(22, 428);
            this.rbAddWay.Name = "rbAddWay";
            this.rbAddWay.Size = new System.Drawing.Size(54, 21);
            this.rbAddWay.TabIndex = 9;
            this.rbAddWay.TabStop = true;
            this.rbAddWay.Text = "Ход";
            this.rbAddWay.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(171, 236);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(182, 164);
            this.listBox1.TabIndex = 10;
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            this.listBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listBox1_KeyPress);
            // 
            // rbAddPrecipice
            // 
            this.rbAddPrecipice.AutoSize = true;
            this.rbAddPrecipice.Enabled = false;
            this.rbAddPrecipice.Location = new System.Drawing.Point(22, 482);
            this.rbAddPrecipice.Name = "rbAddPrecipice";
            this.rbAddPrecipice.Size = new System.Drawing.Size(73, 21);
            this.rbAddPrecipice.TabIndex = 10;
            this.rbAddPrecipice.TabStop = true;
            this.rbAddPrecipice.Text = "Обрыв";
            this.rbAddPrecipice.UseVisualStyleBackColor = true;
            // 
            // rbAddWall
            // 
            this.rbAddWall.AutoSize = true;
            this.rbAddWall.Enabled = false;
            this.rbAddWall.Location = new System.Drawing.Point(22, 400);
            this.rbAddWall.Name = "rbAddWall";
            this.rbAddWall.Size = new System.Drawing.Size(69, 21);
            this.rbAddWall.TabIndex = 8;
            this.rbAddWall.TabStop = true;
            this.rbAddWall.Text = "Стена";
            this.rbAddWall.UseVisualStyleBackColor = true;
            // 
            // cbSpline
            // 
            this.cbSpline.AutoSize = true;
            this.cbSpline.Location = new System.Drawing.Point(6, 373);
            this.cbSpline.Name = "cbSpline";
            this.cbSpline.Size = new System.Drawing.Size(141, 21);
            this.cbSpline.TabIndex = 7;
            this.cbSpline.Text = "Элементы карты";
            this.cbSpline.UseVisualStyleBackColor = true;
            this.cbSpline.CheckedChanged += new System.EventHandler(this.cbSpline_CheckedChanged);
            // 
            // btListFilt
            // 
            this.btListFilt.Location = new System.Drawing.Point(6, 236);
            this.btListFilt.Name = "btListFilt";
            this.btListFilt.Size = new System.Drawing.Size(105, 25);
            this.btListFilt.TabIndex = 6;
            this.btListFilt.Text = "Фильтровать";
            this.btListFilt.UseVisualStyleBackColor = true;
            this.btListFilt.Click += new System.EventHandler(this.btListFilt_Click);
            // 
            // btClrLst
            // 
            this.btClrLst.Location = new System.Drawing.Point(154, 207);
            this.btClrLst.Name = "btClrLst";
            this.btClrLst.Size = new System.Drawing.Size(34, 23);
            this.btClrLst.TabIndex = 5;
            this.btClrLst.Text = "X";
            this.btClrLst.UseVisualStyleBackColor = true;
            this.btClrLst.Click += new System.EventHandler(this.btClrLst_Click);
            // 
            // btRemList
            // 
            this.btRemList.Location = new System.Drawing.Point(111, 207);
            this.btRemList.Name = "btRemList";
            this.btRemList.Size = new System.Drawing.Size(34, 23);
            this.btRemList.TabIndex = 4;
            this.btRemList.Text = "-";
            this.btRemList.UseVisualStyleBackColor = true;
            this.btRemList.Click += new System.EventHandler(this.btRemList_Click);
            // 
            // btAddPiqList
            // 
            this.btAddPiqList.Location = new System.Drawing.Point(71, 207);
            this.btAddPiqList.Name = "btAddPiqList";
            this.btAddPiqList.Size = new System.Drawing.Size(34, 23);
            this.btAddPiqList.TabIndex = 3;
            this.btAddPiqList.Text = "+";
            this.btAddPiqList.UseVisualStyleBackColor = true;
            this.btAddPiqList.Click += new System.EventHandler(this.btAddPiqList_Click);
            // 
            // tbAddPqName
            // 
            this.tbAddPqName.Location = new System.Drawing.Point(6, 207);
            this.tbAddPqName.Name = "tbAddPqName";
            this.tbAddPqName.Size = new System.Drawing.Size(59, 22);
            this.tbAddPqName.TabIndex = 2;
            // 
            // cbTrapez
            // 
            this.cbTrapez.AutoSize = true;
            this.cbTrapez.Checked = true;
            this.cbTrapez.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrapez.Location = new System.Drawing.Point(6, 346);
            this.cbTrapez.Name = "cbTrapez";
            this.cbTrapez.Size = new System.Drawing.Size(159, 21);
            this.cbTrapez.TabIndex = 1;
            this.cbTrapez.Text = "Показать трапеции";
            this.cbTrapez.UseVisualStyleBackColor = true;
            this.cbTrapez.CheckedChanged += new System.EventHandler(this.cbTrapez_CheckedChanged);
            // 
            // lbPiq
            // 
            this.lbPiq.FormattingEnabled = true;
            this.lbPiq.ItemHeight = 16;
            this.lbPiq.Location = new System.Drawing.Point(6, 12);
            this.lbPiq.Name = "lbPiq";
            this.lbPiq.Size = new System.Drawing.Size(182, 180);
            this.lbPiq.TabIndex = 0;
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
            this.anT.Location = new System.Drawing.Point(0, 0);
            this.anT.Name = "anT";
            this.anT.Size = new System.Drawing.Size(893, 629);
            this.anT.StencilBits = ((byte)(0));
            this.anT.TabIndex = 10;
            this.anT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.anT_KeyDown);
            this.anT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.anT_KeyPress);
            this.anT.MouseClick += new System.Windows.Forms.MouseEventHandler(this.anT_MouseClick);
            this.anT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.anT_MouseDown);
            this.anT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.anT_MouseMove);
            this.anT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.anT_MouseUp);
            this.anT.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.anT_MouseWheel);
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 629);
            this.Controls.Add(this.anT);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Graph";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Graph_Load);
            this.Resize += new System.EventHandler(this.Graph_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tVis;
        private System.Windows.Forms.Panel panel1;
        public Tao.Platform.Windows.SimpleOpenGlControl anT;
        private System.Windows.Forms.ListBox lbPiq;
        private System.Windows.Forms.CheckBox cbTrapez;
        private System.Windows.Forms.Button btClrLst;
        private System.Windows.Forms.Button btRemList;
        private System.Windows.Forms.Button btAddPiqList;
        private System.Windows.Forms.TextBox tbAddPqName;
        private System.Windows.Forms.Button btListFilt;
        private System.Windows.Forms.CheckBox cbSpline;
        private System.Windows.Forms.RadioButton rbAddPrecipice;
        private System.Windows.Forms.RadioButton rbAddWall;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RadioButton rbStone;
        private System.Windows.Forms.CheckBox cbCGN;
        private System.Windows.Forms.RadioButton rbAddWay;
    }
}