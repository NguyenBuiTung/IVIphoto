using TakeimgIVI.Camera;
using TakeimgIVI.Function;
using TakeimgIVI.Scan;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeimgIVI
{
    public partial class FormNotification : Form
    {
        private ERRORCODE Code;
        private Color C1 = Color.OrangeRed;
        private Color C2 = Color.Black;
        public FormNotification(ERRORCODE code)
        {
            InitializeComponent();
            Code = code;
            if (code == ERRORCODE.NO_BARCODE || code == ERRORCODE.DIS_SCAN || Code == ERRORCODE.TIME_OUT_TAKE_IMAGE || Code == ERRORCODE.DIS_CAMERA1 || Code == ERRORCODE.DIS_CAMERA2 || Code == ERRORCODE.DIS_ADAM)
            {
                btnRecheck.Visible = true;
                btnReconnect.Visible = true;
            }
            CheckErrorCode(code);
        }

        public FormNotification(string s)
        {
            InitializeComponent();
            lblContent.Text = s;
        }
        // ấn kết nối lại
        private void btnReconnect_Click(object sender, EventArgs e)
        {
            if (Code == ERRORCODE.NO_BARCODE || Code == ERRORCODE.DIS_SCAN)
            {
                FormMain.Scan.StartListening();
            }
            if (Code == ERRORCODE.TIME_OUT_TAKE_IMAGE || Code == ERRORCODE.DIS_CAMERA1 || Code == ERRORCODE.DIS_CAMERA2)
            {
                new Thread(() =>
                {
                    FormMain.Camera1Connect();
                    Thread.Sleep(5000);
                    FormMain.Camera2Connect();
                })
                { IsBackground = true }.Start();
            }
            if (Code == ERRORCODE.DIS_ADAM)
            {
                FormMain.Adam.Disconnect();
                FormMain.Adam.Connect();
            }
;
        }
        // ấn quét lại
        private void btnRecheck_Click(object sender, EventArgs e)
        {
            Close();
        }
        // kiểm tra mã lỗi
        public void CheckErrorCode(ERRORCODE code)
        {
            string str = "";
            string name = Enum.GetName(typeof(ERRORCODE), code);
            ErrorObj err = FormMain.ErrorList.Errors.Where(d => d.Code == name).FirstOrDefault();
            if (err != null)
            {
                GenerateContent(err);
            }
        }

        private void btnOffBuzzer_Click(object sender, EventArgs e)
        {
            FormMain.Adam.SetOutput((int)DO.BUZZER, 0);
            if (Code == ERRORCODE.NO_BARCODE || Code == ERRORCODE.DIS_SCAN)
            {
                Constants.Barcode = "???";
            }
            else if (Code == ERRORCODE.DIS_CAMERA1 || Code == ERRORCODE.NO_IMAGE || Code == ERRORCODE.DIS_CAMERA2)
            {

            }
        }

        private void lblContent_Click(object sender, EventArgs e)
        {
            Close();
        }

        // tạo nội dung
        private void GenerateContent(ErrorObj err)
        {
            lblContent.Text = $"Error: {err.Code}\n";

            lblSolution.SelectionColor = C1;
            lblSolution.AppendText(" Nguyên nhân: \n");
            lblSolution.SelectionColor = C2;
            lblSolution.AppendText(err.GenerateCause() + "\n");

            lblSolution.SelectionColor = C1;
            lblSolution.AppendText(" Giải pháp: \n");
            lblSolution.SelectionColor = C2;
            lblSolution.AppendText(err.GenerateSolution() + "\n");
        }
        // timer nháy 
        private void tmrSplash_Tick(object sender, EventArgs e)
        {
            try
            {
                if (lblContent.ForeColor == C1)
                {
                    lblContent.BeginInvoke(new Action(() =>
                    {
                        lblContent.ForeColor = C2;
                    }));
                }
                else
                {
                    lblContent.BeginInvoke(new Action(() =>
                    {
                        lblContent.ForeColor = C1;
                    }));
                }
            }
            catch
            {

            }
        }

        private void FormNotification_Load(object sender, EventArgs e)
        {

        }

        private void FormNotification_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormMain.Takingimg = false;
            FormMain.Processimging = false;
            Constants.RefreshParameter();
            if (Status.State == STATE.BYPASS)
                DelegateToUI.Updatecolorlight(DO.LIGHTYELLOW);
            else
                DelegateToUI.Updatecolorlight(DO.LIGHTGREEN);
            Warning.IsOpening = false;
        }
    }
}
