using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Drawing;
using System.Timers;

namespace Tracker
{
    public partial class KeyRecord : Form
    {
        private Activity activity;
        public KeyRecord()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // this.MaximizeBox = false;
            // this.MinimizeBox = false;
            // this.WindowState = FormWindowState.Minimized;

            activity = Activity.GetInstance();
            SensocolSocket.GetInstance();
            FormState.SetForm(this);

            Console.WriteLine(activity.AuthToken);
            Console.WriteLine(activity.SensorUUID.Length);

            ActivityTracker.Start();

            Application.ApplicationExit += new EventHandler(this.application_Exit);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Process.Start("http://192.168.96.175:3000/users/sign_in");

            // Activity act = Activity.GetInstance();
            // act.Save();


            // Console.Write(Guid.NewGuid().ToString());


            // formState = new FormState(this);
            //formState.Maximize();
            
           
        }

        private System.Timers.Timer tipTimer;
        private int time;
        private int warnTime;
        private void sendTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if (time <= warnTime)
            {
                notifyIcon.BalloonTipText = "Your screen will be locked in " + (warnTime - time).ToString() + " seconds for a break. \n Click this to ignore.";
                notifyIcon.BalloonTipTitle = "Take a break now";
                notifyIcon.ShowBalloonTip(1000);
                time++;
            }
            else
            {             
                tipTimer.Enabled = false;
            }
        }

        public void PopupNotification(int warn)
        {
            time = 0;
            warnTime = warn;
            tipTimer = new System.Timers.Timer();
            tipTimer.Interval = 1000;
            tipTimer.Elapsed += new ElapsedEventHandler(this.sendTimer_Tick);
            tipTimer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // FormState.GetInstance().Restore();
        }

        private void menuItemSetting_Click(object sender, EventArgs e)
        {
            string url = activity.LoginUrl;
            if (url == null || url.Equals(""))
                url = AppConfig.ServerUrl + "?sensor_uuid=" + activity.SensorUUID;
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

    }
}
