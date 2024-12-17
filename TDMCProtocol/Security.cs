using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TDMCProtocol
{

    public class Security
    {
        private readonly int _setTime;

        public static bool TrialExpired { get; private set; }

        public event EventHandler TrialEvent;

        public Security(int setTime)
        {
            _setTime = setTime;
        }

        public static string GetDriveSerialNumber()
        {
            return "";
            //ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            //ManagementObject managementObject = new ManagementObject();
            //foreach (ManagementObject item in managementObjectSearcher.Get())
            //{
            //    managementObject = item;
            //}

            //return managementObject["SerialNumber"].ToString();
        }

        public static string Hash(string data, string salt, HashAlgorithm algorithm)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data + salt);
            return BitConverter.ToString(algorithm.ComputeHash(bytes));
        }

        public void Run()
        {
            new Thread(Start).Start();
        }

        private void Start()
        {
            int num = 0;
            bool flag = true;
            while (flag)
            {
                if (num >= _setTime)
                {
                    OnTrialEvent();
                    flag = false;
                    TrialExpired = true;
                }
                else
                {
                    Thread.Sleep(1000);
                    num++;
                }
            }
        }

        private void OnTrialEvent()
        {
            if (this.TrialEvent != null)
            {
                this.TrialEvent(this, EventArgs.Empty);
            }
        }
    }
}
