using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Drawing;
using System.Timers;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Brainpage
{
    public partial class KeyRecord : Form
    {
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int which);

        [DllImport("user32.dll")]
        public static extern void
            SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                         int X, int Y, int width, int height, uint flags);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private static IntPtr HWND_TOP = IntPtr.Zero;
        private const int SWP_SHOWWINDOW = 64; // 0×0040

        public static int ScreenX
        {
            get { return GetSystemMetrics(SM_CXSCREEN); }
        }

        public static int ScreenY
        {
            get { return GetSystemMetrics(SM_CYSCREEN); }
        }

        public static void SetWinFullScreen(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }

        private AppData appData;

        public KeyRecord()
        {
            InitializeComponent();
        }

        private delegate void MaximizeDelegate();
        private void MaximizeThread()
        {
            this.Invoke(new MaximizeDelegate(MaximizeImpl));
        }

        private void MaximizeImpl()
        {
            this.Opacity = 0.6;

            fadeTimer = new System.Timers.Timer();
            fadeTimer.Interval = 50;
            fadeTimer.Elapsed += new ElapsedEventHandler(this.fadeTimer_Tick);
            fadeTimer.Enabled = true;

            breakTimer = new System.Timers.Timer();
            breakTimer.Interval = 1000;
            breakTimer.Elapsed += new ElapsedEventHandler(this.breakTimer_Tick);
            breakTimer.Enabled = true;
            seconds = 0;

            appData.FromLastBreak.Stop();
            webBrowser.Hide();
            webBrowser.Navigate(AppConfig.ScreenSaverUrl + "?t=" + appData.FromLastBreak.ElapsedMilliseconds / 1000 + "&uuid=" + appData.SensorUUID);

            this.Show();

        }

        public void MaximizeRemind()
        {
            new Thread(new ThreadStart(MaximizeThread)).Start();
        }

        private delegate void SetOpacityDelegate();
        private void SetOpacityThread()
        {
            this.Invoke(new SetOpacityDelegate(SetOpacityImpl));
        }

        private void SetOpacityImpl()
        {
            this.Opacity = mOpacity;
        }

        public void SetOpacity()
        {
            new Thread(new ThreadStart(SetOpacityThread)).Start();
        }

        private delegate void RestoreDelegate();
        private void RestoreThread()
        {
            this.Invoke(new RestoreDelegate(RestoreImpl));
        }

        private void RestoreImpl()
        {
            this.Hide();

            if (fadeTimer != null)
                fadeTimer.Enabled = false;
            if (breakTimer != null)
                breakTimer.Enabled = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            if (appData.FromLastBreak == null)
                appData.FromLastBreak = new Stopwatch();
            appData.FromLastBreak.Reset();

            notifyIcon.Icon = Properties.Resources.StatusBar0;
            notifyIcon.Visible = true;
            seconds = 0;
        }

        public void Restore()
        {
            new Thread(new ThreadStart(RestoreThread)).Start();
        }

        private delegate void UpdateStatusIconDelegate();
        private void UpdateStatusIconThread()
        {
            this.Invoke(new UpdateStatusIconDelegate(UpdateStatusIconImpl));
        }

        private void UpdateStatusIconImpl()
        {
            notifyIcon.Icon = appData.StatusIcon;
            notifyIcon.Text = Strings.energyLeft + appData.EnergyLeft;
            notifyIcon.Visible = true;
        }

        public void UpdateConnectionStatus()
        {
            new Thread(new ThreadStart(UpdateConnectionStatusThread)).Start();
        }

        private delegate void UpdateConnectionStatusDelegate();
        private void UpdateConnectionStatusThread()
        {
            this.Invoke(new UpdateConnectionStatusDelegate(UpdateConnectionStatusImpl));
        }

        private bool balloonTipShown = false;
        private void UpdateConnectionStatusImpl()
        {
            notifyIcon.Text = appData.ConnectionStatus;
            menuItemSetting.Enabled = appData.Connected;
            menuItemView.Enabled = appData.Connected;

            if (appData.Connected && !balloonTipShown)
            {
                notifyIcon.ShowBalloonTip(5000, Strings.welcome, Strings.welcomeBody, ToolTipIcon.Info);
                notifyIcon.BalloonTipClicked += notifyIcon_BalloonTipClicked;
            }
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            SensocolSocket.GetInstance().RequestLoginTokenFor(AppConfig.ViewAnalysisUrl);
        }

        public void UpdateStatusIcon()
        {
            new Thread(new ThreadStart(UpdateStatusIconThread)).Start();
        }

        private delegate void UpdateBreakTimerDelegate();
        private void UpdateBreakTimerThread()
        {
            this.Invoke(new UpdateBreakTimerDelegate(UpdateBreakTimerImpl));
        }

        private int seconds;
        private void UpdateBreakTimerImpl()
        {
            seconds++;
            int minutes = seconds / 60;
            int hour = minutes / 60;
            int sec = seconds % 60;

            string time = sec.ToString() + "s";
            if (minutes > 0) { time = minutes.ToString() + "m:" + (sec < 10 ? "0" : "") + time; }
            if (hour > 0) { time = hour.ToString() + "h:" + (minutes < 10 ? "0" : "") + time; }

            btnGood.Text = time + "   " + Strings.stopBreak;
        }

        public void UpdateBreakTimer()
        {
            new Thread(new ThreadStart(UpdateBreakTimerThread)).Start();
        }

        private void breakTimer_Tick(object sender, ElapsedEventArgs e)
        {
            UpdateBreakTimer();
        }

        private System.Timers.Timer fadeTimer;
        private System.Timers.Timer breakTimer;

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            FormState.SetForm(this);

            appData = AppData.GetInstance();
            SensocolSocket.GetInstance();

            FormState.GetInstance().Restore();

            this.Text = Strings.connecting;

            this.Width = 600;
            this.Height = 500;
            int boundWidth = Screen.PrimaryScreen.Bounds.Width;
            int boundHeight = Screen.PrimaryScreen.Bounds.Height;
            int x = boundWidth - this.Width;
            int y = boundHeight - this.Height;
            this.Location = new Point(x / 2, y / 2);

            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;

            btnGood.Top = this.Height - btnGood.Height * 3 / 2;
            btnGood.Left = this.Width / 2 - btnGood.Width / 2;

            webBrowser.Width = this.Width;
            webBrowser.Height = btnGood.Top;
            webBrowser.Top = 0;
            webBrowser.Left = 0;

            menuItemExit.Text = Strings.exit;
            menuItemSetting.Text = Strings.viewSetting;
            menuItemView.Text = Strings.viewChart;
            btnGood.Text = Strings.stopBreak;

            ActivityTracker.Start();

            Application.ApplicationExit += new EventHandler(this.application_Exit);

            RegistryKey add = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            add.SetValue("Brainpage", "\"" + Application.ExecutablePath.ToString() + "\"");
        }

        private int mState = 1;
        private double mOpacity;
        private void fadeTimer_Tick(object sender, ElapsedEventArgs e)
        {
            double op = this.Opacity + mState * 0.02;
            if (op >= 1)
            {
                op = 1;
                fadeTimer.Enabled = false;
                mState = 0;
            }
            mOpacity = op;
            SetOpacity();
        }

        private void menuItemSetting_Click(object sender, EventArgs e)
        {
            SensocolSocket.GetInstance().RequestLoginTokenFor(AppConfig.SettingsUrl);
        }

        private void menuItemView_Click(object sender, EventArgs e)
        {
            SensocolSocket.GetInstance().RequestLoginTokenFor(AppConfig.ViewAnalysisUrl);
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void application_Exit(object sender, EventArgs e)
        {
            SensocolSocket.GetInstance().CacheQueueToFile();
        }

        private void btnGood_Click(object sender, EventArgs e)
        {
            FormState.GetInstance().Restore();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.Show();
        }


    }
}
