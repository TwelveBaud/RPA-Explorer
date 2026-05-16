using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RPA_Explorer.Previewers
{
    public partial class ImagePreviewControl : UserControl
    {
        private bool isFirstSize = true;

        private string format;
        public string Format
        {
            get => format;
            set
            {
                format = value;
                UpdateData();
            }
        }

        public Image Image
        {
            get => pbxBitmap.Image;
            set
            {
                pbxBitmap.Image = value;
                UpdateZoom();
                UpdateData();
            }
        }

        private double zoom = 1;
        public double Zoom
        {
            get => zoom;
            set
            {
                zoom = value;
                UpdateZoom();
            }
        }

        public ImagePreviewControl()
        {
            InitializeComponent();

            btnBlack.Tag = Color.Black;
            btnDark.Tag = Resources.BgDarkImpl;
            btnGray.Tag = Color.Gray;
            btnLight.Tag = Resources.BgLightImpl;
            btnWhite.Tag = Color.White;

            mnuiZoom50.Tag = 0.5;
            mnuiZoom100.Tag = 1.0;
            mnuiZoom200.Tag = 2.0;

            var lastBg = Settings.GetSetting("Background") ?? nameof(btnLight);
            foreach (var btn in new[] { btnBlack, btnDark, btnGray, btnLight, btnWhite })
                if (lastBg == btn.Name)
                    btnBackground_Click(btn, EventArgs.Empty);
        }

        public ImagePreviewControl(Stream stream) : this()
        {
            Image = Image.FromStream(stream);
            foreach (var codec in System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders())
                if (codec.FormatID.Equals(Image.RawFormat.Guid))
                    Format = codec.FormatDescription;
        }

        public ImagePreviewControl(Bitmap bitmap, string format) : this()
        {
            Format=format;
            Image= bitmap;
        }

        public ImagePreviewControl(byte[] imageData, string format) : this()
        {
            throw new NotImplementedException();
        }

        public void btnBackground_Click(object sender, EventArgs e)
        {
            var btnBackground = sender as ToolStripButton;
            btnBlack.Checked = btnDark.Checked = btnGray.Checked = btnLight.Checked = btnWhite.Checked = false;
            btnBackground.Checked = true;
            Settings.SetSetting("Background", btnBackground.Name);
            if (btnBackground.Tag is Bitmap bitmap)
            {
                pbxBitmap.BackColor = SystemColors.Control;
                pbxBitmap.BackgroundImage = bitmap;
            }
            else if (btnBackground.Tag is Color color)
            {
                pbxBitmap.BackColor = color;
                pbxBitmap.BackgroundImage = null;
            }
        }

        private void mnuiZoom_Click(object sender, EventArgs e)
        {
            if (sender is Control control && control.Tag is double factor)
            {
                Zoom = factor;
            }
            else if (sender is ToolStripItem tsi && tsi.Tag is double factor2)
            {
                Zoom = factor2;
            }
            else if (sender == mnuiZoomFit)
            {
                var widthRatio = (double)this.Width / Image.Width;
                var heightRatio = (double)(this.Height - toolStrip1.Height) / Image.Height;
                Zoom = Math.Min(widthRatio, heightRatio);
            }
        }

        private void UpdateZoom()
        {
            sbtnZoom.Text = zoom.ToString("P0");
            pbxBitmap.Width = (int)Math.Ceiling(Image.Width * zoom);
            pbxBitmap.Height = (int)Math.Ceiling(Image.Height * zoom);
            pbxBitmap.Left = this.Width / 2 - pbxBitmap.Width / 2;
            pbxBitmap.Top = this.Height / 2 - pbxBitmap.Height / 2;
        }

        private void UpdateData()
        {
            if (Image != null)
                lblImageInfo.Text = $"{format} - {Image.Width} × {Image.Height} @ {Image.GetPixelFormatSize(Image.PixelFormat)}bpp";
            else
                lblImageInfo.Text = string.Empty;
        }

        private void ImagePreviewerControl_Resize(object sender, EventArgs e)
        {
            if (isFirstSize)
            {
                isFirstSize = false;
                if (Image != null)
                {
                    if (Image.Width > this.Width || Image.Height > this.Height - toolStrip1.Height)
                    {
                        mnuiZoomFit.PerformClick();
                    }
                    else
                    {
                        mnuiZoom100.PerformClick();
                    }
                }
            }
            UpdateZoom();
        }
    }
}
