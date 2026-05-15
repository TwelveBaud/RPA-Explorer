using Code.previewer;
using MS.WindowsAPICodePack.Internal;
using RPA_Explorer;
using RPA_Explorer.Previewers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using Lang = Code.previewer.Lang;

internal class RpycPreviewer : IPreviewer
{
    public string Name => "Ren'Py Code Previewer";

    public string MediaType => "Code";

    private ToolStripMenuItem OptionsMenu;
    private WeakReference<RpycConfiguration> ShownConfiguration = new WeakReference<RpycConfiguration>(null);

    public bool CanPreview(string filename, byte[] magic)
    {
        return filename.EndsWith(".rpyc") || filename.EndsWith(".rpymc");
    }

    public void LoadTexts()
    {
        Lang.Culture = RPA_Explorer.Lang.Culture;
        if (OptionsMenu != null)
        {
            OptionsMenu.Text = Lang.Configuration_Title;
        }
        if (ShownConfiguration.TryGetTarget(out var config))
        {
            config.LoadTexts();
        }
    }

    public Control Preview(string filename, Stream contents)
    {
        var pypath = Settings.GetSetting("PythonPath", typeof(RpycPreviewer));
        var scriptpath = Settings.GetSetting("ScriptPath", typeof(RpycPreviewer));

        if (pypath == null || !File.Exists(pypath) || scriptpath == null || !File.Exists(scriptpath))
        {
            var config = new RpycConfiguration();
            ShownConfiguration.SetTarget(config);
            return config;
        }

        string tmpFilename = Path.GetTempFileName();
        string cookedFilename = Path.ChangeExtension(tmpFilename, Path.GetExtension(filename));
        string rawFilename = cookedFilename.Substring(0, cookedFilename.Length - 1);
        string unrpycOutput = string.Empty;
        string rawFile;
        try
        {
            File.Delete(tmpFilename);
            using (var cookedFile = File.Open(cookedFilename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                contents.CopyTo(cookedFile);
            }

            var psi = new ProcessStartInfo()
            {
                FileName = pypath,
                Arguments = string.Format(@"""{0}"" --try-harder --clobber ""{1}""", scriptpath, cookedFilename),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            using (Process process = Process.Start(psi))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    unrpycOutput += reader.ReadToEnd();
                }
                using (StreamReader reader = process.StandardError)
                {
                    unrpycOutput += reader.ReadToEnd();
                }
            }

            if (!File.Exists(rawFilename) || new FileInfo(rawFilename).Length==0)
            {
                throw new Exception("The decompilation was not successful:\n\n" + unrpycOutput);
            }
            else
            {
                rawFile = File.ReadAllText(rawFilename);
            }
        }
        finally
        {
            try { if (File.Exists(cookedFilename)) File.Delete(cookedFilename); } catch { }
            try { if (File.Exists(rawFilename)) File.Delete(rawFilename); } catch { }
        }

        //TODO: Use Scintilla or something
        var ms = new MemoryStream();
        using (var sw = new StreamWriter(ms, Encoding.UTF8, 1024, true))
        {
            sw.Write(rawFile);
        };
        ms.Seek(0, SeekOrigin.Begin);
        var magic = new byte[16];
        Array.Copy(ms.ToArray(), magic, Math.Min(16, ms.Length));

        foreach (var otherPreviewer in Program.Previewers)
        {
            if (otherPreviewer.CanPreview(rawFilename, magic))
                return otherPreviewer.Preview(rawFilename, ms);
        }

        throw new Exception("No previewer exists for code files!");
    }

    public void RegisterOptionsMenu(ToolStripMenuItem parent)
    {
        OptionsMenu = new ToolStripMenuItem()
        {
            Text = Lang.Configuration_Title
        };
        OptionsMenu.Click += mnuiOptions_Click;
        parent.DropDownItems.Add(OptionsMenu);
    }

    private void mnuiOptions_Click(object sender, EventArgs e)
    {
        var oldControl = RPA_Explorer.Program.MainWindow.PreviewPanel.Controls.Cast<Control>().FirstOrDefault();
        RPA_Explorer.Program.MainWindow.PreviewPanel.Controls.Clear();
        if (oldControl is IDisposable trash)
        {
            trash.Dispose();
        }
        var config = new RpycConfiguration() { Dock = DockStyle.Fill };
        ShownConfiguration.SetTarget(config);
        RPA_Explorer.Program.MainWindow.PreviewPanel.Controls.Add(config);
    }
}
