namespace GameofLife1
{
    partial class OptionsDialog
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericGridHeight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericGridWidth = new System.Windows.Forms.NumericUpDown();
            this.numericInterval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericGridHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGridWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(66, 131);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(153, 131);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // numericGridHeight
            // 
            this.numericGridHeight.Location = new System.Drawing.Point(175, 37);
            this.numericGridHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericGridHeight.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericGridHeight.Name = "numericGridHeight";
            this.numericGridHeight.Size = new System.Drawing.Size(53, 20);
            this.numericGridHeight.TabIndex = 2;
            this.numericGridHeight.ThousandsSeparator = true;
            this.numericGridHeight.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Height of Universe in Cells";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Width of Universe in Cells";
            // 
            // numericGridWidth
            // 
            this.numericGridWidth.Location = new System.Drawing.Point(175, 65);
            this.numericGridWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericGridWidth.Name = "numericGridWidth";
            this.numericGridWidth.Size = new System.Drawing.Size(53, 20);
            this.numericGridWidth.TabIndex = 5;
            this.numericGridWidth.ThousandsSeparator = true;
            this.numericGridWidth.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // numericInterval
            // 
            this.numericInterval.Location = new System.Drawing.Point(175, 92);
            this.numericInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericInterval.Name = "numericInterval";
            this.numericInterval.Size = new System.Drawing.Size(53, 20);
            this.numericInterval.TabIndex = 6;
            this.numericInterval.ThousandsSeparator = true;
            this.numericInterval.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Timer Interval in Miliseconds";
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(262, 177);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericInterval);
            this.Controls.Add(this.numericGridWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericGridHeight);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OptionsDialog";
            ((System.ComponentModel.ISupportInitialize)(this.numericGridHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGridWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numericGridHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericGridWidth;
        private System.Windows.Forms.NumericUpDown numericInterval;
        private System.Windows.Forms.Label label3;
    }
}