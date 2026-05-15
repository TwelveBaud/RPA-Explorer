using RPA_Parser;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RPA_Explorer
{
    public partial class MainWindow
    {
        private ToolStripLabel mnulLanguage;
        private ToolStripComboBox cbxLanguage;
        private ToolStripMenuItem mnuiOptions;
        private TextBox txtDescription;
        private Label lblFileList;
        private TreeView tvFileList;
        private Panel pnlPreview;
        internal ToolStripStatusLabel sblblStatus;
        internal ToolStripProgressBar stsprgProgress;
        private Button btnCreateNew;
        private Button btnLoad;
        private Button btnExport;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;
        private ToolStripMenuItem mnuiAbout;
        private BackgroundWorker ioOperation;
        private BackgroundWorker previewerLoadOperation;


        private void InitializeComponent()
        {
            System.Windows.Forms.TableLayoutPanel pnlActionLayout;
            System.Windows.Forms.TableLayoutPanel pnlMainLayout;
            System.Windows.Forms.MenuStrip mnuMenu;
            System.Windows.Forms.StatusStrip sbStatus;
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblFileList = new System.Windows.Forms.Label();
            this.tvFileList = new System.Windows.Forms.TreeView();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.mnulLanguage = new System.Windows.Forms.ToolStripLabel();
            this.cbxLanguage = new System.Windows.Forms.ToolStripComboBox();
            this.mnuiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.sblblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsprgProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.previewerLoadOperation = new System.ComponentModel.BackgroundWorker();
            this.ioOperation = new System.ComponentModel.BackgroundWorker();
            this.mnuiRegisterAssociation = new System.Windows.Forms.ToolStripMenuItem();
            pnlActionLayout = new System.Windows.Forms.TableLayoutPanel();
            pnlMainLayout = new System.Windows.Forms.TableLayoutPanel();
            mnuMenu = new System.Windows.Forms.MenuStrip();
            sbStatus = new System.Windows.Forms.StatusStrip();
            pnlActionLayout.SuspendLayout();
            pnlMainLayout.SuspendLayout();
            mnuMenu.SuspendLayout();
            sbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlActionLayout
            // 
            pnlActionLayout.ColumnCount = 2;
            pnlActionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            pnlActionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            pnlActionLayout.Controls.Add(this.btnCreateNew, 0, 0);
            pnlActionLayout.Controls.Add(this.btnLoad, 0, 1);
            pnlActionLayout.Controls.Add(this.btnExport, 0, 2);
            pnlActionLayout.Controls.Add(this.btnDelete, 0, 3);
            pnlActionLayout.Controls.Add(this.btnSave, 0, 4);
            pnlActionLayout.Controls.Add(this.btnCancel, 1, 2);
            pnlActionLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlActionLayout.Location = new System.Drawing.Point(0, 0);
            pnlActionLayout.Margin = new System.Windows.Forms.Padding(0);
            pnlActionLayout.Name = "pnlActionLayout";
            pnlActionLayout.RowCount = 5;
            pnlActionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            pnlActionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            pnlActionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            pnlActionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            pnlActionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            pnlActionLayout.Size = new System.Drawing.Size(350, 145);
            pnlActionLayout.TabIndex = 0;
            // 
            // btnCreateNew
            // 
            pnlActionLayout.SetColumnSpan(this.btnCreateNew, 2);
            this.btnCreateNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateNew.Location = new System.Drawing.Point(3, 3);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(344, 23);
            this.btnCreateNew.TabIndex = 0;
            this.btnCreateNew.Text = global::RPA_Explorer.Lang.Create_new_archive;
            this.btnCreateNew.UseVisualStyleBackColor = true;
            this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
            // 
            // btnLoad
            // 
            pnlActionLayout.SetColumnSpan(this.btnLoad, 2);
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.Location = new System.Drawing.Point(3, 32);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(344, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = global::RPA_Explorer.Lang.Load_file;
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExport.Location = new System.Drawing.Point(3, 61);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(169, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = global::RPA_Explorer.Lang.Export_checked;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnDelete
            // 
            pnlActionLayout.SetColumnSpan(this.btnDelete, 2);
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(3, 90);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(344, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = global::RPA_Explorer.Lang.Remove_checked;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            pnlActionLayout.SetColumnSpan(this.btnSave, 2);
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Location = new System.Drawing.Point(3, 119);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(344, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = global::RPA_Explorer.Lang.Save_archive;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(178, 61);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(169, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = global::RPA_Explorer.Lang.Cancel_operation;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlMainLayout
            // 
            pnlMainLayout.ColumnCount = 2;
            pnlMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            pnlMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlMainLayout.Controls.Add(pnlActionLayout, 0, 0);
            pnlMainLayout.Controls.Add(this.txtDescription, 0, 1);
            pnlMainLayout.Controls.Add(this.lblFileList, 0, 2);
            pnlMainLayout.Controls.Add(this.tvFileList, 0, 3);
            pnlMainLayout.Controls.Add(this.pnlPreview, 1, 0);
            pnlMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMainLayout.Location = new System.Drawing.Point(0, 27);
            pnlMainLayout.Name = "pnlMainLayout";
            pnlMainLayout.RowCount = 4;
            pnlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            pnlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            pnlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            pnlMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlMainLayout.Size = new System.Drawing.Size(1164, 673);
            pnlMainLayout.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(3, 148);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(344, 154);
            this.txtDescription.TabIndex = 1;
            // 
            // lblFileList
            // 
            this.lblFileList.AutoSize = true;
            this.lblFileList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFileList.Location = new System.Drawing.Point(3, 305);
            this.lblFileList.Name = "lblFileList";
            this.lblFileList.Size = new System.Drawing.Size(344, 13);
            this.lblFileList.TabIndex = 2;
            this.lblFileList.Text = "File list:";
            // 
            // tvFileList
            // 
            this.tvFileList.AllowDrop = true;
            this.tvFileList.CheckBoxes = true;
            this.tvFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvFileList.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvFileList.Location = new System.Drawing.Point(3, 321);
            this.tvFileList.Name = "tvFileList";
            this.tvFileList.PathSeparator = "/";
            this.tvFileList.Size = new System.Drawing.Size(344, 349);
            this.tvFileList.TabIndex = 3;
            this.tvFileList.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvFileList_AfterCheck);
            this.tvFileList.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvFileList_DrawNode);
            this.tvFileList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFileList_AfterSelect);
            this.tvFileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvFileList_DragDrop);
            this.tvFileList.DragOver += new System.Windows.Forms.DragEventHandler(this.tvFileList_DragOver);
            this.tvFileList.DragLeave += new System.EventHandler(this.tvFileList_DragLeave);
            // 
            // pnlPreview
            // 
            this.pnlPreview.AllowDrop = true;
            this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPreview.Location = new System.Drawing.Point(353, 3);
            this.pnlPreview.Name = "pnlPreview";
            pnlMainLayout.SetRowSpan(this.pnlPreview, 4);
            this.pnlPreview.Size = new System.Drawing.Size(808, 667);
            this.pnlPreview.TabIndex = 4;
            this.pnlPreview.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlPreview_DragDrop);
            this.pnlPreview.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlPreview_DragEnter);
            // 
            // mnuMenu
            // 
            mnuMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnulLanguage,
            this.cbxLanguage,
            this.mnuiOptions,
            this.mnuiAbout});
            mnuMenu.Location = new System.Drawing.Point(0, 0);
            mnuMenu.Name = "mnuMenu";
            mnuMenu.Size = new System.Drawing.Size(1164, 27);
            mnuMenu.TabIndex = 2;
            // 
            // mnulLanguage
            // 
            this.mnulLanguage.Name = "mnulLanguage";
            this.mnulLanguage.Size = new System.Drawing.Size(62, 20);
            this.mnulLanguage.Text = global::RPA_Explorer.Lang.Language;
            // 
            // cbxLanguage
            // 
            this.cbxLanguage.AutoToolTip = true;
            this.cbxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLanguage.Name = "cbxLanguage";
            this.cbxLanguage.Size = new System.Drawing.Size(200, 23);
            this.cbxLanguage.SelectedIndexChanged += new System.EventHandler(this.cbxLanguage_SelectedIndexChanged);
            // 
            // mnuiOptions
            // 
            this.mnuiOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiRegisterAssociation});
            this.mnuiOptions.Name = "mnuiOptions";
            this.mnuiOptions.Size = new System.Drawing.Size(61, 23);
            this.mnuiOptions.Text = global::RPA_Explorer.Lang.Options;
            // 
            // mnuiAbout
            // 
            this.mnuiAbout.Name = "mnuiAbout";
            this.mnuiAbout.Size = new System.Drawing.Size(52, 23);
            this.mnuiAbout.Text = global::RPA_Explorer.Lang.About;
            this.mnuiAbout.Click += new System.EventHandler(this.mnuiAbout_Click);
            // 
            // sbStatus
            // 
            sbStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sblblStatus,
            this.stsprgProgress});
            sbStatus.Location = new System.Drawing.Point(0, 700);
            sbStatus.Name = "sbStatus";
            sbStatus.Size = new System.Drawing.Size(1164, 22);
            sbStatus.TabIndex = 1;
            // 
            // sblblStatus
            // 
            this.sblblStatus.Name = "sblblStatus";
            this.sblblStatus.Size = new System.Drawing.Size(1047, 17);
            this.sblblStatus.Spring = true;
            this.sblblStatus.Text = global::RPA_Explorer.Lang.Ready;
            this.sblblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stsprgProgress
            // 
            this.stsprgProgress.Name = "stsprgProgress";
            this.stsprgProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // previewerLoadOperation
            // 
            this.previewerLoadOperation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.previewerLoadOperation_DoWork);
            // 
            // mnuiRegisterAssociation
            // 
            this.mnuiRegisterAssociation.Name = "mnuiRegisterAssociation";
            this.mnuiRegisterAssociation.Size = new System.Drawing.Size(194, 22);
            this.mnuiRegisterAssociation.Text = Lang.File_association;
            this.mnuiRegisterAssociation.Click += new System.EventHandler(this.mnuiRegisterAssociation_Click);
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(1164, 722);
            this.Controls.Add(pnlMainLayout);
            this.Controls.Add(sbStatus);
            this.Controls.Add(mnuMenu);
            this.Icon = global::RPA_Explorer.Resources.MainWindow_Icon;
            this.MainMenuStrip = mnuMenu;
            this.MinimumSize = new System.Drawing.Size(946, 581);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RenPy Archive Explorer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            pnlActionLayout.ResumeLayout(false);
            pnlMainLayout.ResumeLayout(false);
            pnlMainLayout.PerformLayout();
            mnuMenu.ResumeLayout(false);
            mnuMenu.PerformLayout();
            sbStatus.ResumeLayout(false);
            sbStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ToolStripMenuItem mnuiRegisterAssociation;
    }
}
