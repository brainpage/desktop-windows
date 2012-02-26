using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WebSocket4Net;
using System.Net;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Tracker
{
   
       public partial class KeyRecord : Form
    {
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();

        [DllImport("user32")]
        private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);

        public KeyRecord()
        {
            InitializeComponent();
        }

        WebSocket websocket;
        JsonWebSocket jsonWebsocket;

        private void Form1_Load(object sender, EventArgs e)
        {

            websocket = new WebSocket("ws://192.168.96.175:8080/");
websocket.Opened += new EventHandler(websocket_Opened);
//websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
websocket.Closed += new EventHandler(websocket_Closed);
websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
websocket.Open();

jsonWebsocket = new JsonWebSocket("ws://192.168.96.175:8080/");
jsonWebsocket.Opened += new EventHandler(json_websocket_Opened);

jsonWebsocket.Open();

         
         //   UserActivityHook choosesc;
         //   choosesc = new UserActivityHook();
         //   choosesc.OnMouseActivity += new MouseEventHandler(choose_OnMouseActivity);
         //   choosesc.KeyDown += new KeyEventHandler(MyKeyDown);
         //   choosesc.KeyPress += new KeyPressEventHandler(MyKeyPress);
         //   choosesc.KeyUp += new KeyEventHandler(MyKeyUp);
        }

        private void json_websocket_Opened(object sender, EventArgs e)
        {
            UserActivity act = new UserActivity();
            act.Duration = 100;
            act.name = "Hello";
            jsonWebsocket.Send("act", act);
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            websocket.Send("Hello World!");
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Console.Write("closed");
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
           // label1.Text = e.Message;
            Console.Write(e.Message);
        }


        private void setTextje()
        {
            Int32 hwnd = 0;
            hwnd = GetForegroundWindow();
            string appProcessName = Process.GetProcessById(GetWindowProcessID(hwnd)).ProcessName;
            string appExePath = Process.GetProcessById(GetWindowProcessID(hwnd)).MainModule.FileName;
            string app =  Process.GetProcessById(GetWindowProcessID(hwnd)).MainWindowTitle;
            string appExeName = appExePath.Substring(appExePath.LastIndexOf(@"\") + 1);
            label4.Text = appProcessName + " | " + appExePath + " | " + appExeName + " | " + app;
        }

        private Int32 GetWindowProcessID(Int32 hwnd)
        {
            Int32 pid = 1;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        public void MyKeyDown(object sender, KeyEventArgs e)
        {
            label1.Text = e.KeyData.ToString();
        }

        public void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            label2.Text = e.KeyChar.ToString();
        }

        public void MyKeyUp(object sender, KeyEventArgs e)
        {
            label3.Text = e.KeyData.ToString();
        }



        private void choose_OnMouseActivity(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 0)
            {
                if ((MouseButtons)(e.Button) == MouseButtons.Left)
                {
                   label1.Text = e.Location.ToString();
                }
                if ((MouseButtons)(e.Button) == MouseButtons.Right)
                {
                    label1.Text = e.Location.ToString();
                }
            }
            //throw new Exception("The method or operation is not implemented.");
            try
            {
                setTextje();
            }
            catch (Exception err)
            {
                label1.Text = err.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // Process.Start("http://192.168.96.175:3000/users/sign_in");
            string file = GetDataFile();
            UserActivity activity = new UserActivity();
            activity.name = DateTime.Now.ToString();
            Save(file, activity);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string file = GetDataFile();
            UserActivity activity = Read(file);
            label1.Text = activity.name;
        }

        private string GetPublicIP()
        {
            String direction = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    direction = stream.ReadToEnd();
                }
            }

            //Search for the ip in the html
            int first = direction.IndexOf("Address: ") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);

            return direction;
        }

        private string GetDataFile(){
          string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
          return Path.Combine( new string[]{dir, "data"});
        }

        private UserActivity Read(String inFileName)
        {
            UserActivity activity = null;
            try
            {

                using (FileStream cStream = File.Open(inFileName, FileMode.Open, FileAccess.Read))
                {

                    var crypt = new TripleDESCryptoServiceProvider();

                    crypt.IV = iv;
                    crypt.Key = key;
                    crypt.Padding = PaddingMode.Zeros;

                    using (var mStream = new CryptoStream(
                        cStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        var bFormatter = new BinaryFormatter();
                        activity = (UserActivity)bFormatter.Deserialize(mStream);

                        mStream.Close();
                        cStream.Close();
                        
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
               
            }
            return activity;
        }

        private bool Save(String inFileName, UserActivity activity)
        {
            try
            {

                using (FileStream mStream = File.Open(inFileName, FileMode.Create))
                {

                    var crypt = new TripleDESCryptoServiceProvider();
                
                    crypt.IV = iv;
                    crypt.Key = key;
                    crypt.Padding = PaddingMode.Zeros;

                    using (var cStream = new CryptoStream(
                        mStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(cStream, activity);

                        cStream.Close();
                        mStream.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
            return true;
        }

        private byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        private byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7 };

    }
}
