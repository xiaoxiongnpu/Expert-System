﻿namespace _2lab
{
    partial class Test
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Test));
            this.restart_button = new System.Windows.Forms.Button();
            this.back_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // restart_button
            // 
            this.restart_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.restart_button.AutoSize = true;
            this.restart_button.BackColor = System.Drawing.SystemColors.Control;
            this.restart_button.FlatAppearance.BorderSize = 0;
            this.restart_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.restart_button.Location = new System.Drawing.Point(223, 416);
            this.restart_button.Name = "restart_button";
            this.restart_button.Size = new System.Drawing.Size(148, 34);
            this.restart_button.TabIndex = 0;
            this.restart_button.TabStop = false;
            this.restart_button.Text = "Начать с начала";
            this.restart_button.UseVisualStyleBackColor = false;
            this.restart_button.Click += new System.EventHandler(this.restart_button_Click);
            // 
            // back_button
            // 
            this.back_button.AutoSize = true;
            this.back_button.BackColor = System.Drawing.SystemColors.Control;
            this.back_button.FlatAppearance.BorderSize = 0;
            this.back_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.back_button.Location = new System.Drawing.Point(0, 419);
            this.back_button.Name = "back_button";
            this.back_button.Size = new System.Drawing.Size(75, 31);
            this.back_button.TabIndex = 1;
            this.back_button.Text = "<Назад";
            this.back_button.UseVisualStyleBackColor = false;
            this.back_button.Click += new System.EventHandler(this.back_button_Click);
            // 
            // Test
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(604, 449);
            this.Controls.Add(this.back_button);
            this.Controls.Add(this.restart_button);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Test";
            this.Text = "Тестирование";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button restart_button;
        private System.Windows.Forms.Button back_button;
    }
}