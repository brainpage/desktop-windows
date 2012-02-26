using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Net;

namespace Tracker
{
    [Serializable]
    class Activity
    {
        public int Duration { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; private set; }

        private static Activity actInstance = null;
        public static Activity GetInstance()
        {
            if (actInstance == null)
            {
                actInstance = Read();
                actInstance.IpAddress = GetPublicIP();
            }
            return actInstance;
        }

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
