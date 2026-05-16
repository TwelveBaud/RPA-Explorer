using LibVLCSharp.Shared;
using RPA_Explorer.Previewers;
using System.IO;
using System.Linq;
using System.Windows.Forms;

public class MediaPreviewer : IPreviewer
{
    // https://code.videolan.org/videolan/vlc/-/blob/master/include/vlc_interface.h
    public static readonly string[] EXTENSIONS_AUDIO = {".3ga", ".669", ".a52", ".aac", ".ac3", ".adt", ".adts", ".aif",
        ".aifc", ".aiff", ".alac", ".amb", ".amr", ".aob", ".ape", ".au", ".awb", ".caf", ".dts", ".dsf", ".dff", ".flac",
        ".it", ".kar", ".m4a", ".m4b", ".m4p", ".m5p", ".mka", ".mlp", ".mod", ".mpa", ".mp1", ".mp2", ".mp3", ".mpc",
        ".mpga", ".mus", ".oga", ".ogg", ".oma", ".opus", ".qcp", ".ra", ".rmi", ".s3m", ".sid", ".spx", ".tak", ".thd",
        ".tta", ".voc", ".vqf", ".w64", ".wav", ".wma", ".wv", ".xa", ".xm" };

    public static readonly string[] EXTENSIONS_VIDEO = { ".3g2", ".3gp", ".3gp2", ".3gpp", ".amrec", ".amv", ".asf", ".avi",
        ".bik", ".crf", ".dav", ".divx", ".drc", ".dv", ".dvr-ms", ".evo", ".f4v", ".flv", ".gvi", ".gxf", ".iso", ".k3g",
        ".m1v", ".m2v", ".m2t", ".m2ts", ".m4v", ".mkv", ".mov", ".mp2", ".mp2v", ".mp4", ".mp4v", ".mpe", ".mpeg", ".mpeg1",
        ".mpeg2", ".mpeg4", ".mpg", ".mpv2", ".mts", ".mtv", ".mxf", ".mxg", ".nsv", ".nuv", ".ogg", ".ogm", ".ogv", ".ogx",
        ".ps", ".qt", ".rec", ".rm", ".rmvb", ".rpl", ".skm", ".thp", ".tod", ".ts", ".tts", ".txd", ".vob", ".vp6", ".vro",
        ".webm", ".wm", ".wmv", ".wtv", ".xesc" };

    static MediaPreviewer()
    {
        Core.Initialize();
        vlc = new LibVLC();
    }

    internal static LibVLC vlc;

    public string Name => "Media Previewer";

    public string MediaType => "Media";

    public bool CanPreview(string filename, byte[] magic)
    {
        return EXTENSIONS_AUDIO.Any(filename.EndsWith) || EXTENSIONS_VIDEO.Any(filename.EndsWith);
    }

    public void LoadTexts()
    {
        // No texts
    }

    public Control Preview(string filename, Stream contents)
    {
        return new MediaPreviewControl(contents, EXTENSIONS_AUDIO.Any(filename.EndsWith));
    }

    public void RegisterOptionsMenu(ToolStripMenuItem parent)
    {
        // No options
    }
}