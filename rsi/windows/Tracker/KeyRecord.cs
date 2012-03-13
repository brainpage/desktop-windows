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
            this.Opacity = 0.4;
            this.Visible = true;

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            SetWinFullScreen(this.Handle);

            fadeTimer = new System.Timers.Timer();
            fadeTimer.Interval = 50;
            fadeTimer.Elapsed += new ElapsedEventHandler(this.timerFade_Tick);
            fadeTimer.Enabled = true;

            btnStop.Visible = true;
            btnStop.Top = this.ClientSize.Height / 2 - btnStop.Height;
            btnStop.Left = this.ClientSize.Width / 2 + 50;
            btnLater.Visible = true;
            btnLater.Top = this.ClientSize.Height / 2 - btnLater.Height;
            btnLater.Left = this.ClientSize.Width / 2 - btnLater.Width - 50;

            pbClock.Visible = false;
            lblTimeLeft.Visible = false;
            btnGood.Visible = false;

            switchButtonTimer = new System.Timers.Timer();
            switchButtonTimer.AutoReset = false;
            switchButtonTimer.Interval = 5000;
            switchButtonTimer.Elapsed += new ElapsedEventHandler(this.switchButtonTimer_Tick);
            switchButtonTimer.Start();
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

        private delegate void RestoreDelegate();
        private void RestoreThread()
        {
            this.Invoke(new RestoreDelegate(RestoreImpl));
        }

        private void RestoreImpl()
        {
            if (fadeTimer != null)
                fadeTimer.Enabled = false;
            if (switchButtonTimer != null)
                switchButtonTimer.Enabled = false;
            if (updateTimeTimer != null)
                updateTimeTimer.Enabled = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
        }

        public void Restore()
        {
            new Thread(new ThreadStart(RestoreThread)).Start();
        }

        private delegate void SwitchButtonDelegate();
        private void SwitchButtonThread()
        {
            this.Invoke(new SwitchButtonDelegate(SwitchButtonImpl));
        }

        private void SwitchButtonImpl()
        {
            btnStop.Visible = false;
            btnLater.Visible = false;

            pbClock.Visible = true;
            lblTimeLeft.Visible = true;
            btnGood.Visible = true;

            btnGood.Top = this.ClientSize.Height / 2 - btnGood.Height;
            btnGood.Left = this.ClientSize.Width / 2 - btnGood.Width / 2;
            pbClock.Top = btnGood.Top - btnGood.Height;
            pbClock.Left = btnGood.Left - pbClock.Width;
            lblTimeLeft.Top = pbClock.Top;
            lblTimeLeft.Left = pbClock.Left + pbClock.Width;

            lockTime = FormState.GetInstance().lockTime;
            lblTimeLeft.Text = Utils.FormatTime(lockTime);

            updateTimeTimer = new System.Timers.Timer();
            updateTimeTimer.Interval = 1000;
            updateTimeTimer.Elapsed += new ElapsedEventHandler(this.updateTimeTimer_Tick);
            updateTimeTimer.Start();
        }

        public void SwitchButton()
        {
            new Thread(new ThreadStart(SwitchButtonThread)).Start();
        }

        private delegate void UpdateTimeDelegate();
        private void UpdateTimeThread()
        {
            this.Invoke(new UpdateTimeDelegate(UpdateTimeImpl));
        }

        private void UpdateTimeImpl()
        {
            lockTime--;
            lblTimeLeft.Text = Utils.FormatTime(lockTime);

            if (lockTime == 0)
            {
                FormState.GetInstance().Restore("rest_over");
            }
        }

        public void UpdateTime()
        {
            new Thread(new ThreadStart(UpdateTimeThread)).Start();
        }

        private System.Timers.Timer fadeTimer;
        private System.Timers.Timer switchButtonTimer;

        private int lockTime;
        private System.Timers.Timer updateTimeTimer;

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

            RegistryKey add = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            add.SetValue("rsi", "\"" + Application.ExecutablePath.ToString() + "\"");
        }

        private int mState = 1;
        private double mOpacity;
        private void timerFade_Tick(object sender, ElapsedEventArgs e)
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

        private void switchButtonTimer_Tick(object sender, ElapsedEventArgs e)
        {
            SwitchButton();
        }

        private void updateTimeTimer_Tick(object sender, ElapsedEventArgs e)
        {
            UpdateTime();
        }

        public void PopupNotification(int warn)
        {
            notifyIcon.ShowBalloonTip(20000, AppConfig.TipTitle, AppConfig.TipContent, ToolTipIcon.Info);
            notifyIcon.BalloonTipClicked += new EventHandler(this.balloonTip_Click);
        }

        private void balloonTip_Click(object sender, EventArgs e)
        {
            FormState.GetInstance().Restore("ignore");
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
                url = AppConfig.ServerUrl + "?sensor_uuid=" + appData.SensorUUID;
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
            FormState.GetInstance().Restore("rest_over");
        }

        private void btnLater_Click(object sender, EventArgs e)
        {
            FormState.GetInstance().Restore("later");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            FormState.GetInstance().Restore("bother");
        }
    }
}
