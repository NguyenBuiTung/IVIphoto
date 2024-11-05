using TakeimgIVI.Function;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LC_Controller;

namespace TakeimgIVI.Scan
{
    public class Scanner
    {
        public Scanner()
        {

        }

        ~Scanner()
        {
            Dispose(false);
        }

        public SerialPort _serialPort;
        public bool status = false;
        public event EventHandler<SerialDataEventArgs> NewSerialDataReceived;
        System.Timers.Timer timer = new System.Timers.Timer();

        bool _delay = true;

        private static string _value;
        public static string Value { get => _value; set => _value = value; }
        public static bool WaitingBarcode = false;

        static int nbrbefor = 0;
        static string _valuebefor = "";
        static Thread Waittingdata;

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = _serialPort.BytesToRead;
            byte[] data = new byte[dataLength];
            int nbrDataRead = _serialPort.Read(data, 0, dataLength) + nbrbefor;
            _value = _valuebefor + Encoding.ASCII.GetString(data);

            string[] arr = Microsoft.VisualBasic.Strings.Split(_value, "\r\n", -1, Microsoft.VisualBasic.CompareMethod.Binary);

            _valuebefor = arr[0];
            nbrbefor = nbrDataRead;

            Waittingdata = new Thread(() => //Waitting data Scanner.
            {
                Thread.Sleep(100);
                _valuebefor = "";
                nbrbefor = 0;

            })
            { IsBackground = true };
            Waittingdata.Start();

            if (nbrDataRead != Constants.Data_Length)
            {
                if(nbrDataRead>10)
                DelegateToUI.PushToListBox("Barcode []: " + _value + $" data lenhth:{nbrDataRead} != {Constants.Data_Length}", LogTitle.Error);
                return;
            }

            DelegateToUI.PushToListBox("Barcode []: " + _value, LogTitle.Recv);

            if (Constants.Barcode == arr[0])
            {
                return;
            }

            GMES_Data.Barcode = arr[0];

            DelegateToUI.UIUpdateBarcodeValue(GMES_Data.Barcode);
            if (Status.GMES)
            {
                GMES_Data.CEID = Constants.GMES_CEID;
                GMES_Data.RPTID = Constants.GMES_RPTID;
                GMES_Pakage.SendBarcodeValue(GMES_Data.Barcode); //GMESTEST
            }

            WaitingBarcode = false;

            if (Status.GMES)
            {
                DateTime Beforcheck = DateTime.Now;
                while (!GMES_Data.Received_Data) //GMESTEST
                {
                    if(DateTime.Now.Subtract(Beforcheck).TotalMilliseconds>int.Parse(Constants.GMES_TimeOut))
                    {
                        GMES_Data.ACK = "1";
                        GMES_Data.REASON = "Timeout Gmes";
                        Status.GMES = false;
                        GMES_Socket.Disconnect_GMES_Socket();
                        GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                        DelegateToUI.update("Close Connection to GMES", LogTitle.Info);
                        DelegateToUI.update("Close Connection to GMES", LogTitle.Gmes);
                        Warning.Show(GMES_Data.REASON);
                        break;
                    }
                    Thread.Sleep(10);
                }
                Thread.Sleep(100);
                if (GMES_Data.ACK != "0")
                {
                    DelegateToUI.update($"GMES force NG: " + GMES_Data.REASON, LogTitle.Error);
                    //DelegateToUI.UpdateProcessBar(100);
                }
            }
            
            if (_delay)
            {
                if (!Status.ByPass)
                {
                    if (NewSerialDataReceived != null)
                    {
                        NewSerialDataReceived(this, new SerialDataEventArgs(arr[0]));
                    }
                }
                Delay();
            }
        }

        public void TriggerBarcode()
        {
            if (Status.Scan)
            {

                DelegateToUI.PushToListBox("Trigger SCAN");
                Constants.Barcode = "";
                char[] c = new char[1];
                c[0] = 'R';
                if (_serialPort.IsOpen)
                {
                    _serialPort.Write(c, 0, 1);
                }
            }
            else
            {
                Warning.Show(ERRORCODE.DIS_SCAN);
            }
            
        }

        // trigger barcode 3 lần
        public void TriggerBarcodeTimes()
        {
            if (Status.Scan)
            {
                if (Status.GMES) GMES_Data.Received_Data = false; //GMESTEST
                WaitingBarcode = true;
                for (int i = 0; i < 3; i++)
                {
                    FormMain.Scan.TriggerBarcode();
                    DateTime Time = DateTime.Now;
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (!WaitingBarcode || DateTime.Now.Subtract(Time).TotalSeconds >= 1)
                        {
                            break;
                        }
                    }
                    if (!WaitingBarcode)
                    {
                        break;
                    }
                }
                if (WaitingBarcode)
                {
                    Warning.Show(ERRORCODE.NO_BARCODE);
                    WaitingBarcode = false;
                }
            }
            else
            {
                Warning.Show(ERRORCODE.DIS_SCAN);
                WaitingBarcode = false;
            }
        }
        // kiểm tra kết nối
        public void CheckOpen()
        {
            Status.Scan = _serialPort.IsOpen;
        }
        // delay
        void Delay()
        {
            _delay = false;
            timer.Interval = 2500;
            timer.Enabled = true;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _delay = true;
        }
        // khởi tạo scan
        public void StartListening()
        {
            try
            {
                // Closing serial port if it is open
                StopListening();
                // Setting serial port settings
                _serialPort = new SerialPort();
                _serialPort.PortName = Constants.Barcode_Port;
                _serialPort.BaudRate = Convert.ToInt32(Constants.Barcode_Baurate);
                _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), Constants.Barcode_Parity);
                _serialPort.DataBits = Convert.ToInt32(Constants.Barcode_DataBits);
                _serialPort.StopBits = (StopBits)Convert.ToInt32(Constants.Barcode_StopBit);
                // Subscribe to event and open serial port for data                                    
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                _serialPort.Open();
                Status.Scan = true;
            }
            catch
            {
                Status.Scan = false;
            }
        }
        // ngắt kết nối
        public void StopListening()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
        // gửi dữ liệu
        public void WriteLine(string s)
        {
            _serialPort.WriteLine(s);
        }
        // kiểm tra kết nối
        public bool CheckConnection()
        {
            if (_serialPort != null && _serialPort.IsOpen == true)
            {
                Status.Scan = true;
            }
            else
                Status.Scan = false;
            return Status.Scan;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serialPort.DataReceived -= new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            }

            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();

                _serialPort.Dispose();
            }
        }
    }

    public class SerialDataEventArgs : EventArgs
    {
        public SerialDataEventArgs(string dataInByteArray)
        {
            Data = dataInByteArray;
        }

        public string Data;
    }
}
