using Newtonsoft.Json.Linq;
using RPA_Explorer;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Code.previewer
{
    internal class RpycConfiguration : UserControl
    {
        private FlowLayoutPanel pnlLayout;
        private Label lblTitle;
        private LinkLabel lblDescription;
        private Label lblPyVersion;
        private RadioButton rdoPyBrowse;
        private Button btnPyBrowse;
        private RadioButton rdoPyDownload;
        private Label lblUnrpycVersion;
        private Label lblUnrpycPath;
        private Button btnUnrpycBrowse;
        private Button btnUnrpycDownload;
        private Label lblReminder;
        private Button btnExit;
        private FlowLayoutPanel pnlUnRpycButtons;
        private FlowLayoutPanel pnlPythons;
        private Label lblPyPath;

        public RpycConfiguration()
        {
            InitializeComponent();
            LoadTexts();
        }

        private void InitializeComponent()
        {
            this.pnlLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.LinkLabel();
            this.lblPyVersion = new System.Windows.Forms.Label();
            this.lblPyPath = new System.Windows.Forms.Label();
            this.pnlPythons = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoPyBrowse = new System.Windows.Forms.RadioButton();
            this.btnPyBrowse = new System.Windows.Forms.Button();
            this.rdoPyDownload = new System.Windows.Forms.RadioButton();
            this.lblUnrpycVersion = new System.Windows.Forms.Label();
            this.lblUnrpycPath = new System.Windows.Forms.Label();
            this.pnlUnRpycButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUnrpycBrowse = new System.Windows.Forms.Button();
            this.btnUnrpycDownload = new System.Windows.Forms.Button();
            this.lblReminder = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlLayout.SuspendLayout();
            this.pnlPythons.SuspendLayout();
            this.pnlUnRpycButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayout
            // 
            this.pnlLayout.Controls.Add(this.lblTitle);
            this.pnlLayout.Controls.Add(this.lblDescription);
            this.pnlLayout.Controls.Add(this.lblPyVersion);
            this.pnlLayout.Controls.Add(this.lblPyPath);
            this.pnlLayout.Controls.Add(this.pnlPythons);
            this.pnlLayout.Controls.Add(this.lblUnrpycVersion);
            this.pnlLayout.Controls.Add(this.lblUnrpycPath);
            this.pnlLayout.Controls.Add(this.pnlUnRpycButtons);
            this.pnlLayout.Controls.Add(this.lblReminder);
            this.pnlLayout.Controls.Add(this.btnExit);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.Size = new System.Drawing.Size(450, 405);
            this.pnlLayout.TabIndex = 0;
            this.pnlLayout.WrapContents = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(216, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Unrpyc Configuration";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(3, 26);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(434, 26);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.TabStop = true;
            this.lblDescription.Text = "To preview compiled Ren\'Py scripts, you need a copy of Python and a copy of the U" +
    "nrpyc decompiler by CensoredUsername";
            this.lblDescription.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDescription_LinkClicked);
            // 
            // lblPyVersion
            // 
            this.lblPyVersion.AutoSize = true;
            this.lblPyVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPyVersion.Location = new System.Drawing.Point(3, 72);
            this.lblPyVersion.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblPyVersion.Name = "lblPyVersion";
            this.lblPyVersion.Size = new System.Drawing.Size(71, 13);
            this.lblPyVersion.TabIndex = 2;
            this.lblPyVersion.Text = "Python: {0}";
            // 
            // lblPyPath
            // 
            this.lblPyPath.AutoSize = true;
            this.lblPyPath.Location = new System.Drawing.Point(3, 85);
            this.lblPyPath.Name = "lblPyPath";
            this.lblPyPath.Size = new System.Drawing.Size(125, 13);
            this.lblPyPath.TabIndex = 3;
            this.lblPyPath.Text = "C:\\Python27\\Python.exe";
            // 
            // pnlPythons
            // 
            this.pnlPythons.AutoSize = true;
            this.pnlPythons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPythons.Controls.Add(this.rdoPyBrowse);
            this.pnlPythons.Controls.Add(this.btnPyBrowse);
            this.pnlPythons.Controls.Add(this.rdoPyDownload);
            this.pnlPythons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlPythons.Location = new System.Drawing.Point(0, 98);
            this.pnlPythons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPythons.Name = "pnlPythons";
            this.pnlPythons.Size = new System.Drawing.Size(163, 75);
            this.pnlPythons.TabIndex = 16;
            // 
            // rdoPyBrowse
            // 
            this.rdoPyBrowse.AutoSize = true;
            this.rdoPyBrowse.Location = new System.Drawing.Point(3, 3);
            this.rdoPyBrowse.Name = "rdoPyBrowse";
            this.rdoPyBrowse.Size = new System.Drawing.Size(157, 17);
            this.rdoPyBrowse.TabIndex = 5;
            this.rdoPyBrowse.TabStop = true;
            this.rdoPyBrowse.Text = global::Code.previewer.Lang.Configuration_Python_BrowseLabel;
            this.rdoPyBrowse.UseVisualStyleBackColor = true;
            // 
            // btnPyBrowse
            // 
            this.btnPyBrowse.AutoSize = true;
            this.btnPyBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPyBrowse.Location = new System.Drawing.Point(20, 26);
            this.btnPyBrowse.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.btnPyBrowse.Name = "btnPyBrowse";
            this.btnPyBrowse.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnPyBrowse.Size = new System.Drawing.Size(73, 23);
            this.btnPyBrowse.TabIndex = 7;
            this.btnPyBrowse.Text = global::Code.previewer.Lang.Configuration_Python_BrowseButton;
            this.btnPyBrowse.UseVisualStyleBackColor = true;
            this.btnPyBrowse.Click += new System.EventHandler(this.btnPyBrowse_Click);
            // 
            // rdoPyDownload
            // 
            this.rdoPyDownload.AutoSize = true;
            this.rdoPyDownload.Location = new System.Drawing.Point(3, 55);
            this.rdoPyDownload.Name = "rdoPyDownload";
            this.rdoPyDownload.Size = new System.Drawing.Size(156, 17);
            this.rdoPyDownload.TabIndex = 6;
            this.rdoPyDownload.TabStop = true;
            this.rdoPyDownload.Text = global::Code.previewer.Lang.Configuration_Python_Download;
            this.rdoPyDownload.UseVisualStyleBackColor = true;
            // 
            // lblUnrpycVersion
            // 
            this.lblUnrpycVersion.AutoSize = true;
            this.lblUnrpycVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnrpycVersion.Location = new System.Drawing.Point(3, 193);
            this.lblUnrpycVersion.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblUnrpycVersion.Name = "lblUnrpycVersion";
            this.lblUnrpycVersion.Size = new System.Drawing.Size(72, 13);
            this.lblUnrpycVersion.TabIndex = 8;
            this.lblUnrpycVersion.Text = "Unrpyc: {0}";
            // 
            // lblUnrpycPath
            // 
            this.lblUnrpycPath.AutoSize = true;
            this.lblUnrpycPath.Location = new System.Drawing.Point(3, 206);
            this.lblUnrpycPath.Name = "lblUnrpycPath";
            this.lblUnrpycPath.Size = new System.Drawing.Size(208, 13);
            this.lblUnrpycPath.TabIndex = 9;
            this.lblUnrpycPath.Text = "C:\\Python27\\Lib\\site-packages\\unrpyc.py";
            // 
            // pnlUnRpycButtons
            // 
            this.pnlUnRpycButtons.AutoSize = true;
            this.pnlUnRpycButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlUnRpycButtons.Controls.Add(this.btnUnrpycBrowse);
            this.pnlUnRpycButtons.Controls.Add(this.btnUnrpycDownload);
            this.pnlUnRpycButtons.Location = new System.Drawing.Point(0, 219);
            this.pnlUnRpycButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlUnRpycButtons.Name = "pnlUnRpycButtons";
            this.pnlUnRpycButtons.Size = new System.Drawing.Size(162, 29);
            this.pnlUnRpycButtons.TabIndex = 15;
            // 
            // btnUnrpycBrowse
            // 
            this.btnUnrpycBrowse.AutoSize = true;
            this.btnUnrpycBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUnrpycBrowse.Location = new System.Drawing.Point(3, 3);
            this.btnUnrpycBrowse.Name = "btnUnrpycBrowse";
            this.btnUnrpycBrowse.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnUnrpycBrowse.Size = new System.Drawing.Size(73, 23);
            this.btnUnrpycBrowse.TabIndex = 11;
            this.btnUnrpycBrowse.Text = global::Code.previewer.Lang.Configuration_Unrpyc_BrowseButton;
            this.btnUnrpycBrowse.UseVisualStyleBackColor = true;
            this.btnUnrpycBrowse.Click += new System.EventHandler(this.btnUnrpycBrowse_Click);
            // 
            // btnUnrpycDownload
            // 
            this.btnUnrpycDownload.AutoSize = true;
            this.btnUnrpycDownload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUnrpycDownload.Location = new System.Drawing.Point(82, 3);
            this.btnUnrpycDownload.Name = "btnUnrpycDownload";
            this.btnUnrpycDownload.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnUnrpycDownload.Size = new System.Drawing.Size(77, 23);
            this.btnUnrpycDownload.TabIndex = 12;
            this.btnUnrpycDownload.Text = global::Code.previewer.Lang.Configuration_Unrpyc_Download;
            this.btnUnrpycDownload.UseVisualStyleBackColor = true;
            this.btnUnrpycDownload.Click += new System.EventHandler(this.btnUnrpycDownload_Click);
            // 
            // lblReminder
            // 
            this.lblReminder.AutoSize = true;
            this.lblReminder.Location = new System.Drawing.Point(3, 268);
            this.lblReminder.Margin = new System.Windows.Forms.Padding(3, 20, 3, 0);
            this.lblReminder.Name = "lblReminder";
            this.lblReminder.Size = new System.Drawing.Size(443, 26);
            this.lblReminder.TabIndex = 14;
            this.lblReminder.Text = "Be aware that earlier games require Python 2 and Unrpyc 1, while later games requ" +
    "ire Python 3 and Unrpyc 2.";
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExit.Location = new System.Drawing.Point(3, 297);
            this.btnExit.Name = "btnExit";
            this.btnExit.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnExit.Size = new System.Drawing.Size(116, 23);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = global::Code.previewer.Lang.Configuration_Exit;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // RpycConfiguration
            // 
            this.Controls.Add(this.pnlLayout);
            this.Name = "RpycConfiguration";
            this.Size = new System.Drawing.Size(450, 405);
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.pnlPythons.ResumeLayout(false);
            this.pnlPythons.PerformLayout();
            this.pnlUnRpycButtons.ResumeLayout(false);
            this.pnlUnRpycButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        internal void LoadVersions()
        {
            try
            {
                pnlPythons.Enabled = false;

                var pypath = Settings.GetSetting("PythonPath", typeof(RpycPreviewer));
                var scriptpath = Settings.GetSetting("ScriptPath", typeof(RpycPreviewer));
                if (new FileInfo(scriptpath).Directory.Name.ToLowerInvariant() == "scripts")
                {
                    var altpath = Path.Combine(Path.GetDirectoryName(scriptpath), "Lib", "site-packages", "unrpyc.py");
                    if (File.Exists(altpath))
                    {
                        // Unrpyc depends on a few modules outside of the script. When installed in a venv, the script is additionally copied to the "Scripts" folder, but unless using that
                        // venv's copy of Python (and thus its site-packages), those additional modules can't be found there. To mitigate that, automatically select the site-packages copy
                        // if a "Scripts" copy is selected.
                        scriptpath = altpath;
                        Settings.SetSetting("ScriptPath", altpath, typeof(RpycPreviewer));
                    }
                }

                var scriptver = Lang.Configuration_NotAvailable;
                try
                {
                    var scriptText = File.ReadAllText(scriptpath);
                    var rx = new Regex("__version__ = ['\"]v([0-9.]+)['\"]");
                    var m = rx.Match(scriptText);
                    if (m.Success)
                    {
                        scriptver = m.Groups[1].Value;
                    }
                }
                catch { }

                var pyver = Lang.Configuration_NotAvailable;
                try
                {
                    if (File.Exists(pypath))
                    {
                        var psi = new ProcessStartInfo(
                            pypath,
                            "-c \"import sys; import platform; print((platform.python_implementation(), '%d.%d.%d' % (sys.implementation.version[0:3] if 'implementation' in dir(sys) else sys.pypy_version_info[0:3] if 'pypy_version_info' in dir(sys) else sys.version_info[0:3]), platform.python_version(), platform.architecture()[0], (not sys._is_gil_enabled()) if '_is_gil_enabled' in dir(sys) else False))\""
                            )
                        {
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false
                        };
                        var proc = Process.Start(psi);
                        var rawver = proc.StandardOutput.ReadToEnd();
                        proc.WaitForExit();
                        var rx = new Regex("\\('([^']+)', '([^']+)', '([^']+)', '([^']+)', (True|False)\\)");
                        var match = rx.Match(rawver);
                        var interp = match.Groups[1].Value;
                        if (interp == "CPython") interp = "Python";
                        var ver = match.Groups[2].Value;
                        var langver = match.Groups[3].Value;
                        var bit = match.Groups[4].Value;
                        if (char.IsDigit(bit[0])) bit = new string(bit.TakeWhile(char.IsDigit).Union(new char[] { '-' }).Union(bit.SkipWhile(char.IsDigit)).ToArray());
                        var free = match.Groups[5].Value == "True";
                        var sb = new StringBuilder();
                        sb.Append(interp);
                        sb.Append(' ');
                        sb.Append(ver);
                        sb.Append(" (");
                        sb.Append(bit);
                        if (free)
                            sb.Append(", freethreaded");
                        if (langver != ver)
                        {
                            sb.Append(", for Python ");
                            sb.Append(langver);
                        }
                        sb.Append(')');
                        pyver = sb.ToString();
                    }
                }
                catch { }

                lblPyVersion.Text = string.Format(Lang.Configuration_Python_CurrentVersion, pyver);
                lblPyPath.Text = pypath ?? Lang.Configuration_NotSet;
                lblUnrpycVersion.Text = string.Format(Lang.Configuration_Unrypc_CurrentVersion, scriptver);
                lblUnrpycPath.Text = scriptpath ?? Lang.Configuration_NotSet;
            }
            finally
            {
                pnlPythons.Enabled = true;
            }
        }

        internal void LoadTexts()
        {
            Lang.Culture = RPA_Explorer.Lang.Culture;

            LoadVersions();

            var pypath = Settings.GetSetting("PythonPath", typeof(RpycPreviewer));
            var scriptpath = Settings.GetSetting("ScriptPath", typeof(RpycPreviewer));

            pnlPythons.Controls.Clear();
            rdoPyBrowse.CheckedChanged -= rdoPython_CheckedChanged;
            rdoPyDownload.CheckedChanged -= rdoPython_CheckedChanged;


            lblTitle.Text = Lang.Configuration_Title;
            lblDescription.Text = Lang.Configuration_Description;
            lblDescription.Links.Clear();
            lblDescription.Links.Add(new LinkLabel.Link(Lang.Configuration_Description.IndexOf("Python"), 6, "https://www.python.org/"));
            lblDescription.Links.Add(new LinkLabel.Link(Lang.Configuration_Description.IndexOf("Unrpyc"), 6, "https://github.com/CensoredUsername/unrpyc"));
            btnUnrpycBrowse.Text = Lang.Configuration_Unrpyc_BrowseButton;
            btnUnrpycDownload.Text = Lang.Configuration_Unrpyc_Download;
            lblReminder.Text = Lang.Configuration_Reminder;
            btnExit.Text = Lang.Configuration_Exit;

            foreach (var python in PythonInstance.GetLocalInstances())
            {
                var rdo = new RadioButton
                {
                    Text = string.Format(Lang.Configuration_Python_Pep514, python.DisplayName),
                    AutoSize = true,
                    Tag = python.Path,
                    Checked = python.Path == pypath
                };
                rdo.CheckedChanged += rdoPython_CheckedChanged;
                pnlPythons.Controls.Add(rdo);
            }
            pnlPythons.Controls.Add(rdoPyBrowse);
            pnlPythons.Controls.Add(btnPyBrowse);
            pnlPythons.Controls.Add(rdoPyDownload);

            var dlPyPath = new DirectoryInfo(Path.GetDirectoryName(typeof(RpycConfiguration).Assembly.Location)).GetFiles("python.exe", SearchOption.AllDirectories).Where(py => !File.Exists(Path.Combine(py.DirectoryName, "deactivate.bat")));
            rdoPyDownload.Tag = dlPyPath.FirstOrDefault(py => py.FullName == pypath)?.FullName;
            rdoPyDownload.Checked = rdoPyDownload.Tag != null;
            rdoPyDownload.Tag ??= dlPyPath.FirstOrDefault()?.FullName;
            rdoPyBrowse.Checked = pypath != null && !pnlPythons.Controls.OfType<RadioButton>().Any(rdo => rdo.Checked) && !rdoPyDownload.Checked;
            if (rdoPyBrowse.Checked)
            {
                rdoPyBrowse.Tag = pypath;
            }
            rdoPyDownload.CheckedChanged += rdoPython_CheckedChanged;
            rdoPyBrowse.CheckedChanged += rdoPython_CheckedChanged;
        }

        private async void rdoPython_CheckedChanged(object sender, EventArgs e)
        {
            var rdoPython = (RadioButton)sender;
            if (!rdoPython.Checked) return;
            var pypath = rdoPython.Tag as string;
            if (string.IsNullOrEmpty(pypath))
            {
                if (rdoPython == rdoPyBrowse)
                {
                    btnPyBrowse.PerformClick();
                }
                else if (rdoPython == rdoPyDownload)
                {
                    await DownloadPython();
                    if (rdoPyDownload.Tag != null)
                        rdoPython_CheckedChanged(sender, e);
                }
            }
            else
            {
                Settings.SetSetting("PythonPath", pypath, typeof(RpycPreviewer));
                LoadVersions();
            }
        }

        private async Task DownloadPython()
        {
            try
            {
                rdoPyDownload.Enabled = false;
                using var ctx = Program.StatusBar.CreateContext();
                ctx.UpdateStatus(Lang.Download_Python_WhichPython, 0, 100);
                using var http = new HttpClient();
                http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("RPAExplorer", typeof(MainWindow).Assembly.GetName().Version.ToString()));
                var jsonText = await http.GetStringAsync("https://www.python.org/ftp/python/index-windows.json");
                var json = JObject.Parse(jsonText);
                var verrx = new Regex("^\\d+\\.\\d+\\.\\d+$");
                var manifest = (from ver in json["versions"]
                                where ver["company"].Value<string>() == "PythonCore" && verrx.IsMatch(ver["sort-version"].Value<string>())
                                orderby Version.Parse(ver["sort-version"].Value<string>()) descending
                                where ver["tag"].Value<string>().EndsWith(Environment.Is64BitOperatingSystem ? "-64" : "-32")
                                select ver).First();
                var version = manifest["sort-version"].Value<string>();
                var downloadUrl = manifest["url"].Value<string>();
                ctx.UpdateStatus(string.Format(Lang.Download_Python_Fetching, version), 20, 100);
                using var stream = await http.GetStreamAsync(downloadUrl);
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                ctx.UpdateStatus(string.Format(Lang.Download_Python_Extracting, version), 40, 100);
                await Task.Run(() => zip.ExtractToDirectory(Path.Combine(Path.GetDirectoryName(typeof(RpycPreviewer).Assembly.Location), "python")));
                ctx.UpdateStatus(Lang.Download_Python_Extracting, 100, 100);
                rdoPyDownload.Tag = Path.Combine(Path.GetDirectoryName(typeof(RpycPreviewer).Assembly.Location), "python", "python.exe");
            }
            finally
            {
                rdoPyDownload.Enabled = true;
            }
        }

        private void lblDescription_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }

        private void btnPyBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                rdoPyBrowse.CheckedChanged -= rdoPython_CheckedChanged;

                var pypath = Settings.GetSetting("PythonPath", typeof(RpycPreviewer));

                var dlgOpenFile = new OpenFileDialog()
                {
                    CheckFileExists = true,
                    Filter = "Python|python*.exe;pypy*.exe;ipy*.exe;rustpython.exe;jython.exe|All programs|*.exe",
                    InitialDirectory = (pypath != null) ? Path.GetDirectoryName(pypath) : string.Empty
                };
                if (dlgOpenFile.ShowDialog() == DialogResult.OK)
                {
                    rdoPyBrowse.Checked = true;
                    Settings.SetSetting("PythonPath", dlgOpenFile.FileName, typeof(RpycPreviewer));
                    LoadVersions();
                }
            }
            finally
            {
                rdoPyBrowse.CheckedChanged += rdoPython_CheckedChanged;
            }
        }

        private void btnUnrpycBrowse_Click(object sender, EventArgs e)
        {
            var scriptpath = Settings.GetSetting("ScriptPath", typeof(RpycPreviewer));

            var dlgOpenFile = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "Unrpyc|unrpyc.py",
                InitialDirectory = (scriptpath != null) ? Path.GetDirectoryName(scriptpath) : string.Empty
            };
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                Settings.SetSetting("ScriptPath", dlgOpenFile.FileName, typeof(RpycPreviewer));
                LoadVersions();
            }
        }

        private async void btnUnrpycDownload_Click(object sender, EventArgs e)
        {
            try
            {
                btnUnrpycBrowse.Enabled = false;
                btnUnrpycDownload.Enabled = false;
                using var ctx = Program.StatusBar.CreateContext();
                ctx.UpdateStatus(Lang.Download_Unrypc_WhichPython, 0, 100);
                var pypath = Settings.GetSetting("PythonPath", typeof(RpycPreviewer));
                var pyver = Version.Parse("3.9.0");
                try
                {
                    if (File.Exists(pypath))
                    {
                        var psi = new ProcessStartInfo(
                            pypath,
                            "-c \"import platform; print((platform.python_version(),))\""
                            )
                        {
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false
                        };
                        var proc = Process.Start(psi);
                        var rawver = proc.StandardOutput.ReadToEnd();
                        proc.WaitForExit();
                        var pyverrx = new Regex(@"\('([\d.]+).*?',\)");
                        var pyvermatch = pyverrx.Match(rawver);
                        if (pyvermatch.Success)
                        {
                            pyver = Version.Parse(pyvermatch.Groups[1].Value);
                        }
                    }
                }
                catch { }
                var root = Path.Combine(Path.GetDirectoryName(typeof(RpycPreviewer).Assembly.Location), (pyver.Major == 2) ? "unrpyc1" : "unrpyc2");
                if (File.Exists(Path.Combine(root, "unrpyc.py"))) {
                    Settings.SetSetting("ScriptPath", Path.Combine(root, "unrpyc.py"), typeof(RpycPreviewer));
                    LoadVersions();
                    return;
                }
                ctx.UpdateStatus(Lang.Download_Unrypc_WhichUnrpyc, 10, 100);
                using var http = new HttpClient();
                http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("RPAExplorer", typeof(MainWindow).Assembly.GetName().Version.ToString()));
                var response = await http.GetAsync("https://api.github.com/repos/CensoredUsername/unrpyc/tags");
                var jsonText = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                var json = JArray.Parse(jsonText);
                var manifest = (from verJ in json
                                select new { Version = Version.Parse(verJ["name"].Value<string>().Substring(1)), Url = verJ["zipball_url"].Value<string>() } into ver
                                where ver.Version.Major == ((pyver.Major == 2) ? 1 : 2)
                                orderby ver.Version descending
                                select ver).First();
                ctx.UpdateStatus(string.Format(Lang.Download_Unrpyc_Fetching, manifest.Version), 20, 100);
                using var stream = await http.GetStreamAsync(manifest.Url);
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                var skip = zip.Entries[0].FullName.Length;
                Directory.CreateDirectory(root);
                // Assumed: entry 0 is the "root directory" of the archive (which we want to eliminate because messy).
                for (var i = 1; i < zip.Entries.Count; i++)
                {
                    ctx.UpdateStatus(string.Format(Lang.Download_Unrpyc_Extracting, manifest.Version), i, zip.Entries.Count);
                    var entry = zip.Entries[i];
                    var fullpath = Path.Combine(root, entry.FullName.Substring(skip));
                    if (!fullpath.StartsWith(root, StringComparison.OrdinalIgnoreCase)) throw new IOException("File entry outside of the known universe?!");
                    if(Path.GetFileName(fullpath).Length ==0)
                    {
                        if (entry.Length > 0)
                        {
                            throw new IOException("Directory entry with file data?!");
                        }
                        else
                        {
                            Directory.CreateDirectory(fullpath);
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
                        entry.ExtractToFile(fullpath, overwrite: false);
                    }
                }
                Settings.SetSetting("ScriptPath", Path.Combine(root, "unrpyc.py"), typeof(RpycPreviewer));
                LoadVersions();
            }
            finally
            {
                btnUnrpycBrowse.Enabled = true;
                btnUnrpycDownload.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Program.MainWindow.PreviewSelectedItem();
        }
    }
}
