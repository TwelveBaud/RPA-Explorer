using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RPA_Explorer
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            LoadTexts();

            LoadTranslations();
            LoadContributors();
        }

        private void LoadTexts()
        {
            var rawVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.Split('+');
            Text = Lang.About;
            lblProductName.Text = Lang.Explorer_title;
            lblProductVersion.Text = string.Format(Lang.About_version, rawVersion[0]);
            lblCommit.Text = string.Format(Lang.About_commit, rawVersion[1]);
            lblAuthors.Text = Lang.About_creators;
            lblInspiration.Text = Lang.About_inspiration;
            lblInspiration.Links.Clear();
            lblInspiration.Links.Add(Lang.About_inspiration.IndexOf("rpatool"), 7, "https://codeberg.org/shiz/rpatool");
            lblInspiration.Links.Add(Lang.About_inspiration.IndexOf("unrpyc"), 6, "https://github.com/CensoredUsername/unrpyc");
            tabTranslations.Text = Lang.About_translations;
            tabContributors.Text = Lang.About_contributors;
            tabDisclosures.Text = Lang.About_disclosures;
            txtDisclosures.Rtf = Resources.Disclosures;
            btnClose.Text = Lang.About_close;
        }

        private static string UniToRtf(string unicode)
        {
            return unicode.Aggregate(new StringBuilder(), (sb, @char) =>
            {
                var codepoint = (short)@char;
                if (codepoint < 0x20 || codepoint > 0x7f)
                {
                    sb.AppendFormat("\\u{0} ", codepoint);
                }
                else
                {
                    sb.Append(@char);
                }
                return sb;
            }).ToString();
        }

        private void LoadTranslations()
        {
            var rtf = new StringBuilder("{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0 Microsoft Sans Serif;}}\\ud0\r\n\\pard\\fs17");
            foreach (var culture in Program.Languages)
            {
                var resourceSet = Lang.ResourceManager.GetResourceSet(culture, true, false);
                var translator = resourceSet.GetString("!Translator");
                rtf.Append("\\b ");
                rtf.Append(UniToRtf(culture.NativeName));
                rtf.Append(":\\b0  ");
                rtf.Append(UniToRtf(translator));
                rtf.AppendLine("\\par");
            }
            rtf.Append("}");
            txtTranslations.Rtf = rtf.ToString();
        }

        private void LoadContributors()
        {
            var rtf = new StringBuilder("{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0 Microsoft Sans Serif;}}\\ud0\r\n\\pard\\fs17");
            rtf.AppendLine("\\b Refactoring:\\b0  Andrew \"TwelveBaud\" Cook\\par");
            rtf.Append("}");
            txtContributors.Rtf = rtf.ToString();
        }

        private void lblGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void lblInspiration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }
    }
}
