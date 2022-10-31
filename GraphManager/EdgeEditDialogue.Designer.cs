namespace GraphManager
{
    partial class EdgeEditDialogue
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
            this.tbxName = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.tbxWeight = new System.Windows.Forms.TextBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbxName
            // 
            this.tbxName.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.tbxName.Location = new System.Drawing.Point(105, 39);
            this.tbxName.MaxLength = 30;
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(432, 37);
            this.tbxName.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(511, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Edit Edge (Leave blank to preserve current)";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(12, 46);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(87, 30);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblWeight.Location = new System.Drawing.Point(12, 76);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(98, 30);
            this.lblWeight.TabIndex = 3;
            this.lblWeight.Text = "Weight:";
            // 
            // tbxWeight
            // 
            this.tbxWeight.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.tbxWeight.Location = new System.Drawing.Point(105, 76);
            this.tbxWeight.MaxLength = 30;
            this.tbxWeight.Name = "tbxWeight";
            this.tbxWeight.Size = new System.Drawing.Size(117, 37);
            this.tbxWeight.TabIndex = 4;
            // 
            // btnDone
            // 
            this.btnDone.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnDone.Location = new System.Drawing.Point(420, 123);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(117, 41);
            this.btnDone.TabIndex = 5;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.CloseDialogue);
            // 
            // EdgeEditDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 176);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.tbxWeight);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.tbxName);
            this.MaximumSize = new System.Drawing.Size(569, 215);
            this.MinimumSize = new System.Drawing.Size(569, 215);
            this.Name = "EdgeEditDialogue";
            this.Text = "Edit Edge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.TextBox tbxWeight;
        private System.Windows.Forms.Button btnDone;
    }
}