using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RPA_Explorer.Previewers
{
    internal class FontPreviewer : IPreviewer
    {
        public string Name => "Font Previewer";

        public string MediaType => "Font";

        public bool CanPreview(string filename, byte[] magic)
        {
            return filename.EndsWith(".ttf") || filename.EndsWith(".otf");
        }

        public void LoadTexts()
        {
            //TODO: Internationalize, if I care
        }

        public Control Preview(string filename, Stream contents)
        {
            return new FontPreviewControl(contents);
        }

        public void RegisterOptionsMenu(ToolStripMenuItem parent)
        {
            // No options
        }
    }

    internal class FontPreviewControl : Control, IDisposable
    {
        private IntPtr unmanagedFontData;
        private PrivateFontCollection fontCollection;
        private bool disposedValue;

        public FontPreviewControl(Stream stream) : base()
        {
            var rnd = new Random();
            seed = rnd.Next();
            try
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                unmanagedFontData = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, unmanagedFontData, data.Length);
                fontCollection = new PrivateFontCollection();
                fontCollection.AddMemoryFont(unmanagedFontData, data.Length);
                if (fontCollection.Families.Length == 0)
                {
                    throw new Exception("Font file did not contain a recognized font.");
                }
            }
            catch
            {
                if (fontCollection != null)
                {
                    fontCollection.Dispose();
                    fontCollection = null;
                }
                if (unmanagedFontData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(unmanagedFontData);
                    unmanagedFontData = IntPtr.Zero;
                }
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (fontCollection != null)
                    {
                        fontCollection.Dispose();
                    }
                }

                if (unmanagedFontData != IntPtr.Zero)
                    Marshal.FreeHGlobal(unmanagedFontData);
                disposedValue = true;
            }
            base.Dispose(disposing);
        }

        ~FontPreviewControl()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public new void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        int seed;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var family = fontCollection.Families[0];
            var black = new SolidBrush(Color.Black);

            var headerFont = new Font(this.Font.FontFamily, 20);

            e.Graphics.DrawString($"{family.Name}", headerFont, black, 3, 3);
            float y = 40;
            DrawPangram(TOP_PANGRAM, family, black, 10, ref y, e.Graphics, false);
            y += 6;
            var rand = new Random(seed);
            foreach (var i in new[] { 6, 8, 10, 12, 16, 20, 32, 40 })
            {
                DrawPangram(Lang.ResourceManager.GetString("Pangram_" + rand.Next(0, 3).ToString()), family, black, i, ref y, e.Graphics, true);
            }

            //TODO: Sylvie from the Question
        }

        const string TOP_PANGRAM = "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ\n1234567890.:,;'\"(!?)+-*/=";

        private void DrawPangram(string pangram, FontFamily family, Brush brush, float size, ref float y, Graphics graphics, bool gutter)
        {
            var font = new Font(family, size);
            var layout = graphics.MeasureString(pangram, font, new SizeF(this.Width - (gutter ? 28 : 6), this.Height), StringFormat.GenericTypographic, out int _, out int _);
            graphics.DrawString(pangram, font, brush, new RectangleF(gutter ? 25 : 3, y, this.Width - (gutter ? 28 : 6), layout.Height));
            if (gutter)
            {
                var gutterLayout = graphics.MeasureString(size.ToString(), Font, new SizeF(22, 100), StringFormat.GenericTypographic, out int _, out int _);
                graphics.DrawString(size.ToString(), Font, brush, new RectangleF(3, y+layout.Height-((family.GetLineSpacing(FontStyle.Regular)-family.GetEmHeight(FontStyle.Regular))*size/family.GetEmHeight(FontStyle.Regular)) - gutterLayout.Height, 22, gutterLayout.Height));
                y += Math.Max(layout.Height, gutterLayout.Height);
            }
            else
            {
                y += layout.Height;
            }
        }
    }
}
