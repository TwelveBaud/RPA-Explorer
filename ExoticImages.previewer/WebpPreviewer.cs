using RPA_Explorer.Previewers;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WebPWrapper;

public class WebpPreviewer : IPreviewer
{
    public string Name => "WebP Image Previewer";

    public string MediaType => "Image";

    public bool CanPreview(string filename, byte[] magic)
    {
        byte[] moreMagic = { 0x57, 0x45, 0x42, 0x50 };
        if (filename.EndsWith(".webp")) return true;
        for (int i = 0; i < magic.Length - 4; i++)
            if (magic.Skip(i).SequenceEqual(moreMagic))
                return true;
        return false;

    }

    public void LoadTexts()
    {
        // No text
    }

    public Control Preview(string filename, Stream contents)
    {
        var codec = new WebP();
        var ms = new MemoryStream();
        contents.CopyTo(ms);
        var img = codec.Decode(ms.ToArray());
        return new ImagePreviewControl(img, "WEBP");
    }

    public void RegisterOptionsMenu(ToolStripMenuItem parent)
    {
        // No options
    }
}
