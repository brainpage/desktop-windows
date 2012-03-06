using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;

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


            Console.Write(Guid.NewGuid().ToString());


            // formState = new FormState(this);
            //formState.Maximize();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormState.GetInstance().Restore();
        }

        private void menuItemSetting_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.ServerUrl + "?sensor_token=" + activity.SensorUUID + "&auth_token=" + activity.AuthToken);
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
