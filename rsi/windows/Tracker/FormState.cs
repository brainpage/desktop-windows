using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Tracker
{
    class FormState
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

        private FormWindowState winState;
        private FormBorderStyle brdStyle;
        private bool topMost;

        private Form targetForm;
        private Timer timerFade;

        public FormState(Form form)
        {
            targetForm = form;
            timerFade = new Timer();
            timerFade.Interval = 50;

            timerFade.Tick += new System.EventHandler(this.timerFade_Tick);
        }


        private bool IsMaximized = false;

        public void Maximize()
        {
            if (!IsMaximized)
            {
                timerFade.Enabled = true;
                targetForm.Opacity = 0.3;

                IsMaximized = true;
                Save();

                targetForm.WindowState = FormWindowState.Maximized;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.TopMost = true;
                SetWinFullScreen(targetForm.Handle);
            }
        }

        public void Save()
        {
            winState = targetForm.WindowState;
            brdStyle = targetForm.FormBorderStyle;
            topMost = targetForm.TopMost;
        }

        public void Restore()
        {
            targetForm.WindowState = winState;
            targetForm.FormBorderStyle = brdStyle;
            targetForm.TopMost = topMost;

            IsMaximized = false;
        }

        private int mState = 1;
        private void timerFade_Tick(object sender, EventArgs e)
        {
            double op = targetForm.Opacity + mState * 0.02;
            if (op >= 0.8)
            {
                op = 0.8;
                timerFade.Enabled = false;
                mState = 0;
            }
            targetForm.Opacity = op;
        }

    }
}
