using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
using System.Net;

namespace Brainpage
{
    class Utils
    {
        public static string FormatTime(int seconds)
        {

            TimeSpan t = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        public static Object ReadFile(string fileName)
        {
            Object obj = null;
            try
            {
                using (FileStream cStream = File.Open(GetDataFile(fileName), FileMode.Open, FileAccess.Read))
                {
                    var crypt = new TripleDESCryptoServiceProvider();

                    crypt.IV = iv;
                    crypt.Key = key;
                    crypt.Padding = PaddingMode.Zeros;

                    using (var mStream = new CryptoStream(
                        cStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        var bFormatter = new BinaryFormatter();
                        obj = bFormatter.Deserialize(mStream);

                        mStream.Close();
                        cStream.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
            return obj;
        }

        public static bool WriteFile(string fileName, Object data)
        {

            try
            {
                using (FileStream mStream = File.Open(GetDataFile(fileName), FileMode.Create))
                {
                    var crypt = new TripleDESCryptoServiceProvider();

                    crypt.IV = iv;
                    crypt.Key = key;
                    crypt.Padding = PaddingMode.Zeros;

                    using (var cStream = new CryptoStream(
                        mStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        var bFormatter = new BinaryFormatter();
                        bFormatter.Serialize(cStream, data);

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

        public static string Sha1Encrypt(string key)
        {
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

        public static string GetMacAddress()
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

        public static string GetPublicIP()
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

        public static void Log(string content)
        {
            FileInfo logFile = new FileInfo(GetDataFile("log.txt"));
          
            using (StreamWriter logStream = logFile.AppendText())
                logStream.Write(DateTime.Now.ToLongTimeString() + content + "\r\n");
        }


        private static byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        private static byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7 };

        private static string GetDataFile(string fileName)
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(new string[] { dir, fileName });
        }
    }
}
