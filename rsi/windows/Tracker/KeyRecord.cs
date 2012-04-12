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

namespace Tracker
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
            this.Visible = true;

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            SetWinFullScreen(this.Handle);

            watch = new Stopwatch();
            watch.Start();

            progressTimer = new System.Timers.Timer();
            progressTimer.Interval = 100;
            progressTimer.Elapsed += new ElapsedEventHandler(this.progressTimer_Tick);
            progressTimer.Enabled = true;

            progress.Maximum = AppConfig.BreakTime / 100;
            progress.Minimum = 0;
            progress.Step = Convert.ToInt32(1);

            fadeTimer = new System.Timers.Timer();
            fadeTimer.Interval = 50;
            fadeTimer.Elapsed += new ElapsedEventHandler(this.fadeTimer_Tick);
            fadeTimer.Enabled = true;
        }

        public void Maximize()
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

        private delegate void IncreaseProgressDelegate();
        private void IncreaseProgressThread()
        {
            this.Invoke(new IncreaseProgressDelegate(IncreaseProgressImpl));
        }

        private void IncreaseProgressImpl()
        {
            this.progress.PerformStep();
            if (progress.Value == progress.Maximum)
            {
                Console.WriteLine(watch.ElapsedMilliseconds);
                FormState.GetInstance().Restore();
            }
        }

        public void IncreaseProgress()
        {
            new Thread(new ThreadStart(IncreaseProgressThread)).Start();
        }

        private delegate void RestoreDelegate();
        private void RestoreThread()
        {
            this.Invoke(new RestoreDelegate(RestoreImpl));
        }

        private void RestoreImpl()
        {
            if (fadeTimer != null)
                fadeTimer.Enabled = false;
            if (progressTimer != null)
                progressTimer.Enabled = false;
            if (breakTimer != null)
                breakTimer.Enabled = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
        }

        public void Restore()
        {
            new Thread(new ThreadStart(RestoreThread)).Start();
        }

        private System.Timers.Timer fadeTimer;
        private System.Timers.Timer breakTimer;
        private System.Timers.Timer progressTimer;
        private Stopwatch watch;

        private void MainForm_Load(object sender, EventArgs e)
        {
            appData = AppData.GetInstance();
            SensocolSocket.GetInstance();
            FormState.SetForm(this);

            ActivityTracker.Start();

            Application.ApplicationExit += new EventHandler(this.application_Exit);

            RestoreImpl();

            if (!appData.SettingClicked)
            {
                notifyIcon.ShowBalloonTip(20000, AppConfig.WelcomeTitle, AppConfig.WelcomeContent, ToolTipIcon.Info);
                notifyIcon.BalloonTipClicked += new EventHandler(this.menuItemSetting_Click);
            }

            // RegistryKey add = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            // add.SetValue("rsi", "\"" + Application.ExecutablePath.ToString() + "\"");
        }

        private int mState = 1;
        private double mOpacity;
        private void fadeTimer_Tick(object sender, ElapsedEventArgs e)
        {
            double op = this.Opacity + mState * 0.02;
            if (op >= 0.9)
            {
                op = 0.9;
                fadeTimer.Enabled = false;
                mState = 0;
            }
            mOpacity = op;
            SetOpacity();
        }

        private void progressTimer_Tick(object sender, ElapsedEventArgs e)
        {
            IncreaseProgress();
        }

        private void menuItemSetting_Click(object sender, EventArgs e)
        {
            if (!appData.SettingClicked)
            {
                appData.SettingClicked = true;
                appData.Save();
            }

            string url = appData.LoginUrl;
            if (url == null || url.Equals(""))
                url = AppConfig.ServerUrl + "?connect_to_sensor=" + appData.SensorUUID;
            Process.Start(url);

            SensocolSocket.GetInstance().RequestLoginTokenFor(AppConfig.ServerUrl);
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
    }
}
