using RSI.Function;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RSI.Camera;
using RSI.Scan;
using System.Runtime.CompilerServices;
using System.Drawing;
using Advantech.Adam;
using System.Timers;
using System.Windows.Forms;

namespace RSI.ADAM
{
    public class ADAMController
    {
        public AdamSocket ADAMSocket;
        public Adam6000Type ADAM6052 = Adam6000Type.Adam6052;
        public IPAddress IP;
        public EndPoint EndPoint;
        public int Port;
        public int Timeout;
        public int DI, DO, DIO;

        public static bool SS_InJig;
        public static bool SS_PistolMin;
        public static bool SS_PistolMax;
        public static bool SS_DoorOpening;
        public static bool SS_ButtonClick;
        public static bool SS_LightBar;

        public static bool IsMovingIn;
        public static bool IsMovingOut;
        public static bool IsInProgress;
        public static bool IsStart;
        public static bool IgnoreDoor;

        public PROCESS Process;

        public Thread DataThread;
        public Thread ReceiveThread;
        public System.Timers.Timer ReadThread;
        public Thread TriggerThread;
        public bool NewDataReceive;
        public string NewData;
        public static System.Timers.Timer timer;

        public ADAMController()
        {
            NewData = "";
            NewDataReceive = false;
            DI = 8;
            DO = 8;
            DIO = DI + DO;
        }

        public void CheckData()
        {
            if (DataThread != null && DataThread.IsAlive)
            {
                DataThread.Abort();
            }

            DataThread = new Thread(ExecuteProcess);
            DataThread.IsBackground = true;
            DataThread.Start();
        }

        public void RefreshParameter()
        {

        }

        private void ExecuteProcess()
        {
            //while (Status.Camera && Status.Scan && Status.ADAM)
            while (IsConnected())
            {
                if(Status.Camera && (Status.State == STATE.AUTO || Status.State == STATE.SEMIAUTO))
                {
                    //if (NewDataReceive && (Status.State == STATE.AUTO || Status.State == STATE.SEMIAUTO))
                    {
                        if (!SS_DoorOpening || IgnoreDoor)
                        {
                            if (!SS_InJig && SS_PistolMax)
                            {
                                if (Process != PROCESS.READY)
                                {
                                    CalculateTaktTime();
                                }
                                Process = PROCESS.READY;
                                DelegateToUI.UIUpdateProcessLabel("Ready", Color.Azure);
                            }
                            else
                            {
                                if (Process == PROCESS.START && !SS_PistolMax && (Status.State == STATE.AUTO || (Status.State == STATE.SEMIAUTO && SS_ButtonClick)))
                                {
                                    Process = PROCESS.MOVING_IN;
                                    DelegateToUI.UIUpdateProcessLabel("Moving In", Color.Azure);
                                    Constants.TaktTime = DateTime.Now;
                                }
                                else if (Process == PROCESS.TRIGGER && !SS_PistolMin && (Status.State == STATE.AUTO || (Status.State == STATE.SEMIAUTO && SS_ButtonClick)))
                                {
                                    Process = PROCESS.MOVING_OUT;
                                }
                                else
                                {
                                    if (SS_PistolMax && Process == PROCESS.READY)
                                    {
                                        Process = PROCESS.START;
                                        DelegateToUI.UIUpdateProcessLabel("Start", Color.Azure);
                                        StartTaktTime();
                                    }
                                    else if (SS_PistolMin && Process == PROCESS.MOVING_IN)
                                    {
                                        Process = PROCESS.TRIGGER;
                                        DelegateToUI.UIUpdateProcessLabel("Trigger", Color.Azure);
                                    }
                                }
                            }
                            if (Process == PROCESS.START)
                            {
                                START();
                            }
                            else if (Process == PROCESS.MOVING_IN)
                            {
                                MOVINGIN();
                            }
                            else if (Process == PROCESS.TRIGGER)
                            {
                                TRIGGER();
                            }
                            else if (Process == PROCESS.MOVING_OUT)
                            {
                                MOVINGOUT();
                            }
                            else if (Process == PROCESS.READY)
                            {
                                WAITING();
                            }
                        }
                        else
                        {
                            Warning.Show(ERRORCODE.DOOR_OPEN);
                        }
                        NewDataReceive = false;
                    }
                }
                Thread.Sleep(20);
            }
        }

        public void START()
        {
            if (Status.State == STATE.AUTO)
            {
                Thread.Sleep(Convert.ToInt32(Constants.DelayTime));
                SS_PistolMax = SetDI(SENSOR.PISTOL_MAX, true);
            }
            if (SS_PistolMax)
            {
                if (Status.State == STATE.AUTO || (Status.State == STATE.SEMIAUTO && SS_ButtonClick))
                {
                    SS_PistolMax = SetDI(SENSOR.PISTOL_MAX, false);
                    // gui lenh di vao
                }
            }
        }

        public void MOVINGIN()
        {
            IsMovingIn = true;
            Thread.Sleep(Convert.ToInt32(Constants.DelayTime));
            SS_PistolMin = SetDI(SENSOR.PISTOL_MIN, true);
            if (SS_PistolMin)
            {
                IsMovingIn = false;
            }
        }

