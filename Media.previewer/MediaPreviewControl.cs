using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using RPA_Explorer;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

internal class MediaPreviewControl : UserControl, IDisposable
{
    private TableLayoutPanel tlpControls;
    private Button btnPlayPause;
    private Label lblPosition;
    private Label lblDuration;
    private Button btnMute;
    private TrackBar trkPlayhead;
    private TrackBar trkVolume;
    private VideoView vlcPlayer;
    private Timer tmrVolumeDebounce;
    private System.ComponentModel.IContainer components;
    private string formatString = "-\\:--\\:--";
    private bool disposedValue;
    private Media media;
    private Stream stream;

    public MediaPreviewControl(Stream stream, bool isAudio)
    {
        InitializeComponent();
        trkVolume.Value = int.Parse(Settings.GetSetting("Volume") ?? "100");
        this.stream = stream;
        if (isAudio) vlcPlayer.BackgroundImage = Resources.prvMedia_AudioBackdrop;

        vlcPlayer.MediaPlayer = new MediaPlayer(MediaPreviewer.vlc);
        vlcPlayer.MediaPlayer.Volume = trkVolume.Value;
        vlcPlayer.MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
        vlcPlayer.MediaPlayer.EndReached += MediaPlayer_EndReached;
        vlcPlayer.MediaPlayer.Playing += MediaPlayer_Playing;
        vlcPlayer.MediaPlayer.Paused += MediaPlayer_Paused;
        vlcPlayer.MediaPlayer.Muted += MediaPlayer_Muted;
        vlcPlayer.MediaPlayer.Unmuted += MediaPlayer_Unmuted;

        media = new Media(MediaPreviewer.vlc, new StreamMediaInput(stream));
        media.ParsedChanged += Media_ParsedChanged;
        vlcPlayer.MediaPlayer.Media = media;
        vlcPlayer.MediaPlayer.Play();
    }

    private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
    {
        this.SafeInvoke(() =>
        {
            var position = TimeSpan.FromMilliseconds(e.Time);
            trkPlayhead.Value = (int)(position.TotalSeconds);
            lblPosition.Text = position.ToString(formatString);
        });
    }

    private void Media_ParsedChanged(object sender, MediaParsedChangedEventArgs e)
    {
        if (e.ParsedStatus != MediaParsedStatus.Done) return;
        var media = vlcPlayer.MediaPlayer.Media;
        var duration = TimeSpan.FromMilliseconds(media.Duration);
        if (duration.TotalDays > 1)
        {
            formatString = "d\\d h\\:mm\\:ss";
        }
        else if (duration.TotalHours > 1)
        {
            formatString = "h\\:mm\\:ss";
        }
        else if (duration.TotalMinutes > 1)
        {
            formatString = "mm\\:ss";
        }
        else
        {
            formatString = "s\\.ff";
        }
        this.SafeInvoke(() =>
        {
            lblPosition.Text = TimeSpan.Zero.ToString(formatString);
            lblDuration.Text = duration.ToString(formatString);
            trkPlayhead.Maximum=(int)(duration.TotalSeconds);
        });
    }

    private void MediaPlayer_Unmuted(object sender, EventArgs e)
    {
        this.SafeInvoke(() =>
        {
            btnMute.BackColor = SystemColors.Control;
            btnMute.Text = "\uE15D";
        });
    }

    private void MediaPlayer_Muted(object sender, EventArgs e)
    {
        this.SafeInvoke(() =>
        {
            btnMute.BackColor = SystemColors.ControlDark;
            btnMute.Text = "\uE198";
        });
    }

    private void MediaPlayer_Paused(object sender, EventArgs e)
    {
        this.SafeInvoke(() => btnPlayPause.Text = "\uE102");
    }

    private void MediaPlayer_Playing(object sender, EventArgs e)
    {
        this.SafeInvoke(() => btnPlayPause.Text = "\uE103");
    }

