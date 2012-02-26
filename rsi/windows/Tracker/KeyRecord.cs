using System;
using System.Windows.Forms;

namespace Tracker
{
    public partial class KeyRecord : Form
    {
        public KeyRecord()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Activity.GetInstance();
            FormState.SetForm(this);

            ActivityTracker.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Process.Start("http://192.168.96.175:3000/users/sign_in");

            Activity act = Activity.GetInstance();
            // act.Save();

            label1.Text = act.BreakLength.ToString();

            // formState = new FormState(this);
            //formState.Maximize();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormState.GetInstance().Restore();
        }

        private void KeyRecord_Load(object sender, EventArgs e)
        {

        }
    }
}
