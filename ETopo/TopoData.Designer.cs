using System.Collections.Generic;

namespace ETopo
{
    partial class FrTopoData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrTopoData));
            this.tbAutor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbAutor
            // 
            this.tbAutor.Location = new System.Drawing.Point(12, 149);
            this.tbAutor.Multiline = true;
            this.tbAutor.Name = "tbAutor";
            this.tbAutor.Size = new System.Drawing.Size(272, 91);
            this.tbAutor.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Авторы";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Название";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(12, 29);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(272, 22);
            this.tbName.TabIndex = 3;
            // 
            // cbDate
            // 
            this.cbDate.Location = new System.Drawing.Point(12, 85);
            this.cbDate.Name = "cbDate";
            this.cbDate.Size = new System.Drawing.Size(271, 22);
            this.cbDate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Дата";
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(12, 246);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(105, 25);
            this.btOk.TabIndex = 6;
            this.btOk.Text = "ОК";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // FrTopoData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 279);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbDate);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAutor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrTopoData";
            this.Text = "Данные съёмки";
            this.Load += new System.EventHandler(this.FrTopoData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAutor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.DateTimePicker cbDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btOk;
        public string name;
        public string date;
        public List<string> autor;
    }
}