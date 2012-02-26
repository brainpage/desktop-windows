using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace Tracker
{
    public partial class KeyRecord : Form
    {
        public KeyRecord()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActivitySocket.GetInstance().send("hi");
            label1.Text = Activity.GetInstance().IpAddress;
            ActivityTracker.Start();
        }

        FormState formState;
        private void button1_Click(object sender, EventArgs e)
        {
            // Process.Start("http://192.168.96.175:3000/users/sign_in");

            Activity act = Activity.GetInstance();

            act.Name = DateTime.Now.ToString();
            act.Save();

            formState = new FormState(this);
            formState.Maximize();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = Activity.GetInstance().Name;
            formState.Restore();
        }
    }
}
