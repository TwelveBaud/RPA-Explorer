using System.IO;
using System.Windows.Forms;

namespace RPA_Explorer.Previewers
{
    public interface IPreviewer
    {
        public string Name { get; }
        public string MediaType { get; }

        public bool CanPreview(string filename, byte[] magic);

        public Control Preview(string filename, Stream contents);

        public void RegisterOptionsMenu(ToolStripMenuItem parent);

        public void LoadTexts();
    }
}
