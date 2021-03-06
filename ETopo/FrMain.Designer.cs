﻿namespace ETopo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrMain));
            this.dgTopo = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.картаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.съёмкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mData = new System.Windows.Forms.ToolStripMenuItem();
            this.odLoad = new System.Windows.Forms.OpenFileDialog();
            this.sdSave = new System.Windows.Forms.SaveFileDialog();
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
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.toolStripMenuItem2,
            this.ExportToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ExitToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(54, 22);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // новыйToolStripMenuItem
            // 
            this.новыйToolStripMenuItem.Name = "новыйToolStripMenuItem";
            this.новыйToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.новыйToolStripMenuItem.Text = "Новый";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.OpenToolStripMenuItem.Text = "Загрузить";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.LoadMenu_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.SaveToolStripMenuItem.Text = "Сохранить";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveMenu_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(194, 6);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.ExportToolStripMenuItem.Text = "Экспорт";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(194, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenu_Click);
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
            this.dToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.dToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
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
            this.mData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mData.Size = new System.Drawing.Size(232, 22);
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
            // ClFrom
            // 
            this.ClFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClFrom.FillWeight = 40F;
            this.ClFrom.HeaderText = "От";
            this.ClFrom.Name = "ClFrom";
            this.ClFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClTo
            // 
            this.ClTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClTo.FillWeight = 40F;
            this.ClTo.HeaderText = "До";
            this.ClTo.Name = "ClTo";
            this.ClTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClLen
            // 
            this.ClLen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClLen.FillWeight = 50F;
            this.ClLen.HeaderText = "Длина";
            this.ClLen.Name = "ClLen";
            this.ClLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClAz
            // 
            this.ClAz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClAz.FillWeight = 50F;
            this.ClAz.HeaderText = "Азимут";
            this.ClAz.Name = "ClAz";
            this.ClAz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClClino
            // 
            this.ClClino.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClClino.FillWeight = 50F;
            this.ClClino.HeaderText = "Угол";
            this.ClClino.Name = "ClClino";
            this.ClClino.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClLeft
            // 
            this.ClLeft.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClLeft.FillWeight = 50F;
            this.ClLeft.HeaderText = "Влево";
            this.ClLeft.Name = "ClLeft";
            this.ClLeft.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClRight
            // 
            this.ClRight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClRight.FillWeight = 50F;
            this.ClRight.HeaderText = "Вправо";
            this.ClRight.Name = "ClRight";
            this.ClRight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClUp
            // 
            this.ClUp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClUp.FillWeight = 50F;
            this.ClUp.HeaderText = "Вверх";
            this.ClUp.Name = "ClUp";
            this.ClUp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClDown
            // 
            this.ClDown.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClDown.FillWeight = 50F;
            this.ClDown.HeaderText = "Вниз";
            this.ClDown.Name = "ClDown";
            this.ClDown.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClNote
            // 
            this.ClNote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClNote.FillWeight = 135F;
            this.ClNote.HeaderText = "Примечание";
            this.ClNote.Name = "ClNote";
            this.ClNote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FrMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(824, 525);
            this.Controls.Add(this.dgTopo);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(617, 300);
            this.Name = "FrMain";
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
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog odLoad;
        private System.Windows.Forms.ToolStripMenuItem картаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sdSave;
        private System.Windows.Forms.ToolStripMenuItem съёмкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mData;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
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

