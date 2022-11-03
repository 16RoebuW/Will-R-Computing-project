namespace GraphManager
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tstTop = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnOptions = new System.Windows.Forms.ToolStripButton();
            this.tstMiddle = new System.Windows.Forms.ToolStrip();
            this.btnLoad = new System.Windows.Forms.ToolStripButton();
            this.cbxAlgorithmSelect = new System.Windows.Forms.ToolStripComboBox();
            this.btnAlgRun = new System.Windows.Forms.ToolStripButton();
            this.rdbCreate = new System.Windows.Forms.RadioButton();
            this.rdbEdit = new System.Windows.Forms.RadioButton();
            this.rdbDelete = new System.Windows.Forms.RadioButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.icoCreate = new System.Windows.Forms.PictureBox();
            this.zoomPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.icoZoomOut = new System.Windows.Forms.PictureBox();
            this.trbZoom = new System.Windows.Forms.TrackBar();
            this.icoZoomIn = new System.Windows.Forms.PictureBox();
            this.icoEdit = new System.Windows.Forms.PictureBox();
            this.icoDelete = new System.Windows.Forms.PictureBox();
            this.icoG = new System.Windows.Forms.PictureBox();
            this.autosaveTimer = new System.Windows.Forms.Timer(this.components);
            this.tstTop.SuspendLayout();
            this.tstMiddle.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoCreate)).BeginInit();
            this.zoomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoZoomOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoZoomIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoG)).BeginInit();
            this.SuspendLayout();
            // 
            // tstTop
            // 
            this.tstTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tstTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnOptions});
            this.tstTop.Location = new System.Drawing.Point(0, 0);
            this.tstTop.Name = "tstTop";
            this.tstTop.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tstTop.Size = new System.Drawing.Size(1584, 37);
            this.tstTop.TabIndex = 0;
            this.tstTop.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(164, 34);
            this.btnSave.Text = "Save Graph";
            this.btnSave.Click += new System.EventHandler(this.SaveClicked);
            // 
            // btnOptions
            // 
            this.btnOptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnOptions.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOptions.Image")));
            this.btnOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(122, 34);
            this.btnOptions.Text = "Options";
            this.btnOptions.Click += new System.EventHandler(this.OpenOptions);
            // 
            // tstMiddle
            // 
            this.tstMiddle.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tstMiddle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoad,
            this.cbxAlgorithmSelect,
            this.btnAlgRun});
            this.tstMiddle.Location = new System.Drawing.Point(0, 37);
            this.tstMiddle.Name = "tstMiddle";
            this.tstMiddle.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tstMiddle.Size = new System.Drawing.Size(1584, 42);
            this.tstMiddle.TabIndex = 1;
            this.tstMiddle.Text = "toolStrip2";
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(169, 39);
            this.btnLoad.Text = "Open Graph";
            this.btnLoad.Click += new System.EventHandler(this.LoadClicked);
            // 
            // cbxAlgorithmSelect
            // 
            this.cbxAlgorithmSelect.BackColor = System.Drawing.SystemColors.Window;
            this.cbxAlgorithmSelect.DropDownWidth = 800;
            this.cbxAlgorithmSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxAlgorithmSelect.Items.AddRange(new object[] {
            "Shortest path (Accurate)",
            "Shortest path (Fast)",
            "Compute all min distances",
            "Find shortest tour of all nodes",
            "Find minimum network containing all nodes (MST)"});
            this.cbxAlgorithmSelect.Margin = new System.Windows.Forms.Padding(250, 0, 1, 0);
            this.cbxAlgorithmSelect.Name = "cbxAlgorithmSelect";
            this.cbxAlgorithmSelect.Size = new System.Drawing.Size(300, 42);
            this.cbxAlgorithmSelect.SelectedIndexChanged += new System.EventHandler(this.AlgorithmChosen);
            // 
            // btnAlgRun
            // 
            this.btnAlgRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAlgRun.Enabled = false;
            this.btnAlgRun.Image = ((System.Drawing.Image)(resources.GetObject("btnAlgRun.Image")));
            this.btnAlgRun.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAlgRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAlgRun.Name = "btnAlgRun";
            this.btnAlgRun.Size = new System.Drawing.Size(39, 39);
            this.btnAlgRun.Text = "toolStripButton3";
            this.btnAlgRun.ToolTipText = "Select an algorithm from the dropdown";
            // 
            // rdbCreate
            // 
            this.rdbCreate.AutoSize = true;
            this.rdbCreate.Checked = true;
            this.rdbCreate.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.rdbCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rdbCreate.Location = new System.Drawing.Point(232, 0);
            this.rdbCreate.Name = "rdbCreate";
            this.rdbCreate.Size = new System.Drawing.Size(176, 34);
            this.rdbCreate.TabIndex = 2;
            this.rdbCreate.TabStop = true;
            this.rdbCreate.Text = "Create Mode";
            this.rdbCreate.UseVisualStyleBackColor = true;
            // 
            // rdbEdit
            // 
            this.rdbEdit.AutoSize = true;
            this.rdbEdit.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.rdbEdit.Location = new System.Drawing.Point(232, 37);
            this.rdbEdit.Name = "rdbEdit";
            this.rdbEdit.Size = new System.Drawing.Size(145, 34);
            this.rdbEdit.TabIndex = 3;
            this.rdbEdit.Text = "Edit Mode";
            this.rdbEdit.UseVisualStyleBackColor = true;
            // 
            // rdbDelete
            // 
            this.rdbDelete.AutoSize = true;
            this.rdbDelete.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.rdbDelete.Location = new System.Drawing.Point(232, 76);
            this.rdbDelete.Name = "rdbDelete";
            this.rdbDelete.Size = new System.Drawing.Size(174, 34);
            this.rdbDelete.TabIndex = 4;
            this.rdbDelete.Text = "Delete Mode";
            this.rdbDelete.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip.Location = new System.Drawing.Point(0, 809);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1584, 20);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "Information and errors will show up here";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(221, 15);
            this.statusLabel.Text = "Information and errors will show up here";
            // 
            // icoCreate
            // 
            this.icoCreate.Image = ((System.Drawing.Image)(resources.GetObject("icoCreate.Image")));
            this.icoCreate.Location = new System.Drawing.Point(191, 2);
            this.icoCreate.Name = "icoCreate";
            this.icoCreate.Size = new System.Drawing.Size(35, 35);
            this.icoCreate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icoCreate.TabIndex = 9;
            this.icoCreate.TabStop = false;
            // 
            // zoomPanel
            // 
            this.zoomPanel.Controls.Add(this.icoZoomOut);
            this.zoomPanel.Controls.Add(this.trbZoom);
            this.zoomPanel.Controls.Add(this.icoZoomIn);
            this.zoomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.zoomPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.zoomPanel.Location = new System.Drawing.Point(0, 773);
            this.zoomPanel.Name = "zoomPanel";
            this.zoomPanel.Size = new System.Drawing.Size(1584, 36);
            this.zoomPanel.TabIndex = 10;
            // 
            // icoZoomOut
            // 
            this.icoZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("icoZoomOut.Image")));
            this.icoZoomOut.Location = new System.Drawing.Point(1541, 3);
            this.icoZoomOut.Name = "icoZoomOut";
            this.icoZoomOut.Size = new System.Drawing.Size(40, 31);
            this.icoZoomOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icoZoomOut.TabIndex = 11;
            this.icoZoomOut.TabStop = false;
            // 
            // trbZoom
            // 
            this.trbZoom.LargeChange = 100;
            this.trbZoom.Location = new System.Drawing.Point(1431, 3);
            this.trbZoom.Maximum = 200;
            this.trbZoom.Minimum = 25;
            this.trbZoom.Name = "trbZoom";
            this.trbZoom.Size = new System.Drawing.Size(104, 45);
            this.trbZoom.SmallChange = 25;
            this.trbZoom.TabIndex = 8;
            this.trbZoom.TickFrequency = 25;
            this.trbZoom.Value = 100;
            this.trbZoom.Scroll += new System.EventHandler(this.ZoomLvlChanged);
            // 
            // icoZoomIn
            // 
            this.icoZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("icoZoomIn.Image")));
            this.icoZoomIn.Location = new System.Drawing.Point(1385, 3);
            this.icoZoomIn.Name = "icoZoomIn";
            this.icoZoomIn.Size = new System.Drawing.Size(40, 31);
            this.icoZoomIn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icoZoomIn.TabIndex = 12;
            this.icoZoomIn.TabStop = false;
            // 
            // icoEdit
            // 
            this.icoEdit.Image = ((System.Drawing.Image)(resources.GetObject("icoEdit.Image")));
            this.icoEdit.Location = new System.Drawing.Point(191, 37);
            this.icoEdit.Name = "icoEdit";
            this.icoEdit.Size = new System.Drawing.Size(35, 35);
            this.icoEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icoEdit.TabIndex = 11;
            this.icoEdit.TabStop = false;
            // 
            // icoDelete
            // 
            this.icoDelete.Image = ((System.Drawing.Image)(resources.GetObject("icoDelete.Image")));
            this.icoDelete.Location = new System.Drawing.Point(191, 75);
            this.icoDelete.Name = "icoDelete";
            this.icoDelete.Size = new System.Drawing.Size(35, 35);
            this.icoDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icoDelete.TabIndex = 12;
            this.icoDelete.TabStop = false;
            // 
            // icoG
            // 
            this.icoG.Image = ((System.Drawing.Image)(resources.GetObject("icoG.Image")));
            this.icoG.Location = new System.Drawing.Point(0, 732);
            this.icoG.Name = "icoG";
            this.icoG.Size = new System.Drawing.Size(70, 35);
            this.icoG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icoG.TabIndex = 13;
            this.icoG.TabStop = false;
            // 
            // autosaveTimer
            // 
            this.autosaveTimer.Interval = 300000;
            this.autosaveTimer.Tick += new System.EventHandler(this.TimeTick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 829);
            this.Controls.Add(this.icoDelete);
            this.Controls.Add(this.icoEdit);
            this.Controls.Add(this.zoomPanel);
            this.Controls.Add(this.icoG);
            this.Controls.Add(this.icoCreate);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.rdbDelete);
            this.Controls.Add(this.rdbEdit);
            this.Controls.Add(this.rdbCreate);
            this.Controls.Add(this.tstMiddle);
            this.Controls.Add(this.tstTop);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(3840, 1888);
            this.MinimumSize = new System.Drawing.Size(640, 432);
            this.Name = "MainForm";
            this.Text = "GraphVis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgramClosing);
            this.Load += new System.EventHandler(this.ProgramLoaded);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Main_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BackClicked);
            this.tstTop.ResumeLayout(false);
            this.tstTop.PerformLayout();
            this.tstMiddle.ResumeLayout(false);
            this.tstMiddle.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoCreate)).EndInit();
            this.zoomPanel.ResumeLayout(false);
            this.zoomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoZoomOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoZoomIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icoG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tstTop;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStrip tstMiddle;
        private System.Windows.Forms.ToolStripButton btnLoad;
        private System.Windows.Forms.RadioButton rdbCreate;
        private System.Windows.Forms.RadioButton rdbEdit;
        private System.Windows.Forms.RadioButton rdbDelete;
        private System.Windows.Forms.ToolStripButton btnOptions;
        private System.Windows.Forms.ToolStripComboBox cbxAlgorithmSelect;
        private System.Windows.Forms.ToolStripButton btnAlgRun;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.PictureBox icoCreate;
        private System.Windows.Forms.FlowLayoutPanel zoomPanel;
        private System.Windows.Forms.PictureBox icoZoomOut;
        private System.Windows.Forms.TrackBar trbZoom;
        private System.Windows.Forms.PictureBox icoZoomIn;
        private System.Windows.Forms.PictureBox icoEdit;
        private System.Windows.Forms.PictureBox icoDelete;
        private System.Windows.Forms.PictureBox icoG;
        private System.Windows.Forms.Timer autosaveTimer;
    }
}

