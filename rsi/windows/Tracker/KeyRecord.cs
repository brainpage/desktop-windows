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

            breakTimer = new System.Timers.Timer();
            breakTimer.Interval = 1000;
            breakTimer.Elapsed += new ElapsedEventHandler(this.breakTimer_Tick);
            breakTimer.Enabled = true;
            seconds = 0;

            fadeTimer = new System.Timers.Timer();
            fadeTimer.Interval = 50;
            fadeTimer.Elapsed += new ElapsedEventHandler(this.fadeTimer_Tick);
            fadeTimer.Enabled = true;

            btnGood.Top = this.ClientSize.Height - btnGood.Height * 3;
            btnGood.Left = this.ClientSize.Width / 2 - btnGood.Width / 2;
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

        private delegate void UpdateBreakTimerDelegate();
        private void UpdateBreakTimerThread()
        {
            this.Invoke(new UpdateBreakTimerDelegate(UpdateBreakTimerImpl));
        }

        private void UpdateBreakTimerImpl()
        {
            seconds++;
            int minutes = seconds / 60;
            int hour = minutes / 60;
            int sec = seconds % 60;

            string time = sec.ToString() + "s";
            if (minutes > 0) { time = minutes.ToString() + "m:" + (sec < 10 ? "0" : "") + time; }
            if (hour > 0) { time = hour.ToString() + "h:" + (minutes < 10 ? "0" : "") + time; }

            btnGood.Text = time + "   " + AppConfig.StopBreak;
        }

        public void UpdateBreakTimer()
        {
            new Thread(new ThreadStart(UpdateBreakTimerThread)).Start();
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
            if (breakTimer != null)
                breakTimer.Enabled = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.Hide();
        }

        public void Restore()
        {
            new Thread(new ThreadStart(RestoreThread)).Start();
        }

        private delegate void UpdateActivityDelegate();
        private void UpdateActivityThread()
        {
            this.Invoke(new UpdateActivityDelegate(UpdateActivityImpl));
        }

        private void UpdateActivityImpl()
        {
            menuItemActivity.Text = AppConfig.Activity + appData.ActivityPercent;
        }

        public void UpdateActivity()
        {
            new Thread(new ThreadStart(UpdateActivityThread)).Start();
        }

        private System.Timers.Timer fadeTimer;
        private System.Timers.Timer breakTimer;
        private int seconds;

        private void MainForm_Load(object sender, EventArgs e)
        {
            appData = AppData.GetInstance();
            SensocolSocket.GetInstance();
            FormState.SetForm(this);

            ActivityTracker.Start();

            Application.ApplicationExit += new EventHandler(this.application_Exit);

            if (!appData.SettingClicked)
            {
                notifyIcon.ShowBalloonTip(20000, AppConfig.WelcomeTitle, AppConfig.WelcomeContent, ToolTipIcon.Info);
                notifyIcon.BalloonTipClicked += new EventHandler(this.menuItemSetting_Click);
            }

            // RegistryKey add = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            // add.SetValue("rsi", "\"" + Application.ExecutablePath.ToString() + "\"");

            menuItemActivity.Text = AppConfig.Activity;

            FormState.GetInstance().Restore();
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

        private void breakTimer_Tick(object sender, ElapsedEventArgs e)
        {
            UpdateBreakTimer();
        }

        private System.Timers.Timer getLoginTokenTimer;
        private void menuItemSetting_Click(object sender, EventArgs e)
        {
            if (!appData.SettingClicked)
            {
                appData.ClickSetting();
            }

            string url = appData.SettingUrl;
            if (url == null || url.Equals(""))
                url = AppConfig.SettingsUrl + "?connect_to_sensor=" + appData.SensorUUID;
            Process.Start(url);

            getLoginTokenTimer = new System.Timers.Timer();
            getLoginTokenTimer.AutoReset = false;
            getLoginTokenTimer.Interval = 10000;
            getLoginTokenTimer.Elapsed += new ElapsedEventHandler(this.getSettingTimer_Tick);
            getLoginTokenTimer.Enabled = true;
        }

        private void getSettingTimer_Tick(object sender, ElapsedEventArgs e)
        {
            SensocolSocket.GetInstance().RequestLoginTokenFor(AppConfig.SettingsUrl, 1);
        }

        private void menuItemView_Click(object sender, EventArgs e)
        {
            if (!appData.SettingClicked)
            {
                appData.ClickSetting();
            }

            string url = appData.ChartUrl;
            if (url == null || url.Equals(""))
                url = AppConfig.ViewAnalysisUrl + "?connect_to_sensor=" + appData.SensorUUID;
            Process.Start(url);

            getLoginTokenTimer = new System.Timers.Timer();
            getLoginTokenTimer.AutoReset = false;
            getLoginTokenTimer.Interval = 10000;
            getLoginTokenTimer.Elapsed += new ElapsedEventHandler(this.getChartTimer_Tick);
            getLoginTokenTimer.Enabled = true;
        }

        private void getChartTimer_Tick(object sender, ElapsedEventArgs e)
        {
            SensocolSocket.GetInstance().RequestLoginTokenFor(AppConfig.ViewAnalysisUrl, 2);
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
