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
            this.label1 = new System.Windows.Forms.Label();
            this.rbAddEnter = new System.Windows.Forms.RadioButton();
            this.rbAddStalagnate = new System.Windows.Forms.RadioButton();
            this.rbAddStalagmite = new System.Windows.Forms.RadioButton();
            this.rbAddStalactite = new System.Windows.Forms.RadioButton();
            this.rbAddWater = new System.Windows.Forms.RadioButton();
            this.rbAddStone = new System.Windows.Forms.RadioButton();
            this.cbCGN = new System.Windows.Forms.CheckBox();
            this.rbAddWay = new System.Windows.Forms.RadioButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.rbAddPrecipice = new System.Windows.Forms.RadioButton();
            this.rbAddWall = new System.Windows.Forms.RadioButton();
            this.cbSpline = new System.Windows.Forms.CheckBox();
            this.cbTrapez = new System.Windows.Forms.CheckBox();
            this.anT = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.graphMenu = new System.Windows.Forms.MenuStrip();
            this.mFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mExport = new System.Windows.Forms.ToolStripMenuItem();
            this.sdSave = new System.Windows.Forms.SaveFileDialog();
            this.sdPdf = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.graphMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tVis
            // 
            this.tVis.Interval = 25;
            this.tVis.Tick += new System.EventHandler(this.tVis_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.rbAddEnter);
            this.panel1.Controls.Add(this.rbAddStalagnate);
            this.panel1.Controls.Add(this.rbAddStalagmite);
            this.panel1.Controls.Add(this.rbAddStalactite);
            this.panel1.Controls.Add(this.rbAddWater);
            this.panel1.Controls.Add(this.rbAddStone);
            this.panel1.Controls.Add(this.cbCGN);
            this.panel1.Controls.Add(this.rbAddWay);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.rbAddPrecipice);
            this.panel1.Controls.Add(this.rbAddWall);
            this.panel1.Controls.Add(this.cbSpline);
            this.panel1.Controls.Add(this.cbTrapez);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(912, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 603);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 362);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "Список элементов";
            // 
            // rbAddEnter
            // 
            this.rbAddEnter.AutoSize = true;
            this.rbAddEnter.Enabled = false;
            this.rbAddEnter.Location = new System.Drawing.Point(22, 121);
            this.rbAddEnter.Name = "rbAddEnter";
            this.rbAddEnter.Size = new System.Drawing.Size(60, 21);
            this.rbAddEnter.TabIndex = 17;
            this.rbAddEnter.TabStop = true;
            this.rbAddEnter.Text = "Вход";
            this.rbAddEnter.UseVisualStyleBackColor = true;
            // 
            // rbAddStalagnate
            // 
            this.rbAddStalagnate.AutoSize = true;
            this.rbAddStalagnate.Enabled = false;
            this.rbAddStalagnate.Location = new System.Drawing.Point(22, 310);
            this.rbAddStalagnate.Name = "rbAddStalagnate";
            this.rbAddStalagnate.Size = new System.Drawing.Size(107, 21);
            this.rbAddStalagnate.TabIndex = 16;
            this.rbAddStalagnate.TabStop = true;
            this.rbAddStalagnate.Text = "Сталагнаты";
            this.rbAddStalagnate.UseVisualStyleBackColor = true;
            // 
            // rbAddStalagmite
            // 
            this.rbAddStalagmite.AutoSize = true;
            this.rbAddStalagmite.Enabled = false;
            this.rbAddStalagmite.Location = new System.Drawing.Point(22, 283);
            this.rbAddStalagmite.Name = "rbAddStalagmite";
            this.rbAddStalagmite.Size = new System.Drawing.Size(108, 21);
            this.rbAddStalagmite.TabIndex = 15;
            this.rbAddStalagmite.TabStop = true;
            this.rbAddStalagmite.Text = "Сталагмиты";
            this.rbAddStalagmite.UseVisualStyleBackColor = true;
            // 
            // rbAddStalactite
            // 
            this.rbAddStalactite.AutoSize = true;
            this.rbAddStalactite.Enabled = false;
            this.rbAddStalactite.Location = new System.Drawing.Point(22, 256);
            this.rbAddStalactite.Name = "rbAddStalactite";
            this.rbAddStalactite.Size = new System.Drawing.Size(108, 21);
            this.rbAddStalactite.TabIndex = 14;
            this.rbAddStalactite.TabStop = true;
            this.rbAddStalactite.Text = "Сталактиты";
            this.rbAddStalactite.UseVisualStyleBackColor = true;
            // 
            // rbAddWater
            // 
            this.rbAddWater.AutoSize = true;
            this.rbAddWater.Enabled = false;
            this.rbAddWater.Location = new System.Drawing.Point(22, 229);
            this.rbAddWater.Name = "rbAddWater";
            this.rbAddWater.Size = new System.Drawing.Size(63, 21);
            this.rbAddWater.TabIndex = 13;
            this.rbAddWater.TabStop = true;
            this.rbAddWater.Text = "Лужи";
            this.rbAddWater.UseVisualStyleBackColor = true;
            // 
            // rbAddStone
            // 
            this.rbAddStone.AutoSize = true;
            this.rbAddStone.Enabled = false;
            this.rbAddStone.Location = new System.Drawing.Point(22, 202);
            this.rbAddStone.Name = "rbAddStone";
            this.rbAddStone.Size = new System.Drawing.Size(71, 21);
            this.rbAddStone.TabIndex = 12;
            this.rbAddStone.TabStop = true;
            this.rbAddStone.Text = "Камни";
            this.rbAddStone.UseVisualStyleBackColor = true;
            // 
            // cbCGN
            // 
            this.cbCGN.AutoSize = true;
            this.cbCGN.Location = new System.Drawing.Point(6, 148);
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
            this.rbAddWay.Location = new System.Drawing.Point(22, 94);
            this.rbAddWay.Name = "rbAddWay";
            this.rbAddWay.Size = new System.Drawing.Size(176, 21);
            this.rbAddWay.TabIndex = 9;
            this.rbAddWay.TabStop = true;
            this.rbAddWay.Text = "Неисследованный ход";
            this.rbAddWay.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(6, 382);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(217, 244);
            this.listBox1.TabIndex = 10;
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // rbAddPrecipice
            // 
            this.rbAddPrecipice.AutoSize = true;
            this.rbAddPrecipice.Enabled = false;
            this.rbAddPrecipice.Location = new System.Drawing.Point(22, 175);
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
            this.rbAddWall.Location = new System.Drawing.Point(22, 66);
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
            this.cbSpline.Location = new System.Drawing.Point(6, 39);
            this.cbSpline.Name = "cbSpline";
            this.cbSpline.Size = new System.Drawing.Size(141, 21);
            this.cbSpline.TabIndex = 7;
            this.cbSpline.Text = "Элементы карты";
            this.cbSpline.UseVisualStyleBackColor = true;
            this.cbSpline.CheckedChanged += new System.EventHandler(this.cbSpline_CheckedChanged);
            // 
            // cbTrapez
            // 
            this.cbTrapez.AutoSize = true;
            this.cbTrapez.Checked = true;
            this.cbTrapez.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrapez.Location = new System.Drawing.Point(6, 12);
            this.cbTrapez.Name = "cbTrapez";
            this.cbTrapez.Size = new System.Drawing.Size(187, 21);
            this.cbTrapez.TabIndex = 1;
            this.cbTrapez.Text = "Показать ширину ходов";
            this.cbTrapez.UseVisualStyleBackColor = true;
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
            this.anT.Location = new System.Drawing.Point(0, 26);
            this.anT.Name = "anT";
            this.anT.Size = new System.Drawing.Size(912, 603);
            this.anT.StencilBits = ((byte)(0));
            this.anT.TabIndex = 10;
            this.anT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.anT_KeyDown);
            this.anT.MouseClick += new System.Windows.Forms.MouseEventHandler(this.anT_MouseClick);
            this.anT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.anT_MouseDown);
            this.anT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.anT_MouseMove);
            this.anT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.anT_MouseUp);
            this.anT.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.anT_MouseWheel);
            // 
            // graphMenu
            // 
            this.graphMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFile});
            this.graphMenu.Location = new System.Drawing.Point(0, 0);
            this.graphMenu.Name = "graphMenu";
            this.graphMenu.Size = new System.Drawing.Size(1138, 26);
            this.graphMenu.TabIndex = 18;
            this.graphMenu.Text = "menuStrip1";
            // 
            // mFile
            // 
            this.mFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSave,
            this.mExport});
            this.mFile.Name = "mFile";
            this.mFile.Size = new System.Drawing.Size(54, 22);
            this.mFile.Text = "Файл";
            // 
            // mSave
            // 
            this.mSave.Name = "mSave";
            this.mSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mSave.Size = new System.Drawing.Size(197, 22);
            this.mSave.Text = "Сохранить";
            this.mSave.Click += new System.EventHandler(this.mSave_Click);
            // 
            // mExport
            // 
            this.mExport.Name = "mExport";
            this.mExport.Size = new System.Drawing.Size(197, 22);
            this.mExport.Text = "Экспорт в PDF";
            this.mExport.Click += new System.EventHandler(this.mExport_Click);
            // 
            // sdPdf
            // 
            this.sdPdf.DefaultExt = "pdf";
            this.sdPdf.Filter = "Файлы PDF|*.pdf";
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 629);
            this.Controls.Add(this.anT);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.graphMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.graphMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Graph";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Graph_Load);
            this.Resize += new System.EventHandler(this.Graph_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.graphMenu.ResumeLayout(false);
            this.graphMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tVis;
        private System.Windows.Forms.Panel panel1;
        public Tao.Platform.Windows.SimpleOpenGlControl anT;
        private System.Windows.Forms.CheckBox cbTrapez;
        private System.Windows.Forms.CheckBox cbSpline;
        private System.Windows.Forms.RadioButton rbAddPrecipice;
        private System.Windows.Forms.RadioButton rbAddWall;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RadioButton rbAddStone;
        private System.Windows.Forms.CheckBox cbCGN;
        private System.Windows.Forms.RadioButton rbAddWay;
        private System.Windows.Forms.RadioButton rbAddEnter;
        private System.Windows.Forms.RadioButton rbAddStalagnate;
        private System.Windows.Forms.RadioButton rbAddStalagmite;
        private System.Windows.Forms.RadioButton rbAddStalactite;
        private System.Windows.Forms.RadioButton rbAddWater;
        private System.Windows.Forms.MenuStrip graphMenu;
        private System.Windows.Forms.ToolStripMenuItem mFile;
        private System.Windows.Forms.ToolStripMenuItem mSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem mExport;
        private System.Windows.Forms.SaveFileDialog sdSave;
        private System.Windows.Forms.SaveFileDialog sdPdf;
    }
}