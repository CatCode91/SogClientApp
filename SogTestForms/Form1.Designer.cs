﻿
namespace SogTestForms
{
    partial class Form1
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
            this.txt_m = new System.Windows.Forms.TextBox();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_switch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_m
            // 
            this.txt_m.Location = new System.Drawing.Point(25, 12);
            this.txt_m.Multiline = true;
            this.txt_m.Name = "txt_m";
            this.txt_m.Size = new System.Drawing.Size(505, 330);
            this.txt_m.TabIndex = 0;
            // 
            // picBox
            // 
            this.picBox.Location = new System.Drawing.Point(536, 12);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(227, 330);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBox.TabIndex = 1;
            this.picBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(688, 348);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Restart";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btn_switch
            // 
            this.btn_switch.Location = new System.Drawing.Point(607, 348);
            this.btn_switch.Name = "btn_switch";
            this.btn_switch.Size = new System.Drawing.Size(75, 29);
            this.btn_switch.TabIndex = 3;
            this.btn_switch.Text = "Switch";
            this.btn_switch.UseVisualStyleBackColor = true;
            this.btn_switch.Click += new System.EventHandler(this.btn_switch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_switch);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.txt_m);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_m;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_switch;
    }
}

