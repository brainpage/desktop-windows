using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;

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

        private Stopwatch durClock;
        private Timer durTimer;
        private Dictionary<string, Object> currentEvent;

        private double dst;  //The distance covered by the mouse
        private int mnum; //The number of "mousemove" events
        private int keys; //Number of key presses
        private int msclks; //Number of mouse clicks
        private int scrll; //Scroll "distance" of mouse wheel
        private string app; //Identifier of currently active app

        private ActivityTracker()
        {
            currentEvent = new Dictionary<string, object>();

            hook = new ActivityHook();
            hook.OnMouseActivity += new MouseEventHandler(choose_OnMouseActivity);
            hook.KeyDown += new KeyEventHandler(MyKeyDown);
            hook.KeyPress += new KeyPressEventHandler(MyKeyPress);
            hook.KeyUp += new KeyEventHandler(MyKeyUp);

            durTimer = new Timer();
            durTimer.Interval = AppConfig.EventInterval;
            durTimer.Tick += new System.EventHandler(this.durTimer_Tick);
            durTimer.Enabled = true;

            durClock = new Stopwatch();
            durClock.Start();

            beginNewSample();
        }

        private void Restore()
        {
            FormState.GetInstance().Restore();
            durClock.Restart();
        }

        private void beginNewSample()
        {
            currentEvent = new Dictionary<string, object>();
            durClock.Restart();

            dst = 0;
            mnum = 0;
            keys = 0;
            msclks = 0;
            scrll = 0;
        }


        private void durTimer_Tick(object sender, EventArgs e)
        {
            MouseMoveSave();

            currentEvent.Add("dur", durClock.ElapsedMilliseconds);
            currentEvent.Add("dst", dst);
            currentEvent.Add("keys", keys);
            currentEvent.Add("msclks", msclks);
            currentEvent.Add("scrll", scrll);

            ActivitySocket.GetInstance().send(JsonConvert.SerializeObject(currentEvent));

            beginNewSample();
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

        }

        private void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            keys++;
        }

        private void MyKeyUp(object sender, KeyEventArgs e)
        {

        }

        private int mouseLastX = -1;
        private int mouseLastY = -1;
        private long mouseLastTime = 0;
        private bool mouseLastSaved;

        private void MouseMove(MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;

            if (mouseLastX > 0)
            {
                dst += Math.Sqrt(Math.Pow(x - mouseLastX, 2) + Math.Pow(y - mouseLastY, 2));

                if (durClock.ElapsedMilliseconds - mouseLastTime > 100)
                    MouseMoveSave();
            }

            mouseLastX = x;
            mouseLastY = y;
            mouseLastTime = durClock.ElapsedMilliseconds;
            mouseLastSaved = false;
        }

        private void MouseMoveSave()
        {
            if (!mouseLastSaved)
            {
                currentEvent.Add(mouseLastTime.ToString(), xyStr(mouseLastX, mouseLastY));
                mouseLastSaved = true;
            }
        }

        private void MouseClick(MouseEventArgs e)
        {
            string key = "";
            switch (e.Button)
            {
                case MouseButtons.Left:
                    key = "L";
                    break;
                case MouseButtons.Right:
                    key = "R";
                    break;
                default:
                    key = "M";
                    break;
            }
            msclks++;
            currentEvent.Add(durClock.ElapsedMilliseconds.ToString() + "-" + key, xyStr(e.Location.X, e.Location.Y));
        }

        private string xyStr(int x, int y)
        {
            return x.ToString() + "," + y.ToString();
        }

        private void choose_OnMouseActivity(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 0)
            {
                msclks++;
                MouseClick(e);
            }
            MouseMove(e);

            scrll += Math.Abs(e.Delta);

            try
            {
                // setTextje();
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
            }
        }
    }
}
