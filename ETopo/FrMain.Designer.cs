namespace ETopo
{
    partial class FrMain
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
            this.dgTopo = new System.Windows.Forms.DataGridView();
            this.ClFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClAz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClClino = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClRight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClUp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClDown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.картаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.съёмкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mData = new System.Windows.Forms.ToolStripMenuItem();
            this.odLoad = new System.Windows.Forms.OpenFileDialog();
            this.sdSave = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgTopo)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgTopo
            // 
            this.dgTopo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTopo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClFrom,
            this.ClTo,
            this.ClLen,
            this.ClAz,
            this.ClClino,
            this.ClLeft,
            this.ClRight,
            this.ClUp,
            this.ClDown,
            this.ClNote});
            this.dgTopo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTopo.Location = new System.Drawing.Point(0, 26);
            this.dgTopo.Name = "dgTopo";
            this.dgTopo.RowHeadersVisible = false;
            this.dgTopo.Size = new System.Drawing.Size(824, 499);
            this.dgTopo.TabIndex = 0;
            // 
            // ClFrom
            // 
            this.ClFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClFrom.FillWeight = 40F;
            this.ClFrom.HeaderText = "От";
            this.ClFrom.Name = "ClFrom";
            // 
            // ClTo
            // 
            this.ClTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClTo.FillWeight = 40F;
            this.ClTo.HeaderText = "До";
            this.ClTo.Name = "ClTo";
            // 
            // ClLen
            // 
            this.ClLen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClLen.FillWeight = 50F;
            this.ClLen.HeaderText = "Длина";
            this.ClLen.Name = "ClLen";
            // 
            // ClAz
            // 
            this.ClAz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClAz.FillWeight = 50F;
            this.ClAz.HeaderText = "Азимут";
            this.ClAz.Name = "ClAz";
            // 
            // ClClino
            // 
            this.ClClino.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClClino.FillWeight = 50F;
            this.ClClino.HeaderText = "Угол";
            this.ClClino.Name = "ClClino";
            // 
            // ClLeft
            // 
            this.ClLeft.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClLeft.FillWeight = 50F;
            this.ClLeft.HeaderText = "Влево";
            this.ClLeft.Name = "ClLeft";
            // 
            // ClRight
            // 
            this.ClRight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClRight.FillWeight = 50F;
            this.ClRight.HeaderText = "Вправо";
            this.ClRight.Name = "ClRight";
            // 
            // ClUp
            // 
            this.ClUp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClUp.FillWeight = 50F;
            this.ClUp.HeaderText = "Вверх";
            this.ClUp.Name = "ClUp";
            // 
            // ClDown
            // 
            this.ClDown.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClDown.FillWeight = 50F;
            this.ClDown.HeaderText = "Вниз";
            this.ClDown.Name = "ClDown";
            // 
            // ClNote
            // 
            this.ClNote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClNote.FillWeight = 135F;
            this.ClNote.HeaderText = "Примечание";
            this.ClNote.Name = "ClNote";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.картаToolStripMenuItem,
            this.съёмкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(824, 26);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новыйToolStripMenuItem,
            this.загрузитьToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.toolStripMenuItem1,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(54, 22);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // новыйToolStripMenuItem
            // 
            this.новыйToolStripMenuItem.Name = "новыйToolStripMenuItem";
            this.новыйToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.новыйToolStripMenuItem.Text = "Новый";
            // 
            // загрузитьToolStripMenuItem
            // 
            this.загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            this.загрузитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.загрузитьToolStripMenuItem.Text = "Загрузить";
            this.загрузитьToolStripMenuItem.Click += new System.EventHandler(this.LoadMenu_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.SaveMenu_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            // 
            // картаToolStripMenuItem
            // 
            this.картаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dToolStripMenuItem});
            this.картаToolStripMenuItem.Name = "картаToolStripMenuItem";
            this.картаToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.картаToolStripMenuItem.Text = "Карта";
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.dToolStripMenuItem.Text = "2D";
            this.dToolStripMenuItem.Click += new System.EventHandler(this.dToolStripMenuItem_Click);
            // 
            // съёмкаToolStripMenuItem
            // 
            this.съёмкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mData});
            this.съёмкаToolStripMenuItem.Name = "съёмкаToolStripMenuItem";
            this.съёмкаToolStripMenuItem.Size = new System.Drawing.Size(71, 22);
            this.съёмкаToolStripMenuItem.Text = "Съёмка";
            // 
            // mData
            // 
            this.mData.Name = "mData";
            this.mData.Size = new System.Drawing.Size(182, 22);
            this.mData.Text = "Данные съемки";
            this.mData.Click += new System.EventHandler(this.mData_Click);
            // 
            // odLoad
            // 
            this.odLoad.Filter = "Файлы пещеры|*.cv";
            this.odLoad.Title = "Загрузить пещеру";
            // 
            // sdSave
            // 
            this.sdSave.DefaultExt = "cv";
            this.sdSave.Filter = "Файлы пещеры|*.cv";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(824, 525);
            this.Controls.Add(this.dgTopo);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(617, 300);
            this.Name = "Form1";
            this.Text = "ETopo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgTopo)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgTopo;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog odLoad;
        private System.Windows.Forms.ToolStripMenuItem картаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sdSave;
        private System.Windows.Forms.ToolStripMenuItem съёмкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClAz;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClClino;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClRight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClNote;
    }
}