    private void MediaPlayer_EndReached(object sender, EventArgs e)
    {
        this.SafeInvoke(() => btnPlayPause.Text = "\uE102");
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.vlcPlayer = new LibVLCSharp.WinForms.VideoView();
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.btnMute = new System.Windows.Forms.Button();
            this.trkPlayhead = new System.Windows.Forms.TrackBar();
            this.trkVolume = new System.Windows.Forms.TrackBar();
            this.tmrVolumeDebounce = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).BeginInit();
            this.tlpControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkPlayhead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // vlcPlayer
            // 
            this.vlcPlayer.BackColor = System.Drawing.Color.Black;
            this.vlcPlayer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.vlcPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcPlayer.Location = new System.Drawing.Point(0, 0);
            this.vlcPlayer.MediaPlayer = null;
            this.vlcPlayer.Name = "vlcPlayer";
            this.vlcPlayer.Size = new System.Drawing.Size(645, 332);
            this.vlcPlayer.TabIndex = 0;
            this.vlcPlayer.Text = "videoView1";
            this.vlcPlayer.Click += new System.EventHandler(this.vlcPlayer_Click);
            // 
            // tlpControls
            // 
            this.tlpControls.AutoSize = true;
            this.tlpControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpControls.BackColor = System.Drawing.SystemColors.Control;
            this.tlpControls.ColumnCount = 6;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpControls.Controls.Add(this.btnPlayPause, 0, 0);
            this.tlpControls.Controls.Add(this.lblPosition, 1, 0);
            this.tlpControls.Controls.Add(this.lblDuration, 3, 0);
            this.tlpControls.Controls.Add(this.btnMute, 4, 0);
            this.tlpControls.Controls.Add(this.trkPlayhead, 2, 0);
            this.tlpControls.Controls.Add(this.trkVolume, 5, 0);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlpControls.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tlpControls.Location = new System.Drawing.Point(0, 296);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 1;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpControls.Size = new System.Drawing.Size(645, 36);
            this.tlpControls.TabIndex = 1;
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.AutoSize = true;
            this.btnPlayPause.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPlayPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlayPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlayPause.Location = new System.Drawing.Point(3, 3);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(30, 30);
            this.btnPlayPause.TabIndex = 0;
            this.btnPlayPause.Text = "";
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPosition.Location = new System.Drawing.Point(39, 0);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(43, 36);
            this.lblPosition.TabIndex = 1;
            this.lblPosition.Text = "0:00:00";
            this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDuration.Location = new System.Drawing.Point(463, 0);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(43, 36);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.Text = "0:00:00";
            this.lblDuration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMute
            // 
            this.btnMute.AutoSize = true;
            this.btnMute.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMute.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMute.Location = new System.Drawing.Point(512, 3);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(30, 30);
            this.btnMute.TabIndex = 3;
            this.btnMute.Text = "";
            this.btnMute.UseVisualStyleBackColor = true;
            this.btnMute.Click += new System.EventHandler(this.btnMute_Click);
            // 
            // trkPlayhead
            // 
            this.trkPlayhead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trkPlayhead.Enabled = false;
            this.trkPlayhead.LargeChange = 60;
            this.trkPlayhead.Location = new System.Drawing.Point(88, 3);
            this.trkPlayhead.Maximum = 458;
            this.trkPlayhead.Name = "trkPlayhead";
            this.trkPlayhead.Size = new System.Drawing.Size(369, 30);
            this.trkPlayhead.SmallChange = 5;
            this.trkPlayhead.TabIndex = 4;
            this.trkPlayhead.TickFrequency = 60;
            // 
            // trkVolume
            // 
            this.trkVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trkVolume.LargeChange = 20;
            this.trkVolume.Location = new System.Drawing.Point(548, 3);
            this.trkVolume.Maximum = 100;
            this.trkVolume.Name = "trkVolume";
            this.trkVolume.Size = new System.Drawing.Size(94, 30);
            this.trkVolume.SmallChange = 5;
            this.trkVolume.TabIndex = 5;
            this.trkVolume.TickFrequency = 10;
            this.trkVolume.Value = 100;
            this.trkVolume.Scroll += new System.EventHandler(this.trkVolume_Scroll);
            // 
            // tmrVolumeDebounce
            // 
            this.tmrVolumeDebounce.Interval = 1000;
            this.tmrVolumeDebounce.Tick += new System.EventHandler(this.tmrVolumeDebounce_Tick);
            // 
            // MediaPreviewControl
            // 
            this.Controls.Add(this.tlpControls);
            this.Controls.Add(this.vlcPlayer);
            this.Name = "MediaPreviewControl";
            this.Size = new System.Drawing.Size(645, 332);
            ((System.ComponentModel.ISupportInitialize)(this.vlcPlayer)).EndInit();
            this.tlpControls.ResumeLayout(false);
            this.tlpControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkPlayhead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void tmrVolumeDebounce_Tick(object sender, EventArgs e)
    {
        tmrVolumeDebounce.Stop();
        Settings.SetSetting("Volume", trkVolume.Value.ToString());
    }

    private void btnPlayPause_Click(object sender, EventArgs e)
    {
        if(media.State == VLCState.Ended)
        {
            media.Dispose();
            media = new Media(MediaPreviewer.vlc, new StreamMediaInput(stream));
            media.ParsedChanged+= Media_ParsedChanged;
            vlcPlayer.MediaPlayer.Media = media;
            vlcPlayer.MediaPlayer.Play();
        }
        else vlcPlayer.MediaPlayer.Pause();
    }

    private void btnMute_Click(object sender, EventArgs e)
    {
        if (vlcPlayer.MediaPlayer.Mute)
        {
            vlcPlayer.MediaPlayer.Mute = false;
        }
        else
        {
            vlcPlayer.MediaPlayer.Mute = true;
        }
    }

    private void trkVolume_Scroll(object sender, EventArgs e)
    {
        vlcPlayer.MediaPlayer.Volume = trkVolume.Value;
        tmrVolumeDebounce.Start();
    }

    private void vlcPlayer_Click(object sender, EventArgs e)
    {
        btnPlayPause.Focus();
        btnPlayPause_Click(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                vlcPlayer.MediaPlayer.Stop();
                vlcPlayer.MediaPlayer.Dispose();
                vlcPlayer.Dispose();
            }
            disposedValue = true;
        }
        base.Dispose(disposing);
    }

    public new void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void SafeInvoke(Action runInUIThread)
    {
        if (!this.IsHandleCreated) return;
        this.Invoke(runInUIThread);
    }
}
