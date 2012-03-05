using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Net;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Tracker
{
    [Serializable]
    class Activity
    {
        public string IpAddress { get; private set; }

        public string SensorUUID { get; private set; }
        public string AuthToken { get; private set; }
        public List<Dictionary<string, Object>> EventQueue { get; private set; }

        private static Activity actInstance = null;
        public static Activity GetInstance()
        {
            if (actInstance == null)
            {
                actInstance = Read();
                actInstance.SetValues();
            }
            return actInstance;
        }

        // Save encrypted data in local file.
        public bool Save()
        {
            try
            {
                using (FileStream mStream = File.Open(GetDataFile(), FileMode.Create))
                {
                    var crypt = new TripleDESCryptoServiceProvider();

                    crypt.IV = iv;
                    crypt.Key = key;
                    crypt.Padding = PaddingMode.Zeros;

                    using (var cStream = new CryptoStream(
                        mStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(cStream, this);

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

        private Activity()
        {
        }

        // Initilize values
        private void SetValues()
        {
            if (this.SensorUUID == null)
                this.SensorUUID = Sha1Encrypt(GetMacAddress());

            if (this.AuthToken == null)
                this.AuthToken = Sha1Encrypt(Guid.NewGuid().ToString());

            if (this.EventQueue == null)
                this.EventQueue = new List<Dictionary<string, object>>();

            this.Save();
        }

        private string GetMacAddress()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            if ("".Equals(macAddresses))
                macAddresses = Guid.NewGuid().ToString();

            return macAddresses;
        }


        private string Sha1Encrypt(string plain)
        {
            string key = plain + AppConfig.SecretKey;

            byte[] bytes = System.Text.Encoding.Default.GetBytes(key);
            SHA1 sha = new SHA1CryptoServiceProvider();

            bytes = sha.ComputeHash(bytes);
            string str = "";
            foreach (byte i in bytes)
            {
                str += i.ToString("x2");
            }
            return str;
        }

        private static byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        private static byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7 };

        private static Activity Read()
        {
            Activity activity = new Activity();
            try
            {
                using (FileStream cStream = File.Open(GetDataFile(), FileMode.Open, FileAccess.Read))
                {
                    var crypt = new TripleDESCryptoServiceProvider();

                    crypt.IV = iv;
                    crypt.Key = key;
                    crypt.Padding = PaddingMode.Zeros;

                    using (var mStream = new CryptoStream(
                        cStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        var bFormatter = new BinaryFormatter();
                        activity = (Activity)bFormatter.Deserialize(mStream);

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

        private static string GetDataFile()
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(new string[] { dir, "data" });
        }

        private static string GetPublicIP()
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
    }
}
