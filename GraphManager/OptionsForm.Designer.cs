namespace GraphManager
{
    partial class OptionsForm
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
            this.optionsBox = new System.Windows.Forms.CheckedListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.closePanel = new System.Windows.Forms.Panel();
            this.lblLargeText = new System.Windows.Forms.Label();
            this.darkThemeIndicator = new System.Windows.Forms.Panel();
            this.closePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsBox
            // 
            this.optionsBox.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.optionsBox.FormattingEnabled = true;
            this.optionsBox.Items.AddRange(new object[] {
            "Large text?",
            "Dark theme?"});
            this.optionsBox.Location = new System.Drawing.Point(41, 12);
            this.optionsBox.Name = "optionsBox";
            this.optionsBox.Size = new System.Drawing.Size(259, 228);
            this.optionsBox.TabIndex = 0;
            this.optionsBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.itemChecked);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(139, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(173, 46);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.closeOptions);
            // 
            // closePanel
            // 
            this.closePanel.Controls.Add(this.btnClose);
            this.closePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.closePanel.Location = new System.Drawing.Point(0, 294);
            this.closePanel.Name = "closePanel";
            this.closePanel.Size = new System.Drawing.Size(312, 46);
            this.closePanel.TabIndex = 2;
            // 
            // lblLargeText
            // 
            this.lblLargeText.AutoSize = true;
            this.lblLargeText.BackColor = System.Drawing.SystemColors.Control;
            this.lblLargeText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLargeText.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblLargeText.Location = new System.Drawing.Point(-2, 12);
            this.lblLargeText.Name = "lblLargeText";
            this.lblLargeText.Size = new System.Drawing.Size(43, 30);
            this.lblLargeText.TabIndex = 3;
            this.lblLargeText.Text = "Aa";
            // 
            // darkThemeIndicator
            // 
            this.darkThemeIndicator.BackColor = System.Drawing.SystemColors.ControlText;
            this.darkThemeIndicator.Location = new System.Drawing.Point(3, 45);
            this.darkThemeIndicator.Name = "darkThemeIndicator";
            this.darkThemeIndicator.Size = new System.Drawing.Size(32, 32);
            this.darkThemeIndicator.TabIndex = 4;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 340);
            this.Controls.Add(this.darkThemeIndicator);
            this.Controls.Add(this.lblLargeText);
            this.Controls.Add(this.closePanel);
            this.Controls.Add(this.optionsBox);
            this.MaximumSize = new System.Drawing.Size(328, 379);
            this.Name = "OptionsForm";
            this.Text = "OptionsForm";
            this.closePanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox optionsBox;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel closePanel;
        private System.Windows.Forms.Label lblLargeText;
        private System.Windows.Forms.Panel darkThemeIndicator;
    }
}