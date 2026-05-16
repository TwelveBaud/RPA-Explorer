namespace RPA_Explorer.Previewers
{
    partial class ImagePreviewControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripSeparator sep1;
            System.Windows.Forms.ToolStripSeparator sep3;
            System.Windows.Forms.ToolStripSeparator sep2;
            this.sbtnZoom = new System.Windows.Forms.ToolStripSplitButton();
            this.mnuiZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiZoomFit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnWhite = new System.Windows.Forms.ToolStripButton();
            this.btnLight = new System.Windows.Forms.ToolStripButton();
            this.btnGray = new System.Windows.Forms.ToolStripButton();
            this.btnDark = new System.Windows.Forms.ToolStripButton();
            this.btnBlack = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblImageInfo = new System.Windows.Forms.ToolStripLabel();
            this.pbxBitmap = new System.Windows.Forms.PictureBox();
            sep1 = new System.Windows.Forms.ToolStripSeparator();
            sep3 = new System.Windows.Forms.ToolStripSeparator();
            sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBitmap)).BeginInit();
            this.SuspendLayout();
            // 
            // sep1
            // 
            sep1.Name = "sep1";
            sep1.Size = new System.Drawing.Size(6, 25);
            // 
            // sep3
            // 
            sep3.Name = "sep3";
            sep3.Size = new System.Drawing.Size(99, 6);
            // 
            // sep2
            // 
            sep2.Name = "sep2";
            sep2.Size = new System.Drawing.Size(6, 25);
            // 
            // sbtnZoom
            // 
            this.sbtnZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.sbtnZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiZoom50,
            this.mnuiZoom100,
            this.mnuiZoom200,
            sep3,
            this.mnuiZoomFit});
            this.sbtnZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sbtnZoom.Name = "sbtnZoom";
            this.sbtnZoom.Size = new System.Drawing.Size(55, 22);
            this.sbtnZoom.Text = "Zoom";
            // 
            // mnuiZoom50
            // 
            this.mnuiZoom50.Name = "mnuiZoom50";
            this.mnuiZoom50.Size = new System.Drawing.Size(102, 22);
            this.mnuiZoom50.Text = "50%";
            this.mnuiZoom50.Click += new System.EventHandler(this.mnuiZoom_Click);
            // 
            // mnuiZoom100
            // 
            this.mnuiZoom100.Name = "mnuiZoom100";
            this.mnuiZoom100.Size = new System.Drawing.Size(102, 22);
            this.mnuiZoom100.Text = "100%";
            this.mnuiZoom100.Click += new System.EventHandler(this.mnuiZoom_Click);
            // 
            // mnuiZoom200
            // 
            this.mnuiZoom200.Name = "mnuiZoom200";
            this.mnuiZoom200.Size = new System.Drawing.Size(102, 22);
            this.mnuiZoom200.Text = "200%";
            this.mnuiZoom200.Click += new System.EventHandler(this.mnuiZoom_Click);
            // 
            // mnuiZoomFit
            // 
            this.mnuiZoomFit.Name = "mnuiZoomFit";
            this.mnuiZoomFit.Size = new System.Drawing.Size(102, 22);
            this.mnuiZoomFit.Text = "Fit";
            this.mnuiZoomFit.Click += new System.EventHandler(this.mnuiZoom_Click);
            // 
            // btnWhite
            // 
            this.btnWhite.AutoToolTip = false;
            this.btnWhite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnWhite.Image = global::RPA_Explorer.Resources.BgWhite;
            this.btnWhite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnWhite.Name = "btnWhite";
            this.btnWhite.Size = new System.Drawing.Size(23, 22);
            this.btnWhite.Text = "toolStripButton1";
            this.btnWhite.Click += new System.EventHandler(this.btnBackground_Click);
            // 
            // btnLight
            // 
            this.btnLight.AutoToolTip = false;
            this.btnLight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLight.Image = global::RPA_Explorer.Resources.BgLight;
            this.btnLight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLight.Name = "btnLight";
            this.btnLight.Size = new System.Drawing.Size(23, 22);
            this.btnLight.Text = "toolStripButton2";
            this.btnLight.Click += new System.EventHandler(this.btnBackground_Click);
            // 
            // btnGray
            // 
            this.btnGray.AutoToolTip = false;
            this.btnGray.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGray.Image = global::RPA_Explorer.Resources.BgGray;
            this.btnGray.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGray.Name = "btnGray";
            this.btnGray.Size = new System.Drawing.Size(23, 22);
            this.btnGray.Text = "toolStripButton5";
            this.btnGray.Click += new System.EventHandler(this.btnBackground_Click);
            // 
            // btnDark
            // 
            this.btnDark.AutoToolTip = false;
            this.btnDark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDark.Image = global::RPA_Explorer.Resources.BgDark;
            this.btnDark.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDark.Name = "btnDark";
            this.btnDark.Size = new System.Drawing.Size(23, 22);
            this.btnDark.Text = "toolStripButton3";
            this.btnDark.Click += new System.EventHandler(this.btnBackground_Click);
            // 
            // btnBlack
            // 
            this.btnBlack.AutoToolTip = false;
            this.btnBlack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBlack.Image = global::RPA_Explorer.Resources.BgBlack;
            this.btnBlack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBlack.Name = "btnBlack";
            this.btnBlack.Size = new System.Drawing.Size(23, 22);
            this.btnBlack.Text = "toolStripButton4";
            this.btnBlack.Click += new System.EventHandler(this.btnBackground_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnWhite,
            this.btnLight,
            this.btnGray,
            this.btnDark,
            this.btnBlack,
            sep1,
            this.sbtnZoom,
            sep2,
            this.lblImageInfo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 185);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(472, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblImageInfo
            // 
            this.lblImageInfo.Name = "lblImageInfo";
            this.lblImageInfo.Size = new System.Drawing.Size(135, 22);
            this.lblImageInfo.Text = "PPM - 128 × 128 - 32-bit";
            // 
            // pbxBitmap
            // 
            this.pbxBitmap.Location = new System.Drawing.Point(0, 0);
            this.pbxBitmap.Name = "pbxBitmap";
            this.pbxBitmap.Size = new System.Drawing.Size(38, 38);
            this.pbxBitmap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxBitmap.TabIndex = 1;
            this.pbxBitmap.TabStop = false;
            // 
            // ImagePreviewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pbxBitmap);
            this.Name = "ImagePreviewerControl";
            this.Size = new System.Drawing.Size(472, 210);
            this.Resize += new System.EventHandler(this.ImagePreviewerControl_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBitmap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblImageInfo;
        private System.Windows.Forms.PictureBox pbxBitmap;
        private System.Windows.Forms.ToolStripMenuItem mnuiZoom50;
        private System.Windows.Forms.ToolStripMenuItem mnuiZoom100;
        private System.Windows.Forms.ToolStripMenuItem mnuiZoom200;
        private System.Windows.Forms.ToolStripMenuItem mnuiZoomFit;
        private System.Windows.Forms.ToolStripButton btnWhite;
        private System.Windows.Forms.ToolStripButton btnLight;
        private System.Windows.Forms.ToolStripButton btnGray;
        private System.Windows.Forms.ToolStripButton btnDark;
        private System.Windows.Forms.ToolStripButton btnBlack;
        private System.Windows.Forms.ToolStripSplitButton sbtnZoom;
    }
}
