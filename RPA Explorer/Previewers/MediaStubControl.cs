#if !MINIMAL
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;


namespace RPA_Explorer.Previewers
{
    public class MediaStubControl : UserControl
    {
        private bool DownloadComplete => File.Exists(Path.Combine(typeof(MediaStub).Assembly.Location, "..", "Media.previewer.dll"));
        private Exception Exception;

        public MediaStubControl()
        {
            InitializeComponent();
            btnDownload.Enabled = !DownloadComplete;
            LoadTexts();
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaStubControl));
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(194, 26);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Download required";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(5, 35);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(473, 91);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = resources.GetString("lblMessage.Text");
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(8, 148);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(106, 41);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = global::RPA_Explorer.Lang.MediaStub_Download;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // MediaStubControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblHeader);
            this.Name = "MediaStubControl";
            this.Size = new System.Drawing.Size(551, 440);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblHeader;
        private Label lblMessage;
        private Button btnDownload;


        public void LoadTexts()
        {
            lblHeader.Text = Lang.MediaStub_Download_required;
            lblMessage.Text = (Exception is not null) ? $"{Exception.GetType().Name}: {Exception.Message}" : DownloadComplete ? Lang.MediaStub_Restart_required : Lang.MediaStub_Download_explained;
            btnDownload.Text = Lang.MediaStub_Download;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                btnDownload.Enabled = false;
                using var ctx = Program.StatusBar.CreateContext();
                ctx.UpdateStatus(Lang.MediaStub_Download_checking, 0, 0);
                var rawVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.Split('+');
                var http = new HttpClient();
                http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("RPAExplorer", typeof(MainWindow).Assembly.GetName().Version.ToString()));
                var manifestStr = await http.GetStringAsync("https://api.github.com/repos/TwelveBaud/RPA-Explorer/releases/tags/" + rawVersion[0]);
                var manifestObj = JObject.Parse(manifestStr);
                var downloadUrl = manifestObj["assets"].FirstOrDefault(asset => asset["name"].Value<string>().Contains("Media"))["browser_download_url"].Value<string>();
                ctx.UpdateStatus(string.Format(Lang.MediaStub_Downloading_file, downloadUrl), 20, 100);
                var downloadZip = await http.GetStreamAsync(downloadUrl);
                var downloadPkg = await Task.Run(() => new ZipArchive(downloadZip));
                ctx.UpdateStatus(Lang.MediaStub_Extracting, 60, 100);
                await Task.Run(() => downloadPkg.ExtractToDirectory(new FileInfo(typeof(MediaStub).Assembly.Location).DirectoryName));
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            finally
            {
                btnDownload.Enabled = !DownloadComplete;
                LoadTexts();
            }
        }
    }
}
#endif