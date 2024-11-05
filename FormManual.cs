using MvCamCtrl.NET;
using TakeimgIVI.Camera;
using TakeimgIVI.Function;
using TakeimgIVI.Scan;
using System;
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
    public partial class FormManual : Form
    {
        public static DateTime Begin;
        public FormManual()
        {
            InitializeComponent();
        }
        // ấn trigger cam
        private void btnTrigger_Click(object sender, EventArgs e)
        {
            Begin = DateTime.Now;
            CameraCIC.ManualTrigger = true;
            FormMain.Camera1.TriggerCam();
            new Thread(() =>
            {
                while (CameraCIC.WaitingImage)
                {
                    Thread.Sleep(10);
                }
                DelegateToUI.updateprocesstime((DateTime.Now - Begin).TotalSeconds);
            })
            { IsBackground = true}.Start();
        }
        // ấn ngắt kết nối cam
        private void btnCloseDevice_Click(object sender, EventArgs e)
        {
            if (Status.Camera1)
            {
                DialogResult res = MessageBox.Show("Do you want to close Camera?", "CLOSE CAMERA", MessageBoxButtons.YesNo);
                Thread t = new Thread(() =>
                {
                    FormMain.Camera1.CloseDevice();
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("Camera is not connecting");
            }
        }
        // load form
        private void FormManual_Load(object sender, EventArgs e)
        {

        }
        // ấn nút scan
        private void btnScan_Click(object sender, EventArgs e)
        {
            if (FormMain.Scan.CheckConnection())
            {
                FormMain.Scan.TriggerBarcode();
            }
            else
            {
                MessageBox.Show("Scanner is unavailable!");
            }
        }
        // ấn ngắt kết nối scan
        private void btnScanDisconnect_Click(object sender, EventArgs e)
        {
            FormMain.Scan.StopListening();
        }

        // ấn tắt còi
        private void btnOffBuzzer_Click(object sender, EventArgs e)
        {
            FormMain.Adam.SetOutput((int)DO.BUZZER, 0);
        }
        
        // ấn reset lỗi
        private void btnResetError_Click(object sender, EventArgs e)
        {
            if (Warning.IsOpening)
            {
                FormMain.Takingimg = false;
                FormMain.Processimging = false;
                Warning.CloseForm();
            }

        }

        private void manTriggerCam2_Click(object sender, EventArgs e)
        {
            Begin = DateTime.Now;
            CameraCIC2.ManualTrigger = true;
            FormMain.Camera2.TriggerCam();
            new Thread(() =>
            {
                while (CameraCIC2.WaitingImage)
                {
                    Thread.Sleep(10);
                }
                DelegateToUI.updateprocesstime((DateTime.Now - Begin).TotalSeconds);
            })
            { IsBackground = true }.Start();
        }

        private void btnEditmodel_Click(object sender, EventArgs e)
        {
            frmeditModel frm = new frmeditModel();
            frm.ShowDialog();
        }

        private void btnCameratest_Click(object sender, EventArgs e)
        {
            if (Model.Modelnow != null)
            {
                Constants.Barcode = "";
                FormMain.MathTakttime(0);
                FormMain.TakeimgAuto();
            }
            else
            {
                MessageBox.Show("Chosse one Model and again");
            }

        }

        private void btnckIO_Click(object sender, EventArgs e)
        {
            if (!Warning.CheckOpened("FormCheckIO"))
            {
                FormCheckIO f = new FormCheckIO();
                f.Show();
                f.BringToFront();
            }
        }

        private void btndiscam2_Click(object sender, EventArgs e)
        {
            if (Status.Camera2)
            {
                DialogResult res = MessageBox.Show("Do you want to close Camera?", "CLOSE CAMERA", MessageBoxButtons.YesNo);
                Thread t = new Thread(() =>
                {
                    FormMain.Camera2.CloseDevice();
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                MessageBox.Show("Camera is not connecting");
            }
        }
    }
}
