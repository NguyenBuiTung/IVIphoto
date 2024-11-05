using Microsoft.VisualBasic.Devices;
using TakeimgIVI.Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;

namespace TakeimgIVI
{
	public partial class FormConfig : Form
	{
		public FormConfig()
		{
			InitializeComponent();
		}
        // load dữ liệu plc
        private void LoadADAMInfo()
		{
            txtADPort.Text = Constants.ADAMPort.ToString();
            txtADIP.Text = Constants.ADAMIP;

        }
        // load dữ liệu scan
        private void LoadScanInfo()
		{
            tbPort.Text = Constants.Barcode_Port;
			tbBaudrate.Text = Constants.Barcode_Baurate;
		}
        // load ui
        private void FormConfig_Load(object sender, EventArgs e)
        {
            LoadADAMInfo();
			LoadScanInfo();
            LoadCameraInfo();
            LoadGmesInfo();
        }
        // load dữ liệu cam
        private void LoadCameraInfo()
        {
            tbIp1.Text = INI.ReadIni(Constants.Common, "Camera", "IP1");
            tbIp2.Text = INI.ReadIni(Constants.Common, "Camera", "IP2");
            btncolor.BackColor = (new Pen(Processimage.Colortxt)).Color;
            colorDialog1.Color = btncolor.BackColor;
        }
        // lưu dữ liệu cam
        private void btnSaveCamera_Click(object sender, EventArgs e)
        {
            if (tbIp1.Text != "")
            {
                Constants.Camera_IP1 = tbIp1.Text;
                INI.WriteIni(Constants.Common, "Camera", "IP1", Constants.Camera_IP1);
            }
            if (tbIp2.Text != "")
            {
                Constants.Camera_IP2 = tbIp2.Text;
                INI.WriteIni(Constants.Common, "Camera", "IP2", Constants.Camera_IP2);
            }
            Processimage.Colortxt = new SolidBrush(colorDialog1.Color);
            INI.WriteIni(Constants.Common, "Camera", "Colortext", colorDialog1.Color.ToArgb().ToString());
            LoadCameraInfo();
        }
        // lưu dữ liệu scan
        private void btnSaveScan_Click(object sender, EventArgs e)
        {
            if (tbPort.Text != "")
            {
                Constants.Barcode_Port = tbPort.Text;
                Constants.Barcode_Baurate = tbBaudrate.Text;
                INI.WriteIni(Constants.Common, "Barcode", "Port", Constants.Barcode_Port);
                INI.WriteIni(Constants.Common, "Barcode", "Baudrate", Constants.Barcode_Baurate);
            }
            LoadScanInfo();
        }
        // lưu dữ liệu plc
        private void btnSaveADAM_Click(object sender, EventArgs e)
        {
            if (txtADIP.Text != "" && int.TryParse(txtADPort.Text,out Constants.ADAMPort))
            {
                Constants.ADAMIP = txtADIP.Text;
                INI.WriteIni(Constants.Common, "ADAM", "IP", Constants.ADAMIP);
                INI.WriteIni(Constants.Common, "ADAM", "PORT", Constants.ADAMPort.ToString());
            }
            LoadADAMInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormErrorControl f = new FormErrorControl();   
            f.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadGmesInfo()
        {
            txtgmesip.Text = INI.ReadIni(Constants.Common, "Gmes", "IP");
            txtgmesport.Text = INI.ReadIni(Constants.Common, "Gmes", "PORT");
            txttimeogmes.Text = INI.ReadIni(Constants.Common, "Gmes", "Timeout");
            txtceid.Text = INI.ReadIni(Constants.Common, "Gmes", "CEID", "20100");
            txtrptid.Text = INI.ReadIni(Constants.Common, "Gmes", "RPTID", "20101");
        }

        private void btnsavegmes_Click(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(txtgmesip.Text,out _) && int.TryParse(txtgmesport.Text, out _) && double.TryParse(txttimeogmes.Text, out _) && txtceid.Text!= "" && txtrptid.Text!="")
            {
                Constants.GMESIP = txtgmesip.Text;
                Constants.GMESPORT = txtgmesport.Text;
                Constants.GMES_TimeOut = txttimeogmes.Text;
                Constants.GMES_CEID = txtceid.Text;
                Constants.GMES_RPTID = txtrptid.Text;
                INI.WriteIni(Constants.Common, "Gmes", "IP", Constants.GMESIP);
                INI.WriteIni(Constants.Common, "Gmes", "PORT", Constants.GMESPORT);
                INI.WriteIni(Constants.Common, "Gmes", "Timeout", Constants.GMES_TimeOut);
                INI.WriteIni(Constants.Common, "Gmes", "CEID", Constants.GMES_CEID);
                INI.WriteIni(Constants.Common, "Gmes", "RPTID", Constants.GMES_RPTID);

            }
            LoadGmesInfo();
        }

        private void btncolor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = (new Pen(Processimage.Colortxt).Color);
            colorDialog1.ShowDialog();
            btncolor.BackColor = colorDialog1.Color;
        }
    }
}
