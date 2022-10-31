namespace GraphManager
{
    partial class AlgorithmInputDialogue
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
            this.btnDone = new System.Windows.Forms.Button();
            this.lblStart = new System.Windows.Forms.Label();
            this.tbxStart = new System.Windows.Forms.TextBox();
            this.lblEnd = new System.Windows.Forms.Label();
            this.tbxEnd = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDone
            // 
            this.btnDone.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnDone.Location = new System.Drawing.Point(420, 92);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(117, 41);
            this.btnDone.TabIndex = 13;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.CloseDialogue);
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblStart.Location = new System.Drawing.Point(12, 9);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(77, 30);
            this.lblStart.TabIndex = 12;
            this.lblStart.Text = "Start:";
            // 
            // tbxStart
            // 
            this.tbxStart.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.tbxStart.Location = new System.Drawing.Point(95, 6);
            this.tbxStart.MaxLength = 30;
            this.tbxStart.Name = "tbxStart";
            this.tbxStart.Size = new System.Drawing.Size(442, 37);
            this.tbxStart.TabIndex = 10;
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblEnd.Location = new System.Drawing.Point(12, 52);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(63, 30);
            this.lblEnd.TabIndex = 15;
            this.lblEnd.Text = "End:";
            // 
            // tbxEnd
            // 
            this.tbxEnd.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.tbxEnd.Location = new System.Drawing.Point(81, 49);
            this.tbxEnd.MaxLength = 30;
            this.tbxEnd.Name = "tbxEnd";
            this.tbxEnd.Size = new System.Drawing.Size(456, 37);
            this.tbxEnd.TabIndex = 14;
            // 
            // AlgorithmInputDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 145);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.tbxEnd);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.tbxStart);
            this.Name = "AlgorithmInputDialogue";
            this.Text = "AlgorithmInputDialogue";
            this.Load += new System.EventHandler(this.FormOpened);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.TextBox tbxStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.TextBox tbxEnd;
    }
}