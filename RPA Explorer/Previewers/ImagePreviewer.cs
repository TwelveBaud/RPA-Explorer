using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPA_Explorer.Previewers
{
    internal class ImagePreviewer : IPreviewer
    {
        public string Name => "Image Previewer";

        public string MediaType => "Image";

        public bool CanPreview(string filename, byte[] magic)
        {
            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                var extensions = codec.FilenameExtension.Split(';').Select(ext => ext.TrimStart('*').ToLowerInvariant());
                if (extensions.Any(filename.EndsWith)) return true;

                //for (int i = 0; i < codec.SignatureMasks.Length; i++)
                //{
                //    if (codec.SignaturePatterns[i].SequenceEqual(codec.SignatureMasks[i].Zip(magic, (mask, data) => (byte)(mask & data)))) return true;
                //}
            }
            return false;
        }

        public void LoadTexts()
        {
            // No text used in UI
        }

        public Control Preview(string filename, Stream contents)
        {
            return new ImagePreviewControl(contents);
        }

        public void RegisterOptionsMenu(ToolStripMenuItem parent)
        {
            // No options
        }
    }
}
