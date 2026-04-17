namespace RPA_Explorer
{
    partial class About
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
            System.Windows.Forms.TableLayoutPanel tlpTopMatter;
            System.Windows.Forms.PictureBox pbxLogo;
            System.Windows.Forms.Panel pnlTopMatterText;
            System.Windows.Forms.TableLayoutPanel tlpClose;
            System.Windows.Forms.TabControl tabExtendedCredits;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.lblInspiration = new System.Windows.Forms.LinkLabel();
            this.lblAuthors = new System.Windows.Forms.Label();
            this.lblGithub = new System.Windows.Forms.LinkLabel();
            this.lblProductVersion = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabTranslations = new System.Windows.Forms.TabPage();
            this.txtTranslations = new System.Windows.Forms.RichTextBox();
            this.tabContributors = new System.Windows.Forms.TabPage();
            this.txtContributors = new System.Windows.Forms.RichTextBox();
            this.tabDisclosures = new System.Windows.Forms.TabPage();
            this.txtDisclosures = new System.Windows.Forms.RichTextBox();
            tlpTopMatter = new System.Windows.Forms.TableLayoutPanel();
            pbxLogo = new System.Windows.Forms.PictureBox();
            pnlTopMatterText = new System.Windows.Forms.Panel();
            tlpClose = new System.Windows.Forms.TableLayoutPanel();
            tabExtendedCredits = new System.Windows.Forms.TabControl();
            tlpTopMatter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pbxLogo)).BeginInit();
            pnlTopMatterText.SuspendLayout();
            tlpClose.SuspendLayout();
            tabExtendedCredits.SuspendLayout();
            this.tabTranslations.SuspendLayout();
            this.tabContributors.SuspendLayout();
            this.tabDisclosures.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpTopMatter
            // 
            tlpTopMatter.AutoSize = true;
            tlpTopMatter.ColumnCount = 2;
            tlpTopMatter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            tlpTopMatter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpTopMatter.Controls.Add(pbxLogo, 0, 0);
            tlpTopMatter.Controls.Add(pnlTopMatterText, 1, 0);
            tlpTopMatter.Dock = System.Windows.Forms.DockStyle.Top;
            tlpTopMatter.Location = new System.Drawing.Point(0, 0);
            tlpTopMatter.Name = "tlpTopMatter";
            tlpTopMatter.RowCount = 1;
            tlpTopMatter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpTopMatter.Size = new System.Drawing.Size(418, 128);
            tlpTopMatter.TabIndex = 0;
            // 
            // pbxLogo
            // 
            pbxLogo.Image = global::RPA_Explorer.Resources.About_Logo;
            pbxLogo.Location = new System.Drawing.Point(0, 0);
            pbxLogo.Margin = new System.Windows.Forms.Padding(0);
            pbxLogo.Name = "pbxLogo";
            pbxLogo.Size = new System.Drawing.Size(128, 128);
            pbxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pbxLogo.TabIndex = 0;
            pbxLogo.TabStop = false;
            // 
            // pnlTopMatterText
            // 
            pnlTopMatterText.Controls.Add(this.lblInspiration);
            pnlTopMatterText.Controls.Add(this.lblAuthors);
            pnlTopMatterText.Controls.Add(this.lblGithub);
            pnlTopMatterText.Controls.Add(this.lblProductVersion);
            pnlTopMatterText.Controls.Add(this.lblProductName);
            pnlTopMatterText.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTopMatterText.Location = new System.Drawing.Point(128, 0);
            pnlTopMatterText.Margin = new System.Windows.Forms.Padding(0);
            pnlTopMatterText.Name = "pnlTopMatterText";
            pnlTopMatterText.Size = new System.Drawing.Size(290, 128);
            pnlTopMatterText.TabIndex = 1;
            // 
            // lblInspiration
            // 
            this.lblInspiration.AutoSize = true;
            this.lblInspiration.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInspiration.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblInspiration.Location = new System.Drawing.Point(0, 71);
            this.lblInspiration.Margin = new System.Windows.Forms.Padding(3);
            this.lblInspiration.Name = "lblInspiration";
            this.lblInspiration.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblInspiration.Size = new System.Drawing.Size(152, 17);
            this.lblInspiration.TabIndex = 4;
            this.lblInspiration.Text = "Inspired by rpatool and unrpyc.";
            this.lblInspiration.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblInspiration_LinkClicked);
            // 
            // lblAuthors
            // 
            this.lblAuthors.AutoSize = true;
            this.lblAuthors.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAuthors.Location = new System.Drawing.Point(0, 54);
            this.lblAuthors.Margin = new System.Windows.Forms.Padding(3);
            this.lblAuthors.Name = "lblAuthors";
            this.lblAuthors.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblAuthors.Size = new System.Drawing.Size(206, 17);
            this.lblAuthors.TabIndex = 3;
            this.lblAuthors.Text = "Created by Martin \"UniverseDevel\" Suchy";
            // 
            // lblGithub
            // 
            this.lblGithub.AutoSize = true;
            this.lblGithub.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGithub.Location = new System.Drawing.Point(0, 39);
            this.lblGithub.Name = "lblGithub";
            this.lblGithub.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblGithub.Size = new System.Drawing.Size(228, 15);
            this.lblGithub.TabIndex = 2;
            this.lblGithub.TabStop = true;
            this.lblGithub.Text = "https://github.com/TwelveBaud/RPA-Explorer";
            this.lblGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblGithub_LinkClicked);
            // 
            // lblProductVersion
            // 
            this.lblProductVersion.AutoSize = true;
            this.lblProductVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductVersion.Location = new System.Drawing.Point(0, 26);
            this.lblProductVersion.Name = "lblProductVersion";
            this.lblProductVersion.Size = new System.Drawing.Size(69, 13);
            this.lblProductVersion.TabIndex = 1;
            this.lblProductVersion.Text = "version {0}";
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductName.Location = new System.Drawing.Point(0, 0);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(244, 26);
            this.lblProductName.TabIndex = 0;
            this.lblProductName.Text = "RenPy Archive Explorer";
            // 
            // tlpClose
            // 
            tlpClose.ColumnCount = 3;
            tlpClose.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpClose.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            tlpClose.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpClose.Controls.Add(this.btnClose, 1, 0);
            tlpClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            tlpClose.Location = new System.Drawing.Point(0, 413);
            tlpClose.Name = "tlpClose";
            tlpClose.RowCount = 1;
            tlpClose.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpClose.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tlpClose.Size = new System.Drawing.Size(418, 30);
            tlpClose.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(169, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 24);
            this.btnClose.TabIndex = 0;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Text = Lang.About_close;
            // 
            // tabExtendedCredits
            // 
            tabExtendedCredits.Controls.Add(this.tabTranslations);
            tabExtendedCredits.Controls.Add(this.tabContributors);
            tabExtendedCredits.Controls.Add(this.tabDisclosures);
            tabExtendedCredits.Dock = System.Windows.Forms.DockStyle.Fill;
            tabExtendedCredits.Location = new System.Drawing.Point(0, 128);
            tabExtendedCredits.Name = "tabExtendedCredits";
            tabExtendedCredits.SelectedIndex = 0;
            tabExtendedCredits.Size = new System.Drawing.Size(418, 285);
            tabExtendedCredits.TabIndex = 2;
            // 
            // tabTranslations
            // 
            this.tabTranslations.Controls.Add(this.txtTranslations);
            this.tabTranslations.Location = new System.Drawing.Point(4, 22);
            this.tabTranslations.Name = "tabTranslations";
            this.tabTranslations.Padding = new System.Windows.Forms.Padding(3);
            this.tabTranslations.Size = new System.Drawing.Size(410, 259);
            this.tabTranslations.TabIndex = 0;
            this.tabTranslations.UseVisualStyleBackColor = true;
            this.tabTranslations.Text = Lang.About_translations;
            // 
            // txtTranslations
            // 
            this.txtTranslations.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTranslations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTranslations.Location = new System.Drawing.Point(3, 3);
            this.txtTranslations.Name = "txtTranslations";
            this.txtTranslations.ReadOnly = true;
            this.txtTranslations.Size = new System.Drawing.Size(404, 253);
            this.txtTranslations.TabIndex = 0;
            this.txtTranslations.Text = "";
            // 
            // tabContributors
            // 
            this.tabContributors.Controls.Add(this.txtContributors);
            this.tabContributors.Location = new System.Drawing.Point(4, 22);
            this.tabContributors.Name = "tabContributors";
            this.tabContributors.Padding = new System.Windows.Forms.Padding(3);
            this.tabContributors.Size = new System.Drawing.Size(410, 259);
            this.tabContributors.TabIndex = 1;
            this.tabContributors.UseVisualStyleBackColor = true;
            this.tabContributors.Text = Lang.About_contributors;
            // 
            // txtContributors
            // 
            this.txtContributors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContributors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContributors.Location = new System.Drawing.Point(3, 3);
            this.txtContributors.Name = "txtContributors";
            this.txtContributors.ReadOnly = true;
            this.txtContributors.Size = new System.Drawing.Size(404, 253);
            this.txtContributors.TabIndex = 1;
            this.txtContributors.Text = "";
            // 
            // tabDisclosures
            // 
            this.tabDisclosures.Controls.Add(this.txtDisclosures);
            this.tabDisclosures.Location = new System.Drawing.Point(4, 22);
            this.tabDisclosures.Name = "tabDisclosures";
            this.tabDisclosures.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisclosures.Size = new System.Drawing.Size(410, 259);
            this.tabDisclosures.TabIndex = 2;
            this.tabDisclosures.UseVisualStyleBackColor = true;
            this.tabDisclosures.Text = Lang.About_disclosures;
            // 
            // txtDisclosures
            // 
            this.txtDisclosures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisclosures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDisclosures.Location = new System.Drawing.Point(3, 3);
            this.txtDisclosures.Name = "txtDisclosures";
            this.txtDisclosures.ReadOnly = true;
            this.txtDisclosures.Size = new System.Drawing.Size(404, 253);
            this.txtDisclosures.TabIndex = 0;
            this.txtDisclosures.Text = "";
            // 
            // About
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(418, 443);
            this.ControlBox = false;
            this.Controls.Add(tabExtendedCredits);
            this.Controls.Add(tlpClose);
            this.Controls.Add(tlpTopMatter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            tlpTopMatter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(pbxLogo)).EndInit();
            pnlTopMatterText.ResumeLayout(false);
            pnlTopMatterText.PerformLayout();
            tlpClose.ResumeLayout(false);
            tabExtendedCredits.ResumeLayout(false);
            this.tabTranslations.ResumeLayout(false);
            this.tabContributors.ResumeLayout(false);
            this.tabDisclosures.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.LinkLabel lblInspiration;
        private System.Windows.Forms.Label lblAuthors;
        private System.Windows.Forms.LinkLabel lblGithub;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tabTranslations;
        private System.Windows.Forms.TabPage tabContributors;
        private System.Windows.Forms.TabPage tabDisclosures;
        private System.Windows.Forms.RichTextBox txtTranslations;
        private System.Windows.Forms.RichTextBox txtContributors;
        private System.Windows.Forms.RichTextBox txtDisclosures;
    }
}