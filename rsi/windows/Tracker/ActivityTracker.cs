using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tracker
{
    class ActivityTracker
    {
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        private static ActivityTracker trackerInstance = null;

        public static void Start()
        {
            if (trackerInstance == null)
            {
                trackerInstance = new ActivityTracker();
            }
        }

        public static ActivityTracker GetInstance()
        {
            Start();
            return trackerInstance;
        }

        private ActivityHook hook;

        public Timer workTimer;
        public Timer breakTimer;

        private Stopwatch noActionTime;
        private int bufferTime = 60 * 1000;

        private ActivityTracker()
        {
            hook = new ActivityHook();
            hook.OnMouseActivity += new MouseEventHandler(choose_OnMouseActivity);
            hook.KeyDown += new KeyEventHandler(MyKeyDown);
            hook.KeyPress += new KeyPressEventHandler(MyKeyPress);
            hook.KeyUp += new KeyEventHandler(MyKeyUp);

            workTimer = new Timer();
            workTimer.Interval = Activity.GetInstance().WorkLength;
            workTimer.Enabled = true;
            workTimer.Tick += new System.EventHandler(this.workTimer_Tick);

            breakTimer = new Timer();
            breakTimer.Interval = Activity.GetInstance().BreakLength;
            breakTimer.Tick += new System.EventHandler(this.breakTimer_Tick);

            noActionTime = new Stopwatch();
            noActionTime.Start();
        }

        private bool Resting()
        {
            return noActionTime.ElapsedMilliseconds > bufferTime;
        }

        private void workTimer_Tick(object sender, EventArgs e)
        {
            if (Resting())
            {
                workTimer.Enabled = false;
            }
            else
            {
                FormState.GetInstance().Maximize();
            }
        }

        private void breakTimer_Tick(object sender, EventArgs e)
        {
            Restore();
        }

        private void Restore()
        {
            FormState.GetInstance().Restore();
            noActionTime.Restart();
        }

        private void ActionHappened()
        {
            if (breakTimer.Enabled)
                return;

            if (workTimer.Enabled)
            {
                noActionTime.Restart();
            }
            else if (Resting())
            {
                if (noActionTime.ElapsedMilliseconds > breakTimer.Interval)
                {
                    Restore();
                }
                else
                {
                    breakTimer.Interval -= (int)noActionTime.ElapsedMilliseconds;
                    FormState.GetInstance().Maximize();
                }
            }
        }

        private void setTextje()
        {
            Int32 hwnd = 0;
            hwnd = GetForegroundWindow();
            string appProcessName = Process.GetProcessById(GetWindowProcessID(hwnd)).ProcessName;
            string appExePath = Process.GetProcessById(GetWindowProcessID(hwnd)).MainModule.FileName;
            string app = Process.GetProcessById(GetWindowProcessID(hwnd)).MainWindowTitle;
            string appExeName = appExePath.Substring(appExePath.LastIndexOf(@"\") + 1);

            ActivitySocket.GetInstance().send(appProcessName + " | " + appExePath + " | " + appExeName + " | " + app);
            Console.Write(appProcessName + " | " + appExePath + " | " + appExeName + " | " + app);
        }

        private Int32 GetWindowProcessID(Int32 hwnd)
        {
            Int32 pid = 1;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        private void MyKeyDown(object sender, KeyEventArgs e)
        {
            Console.Write(e.KeyData.ToString());
            ActionHappened();
        }

        private void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            Console.Write(e.KeyChar.ToString());
            ActionHappened();
        }

        private void MyKeyUp(object sender, KeyEventArgs e)
        {
            Console.Write(e.KeyData.ToString());
            ActionHappened();
        }

        private void choose_OnMouseActivity(object sender, MouseEventArgs e)
        {
            ActionHappened();
            if (e.Clicks > 0)
            {
                if ((MouseButtons)(e.Button) == MouseButtons.Left)
                {
                    Console.Write(e.Location.ToString());
                }
                if ((MouseButtons)(e.Button) == MouseButtons.Right)
                {
                    Console.Write(e.Location.ToString());
                }
            }
            //throw new Exception("The method or operation is not implemented.");
            try
            {
                setTextje();
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
            }
        }
    }
}
