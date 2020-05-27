﻿namespace pk3DS
{
    partial class TMHMEditor6
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
            this.dgvTM = new System.Windows.Forms.DataGridView();
            this.L_TM = new System.Windows.Forms.Label();
            this.dgvHM = new System.Windows.Forms.DataGridView();
            this.L_HM = new System.Windows.Forms.Label();
            this.B_RTM = new System.Windows.Forms.Button();
            this.CHK_RandomizeHM = new System.Windows.Forms.CheckBox();
            this.CHK_RandomizeField = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHM)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTM
            // 
            this.dgvTM.AllowUserToAddRows = false;
            this.dgvTM.AllowUserToDeleteRows = false;
            this.dgvTM.AllowUserToResizeColumns = false;
            this.dgvTM.AllowUserToResizeRows = false;
            this.dgvTM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvTM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTM.Location = new System.Drawing.Point(9, 25);
            this.dgvTM.Name = "dgvTM";
            this.dgvTM.Size = new System.Drawing.Size(240, 300);
            this.dgvTM.TabIndex = 1;
            // 
            // L_TM
            // 
            this.L_TM.AutoSize = true;
            this.L_TM.Location = new System.Drawing.Point(9, 9);
            this.L_TM.Name = "L_TM";
            this.L_TM.Size = new System.Drawing.Size(26, 13);
            this.L_TM.TabIndex = 2;
            this.L_TM.Text = "TM:";
            // 
            // dgvHM
            // 
            this.dgvHM.AllowUserToAddRows = false;
            this.dgvHM.AllowUserToDeleteRows = false;
            this.dgvHM.AllowUserToResizeColumns = false;
            this.dgvHM.AllowUserToResizeRows = false;
            this.dgvHM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvHM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHM.Location = new System.Drawing.Point(256, 25);
            this.dgvHM.Name = "dgvHM";
            this.dgvHM.Size = new System.Drawing.Size(240, 231);
            this.dgvHM.TabIndex = 3;
            // 
            // L_HM
            // 
            this.L_HM.AutoSize = true;
            this.L_HM.Location = new System.Drawing.Point(253, 9);
            this.L_HM.Name = "L_HM";
            this.L_HM.Size = new System.Drawing.Size(27, 13);
            this.L_HM.TabIndex = 4;
            this.L_HM.Text = "HM:";
            // 
            // B_RTM
            // 
            this.B_RTM.Location = new System.Drawing.Point(41, 1);
            this.B_RTM.Name = "B_RTM";
            this.B_RTM.Size = new System.Drawing.Size(75, 23);
            this.B_RTM.TabIndex = 5;
            this.B_RTM.Text = "Randomize";
            this.B_RTM.UseVisualStyleBackColor = true;
            this.B_RTM.Click += new System.EventHandler(this.B_RandomTM_Click);
            // 
            // CHK_RandomizeHM
            // 
            this.CHK_RandomizeHM.AutoSize = true;
            this.CHK_RandomizeHM.Location = new System.Drawing.Point(13, 19);
            this.CHK_RandomizeHM.Name = "CHK_RandomizeHM";
            this.CHK_RandomizeHM.Size = new System.Drawing.Size(104, 17);
            this.CHK_RandomizeHM.TabIndex = 280;
            this.CHK_RandomizeHM.Text = "Randomize HMs";
            this.CHK_RandomizeHM.UseVisualStyleBackColor = true;
            // 
            // CHK_RandomizeField
            // 
            this.CHK_RandomizeField.AutoSize = true;
            this.CHK_RandomizeField.Location = new System.Drawing.Point(13, 36);
            this.CHK_RandomizeField.Name = "CHK_RandomizeField";
            this.CHK_RandomizeField.Size = new System.Drawing.Size(139, 17);
            this.CHK_RandomizeField.TabIndex = 281;
            this.CHK_RandomizeField.Text = "Randomize Field Moves";
            this.CHK_RandomizeField.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CHK_RandomizeHM);
            this.groupBox1.Controls.Add(this.CHK_RandomizeField);
            this.groupBox1.Location = new System.Drawing.Point(256, 262);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 63);
            this.groupBox1.TabIndex = 282;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Randomization Settings";
            // 
            // TMHMEditor6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 332);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.B_RTM);
            this.Controls.Add(this.L_HM);
            this.Controls.Add(this.dgvHM);
            this.Controls.Add(this.L_TM);
            this.Controls.Add(this.dgvTM);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(520, 670);
            this.MinimumSize = new System.Drawing.Size(520, 370);
            this.Name = "TMHMEditor6";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TM / HM Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTM;
        private System.Windows.Forms.Label L_TM;
        private System.Windows.Forms.DataGridView dgvHM;
        private System.Windows.Forms.Label L_HM;
        private System.Windows.Forms.Button B_RTM;
        private System.Windows.Forms.CheckBox CHK_RandomizeHM;
        private System.Windows.Forms.CheckBox CHK_RandomizeField;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}