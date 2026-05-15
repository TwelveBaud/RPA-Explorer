using LibHeifSharp;
using RPA_Explorer.Previewers;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class HeifPreviewer : IPreviewer
{
    public string Name => "HEIF Image Previewer";

    public string MediaType => "Image";

    public bool CanPreview(string filename, byte[] magic)
    {
        return filename.EndsWith(".avif") || filename.EndsWith(".heif");
    }

    public void LoadTexts()
    {
        // No localization.
    }

    public Control Preview(string filename, Stream contents)
    {
        var ctx = new HeifContext(contents, false);
        using var handle = ctx.GetPrimaryImageHandle();
        using var heifImage = handle.Decode(HeifColorspace.Rgb, handle.HasAlphaChannel ? HeifChroma.InterleavedRgba32 : HeifChroma.InterleavedRgb24);
        var heifPlane = heifImage.GetPlane(HeifChannel.Interleaved);
        var gdiImage = new Bitmap(handle.Width, handle.Height, handle.HasAlphaChannel ? (handle.IsPremultipliedAlpha ? PixelFormat.Format32bppPArgb : PixelFormat.Format32bppArgb) : PixelFormat.Format24bppRgb);
        var bits = gdiImage.LockBits(new Rectangle(0, 0, handle.Width, handle.Height), ImageLockMode.WriteOnly, gdiImage.PixelFormat);
        byte[] buf = new byte[heifImage.Width * (handle.HasAlphaChannel ? 4 : 3)];
        for (int y = 0; y < handle.Height; y++)
        {
            Marshal.Copy(heifPlane.Scan0 + heifPlane.Stride * y, buf, 0, buf.Length);
            switch (gdiImage.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    // RGB => BGR
                    for (int x = 0; x < buf.Length; x += 3)
                    {
                        (buf[x + 2], buf[x]) = (buf[x], buf[x + 2]);
                    }
                    Marshal.Copy(buf, 0, bits.Scan0 + bits.Stride * y, buf.Length);
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    // RGBA => BGRA
                    for (int x = 0; x < buf.Length; x += 4)
                    {
                        (buf[x + 2], buf[x]) = (buf[x], buf[x + 2]);
                    }
                    Marshal.Copy(buf, 0, bits.Scan0 + bits.Stride * y, buf.Length);
                    break;
                default:
                    // Hoo nose.
                    Marshal.Copy(buf, 0, bits.Scan0 + bits.Stride * y, buf.Length);
                    break;
            }
        }
        gdiImage.UnlockBits(bits);
        return new ImagePreviewControl(gdiImage, "HEIF");
    }

    public void RegisterOptionsMenu(ToolStripMenuItem parent)
    {
        // No options.
    }
}
