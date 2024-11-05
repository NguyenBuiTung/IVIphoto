using Microsoft.VisualBasic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TakeimgIVI.Function;

namespace TakeimgIVI
{
    public class GMES_Socket
    {
        private static Thread ConnectThread;
        private static Thread RecieveThread;
        public static Socket GMES_Socket_;
        private static IPAddress GMES_IP;
        private static EndPoint GMES_EndPoint;
        public static bool GetInfoOnly;

        public static void Connect_GMES_Socket()
        {
            if (ConnectThread is null || ConnectThread.IsAlive == false)
            {
                ConnectThread = new Thread(new ThreadStart(OpenSocket));
                ConnectThread.IsBackground = true;
                ConnectThread.Start();
            }
            else if (ConnectThread.IsAlive == true && GMES_Socket_.Connected == false)
            {
                ConnectThread.Abort();
                ConnectThread = new Thread(new ThreadStart(OpenSocket));
                ConnectThread.IsBackground = true;
                ConnectThread.Start();
            }
        }

        public static void Disconnect_GMES_Socket()
        {
            try
            {
                if (Status.GMES || GMES_Socket_ != null)
                {
                    GMES_Socket_.Close();
                    ConnectThread.Abort();
                    ConnectThread = null;
                    if (RecieveThread !=null && RecieveThread.IsAlive)
                    {
                        RecieveThread.Abort();
                        RecieveThread = null;
                    }
                    Status.GMES = false;
                }
            }
            catch (Exception ex)
            {
                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                DelegateToUI.update("ERROR: "+ex.Message, LogTitle.Gmes);
            }
        }
        public static void OpenSocket()
        {
            if (GMES_Socket_ != null)
            {
                GMES_Socket_.Close();
            }
            GMES_Socket_ = null;
            GMES_Socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            GMES_IP = IPAddress.Parse(Function.Constants.GMESIP);
            GMES_EndPoint = new IPEndPoint(GMES_IP, Convert.ToInt32(Function.Constants.GMESPORT));
            try
            {
                GMES_Socket_.Connect(GMES_EndPoint);
                Status.GMES = GMES_Socket_.Connected;
            }
            catch (SocketException ex)
            {
                DelegateToUI.update("Connect To GMES Failed", LogTitle.Error);
                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                DelegateToUI.update("[ERROR]" + ex.Message, LogTitle.Gmes);
                Status.GMES = false;
            }
            if (!GMES_Socket_.Connected)
            {
                if (!(RecieveThread is null))
                {
                    if (RecieveThread.IsAlive)
                    {
                        RecieveThread.Abort();
                    }
                }
            }
            else
            {
                RecieveThread = new Thread(new ThreadStart(RecieveFromServer));
                RecieveThread.IsBackground = true;
                RecieveThread.Start();
                TimeOutSocket();
                GMES_Socket_.Blocking = true;
                GMES_Pakage.DocLoad();
                //GMES_Pakage2.Send_GMES_Server(GMES_Data.EAYT);
            }
        }

        private static bool IsConnected
        {
            get
            {
                try
                {
                    if (GMES_Socket_ == null)
                        return false;

#if NETFX_CORE
                    return _mSocket.Connected;
#else
                    return !((GMES_Socket_.Poll(1000, SelectMode.SelectRead) && (GMES_Socket_.Available == 0)) || !GMES_Socket_.Connected);
#endif
                }
                catch { return false; }
            }
        }
        public static void RecieveFromServer()
        {
            string str = null;
            string temp = null;
            byte[] buffer = new byte[0x2001];
            while (true)
            {
                try
                {
                    if (IsConnected)
                    {
                        GMES_Socket_.Receive(buffer, buffer.Length, SocketFlags.None);
                        Stop_TimeOut_Timer();
                        str = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                        //str = str.Substring(str.IndexOf("{"), str.LastIndexOf("}"));
                        Array.Clear(buffer, 0, buffer.Length);
                        if (str != "")
                        {
                            #region Old
                            if (/* str.Contains("VERSION") &&*/ str.Contains("</EIF>"))
                            {
                                string expression = temp + str;
                                temp = "";
                                str = "";
                                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                                DelegateToUI.update("[RECEIVE]: " +expression, LogTitle.Gmes);
                                string[] strArray = Strings.Split(expression, "</EIF>", -1, CompareMethod.Binary);
                                int index = 0;
                                while (true)
                                {
                                    if (strArray[index] != "")
                                    {
                                        if (GMES_Socket_.Connected && Status.GMES)
                                        {
                                            GMES_Pakage.MSG_Send(strArray[index] + "</EIF>");
                                        }
                                        index++;
                                        if (index < 1)
                                        {
                                            continue;
                                        }
                                    }
                                    break;
                                }
                                continue;
                            }
                            else if (str.Contains("VERSION") && !str.Contains("</EIF>"))
                            {
                                temp = Strings.Split(str, "</EIF>", -1, CompareMethod.Binary)[0];
                                str = "";
                                continue;
                            }
                            #endregion
                            GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                            //Log.EventLogging(str, LogPath.SystemPath, LogTitle.Recv);
                            DelegateToUI.update("[RECEIVE]: " + str,LogTitle.Gmes);
                            //Programm.Line.UI.UpdateTestGMES(str, LogTitle.Recv);
                            //GMES_Pakage2.GetData(str);
                            str = "";

                        }
                    }
                    else
                    {
                        Status.GMES = false;
                    }
                }
                catch (Exception ex)
                {
                    GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                    DelegateToUI.update("[ERROR]" + ex.Message, LogTitle.Gmes);
                }
            }
        }
        public static void SendToServer(ref string s)
        {
            GMES_Data.DataSend_2 = s;
            byte[] buffer = new byte[8000];
            buffer = Encoding.ASCII.GetBytes(s);
            if (GMES_Socket_.Connected)
            {
                GMES_Socket_.Send(buffer);
                DelegateToUI.update("[SEND]: " + GMES_Data.DataSend_2,LogTitle.Gmes);
                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
            }
            else
            {
                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                DelegateToUI.update("[ERROR]: Lost Connection to GMES", LogTitle.Gmes);
                Status.GMES = false;
            }
        }
        static System.Timers.Timer timer;
        private static void TimeOutSocket()
        {
            timer = new System.Timers.Timer();
            timer.Interval = Convert.ToInt32(Function.Constants.GMES_TimeOut);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = false;
        }
        public static void Start_TimeOut_Timer()
        {
            //timer.Enabled = true; timer.Start();
        }

        public static void Stop_TimeOut_Timer()
        {
            //timer.Enabled = false; timer.Stop();
        }
        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Stop_TimeOut_Timer();
            GMES_Data.Reason = "Time Out Socket";
            GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
            DelegateToUI.update("[ERROR]: Time Out Socket:[]" + "\t" + GMES_Data.DataSend_2, LogTitle.Gmes);
            Warning.Show("Time Out Socket");
        }
    }
}