        public void TRIGGER()
        {
            if (!IsInProgress)
            {
                IsInProgress = true;
                Scanner.WaitingBarcode = true;
                CameraCIC.WaitingImage = true;

                Thread t = new Thread(() =>
                {
                    FormMain.Camera.TriggerCam();
                    //bool res = FormMain.Scan.TriggerBarcodeTimes();
                    //if (!res)
                    //{
                    //    Warning.Show(ERRORCODE.NO_BARCODE);
                    //}
                });
                t.Start();
                DateTime Start = DateTime.Now;
                while (Scanner.WaitingBarcode || CameraCIC.WaitingImage)
                {
                    if (DateTime.Now.Subtract(Start).TotalSeconds >= 4)
                    {
                        if (Scanner.WaitingBarcode)
                        {
                            Constants.Barcode = "???";
                        }
                        break;
                    }
                }
                if (!Scanner.WaitingBarcode && !CameraCIC.WaitingImage)
                {
                    DelegateToUI.UIUpdateProcessLabel("OK", Color.Azure);
                }
                else
                {
                    DelegateToUI.UIUpdateProcessLabel("NG", Color.Black, "Red");
                }
                if (!CameraCIC.WaitingImage)
                {
                    FormMain.Camera.DrawToImage();
                    DelegateToUI.UISaveImage();
                    DelegateToUI.UIUpdatePicture();
                    if (Status.State == STATE.AUTO || Status.State == STATE.SEMIAUTO)
                    {
                        DelegateToUI.UIUpdateProductCount();
                    }
                }
                else
                {
                    Warning.Show(ERRORCODE.NO_IMAGE);
                    CameraCIC.WaitingImage = false;
                }
                Constants.RefreshParameter();

                if (Scanner.WaitingBarcode)
                {
                    
                    Scanner.WaitingBarcode = false;
                }

                if (Status.State == STATE.AUTO)
                {
                    Thread.Sleep(Convert.ToInt32(Constants.DelayTime));
                }
                SS_PistolMin = SetDI(SENSOR.PISTOL_MIN, false);
            }
        }

        public void MOVINGOUT()
        {
            IsInProgress = false;
            IsMovingOut = true;
            if (Status.State == STATE.AUTO)
            {
                Thread.Sleep(Convert.ToInt32(Constants.DelayTime));
            }
            SS_PistolMax = SetDI(SENSOR.PISTOL_MAX, true);
            //SS_InJig = false;
        }

        public void WAITING()
        {
            IsMovingOut = false;
            IsMovingIn = false;
            IsInProgress = false;
            //SS_PistolMax = true;
        }

        public void StartTaktTime()
        {
            Constants.TaktTime = DateTime.Now;
        }

        public void CalculateTaktTime()
        {
            DelegateToUI.UIUpdateProcessTime(DateTime.Now.Subtract(Constants.TaktTime).TotalSeconds);
        }

        public string GetProcess()
        {
            return Enum.GetName(typeof(PROCESS), Process);
        }

        public bool IsConnected()
        {
            if (ADAMSocket != null)
            {
                return ADAMSocket.Connected;
            }
            return false;
        }

        public void CheckConnection()
        {
            if (ADAMSocket != null)
            {
                Status.ADAM = ADAMSocket.Connected;
            }
            else
            {
                Status.ADAM = false;
            }
        }

        public void DisconnectSocket()
        {
            try
            {
                Dispose();
                if (Status.ADAM || ADAMSocket != null)
                {
                    ADAMSocket.Disconnect();
                    Status.ADAM = false;
                    DelegateToUI.PushToListBox("Disconnect to ADAM ");
                }
            }
            catch (Exception e)
            {
                Log.LogEvent(e.Message, 3, LogTitle.Error);
            }
        }

        public void OpenSocket()
        {
            if (ADAMSocket != null)
            {
                ADAMSocket.Disconnect();
            }
            try
            {
                ADAMSocket = new AdamSocket(AdamType.Adam6000);
                ADAMSocket.SetTimeout(1000, 1000, 1000);
                Status.ADAM = ADAMSocket.Connect(Constants.ADAM_IP, ProtocolType.Tcp, Convert.ToInt32(Constants.ADAM_Port));
            }
            catch (SocketException ex)
            {
                Log.LogEvent(ex.Message, 3, LogTitle.Error);
                Status.ADAM = false;
            }
            if (!ADAMSocket.Connected)
            {
                DelegateToUI.PushToListBox("Connect to ADAM Failed !", LogTitle.Error);
                if (!(ReceiveThread is null))
                {
                    if (ReceiveThread.IsAlive)
                    {
                        ReceiveThread.Abort();
                    }
                }
            }
            else
            {
                DelegateToUI.PushToListBox("Connect to ADAM Succesfully !");
                CheckData();
                ReadThread = new System.Timers.Timer();
                ReadThread.Interval = 1000;
                ReadThread.Elapsed += ReadSignalsFromServer;
                ReadThread.Enabled = true;
                ReadThread.Start();
            }
        }

