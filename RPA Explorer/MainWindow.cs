#if !MINIMAL
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
#endif
using NeoSmart.PrettySize;
using RPA_Explorer.Previewers;
using RPA_Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RPA_Explorer
{
    public partial class MainWindow : Form
    {

        private RpaParser _rpaParser;
        private bool _dirty;

        public Panel PreviewPanel => pnlPreview;

        internal MainWindow()
        {
            InitializeComponent();

            var _ = this.Handle; // Force handle creation so we're owned by the main thread
            Program.StatusBar = new StatusBarBroker(this);

            InitializeLocalization();

            var ilFileTypes = new ImageList()
            {
                ImageSize = new System.Drawing.Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit,
            };
            ilFileTypes.Images.Add(Resources.tvFileList_Folder);
            ilFileTypes.Images.Add(Resources.tvFileList_File);
            tvFileList.ImageList = ilFileTypes;

            LoadTexts();
            SetButtonStates();

            foreach (var previewer in Program.Previewers)
            {
                previewer.RegisterOptionsMenu(mnuiOptions);
            }
        }

        private void previewerLoadOperation_DoWork(object sender, DoWorkEventArgs e)
        {
            using var ctx = Program.StatusBar.CreateContext();
            var appDir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            var externalPreviewers = appDir.GetFiles("*.previewer.dll");
            for (int i = 0; i < externalPreviewers.Length; i++)
            {
                ctx.UpdateStatus(Lang.Loading_additional_previewers, i, externalPreviewers.Length);
                var assembly = Assembly.Load(AssemblyName.GetAssemblyName(externalPreviewers[i].FullName));
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IPreviewer).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null)
                    {
                        var previewer = (IPreviewer)Activator.CreateInstance(type);
                        Program.Previewers.Add(previewer);
                        previewer.RegisterOptionsMenu(mnuiOptions);
                    }
                }
            }
        }

        private void InitializeLocalization()
        {
            var availableCultures = Program.Languages;
            var neutralCulture = CultureInfo.GetCultureInfo("en-US");

            // Only show the name of the language if there aren't country-specific variants available
            string SimpleName(CultureInfo culture)
            {
                if (availableCultures.Count(otherCulture => otherCulture.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName) == 1)
                {
                    while (culture.Name != culture.TwoLetterISOLanguageName) { culture = culture.Parent; }
                }
                return culture.NativeName;
            }
            var availableLanguages = availableCultures.Select(culture => new { Display = SimpleName(culture), Value = culture }).ToList();

            cbxLanguage.ComboBox.DisplayMember = "Display";
            cbxLanguage.ComboBox.ValueMember = "Value";
            cbxLanguage.ComboBox.DataSource = availableLanguages;
            cbxLanguage.ComboBox.SelectedIndex = availableCultures.IndexOf(Lang.Culture ?? neutralCulture);
        }

        private void LoadTexts()
        {
            Text = Lang.Explorer_title;
            mnulLanguage.Text = Lang.Language;
            btnLoad.Text = Lang.Load_file;
            btnExport.Text = Lang.Export_checked;
            sblblStatus.Text = Lang.Ready;
            btnCancel.Text = Lang.Cancel_operation;
            btnCreateNew.Text = Lang.Create_new_archive;
            lblFileList.Text = Lang.File_list;
            btnDelete.Text = Lang.Remove_checked;
            btnSave.Text = Lang.Save_archive;
            mnuiOptions.Text = Lang.Options;
            mnuiRegisterAssociation.Text = Lang.File_association;
            mnuiAbout.Text = Lang.About;

            foreach (var previewer in Program.Previewers)
                previewer.LoadTexts();

            GenerateArchiveInfo();
            PreviewSelectedItem();
        }

        private void GenerateArchiveInfo()
        {
            if (this._rpaParser == null)
            {
                txtDescription.Text = string.Empty;
                return;
            }
            var archiveInfo = new StringBuilder();

            if (!_rpaParser.CheckVersion(_rpaParser.ArchiveVersion, RpaParser.Version.Unknown))
            {
                archiveInfo.Append(Lang.Archive_version);
                archiveInfo.AppendLine(_rpaParser.ArchiveVersion.ToString());
                archiveInfo.Append(Lang.Archive_file_location);
                archiveInfo.AppendLine(_rpaParser.ArchiveInfo.FullName);
                archiveInfo.Append(Lang.Archive_file_size);
                archiveInfo.AppendLine(PrettySize.Format(_rpaParser.ArchiveInfo.Length));
                if (_rpaParser.IndexInfo != null)
                {
                    archiveInfo.Append(Lang.Index_file_location);
                    archiveInfo.AppendLine(_rpaParser.IndexInfo.FullName);
                    archiveInfo.Append(Lang.Index_file_size);
                    archiveInfo.AppendLine(PrettySize.Format(_rpaParser.IndexInfo.Length));
                }
            }

            archiveInfo.Append(Lang.Files_count);
            archiveInfo.AppendLine(_rpaParser.Index.Count.ToString());
            archiveInfo.Append(Lang.Unsaved_files_count);
            archiveInfo.AppendLine(_rpaParser.Index.Count(entry => !entry.Value.InArchive).ToString());

            var checkedNodes = tvFileList.Nodes[0].All().Where(n => n.Checked && n.Tag != null).ToList();
            if (checkedNodes.Count > 0)
            {
                archiveInfo.Append(Lang.Checked_files_count);
                archiveInfo.AppendLine(checkedNodes.Count.ToString());
                archiveInfo.Append(Lang.Checked_files_size);
                archiveInfo.AppendLine(PrettySize.Format(checkedNodes.Sum(node => (node.Tag as RpaParser.ArchiveIndex).Length)));
            }

            if (tvFileList.SelectedNode != null)
            {
                archiveInfo.Append(Lang.Selected_file_path);
                archiveInfo.AppendLine(NormalizeTreePath(tvFileList.SelectedNode.FullPath));
                archiveInfo.Append(Lang.Selected_file_size);
                archiveInfo.AppendLine(PrettySize.Format(CalculateSize(tvFileList.SelectedNode)));
            }

            txtDescription.Text = archiveInfo.ToString();
        }

        private static long CalculateSize(TreeNode node)
        {
            if (node.Tag is RpaParser.ArchiveIndex index)
                return index.Length;
            long size = 0;
            foreach (TreeNode child in node.Nodes)
                size += CalculateSize(child);
            return size;
        }

        private void CreateNewArchive()
        {
            if (CheckIfChanged(Lang.Archive_modified_new)) return;

            _rpaParser = new RpaParser();
            _dirty = false;
            GenerateTreeView();
            PreviewSelectedItem();
        }

        private void LoadArchive(string filename, bool ignoreChanges = false)
        {
            if (!ignoreChanges && CheckIfChanged(Lang.Archive_modified_load))
            {
                return;
            }

            var initialDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var lastOpenedFilename = Settings.GetSetting("LastOpenedFile");
            if (!string.IsNullOrWhiteSpace(lastOpenedFilename) && new FileInfo(lastOpenedFilename).Directory.Exists)
                initialDirectory = new FileInfo(lastOpenedFilename).DirectoryName;

            if (filename == null)
            {
                using OpenFileDialog dialog = new OpenFileDialog()
                {
                    Title = Lang.Load_RenPy_Archive,
                    Filter = Lang.RPA_RPI_files + " (*.rpa,*.rpi)|*.rpa;*.rpi",
                    InitialDirectory = initialDirectory,
                    Multiselect = false
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                    filename = dialog.FileName;
            }
            if (filename == null || !File.Exists(filename))
                return;

            if (ioOperation?.IsBusy == true)
            {
                throw new InvalidOperationException("An I/O operation is already in progress.");
            }
            ioOperation = new BackgroundWorker();
            ioOperation.DoWork += (sender, e) =>
            {
                using (var ctx = Program.StatusBar.CreateContext())
                {
                    ctx.UpdateStatus(Lang.Loading_file + filename, 0, 100);
                    RpaParser parser = new RpaParser();
                    parser.LoadArchive(filename);
                    e.Result = parser;
                }
            };
            ioOperation.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(string.Format(Lang.Load_failed_reason, e.Error.Message),
                                Lang.Load_failed, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetButtonStates();
                }
                else
                {
                    Settings.SetSetting("LastOpenedFile", filename, typeof(MainWindow));
                    Program.CurrentFile = filename;
                    this._rpaParser = e.Result as RpaParser;
                    _dirty = false;

                    GenerateTreeView();
                    GenerateArchiveInfo();
                    PreviewSelectedItem();
                    SetButtonStates();
                }
            };
            ioOperation.RunWorkerAsync();
            SetButtonStates();
        }

        private void SaveArchive()
        {
            if (_rpaParser.Index.Count == 0)
            {
                MessageBox.Show(Lang.Empty_archive_save, Lang.Empty_archive, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using var dialog = SetUpSaveDialog();
            if ((int?)dialog?.ShowDialog() != 1)
            {
                return;
            }
            var filename = dialog.FileName;
            if (ioOperation?.IsBusy == true)
            {
                throw new InvalidOperationException("An I/O operation is already in progress.");
            }
            ioOperation = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            ioOperation.DoWork += (sender, e) =>
            {
                using (var ctx = Program.StatusBar.CreateContext())
                {
                    void ProgressReportHandler(object sender, ProgressChangedEventArgs prgE)
                    {
                        if (e.Cancel) throw new OperationCanceledException();
                        ctx.UpdateStatus(Lang.Saving_archive + filename, prgE.ProgressPercentage, 100);
                    }
                    try
                    {
                        ctx.UpdateStatus(Lang.Saving_archive + filename, 0, 100);
                        _rpaParser.SaveProgress += ProgressReportHandler;
                        e.Result = _rpaParser.SaveArchive(filename);
                    }
                    finally
                    {
                        _rpaParser.SaveProgress -= ProgressReportHandler;
                    }
                }
            };
            ioOperation.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    if (e.Error is not OperationCanceledException)
                    {
                        MessageBox.Show(string.Format(Lang.Save_failed_reason, e.Error.Message), Lang.Load_failed, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    SetButtonStates();
                }
                else
                {
                    LoadArchive(e.Result as string, true);
                }
            };
            ioOperation.RunWorkerAsync();
            SetButtonStates();
        }

        private void GenerateTreeView()
        {
            tvFileList.BeginUpdate();
            tvFileList.Nodes.Clear();
            var root = new TreeNode()
            {
                Name = "",
                Text = "/",
                ImageIndex = 0,
            };
            foreach (var entry in _rpaParser.Index)
            {
                AddIndexEntry(root, entry);
            }
            tvFileList.Nodes.Add(root);
            root.Expand();
            tvFileList.EndUpdate();
        }

        private static TreeNode AddIndexEntry(TreeNode root, KeyValuePair<string, RpaParser.ArchiveIndex> entry)
        {
            TreeNode node = root;
            var pathElements = entry.Value.TreePath.Split('/');
            for (int i = 0; i < pathElements.Length; i++)
            {
                var child = node.Nodes[pathElements[i]];
                if (child == null)
                {
                    child = new TreeNode()
                    {
                        Name = pathElements[i],
                        Text = pathElements[i],
                        ImageIndex = (i < pathElements.Length - 1) ? 0 : 1
                    };
                    child.SelectedImageIndex = child.ImageIndex;
                    var insertionPoint = node.Nodes.Cast<TreeNode>().FirstOrDefault(ch => ch.ImageIndex == 1);
                    if (child.ImageIndex == 0 && insertionPoint != null)
                    {
                        node.Nodes.Insert(node.Nodes.IndexOf(insertionPoint), child);
                    }
                    else
                    {
                        node.Nodes.Add(child);
                    }
                }
                if (child.ImageIndex == 1)
                    child.Tag = entry.Value;
                node = child;
            }
            return node;
        }

        private bool CheckIfChanged(string message)
        {
            if (!_dirty) return false;
            return MessageBox.Show(message, Lang.Archive_modified, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
        }

        private void AddFilesToArchive(string[] filenames, TreeNode leaf)
        {
            if (filenames.Length == 0) return;
            var fsoInfos = filenames.Select(name => (FileSystemInfo)(((File.GetAttributes(name) & FileAttributes.Directory) == FileAttributes.Directory) ? new DirectoryInfo(name) : new FileInfo(name))).ToList();
            if (leaf.ImageIndex == 1)
            {
                if (fsoInfos.Count == 1 && fsoInfos[0] is FileInfo fi)
                {
                    // Replacing one file with another file
                    AddPathToIndex(fi.FullName, NormalizeTreePath(leaf.FullPath));
                    SetButtonStates();
                    return;
                }
                else
                {
                    // Replacing one file with nothing, multiple files, or one or more directories
                    throw new InvalidOperationException(string.Format("Can't replace a single file with {0} file(s) and {1} folder(s)", fsoInfos.OfType<FileInfo>().Count(), fsoInfos.OfType<DirectoryInfo>().Count()));
                }
            }
            DirectoryInfo deepestCommonAncestor = null;
            if (fsoInfos[0] is DirectoryInfo di) deepestCommonAncestor = di.Parent;
            else if (fsoInfos[0] is FileInfo fi) deepestCommonAncestor = fi.Directory;
            foreach (var fileInfo in fsoInfos)
            {
                while (!fileInfo.FullName.StartsWith(deepestCommonAncestor.FullName))
                {
                    deepestCommonAncestor = deepestCommonAncestor.Parent;
                    if (deepestCommonAncestor == null) throw new DirectoryNotFoundException("Couldn't find a common ancestor.");
                }
            }
            var fileInfos = fsoInfos.SelectMany(fsi => fsi is DirectoryInfo di ? di.EnumerateFiles("*", SearchOption.AllDirectories) : new[] { fsi as FileInfo });
            foreach (var fileInfo in fileInfos)
            {
                AddPathToIndex(fileInfo.FullName, NormalizeTreePath(fileInfo.FullName.Replace(deepestCommonAncestor.FullName, NormalizeTreePath(leaf.FullPath)).Replace("\\", "/")));
            }
            SetButtonStates();
        }

        private void AddPathToIndex(string diskpath, string treepath)
        {
            _dirty = true;
            var entry = new RpaParser.ArchiveIndex()
            {
                FullPath = diskpath,
                TreePath = treepath,
                ParentPath = Path.GetDirectoryName(treepath),
                Length = new FileInfo(diskpath).Length,
                InArchive = false
            };
            _rpaParser.Index[entry.TreePath] = entry;
            AddIndexEntry(tvFileList.Nodes[0], new KeyValuePair<string, RpaParser.ArchiveIndex>(entry.TreePath, entry));
            tvFileList.Invalidate();
        }

        private void ExportFiles()
        {
            var folderbrowser = new FolderBrowserDialog();
            if (folderbrowser.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var files = tvFileList.Nodes[0].All().Where(node => node.Checked && node.Tag is RpaParser.ArchiveIndex).Select(node => node.Tag as RpaParser.ArchiveIndex).ToList();
            if (ioOperation?.IsBusy == true)
            {
                throw new InvalidOperationException("An I/O operation is already in progress.");
            }
            ioOperation = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            ioOperation.DoWork += (sender, e) =>
            {
                using var ctx = Program.StatusBar.CreateContext();
                for (int i = 0; i < files.Count; i++)
                {
                    if (e.Cancel) return;
                    ctx.UpdateStatus(Lang.Exporting_file + files[i].TreePath, i, files.Count);
                    _rpaParser.Extract(NormalizeTreePath(files[i].TreePath), folderbrowser.SelectedPath);
                }
            };
            ioOperation.RunWorkerCompleted += (sender, e) =>
            {
                SetButtonStates();
            };
            ioOperation.RunWorkerAsync();
            SetButtonStates();
        }

        // `public` to allow previewer stubs to try again
        public void PreviewSelectedItem()
        {
            var oldControl = pnlPreview.Controls.Cast<Control>().FirstOrDefault();
            pnlPreview.Controls.Clear();
            if (oldControl is IDisposable trash)
            {
                trash.Dispose();
            }

            if (_rpaParser == null)
            {
                pnlPreview.Controls.Add(BackstopLabel(Lang.Usage_instructions_new));
                return;
            }

            if (tvFileList.SelectedNode?.Tag == null)
            {
                pnlPreview.Controls.Add(BackstopLabel(Lang.Usage_instructions_loaded));
                return;
            }

            Exception lastError = null;
            var magic = new byte[16];
            var source = _rpaParser.ExtractData(NormalizeTreePath(tvFileList.SelectedNode.FullPath));
            source.Read(magic, 0, 16);
            source.Seek(0, SeekOrigin.Begin);
            foreach (var previewer in Program.Previewers)
            {
                if (previewer.CanPreview(NormalizeTreePath(tvFileList.SelectedNode.FullPath).ToLowerInvariant(), magic))
                {
                    try
                    {
                        var control = previewer.Preview(NormalizeTreePath(tvFileList.SelectedNode.FullPath).ToLowerInvariant(), source);
                        if (control == null)
                        {
                            source.Dispose();
                            _rpaParser.ExtractData(NormalizeTreePath(tvFileList.SelectedNode.FullPath));
                            continue;
                        }
                        pnlPreview.Controls.Add(control);
                        control.Dock = DockStyle.Fill;
                        return;
                    }
                    catch (Exception ex)
                    {
                        source.Dispose();
                        _rpaParser.ExtractData(NormalizeTreePath(tvFileList.SelectedNode.FullPath));
                        lastError = ex;
                        continue;
                    }
                }
            }
            source.Dispose();
            if (lastError != null)
            {
                pnlPreview.Controls.Add(BackstopLabel(string.Format(Lang.Preview_failed_reason, $"{lastError.GetType().FullName}: {lastError.Message}")));
                return;
            }

            pnlPreview.Controls.Add(BackstopLabel(Lang.Preview_is_not_supported));
        }

        private Label BackstopLabel(string message)
        {
            return new Label()
            {
                Font = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size * 2),
                Text = message,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };
        }

        private string NormalizeTreePath(string path)
        {
            return Regex.Replace(path, "^/+", "");
        }

        private void SetButtonStates()
        {
            // Can always create a new file unless a load/save/export is in progress
            btnCreateNew.Enabled = !(ioOperation?.IsBusy ?? false);

            // Can always load a file unless a load/save/export is in progress
            btnLoad.Enabled = !(ioOperation?.IsBusy ?? false);

            // Can export if any files checked and a load/save/export is not in progress
            btnExport.Enabled = !(ioOperation?.IsBusy ?? false) && tvFileList.Nodes.Count > 0 && tvFileList.Nodes[0].All().Count(node => node.Checked) > 0;

            // Can cancel a save/export if it's in progress
            btnCancel.Enabled = (ioOperation?.IsBusy ?? false) && ioOperation.WorkerSupportsCancellation;

            // Can delete if any files checked and a load/save/export is not in progress
            btnDelete.Enabled = !(ioOperation?.IsBusy ?? false) && tvFileList.Nodes.Count > 0 && tvFileList.Nodes[0].All().Count(node => node.Checked) > 0;

            // Can save if dirty and a load/save/export is not in progress
            btnSave.Enabled = !(ioOperation?.IsBusy ?? false) && _dirty;

            // Tree view shouldn't be manipulated while a load/save/export is in progress
            tvFileList.Enabled = !(ioOperation?.IsBusy ?? false);
        }

#if MINIMAL
        private SaveFileDialog SetUpSaveDialog()
        {
            var initialDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var lastOpenedFilename = Settings.GetSetting("LastOpenedFile");
            if (!string.IsNullOrWhiteSpace(lastOpenedFilename) && new FileInfo(lastOpenedFilename).Directory.Exists)
                initialDirectory = new FileInfo(lastOpenedFilename).DirectoryName;

            var btnOptionsAccept = new Button
            {
                Text = Lang.Archive_save_continue,
                Dock = DockStyle.Fill,
                Margin = new Padding(3)
            };
            var btnOptionsCancel = new Button
            {
                Text = Lang.Archive_save_cancel,
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Margin = new Padding(3)
            };
            var cbxVersion = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                Margin = new Padding(3)
            };
            cbxVersion.Items.Add(RpaParser.Version.RPA_3_2);
            cbxVersion.Items.Add(RpaParser.Version.RPA_3);
            cbxVersion.Items.Add(RpaParser.Version.RPA_2);
            cbxVersion.Items.Add(RpaParser.Version.RPA_1);
            if (cbxVersion.Items.Contains(_rpaParser.ArchiveVersion)) cbxVersion.SelectedItem = _rpaParser.ArchiveVersion;
            else cbxVersion.SelectedItem = RpaParser.Version.RPA_3;
            var txtPadding = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3)
            };
            var txtObfuscationKey = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3)
            };
            void VersionChange(object sender, EventArgs e)
            {
                txtPadding.Enabled = cbxVersion.SelectedIndex < 3;
                txtPadding.Text = txtPadding.Enabled ? "0x" + _rpaParser.Padding.ToString("X8") : "0x00000000";
                txtObfuscationKey.Enabled = cbxVersion.SelectedIndex < 2;
                txtObfuscationKey.Text = txtObfuscationKey.Enabled ? "0x" + _rpaParser.ObfuscationKey.ToString("X8") : "0x00000000";
            }
            VersionChange(cbxVersion, EventArgs.Empty);
            cbxVersion.SelectedIndexChanged += VersionChange;
            var pnlOptionsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(12)
            };
            pnlOptionsLayout.RowStyles.Clear();
            for (int i = 0; i < 4; i++) pnlOptionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            pnlOptionsLayout.ColumnStyles.Clear();
            for (int i = 0; i < 2; i++) pnlOptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pnlOptionsLayout.Controls.Add(new Label
            {
                Text = Lang.Archive_save_version,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
            }, 0, 0);
            pnlOptionsLayout.Controls.Add(cbxVersion, 1, 0);
            pnlOptionsLayout.Controls.Add(new Label
            {
                Text = Lang.Archive_save_padding,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
            }, 0, 1);
            pnlOptionsLayout.Controls.Add(txtPadding, 1, 1);
            pnlOptionsLayout.Controls.Add(new Label
            {
                Text = Lang.Archive_save_obfuscationkey,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
            }, 0, 2);
            pnlOptionsLayout.Controls.Add(txtObfuscationKey, 1, 2);
            pnlOptionsLayout.Controls.Add(btnOptionsAccept, 0, 3);
            pnlOptionsLayout.Controls.Add(btnOptionsCancel, 1, 3);
            using var optionsDialog = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ControlBox = false,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.CenterParent,
                Text = Lang.Archive_save_title,
                Width = 360,
                Height = 146,
                AcceptButton = btnOptionsAccept,
                CancelButton = btnOptionsCancel,
            };
            optionsDialog.Controls.Add(pnlOptionsLayout);
            btnOptionsAccept.Click += (s, e) =>
            {
                try
                {
                    _rpaParser.ArchiveVersion = _rpaParser.CheckSupportedVersion((double)cbxVersion.SelectedItem);
                    if (txtPadding.Text.StartsWith("0x"))
                        _rpaParser.Padding = int.Parse(txtPadding.Text.Substring(2), NumberStyles.HexNumber);
                    else
                        _rpaParser.Padding = int.Parse(txtPadding.Text, NumberStyles.Integer);
                    if (txtObfuscationKey.Text.StartsWith("0x"))
                        _rpaParser.ObfuscationKey = uint.Parse(txtObfuscationKey.Text.Substring(2), NumberStyles.HexNumber);
                    else
                        _rpaParser.ObfuscationKey = uint.Parse(txtObfuscationKey.Text, NumberStyles.Integer);
                    _rpaParser.OptionsConfirmed = true; // No longer needed, but for completeness...
                    optionsDialog.DialogResult = DialogResult.OK;
                    optionsDialog.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.GetType().Name}: {ex.Message}", Lang.Invalid_values, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            if (optionsDialog.ShowDialog() != DialogResult.OK) return null;
            return new SaveFileDialog
            {
                Title = Lang.Save_RenPy_Archive,
                Filter = Lang.RPA_RPI_files + " (*.rpa,*.rpi)|*.rpa;*.rpi",
                InitialDirectory = initialDirectory,
                CheckFileExists = false,
                CheckPathExists = true,
            };
        }
