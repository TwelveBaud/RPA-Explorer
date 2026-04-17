using RPA_Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace RPA_Explorer
{
    internal partial class MainWindow
    {
        private void MainWindow_Load(object sender, EventArgs e)
        {
            previewerLoadOperation.RunWorkerAsync();

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                LoadArchive(args[1]);
            }
        }

        private void mnuiRegisterAssociation_Click(object sender, EventArgs e)
        {
            var filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            FileAssociations.EnsureAssociationsSet(
                new FileAssociations.FileAssociation
                {
                    Extension = ".rpi",
                    ProgId = "RPA Explorer",
                    FileTypeDescription = "RenPy Index File",
                    ExecutableFilePath = filePath
                },
                new FileAssociations.FileAssociation
                {
                    Extension = ".rpa",
                    ProgId = "RPA Explorer",
                    FileTypeDescription = "RenPy Archive File",
                    ExecutableFilePath = filePath
                });
        }

        private void mnuiAbout_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void cbxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var culture = cbxLanguage.ComboBox.SelectedValue as CultureInfo;
            if (culture == null) return;
            Lang.Culture = culture;
            Settings.SetSetting("Language", culture.Name);
            LoadTexts();
        }

        private void pnlPreview_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (filenames.Length != 1)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                if (filenames[0].EndsWith(".rpa") || filenames[0].EndsWith(".rpi"))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }

                if (tvFileList.SelectedNode != null)
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void pnlPreview_DragDrop(object sender, DragEventArgs e)
        {
            pnlPreview_DragEnter(sender, e);
            if (e.Effect == DragDropEffects.Copy)
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (filenames[0].EndsWith(".rpa") || filenames[0].EndsWith(".rpi"))
                {
                    LoadArchive(filenames[0]);
                }
                else
                {
                    AddFilesToArchive(filenames, tvFileList.SelectedNode);
                }
            }
        }

        private void tvFileList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GenerateArchiveInfo();
            PreviewSelectedItem();
        }

        private void tvFileList_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode child in e.Node.Nodes)
            {
                child.Checked = e.Node.Checked;
            }
            GenerateArchiveInfo();
            SetButtonStates();
        }

        TreeNode lastTarget = null;
        Stopwatch hoveredOverTreeNode = new Stopwatch();
        private void tvFileList_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            var filenames = ((string[])e.Data.GetData(DataFormats.FileDrop)).Where(name => !name.EndsWith(".rpa") && !name.EndsWith(".rpi"));
            if (!filenames.Any())
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var point = new Point(e.X, e.Y);
            point = tvFileList.PointToClient(point);
            var target = tvFileList.GetNodeAt(point);
            if (target != null && target.ImageIndex == 0)
            {
                if (lastTarget != target)
                {
                    tvFileList_DragLeave(sender, e);
                    lastTarget = target;
                    target.BackColor = SystemColors.Highlight;
                    target.ForeColor = SystemColors.HighlightText;
                    hoveredOverTreeNode.Start();
                }
                if (!target.IsExpanded && hoveredOverTreeNode.ElapsedMilliseconds > 1500) target.Expand();
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
                tvFileList_DragLeave(sender, e);
            }
        }

        private void tvFileList_DragDrop(object sender, DragEventArgs e)
        {
            tvFileList_DragLeave(sender, e);
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            var filenames = ((string[])e.Data.GetData(DataFormats.FileDrop)).Where(name => !name.EndsWith(".rpa") && !name.EndsWith(".rpi"));
            if (!filenames.Any())
            {
                return;
            }
            var point = new Point(e.X, e.Y);
            point = tvFileList.PointToClient(point);
            var target = tvFileList.GetNodeAt(point);
            if (target != null && target.ImageIndex == 0)
            {
                AddFilesToArchive(filenames.ToArray(), target);
            }
        }

        private void tvFileList_DragLeave(object sender, EventArgs e)
        {
            if (lastTarget != null)
            {
                hoveredOverTreeNode.Reset();
                lastTarget.BackColor = Color.Empty;
                lastTarget.ForeColor = SystemColors.WindowText;
            }
        }

        private void tvFileList_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // We want .NET to draw most of the node since we're just adding a "new" indicator to the side
            e.DrawDefault = true;
            if (e.Node.All().Any(node => !((node.Tag as RpaParser.ArchiveIndex)?.InArchive ?? true)))
            {
                var originalTransform = e.Graphics.Transform;
                e.Graphics.TranslateTransform(e.Bounds.Right + 1, e.Bounds.Top + e.Bounds.Height / 3);
                for (int i = 0; i < 3; i++)
                {
                    e.Graphics.DrawLine(Pens.DarkGoldenrod, 0, -e.Bounds.Height / 6, 0, -e.Bounds.Height / 3);
                    e.Graphics.RotateTransform(45);
                }
                e.Graphics.Transform = originalTransform;
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.TaskManagerClosing)
                return;
            e.Cancel = CheckIfChanged(Lang.Archive_modified_close);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveArchive();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool dirtying = false;
            var expandList = new List<string>();
            foreach (var node in tvFileList.Nodes[0].All().ToList())
            {
                if (node.IsExpanded) expandList.Add(NormalizeTreePath(node.FullPath));
                if (node.Checked && node.Tag != null)
                {
                    _rpaParser.Index.Remove((node.Tag as RpaParser.ArchiveIndex).TreePath);
                    dirtying = true;
                }
            }
            if (dirtying)
            {
                _dirty = true;
                GenerateTreeView();
                foreach (var path in expandList)
                {
                    try
                    {
                        tvFileList.Nodes[0].Nodes[path].Expand();
                    }
                    catch { }
                }
                GenerateArchiveInfo();
                SetButtonStates();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportFiles();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ioOperation.CancelAsync();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadArchive(null);
        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            CreateNewArchive();
        }
    }
}
