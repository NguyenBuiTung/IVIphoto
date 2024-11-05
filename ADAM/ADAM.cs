using Advantech.Adam;
using TakeimgIVI.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace TakeimgIVI
{
    public class ADAM6060
    {
        private bool m_bStart;
        private AdamSocket adamModbus;
        private Adam6000Type m_Adam6000Type;
        private int retry = 0;
        private bool beginretry = false;
        private Thread Check;

        static bool waitset = false;
        static bool waitread = false;

        public bool[] bDiData = new bool[6];
        public bool[] bDoData = new bool[6];

        public ADAM6060()
        {
            adamModbus = new AdamSocket();
            adamModbus.SetTimeout(1000, 1000, 1000); // set timeout for TCP
            new Thread(() => CheckRetry()) { IsBackground = true }.Start();
        }

        private void CheckRetry()
        {
            while (true)
            {
                Thread.Sleep(500);
                if (beginretry && retry < 5)
                {
                    if(Connect()) beginretry = false;
                    retry++;
                }
                else
                {
                    if (!beginretry && retry >= 5)
                    {
                        Status.ADAM = false;
                        Warning.Show(ERRORCODE.DIS_ADAM);
                    }
                    retry = 0;
                    beginretry = false;
                }
            }
        }

        public bool Connect()
        {
            bool reslut = adamModbus.Connect(Function.Constants.ADAMIP, ProtocolType.Tcp, Function.Constants.ADAMPort);
            if (reslut)
            {
                if (Check != null && Check.IsAlive)
                {
                    Check.Abort();
                }
                Check = new Thread(() =>
                {
                    Status.ADAM = true;
                    while (Status.ADAM)
                    {
                        while (ReadAll()) { Thread.Sleep(200); }
                        beginretry = true;
                    }
                })
                { IsBackground = true };
                Check.Start();
                DelegateToUI.PushToListBox("ADMAM[]: Connected!", LogTitle.Info);
            }
            else
            {
                DelegateToUI.PushToListBox("ADMAM[]: Connection Failed!", LogTitle.Info);
            }
            return reslut;
        }

        public void Disconnect()
        {
            if (Check != null && Check.IsAlive)
            {
                Check.Abort();
            }
            adamModbus.Disconnect();
            DelegateToUI.PushToListBox("ADMAM[]: Disconnected!", LogTitle.Info);
        }

        public bool ReadAll()
        {
            if (!waitset)
            {
                waitread = true;
                bool[] dodata;
                bool[] didata;

                bool check = adamModbus.Modbus().ReadCoilStatus(1, 6, out didata);
                bool check2 = adamModbus.Modbus().ReadCoilStatus(17, 6, out dodata);


                if (check)
                {
                    if (didata != null && dodata != null)
                    {
                        bDiData = didata;
                        bDoData = dodata;
                    }
                }
                waitread = false;
                return check && check2;
            }
            else { return true; }
        }

        public bool SetOutput(int DO_id, int Value)
        {
            while (waitread) { Thread.Sleep(10); }
            waitset = true;
            DO_id += 17;
            bool reslut = adamModbus.Modbus().ForceSingleCoil(DO_id, Value);
            waitset = false;
            return reslut;
        }

        public bool ADAMConnected()
        {
            return adamModbus.Connected;
        }

        public void SetColorLight(DO DI_id)
        {
            if (Status.ADAM)
            {
                //Reset Color
                for (int i = 0; i < 4; i++)
                {
                    SetOutput(i, 0);
                }

                //Set Color
                SetOutput((int)DI_id, 1);
                if(DI_id == DO.LIGHTRED) { SetOutput((int)DO.BUZZER, 1); }
                else { SetOutput((int)DO.BUZZER, 0); }

            }
            else DelegateToUI.PushToListBox("Check ADAM Connection");
        }

    }

    public enum DI 
    {
        SETCHECK,
        SETDETECT1,
        SETDETECT2,
        NG_GMES,
        OK_GMES
    }

    public enum DO 
    {
        LIGHTRED,
        LIGHTYELLOW,
        LIGHTGREEN,
        BUZZER
    }
}
