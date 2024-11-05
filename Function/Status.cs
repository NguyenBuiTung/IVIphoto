using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace TakeimgIVI.Function
{

    public class Status
    {
        // dùng lại
        public static bool Camera1 = false;
        public static bool Camera2 = false;
        public static bool Scan = false;
        public static bool ADAM = false;
        public static bool Lens1 = false;
        public static bool Lens2 = false;

        public static bool ProgramStart = false;
        public static bool IsAuto = false;
        public static bool IsManual = false;
        public static bool ByPass = false;

        public static bool GMES = false;

        public static STATE State 
        {
            get
            {
                if (ByPass) return STATE.BYPASS;
                if (IsManual) return STATE.MANUAL;
                if (IsAuto) return STATE.AUTO;
                ByPass = true;
                return STATE.BYPASS;
            }

            set
            {
                IsAuto = IsManual = ByPass = false;
                if(value == STATE.BYPASS) { ByPass = true; }
                else if(value == STATE.MANUAL) {  IsManual = true; }
                else if(value == STATE.AUTO) {  IsAuto = true; }
            }
        }

        public static bool ProductIsNG { set; get; }

        public static bool LineMode;
        private static Thread PingThread;

        public static string FormatString(string s)
        {
            return DateTime.Now.ToString("[HH:mm:ss:fff]") + " " + s;
        }

        public static void PingCheck()
        {
            if (PingThread != null)
            {
                PingThread = null;
            }
            PingThread = new Thread(new ThreadStart(Ping));
            PingThread.IsBackground = true;
            PingThread.Start();
        }
        private static void Ping()
        {
            while (true)
            {
                if (!PingHost(Constants.Camera_IP1) && Camera1)
                {
                    Camera1 = false;
                    Warning.Show(ERRORCODE.DIS_CAMERA1);
                }
                else
                {

                }
                if (!PingHost(Constants.Camera_IP2) && Camera2)
                {
                    Camera2 = false;
                    Warning.Show(ERRORCODE.DIS_CAMERA2);

                }
                else
                {

                }
                if (!PingHost(Constants.ADAMIP) && ADAM)
                {
                    ADAM = false;
                    Warning.Show(ERRORCODE.DIS_ADAM);
                }
                else
                {

                }
                Thread.Sleep(1000);
            }
        }
        public static bool PingHost(string IP)
        {
            if (IP == null || IP == "") return false;
            bool pingable = false;
            Ping pinger = null;
            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(IP, 1500);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                pingable = false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            return pingable;
        }

        public static void Dispose()
        {
            if(PingThread != null & PingThread.IsAlive)
            {
                PingThread.Abort();
            }
        }
    }
}
