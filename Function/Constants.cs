using Advantech.Adam;
using TakeimgIVI.Camera;
using TakeimgIVI.Scan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TakeimgIVI.Function
{
	public class Constants
	{
		//khai báo config file path
		public static string Common = Application.StartupPath + "\\Communication\\Common.ini";
        //Scanner
        public static string Barcode_Port;
		public static string Barcode_Baurate;
		public static string Barcode_DataBits;
		public static string Barcode_Parity;
		public static string Barcode_StopBit;
        public static int Data_Length { get => Model.Modelnow != null ? Model.Modelnow.lengthbarcode : 15; }
		//Camera
		public static string Camera_CurrentJob;
		public static string Camera_Link;
		public static string Camera_IP1;
		public static string Camera_IP2;
        public static string Url;
        //ADAM
        public static string ADAMIP;
        public static int ADAMPort;

        //GMES
        public static string GMESIP;
        public static string GMESPORT;
        public static string GMES_TimeOut;
        public static string GMES_CEID;
        public static string GMES_RPTID;

        //Variable
        public static string Total_Count { get; set; }
        public static string Barcode = "";
        public static string DelayTime = "5";
        public static DateTime TaktTime;
        public static float Timeout = 6f;

        // đọc ini từ file
        public static void ReadCommon()
		{
			Barcode_Port = INI.ReadIni(Common, "Barcode", "Port", "");
			Barcode_Baurate = INI.ReadIni(Common, "Barcode", "Baudrate", "");
			Barcode_DataBits = INI.ReadIni(Common, "Barcode", "DataBits", "");
			Barcode_Parity = INI.ReadIni(Common, "Barcode", "Parity", "");
			Barcode_StopBit = INI.ReadIni(Common, "Barcode", "StopBit", "");

            Camera_IP1 = INI.ReadIni(Common, "Camera", "IP1", "");
            Camera_IP2 = INI.ReadIni(Common, "Camera", "IP2", "");
            try { Processimage.Colortxt = new SolidBrush(Color.FromArgb(int.Parse(INI.ReadIni(Common, "Camera", "Colortext", "LimeGreen")))); }
            catch { Processimage.Colortxt = Brushes.LimeGreen; }

            ADAMIP = INI.ReadIni(Common, "ADAM", "IP", "");
            if (!int.TryParse(INI.ReadIni(Common, "ADAM", "PORT", ""), out ADAMPort))
                ADAMPort = 0;

            Total_Count = INI.ReadIni(Common, "Product", "Count");
            DelayTime = INI.ReadIni(Common, "Product", "DelayTime");
            Url = INI.ReadIni(Common, "Url", "ListError", Application.StartupPath + @"\Communication\Errorlist.xlsx");
            TaktTime = DateTime.Now;

            Model.Readmodelinto();

            GMESIP = INI.ReadIni(Common, "Gmes", "IP");
            GMESPORT = INI.ReadIni(Common, "Gmes", "PORT");
            GMES_TimeOut = INI.ReadIni(Common, "Gmes", "Timeout");
            GMES_CEID = INI.ReadIni(Common, "Gmes", "CEID", "20100");
            GMES_RPTID = INI.ReadIni(Common, "Gmes", "RPTID", "20101");
        }

        public static void RefreshParameter()
        {
            Barcode = string.Empty;
            Scanner.WaitingBarcode = false;
            CameraCIC.WaitingImage = false;
            CameraCIC2.WaitingImage = false;
            
            //MXProtocol.IgnoreTrigger = false;
        }
    }

    public enum LogTitle
    {
        Info = 1,
        Send,
        Recv,
        Error,
        Gmes,
		None
    }


    public enum STATE
    {
        BYPASS = 1,
        MANUAL,
        SEMIAUTO,
		AUTO
    }
}
