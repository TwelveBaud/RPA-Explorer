using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RPA_Explorer.Previewers
{
    internal class TextPreviewer : IPreviewer
    {
        public string Name => "Text Previewer";

        public string MediaType => "Text";

        private static readonly string[] EXTENSIONS = [".py", ".rpy", ".log", ".nfo", ".htm", ".html", ".xml", ".json", ".yaml", ".toml", ".csv"];

        public bool CanPreview(string filename, byte[] magic)
        {
            if (filename.EndsWith(".txt"))
                return true;

            foreach (var otherPreviewer in Program.Previewers.Where(p => p.GetType().Assembly != typeof(TextPreviewer).Assembly))
            {
                if (otherPreviewer.CanPreview(filename, magic)) return false;
            }

            return EXTENSIONS.Any(filename.EndsWith);
        }

        public void LoadTexts()
        {
            if (lastSearchLabel?.TryGetTarget(out var label) ?? false)
            {
                label.Text = Lang.Search;
            }
            if (lastSearchButton?.TryGetTarget(out var button) ?? false)
            {
                button.Text = Lang.Search_next;
            }
        }

        WeakReference<Label> lastSearchLabel;
        WeakReference<Button> lastSearchButton;
        static Regex lineEndingNormalizer = new Regex("\\r?\\n");
        public Control Preview(string filename, Stream contents)
        {
            FontFamily monospace = null;
            foreach (var name in new string[] { "Cascadia Code", "Consolas", "Lucida Console", "Monaco" })
            {
                try
                {
                    monospace = new FontFamily(name);
                    break;
                }
                catch (ArgumentException) { }
            }
            if (monospace == null)
                monospace = FontFamily.GenericMonospace;

            var text = lineEndingNormalizer.Replace(new StreamReader(contents).ReadToEnd(), "\r\n");

            var textbox = new TextBox()
            {
                Multiline = true,
                ReadOnly = true,
                ShortcutsEnabled = true,
                Dock = DockStyle.Fill,
                Text = text,
                Font = new Font(monospace, 10),
                ScrollBars = ScrollBars.Both,
            };

            var panel = new TableLayoutPanel();
            panel.RowStyles.Clear();
            panel.RowStyles.Add(new RowStyle());
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            panel.ColumnStyles.Clear();
            panel.ColumnStyles.Add(new ColumnStyle());
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 111));

            var searchLabel = new Label()
            {
                Text = Lang.Search,
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            lastSearchLabel = new(searchLabel);

            var searchTextbox = new TextBox()
            {
                Dock = DockStyle.Fill,
                ShortcutsEnabled = true,
            };

            var searchButton = new Button()
            {
                Text = Lang.Search_next,
                Dock = DockStyle.Fill,
            };
            lastSearchButton = new(searchButton);

            void Search(object sender, EventArgs e)
            {
                var index = textbox.SelectionStart + 1;
                if (index >= text.Length || (index == 1 && textbox.SelectionLength == 0)) index = 0;
                var found = text.IndexOf(searchTextbox.Text, index, StringComparison.CurrentCultureIgnoreCase);
                if (found != -1)
                {
                    textbox.Select(found, searchTextbox.Text.Length);
                    textbox.Focus();
                    textbox.ScrollToCaret();
                    return;
                }
                found = text.IndexOf(searchTextbox.Text, StringComparison.CurrentCultureIgnoreCase);
                if (found != -1)
                {
                    textbox.Select(found, searchTextbox.Text.Length);
                    textbox.Focus();
                    textbox.ScrollToCaret();
                    return;
                }
                textbox.Select(index, 0);
                MessageBeep(1);
            }
            void F3Search(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.F3) { e.Handled = true; e.SuppressKeyPress = true; Search(sender, e); }

            }
            searchButton.Click += Search;
            searchTextbox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter) { e.Handled = true; e.SuppressKeyPress = true; Search(s, e); } };
            searchTextbox.KeyDown += F3Search;
            textbox.KeyDown += F3Search;
            searchButton.KeyDown += F3Search;

            panel.Controls.Add(searchLabel, 0, 0);
            panel.Controls.Add(searchTextbox, 1, 0);
            panel.Controls.Add(searchButton, 2, 0);
            panel.Controls.Add(textbox, 0, 1);
            panel.SetColumnSpan(textbox, 3);

            return panel;
        }

        [DllImport("user32.dll")]
        private static extern bool MessageBeep(int type);

        public void RegisterOptionsMenu(ToolStripMenuItem parent)
        {
            // No options.
        }
    }
}