#else
        private CommonSaveFileDialog SetUpSaveDialog()
        {
            var initialDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var lastOpenedFilename = Settings.GetSetting("LastOpenedFile");
            if (!string.IsNullOrWhiteSpace(lastOpenedFilename) && new FileInfo(lastOpenedFilename).Directory.Exists)
                initialDirectory = new FileInfo(lastOpenedFilename).DirectoryName;

            CommonSaveFileDialog dialog = new CommonSaveFileDialog()
            {
                Title = Lang.Save_RenPy_Archive,
                InitialDirectory = initialDirectory,
                OverwritePrompt = true,
            };
            dialog.Filters.Clear();
            dialog.Filters.Add(new CommonFileDialogFilter(Lang.RPA_RPI_files, "*.rpa;*.rpi"));
            dialog.AlwaysAppendDefaultExtension = true;
            var grpVersion = new CommonFileDialogGroupBox(Lang.Archive_save_version);
            var cbxVersion = new CommonFileDialogComboBox("cbxVersion");
            cbxVersion.Items.Add(new CommonFileDialogComboBoxItem(RpaParser.Version.RPA_3_2.ToString()));
            cbxVersion.Items.Add(new CommonFileDialogComboBoxItem(RpaParser.Version.RPA_3.ToString()));
            cbxVersion.Items.Add(new CommonFileDialogComboBoxItem(RpaParser.Version.RPA_2.ToString()));
            cbxVersion.Items.Add(new CommonFileDialogComboBoxItem(RpaParser.Version.RPA_1.ToString()));
            switch (_rpaParser.ArchiveVersion)
            {
                case RpaParser.Version.RPA_3_2:
                    cbxVersion.SelectedIndex = 0;
                    break;
                case RpaParser.Version.RPA_3:
                    cbxVersion.SelectedIndex = 1;
                    break;
                case RpaParser.Version.RPA_2:
                    cbxVersion.SelectedIndex = 2;
                    break;
                case RpaParser.Version.RPA_1:
                    cbxVersion.SelectedIndex = 3;
                    break;
                default:
                    cbxVersion.SelectedIndex = 1;
                    break;
            }
            grpVersion.Items.Add(cbxVersion);
            var grpPadding = new CommonFileDialogGroupBox(Lang.Archive_save_padding);
            var txtPadding = new CommonFileDialogTextBox("txtPadding", "");
            grpPadding.Items.Add(txtPadding);
            var grpObfuscationKey = new CommonFileDialogGroupBox(Lang.Archive_save_obfuscationkey);
            var txtObfuscationKey = new CommonFileDialogTextBox("txtObfuscationKey", "");
            grpObfuscationKey.Items.Add(txtObfuscationKey);
            void VersionChange(object sender, EventArgs e)
            {
                txtPadding.Enabled = cbxVersion.SelectedIndex < 3;
                txtPadding.Text = txtPadding.Enabled ? "0x" + _rpaParser.Padding.ToString("X8") : "0x00000000";
                txtObfuscationKey.Enabled = cbxVersion.SelectedIndex < 2;
                txtObfuscationKey.Text = txtObfuscationKey.Enabled ? "0x" + _rpaParser.ObfuscationKey.ToString("X8") : "0x00000000";
            }
            VersionChange(cbxVersion, EventArgs.Empty);
            cbxVersion.SelectedIndexChanged += VersionChange;
            dialog.FileOk += (sender, e) =>
            {
                try
                {
                    double version;
                    switch (cbxVersion.SelectedIndex)
                    {
                        case 0:
                            version = RpaParser.Version.RPA_3_2;
                            break;
                        case 1:
                            version = RpaParser.Version.RPA_3;
                            break;
                        case 2:
                            version = RpaParser.Version.RPA_2;
                            break;
                        case 3:
                            version = RpaParser.Version.RPA_1;
                            break;
                        default:
                            version = RpaParser.Version.RPA_3_2;
                            break;
                    }
                    _rpaParser.ArchiveVersion = _rpaParser.CheckSupportedVersion(version);
                    if (txtPadding.Text.StartsWith("0x"))
                        _rpaParser.Padding = int.Parse(txtPadding.Text.Substring(2), NumberStyles.HexNumber);
                    else
                        _rpaParser.Padding = int.Parse(txtPadding.Text, NumberStyles.Integer);
                    if (txtObfuscationKey.Text.StartsWith("0x"))
                        _rpaParser.ObfuscationKey = uint.Parse(txtObfuscationKey.Text.Substring(2), NumberStyles.HexNumber);
                    else
                        _rpaParser.ObfuscationKey = uint.Parse(txtObfuscationKey.Text, NumberStyles.Integer);
                    _rpaParser.OptionsConfirmed = true; // No longer needed, but for completeness...
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    MessageBox.Show($"{ex.GetType().Name}: {ex.Message}", Lang.Invalid_values, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            dialog.Controls.Add(grpVersion);
            dialog.Controls.Add(grpPadding);
            dialog.Controls.Add(grpObfuscationKey);
            return dialog;
        }
#endif
    }

    public static class Extensions
    {
        public static IList<TreeNode> All(this TreeNode node)
        {
            var list = new List<TreeNode>();
            AllImpl(list, node);
            return list;
        }

        private static void AllImpl(List<TreeNode> list, TreeNode node)
        {
            list.Add(node);
            foreach (TreeNode child in node.Nodes)
            {
                AllImpl(list, child);
            }
        }
    }
}