        public void ReceiveFromServer()
        {
            byte[] buffer = new byte[8000];
            int len = buffer.Length;
            string str = null;
            while (true)
            {
                try
                {
                    if (ADAMSocket.Connected)
                    {
                        if (ADAMSocket.Receive(buffer, out len))
                        {
                            str = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                            if (str != "")
                            {
                                NewDataReceive = true;
                                DelegateToUI.PushToListBox(str);
                                int index = 0;
                            }
                        }
                        //Stop_TimeOut_Timer();
                    }
                    else
                    {
                        Status.ADAM = false;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void ReadSignalsFromServer(object obj, ElapsedEventArgs e)
        {
            if (ADAMSocket.Connected)
            {
                ReadThread.Enabled = true;
                RefreshDIO();
                ReadThread.Enabled = true;
            }
        }

        private void RefreshDIO()
        {
            int iDiStart = 1, iDoStart = 17;
            int iChTotal;
            bool[] bDiData, bDoData, bData;

            if (ADAMSocket.Modbus().ReadCoilStatus(iDiStart, DI, out bDiData) && ADAMSocket.Modbus().ReadCoilStatus(iDoStart, DO, out bDoData))
            {
                iChTotal = DIO;
                bData = new bool[iChTotal];
                Array.Copy(bDiData, 0, bData, 0, DI);
                Array.Copy(bDoData, 0, bData, DI, DO);

                SS_InJig = bData[0];
                SS_ButtonClick = bData[1];
                SS_PistolMax = bData[2];
                SS_PistolMin = bData[3];
                SS_DoorOpening = bData[4];
                SS_LightBar = bData[5];

                //if (iChTotal > 5)
                //    txtCh5.Text = bData[5].ToString();
                //if (iChTotal > 6)
                //    txtCh6.Text = bData[6].ToString();
                //if (iChTotal > 7)
                //    txtCh7.Text = bData[7].ToString();
                //if (iChTotal > 8)
                //    txtCh8.Text = bData[8].ToString();
                //if (iChTotal > 9)
                //    txtCh9.Text = bData[9].ToString();
                //if (iChTotal > 10)
                //    txtCh10.Text = bData[10].ToString();
                //if (iChTotal > 11)
                //    txtCh11.Text = bData[11].ToString();
                //if (iChTotal > 12)
                //    txtCh12.Text = bData[12].ToString();
                //if (iChTotal > 13)
                //    txtCh13.Text = bData[13].ToString();
                //if (iChTotal > 14)
                //    txtCh14.Text = bData[14].ToString();
                //if (iChTotal > 15)
                //    txtCh15.Text = bData[15].ToString();
                //if (iChTotal > 16)
                //    txtCh16.Text = bData[16].ToString();
                //if (iChTotal > 17)
                //    txtCh17.Text = bData[17].ToString();
            }
            else
            {
                //txtCh0.Text = "Fail";
                //txtCh1.Text = "Fail";
                //txtCh2.Text = "Fail";
                //txtCh3.Text = "Fail";
                //txtCh4.Text = "Fail";
                //txtCh5.Text = "Fail";
                //txtCh6.Text = "Fail";
                //txtCh7.Text = "Fail";
                //txtCh8.Text = "Fail";
                //txtCh9.Text = "Fail";
                //txtCh10.Text = "Fail";
                //txtCh11.Text = "Fail";
                //txtCh12.Text = "Fail";
                //txtCh13.Text = "Fail";
                //txtCh14.Text = "Fail";
                //txtCh15.Text = "Fail";
                //txtCh16.Text = "Fail";
                //txtCh17.Text = "Fail";
            }
            //}

            System.GC.Collect();
        }

        private bool SetDI(SENSOR ss, bool value)
        {
            int iStart = 17 + (int)ss - DIO;
            ReadThread.Enabled = false;
            
            if (ADAMSocket.Modbus().ForceSingleCoil(iStart, value))
                RefreshDIO();
            else
                MessageBox.Show("Set digital output failed!", "Error");
            ReadThread.Enabled = true;
            return value;
        }

        private void TimeOutSocket()
        {
            timer = new System.Timers.Timer();
            timer.Interval = Convert.ToInt32(Constants.ADAM_Timeout);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = false;
        }

        public void Start_TimeOut_Timer()
        {
            timer.Enabled = true; timer.Start();
        }

        public void Stop_TimeOut_Timer()
        {
            timer.Enabled = false; timer.Stop();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Stop_TimeOut_Timer();
            DelegateToUI.PushToListBox("Timeout socket");
            Warning.Show("Time Out Socket");
        }

        public void Dispose()
        {
            if (ReceiveThread != null && ReceiveThread.IsAlive)
            {
                ReceiveThread.Abort();
            }
            if (ReceiveThread != null && DataThread.IsAlive)
            {
                DataThread.Abort();
            }
        }
    }
}
public enum PROCESS
{
    READY,
    MOVING_IN,
    MOVING_OUT,
    TRIGGER,
    START,
    NONE
}

public enum SENSOR
{
    IN_JIG,
    BUTTON_CLICK,
    PISTOL_MAX,
    PISTOL_MIN,
    DOOR_OPENING,
    LIGHT_BAR,
    NONE
}