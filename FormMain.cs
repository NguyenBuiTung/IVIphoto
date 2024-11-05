using TakeimgIVI.Camera;
using TakeimgIVI.Function;
using TakeimgIVI.Scan;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TakeimgIVI
{
    public partial class FormMain : Form
    {
        public static Scanner Scan = new Scanner(); // scanner
        public static IntPtr PictureBoxHandle { get; set; } = IntPtr.Zero; // pointer camera
        public static CameraCIC Camera1; // camera1
        public static CameraCIC2 Camera2; // camera2
        public static ADAM6060 Adam;
        public static Lens lens1;
        public static Lens2 lens2;
        public static Color ColorOK = Color.ForestGreen;
        public static Color ColorNG = Color.Silver;
        public static ErrorControl ErrorList;
        public static Thread ProcessMain;

        public static bool Takingimg = false;
        public static bool Processimging = false;

        private static bool ConnectingToCamera1;
        private static bool ConnectingToCamera2;
        public FormMain()
        {
            // kiểm tra chạy 1 phần mềm
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                Close();
            }
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        // hàm load ui
        private void FormMain_Load(object sender, EventArgs e)
        {
            Init();
            DelegateToUI.PushToListBox("Initializing", LogTitle.Info);
            Constants.ReadCommon(); // đọc file ini
            DelegateToUI.PushToListBox("Reading Data", LogTitle.Info);
            // mở kết nối đến các đối tượng
            ErrorList = new ErrorControl();
            DelegateToUI.PushToListBox("Connecting . . .", LogTitle.Info);
            Camera1 = new CameraCIC();
            Camera2 = new CameraCIC2();
            Adam = new ADAM6060();
            lens1 = new Lens(0);
            lens2 = new Lens2(1);
            Scan = new Scanner();
            btnTotal.Text = Constants.Total_Count;
            tbDelay.Text = Constants.DelayTime;
            ConnectingToCamera1 = false;
            ConnectingToCamera2 = false;
            Status.PingCheck();
            DelegateToUI.updatecolorlight += Adam.SetColorLight;
            UpProcessMain();
            // ấn kết nối
            new Thread(() =>
            {
                btnScan.PerformClick();
                btnADAM.PerformClick();
                btnlens1.PerformClick();
                btnlens2.PerformClick();
                btnCamera.PerformClick();
                Thread.Sleep(5000);
                btnCamera2.PerformClick();
                new Thread(() =>
                {
                    Status.State = STATE.MANUAL;
                    DateTime timebg = DateTime.Now;
                    while (!Status.ADAM && DateTime.Now.Subtract(timebg).TotalSeconds < 3)
                    {
                        Thread.Sleep(500);
                    }
                    btnBypass.PerformClick();
                })
                { IsBackground = true }.Start();
                DelegateToUI.PushToListBox("Done Load", LogTitle.Info);
            })
            { IsBackground = true }.Start();

        }
        //đăng kí sự kiện
        private void Init()
        {
            DelegateToUI.update += PushToListBox;
            DelegateToUI.updateBarcodeValue += UIUpdateBarcode;
            DelegateToUI.updateprocesstime += UIUpdateTaktTime;
            DelegateToUI.saveImage += UISaveImage;
            DelegateToUI.updateProductCount += UIUpdateProductCount;
            DelegateToUI.updatePicture += UIUpdatePicture;
            DelegateToUI.updateProcessLabel += UIUpdateProcessLabel;
            DelegateToUI.updateModelname += Updatemodelname;
            Scan.NewSerialDataReceived += Scan_NewSerialDataRecieved;
            DelegateToUI._updateProcessBar += UpdateProcessBar;

        }

        //hủy kí sự kiện
        private void Uninit()
        {
            DelegateToUI.updateBarcodeValue -= UIUpdateBarcode;
            DelegateToUI.updateprocesstime -= UIUpdateTaktTime;
            DelegateToUI.saveImage -= UISaveImage;
            DelegateToUI.updateProductCount -= UIUpdateProductCount;
            DelegateToUI.updatePicture -= UIUpdatePicture;
            DelegateToUI.updateProcessLabel -= UIUpdateProcessLabel;
            DelegateToUI.updateModelname -= Updatemodelname;
            Scan.NewSerialDataReceived -= Scan_NewSerialDataRecieved;
            DelegateToUI.update -= PushToListBox;
        }

        // update ảnh
        public delegate void UpdatePicture(Bitmap Truebmp);
        private void UIUpdatePicture(Bitmap Truebmp)
        {
            try
            {
                if (pbImage.InvokeRequired)
                {
                    pbImage.Invoke(new UpdatePicture(UIUpdatePicture), Truebmp);
                }
                else
                {
                    try
                    {
                        pbImage.Image = Truebmp;
                    }
                    catch
                    {
                        DelegateToUI.PushToListBox("Can't update Image", LogTitle.Error);
                    }
                }
            }
            catch { }
        }
        // lưu ảnh
        public delegate void SaveImage();
        private void UISaveImage()
        {

        }
        //update quá trình
        public delegate void UpdateProcessLabel(string str, Color background, string fore = "Lime");
        private void UIUpdateProcessLabel(string str, Color background, string fore = "Lime")
        {
            try
            {
                if (lblResultVisionCheck.InvokeRequired)
                {
                    lblResultVisionCheck.Invoke(new UpdateProcessLabel(UIUpdateProcessLabel), str, background, fore);
                }
                else
                {
                    lblResultVisionCheck.Text = str;
                    lblResultVisionCheck.BackColor = background;
                    lblResultVisionCheck.ForeColor = Color.FromName(fore);
                }
            }
            catch { }
        }

        private void Scan_NewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            if (e.Data != null)
            {

            }
        }
        // update thông tin barcode 
        public delegate void UpdateBarcode(string s);
        private void UIUpdateBarcode(string s)
        {
            if (btnBarcode.InvokeRequired)
            {
                btnBarcode.Invoke(new UpdateBarcode(UIUpdateBarcode), s);
            }
            else
            {
                btnBarcode.Text = s;
                Constants.Barcode = s;
            }
        }

        // update Model name
        public delegate void UpdateModelname(string s);
        private void Updatemodelname(string s)
        {
            if (txtModel.InvokeRequired)
            {
                txtModel.Invoke(new UpdateModelname(Updatemodelname), s);
            }
            else
            {
                txtModel.Text = s;
                DelegateToUI.PushToListBox("Model Change []: " + s, LogTitle.Recv);
            }
        }

        // update thông tin takt time
        public delegate void UpdateTaktTime(double s);
        private void UIUpdateTaktTime(double s)
        {
            if (btnBarcode.InvokeRequired)
            {
                btnProcesstime.Invoke(new UpdateTaktTime(UIUpdateTaktTime), s);
            }
            else
            {
                btnProcesstime.Text = Math.Round(s, 2).ToString() + " s";
            }
        }

        // update thanh process
        public delegate void UpdateProcessbar(int i);
        public void UpdateProcessBar(int i)
        {
            if (ProcessBar.InvokeRequired)
            {
                ProcessBar.Invoke(new UpdateProcessbar(UpdateProcessBar), i);
            }
            else
            {
                if (i <= 100)
                {
                    ProcessBar.Value = i;
                }
            }
        }
        // update trạng thái nút
        public void UIUpdateStatus(Button c, bool status)
        {
            if (status == true)
            {
                c.Invoke(new Action(() => { c.BackColor = ColorOK; c.ImageIndex = 1; }));
            }
            else
                c.Invoke(new Action(() => { c.BackColor = ColorNG; c.ImageIndex = 0; }));
        }
        // update số lượng sản phẩm
        private delegate void UpdateProductCount();
        private void UIUpdateProductCount()
        {
            if (btnTotal.InvokeRequired)
            {
                btnTotal.Invoke(new UpdateProductCount(UIUpdateProductCount));
            }
            else
            {
                Constants.Total_Count = (Convert.ToInt32(Constants.Total_Count) + 1).ToString();
                btnTotal.Text = Constants.Total_Count;
                SaveProductCount();
            }
        }
        // lưu số lượng sản phẩm
        private void SaveProductCount()
        {
            string str = Application.StartupPath + "\\Communication\\Common.ini";
            if (!File.Exists(str))
            {
                string core = "[Product]\r\nCount =0\r\n";
                using (FileStream f = new FileStream(str, FileMode.OpenOrCreate))
                {
                    f.Write(Encoding.ASCII.GetBytes(core), 1, 1);
                }
            }
            INI.WriteIni(str, "Product", "Count", Constants.Total_Count);
        }
        // update log
        public delegate void UpdateListbox(string s, LogTitle _tab);
        public void PushToListBox(string s, LogTitle _tab = LogTitle.Info)
        {
            if (lsbProgram.InvokeRequired)
            {
                lsbProgram.Invoke(new UpdateListbox(PushToListBox), s, _tab);
            }
            else
            {
                string content = Status.FormatString(s);
                if (_tab != LogTitle.Gmes)
                {
                    if (lsbProgram.Items.Count > 200)
                    {
                        lsbProgram.Items.RemoveAt(200);
                    }
                    lsbProgram.Items.Insert(0, content);
                    Log.LogEvent(content, 2, _tab);
                }
                else
                {
                    if (lsbGmes.Items.Count > 200)
                    {
                        lsbGmes.Items.RemoveAt(200);
                    }
                    lsbGmes.Items.Insert(0, content);
                    Log.LogEvent(content, 4, _tab);
                }
                
            }
        }
        // ấn tắt phần mềm
        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        // đóng phần mềm, ngắt kết nối 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult rs = MessageBox.Show("Do you really want to quit ?", "Quit", MessageBoxButtons.YesNo);
            if (rs == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            Log.LogEvent("Close Application", 2, LogTitle.Info);

            Camera1.CloseDevice();
            Camera2.CloseDevice();

            if (CameraCIC.ReceiveProcess != null)
            {
                CameraCIC.ReceiveProcess.Abort();
            }
            if (CameraCIC2.ReceiveProcess != null)
            {
                CameraCIC2.ReceiveProcess.Abort();
            }
            Scan.StopListening();
            Status.Dispose();
            Uninit();

        }
        // ấn nút ẩn phần mềm
        private void Hide_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        // ấn nút kết nối camera
        private void btnCamera_Click(object sender, EventArgs e)
        {
            Camera1Connect();
        }
        public static void Camera1Connect()
        {
            if (!ConnectingToCamera1)
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        ConnectingToCamera1 = true;
                        Camera1.GetDeviceInfo(); // lấy dữ liệu cam
                        if (CameraCIC.DeviceCIC != IntPtr.Zero)
                        {
                            if (Camera1 == null)
                            {
                                Camera1 = new CameraCIC(); // tạo camera
                                if (Camera1 == null)
                                {
                                    DelegateToUI.PushToListBox("Applying resource fail! Camera 1", LogTitle.Error);
                                    Status.Camera1 = false;
                                    return;
                                }
                            }
                            if (Camera1.OpenDevice()) // kiểm tra mở cam
                            {
                                Camera1.SetTriggerMode(); // đặt chế độ chụp
                                if (Camera1.StartGrabbing()) // kích hoạt chế độ điều khiển
                                {
                                    DelegateToUI.PushToListBox("The camera 1 can be controlled now");
                                    Status.Camera1 = true;
                                }
                                else
                                {
                                    DelegateToUI.PushToListBox("Fail to control Camera 1", LogTitle.Error);
                                    Status.Camera1 = false;
                                }
                            }
                            else
                            {
                                Status.Camera1 = false;
                            }
                            //btnGetParam_Click(null, null);
                        }
                        else
                        {
                            Status.Camera1 = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Warning.Show(ex.Message);
                    }
                    ConnectingToCamera1 = false;
                });
                t.IsBackground = true;
                t.Start();
            }
        }
        private void btnCamera2_Click(object sender, EventArgs e)
        {
            Camera2Connect();
        }
        public static void Camera2Connect()
        {
            if (!ConnectingToCamera2)
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        ConnectingToCamera2 = true;
                        Camera2.GetDeviceInfo(); // lấy dữ liệu cam
                        if (CameraCIC2.DeviceCIC != IntPtr.Zero)
                        {
                            if (Camera2 == null)
                            {
                                Camera2 = new CameraCIC2(); // tạo camera
                                if (Camera2 == null)
                                {
                                    DelegateToUI.PushToListBox("Applying resource fail! Camera 2", LogTitle.Error);
                                    Status.Camera2 = false;
                                    return;
                                }
                            }
                            if (Camera2.OpenDevice()) // kiểm tra mở cam
                            {
                                Camera2.SetTriggerMode(); // đặt chế độ chụp
                                if (Camera2.StartGrabbing()) // kích hoạt chế độ điều khiển
                                {
                                    DelegateToUI.PushToListBox("The camera 2 can be controlled now");
                                    Status.Camera2 = true;
                                }
                                else
                                {
                                    DelegateToUI.PushToListBox("Fail to control Camera 2", LogTitle.Error);
                                    Status.Camera2 = false;
                                }
                            }
                            else
                            {
                                Status.Camera2 = false;
                            }
                            //btnGetParam_Click(null, null);
                        }
                        else
                        {
                            Status.Camera2 = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Warning.Show(ex.Message);
                    }
                    ConnectingToCamera2 = false;
                });
                t.IsBackground = true;
                t.Start();
            }
        }
        // không dùng
        private void btnGetParam_Click(object sender, EventArgs e)
        {
            //CameraCIC.MVCC_FLOATVALUE stParam = new CameraCIC.MVCC_FLOATVALUE();
            //int Status = Camera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            //Camera.MV_CC_SetFloatValue_NET("ExposureTime", 1.2f);
            //if (CameraCIC.MV_OK == Status)
            //{
            //	//tbExposure.Text = stParam.fCurValue.ToString("F1");
            //}

            //Status = Camera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
            //if (CameraCIC.MV_OK == Status)
            //{
            //	//tbGain.Text = stParam.fCurValue.ToString("F1");
            //}

            //Status = Camera.MV_CC_GetFloatValue_NET("ResultingFrameRate", ref stParam);
            //if (CameraCIC.MV_OK == Status)
            //{
            //	//tbFrameRate.Text = stParam.fCurValue.ToString("F1");
            //}
        }
        //ấn nút config 
        private void Config_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;

            if (fc.Cast<Form>().Where(d => d.Name == "FormConfig").Count() == 0)
            {
                if (MessageBox.Show("Log In Before Config ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    FormLogin f = new FormLogin();
                    f.ShowDialog();
                    if (Customer.Passed)
                    {
                        FormConfig config = new FormConfig();
                        config.ShowDialog();
                        Customer.Passed = false;
                    }
                }
            }
        }
        // ấn nút user
        private void User_Click(object sender, EventArgs e)
        {
            FormLogin f = new FormLogin();
            f.ShowDialog();
            if (Customer.Passed)
            {
                FormAccount frm = new FormAccount();
                frm.ShowDialog();
                Customer.Passed = false;
            }
        }
        // ấn nút kết nối scan
        private void btnScan_Click(object sender, EventArgs e)
        {
            Scan.StartListening();
            if (Status.Scan)
            {
                DelegateToUI.PushToListBox("Connect to Barcode successfully!");
            }
            else
            {
                DelegateToUI.PushToListBox("Can't connect to Barcode.");
            }
        }
        // ấn nút bypass
        private void btnBypass_Click(object sender, EventArgs e)
        {
            if (Status.ADAM)
            {
                if (Warning.IsOpening) return;
                if (Status.State != STATE.BYPASS)
                {
                    Adam.SetColorLight(DO.LIGHTYELLOW);
                    Status.State = STATE.BYPASS;
                    RefreshColor();
                    UIUpdateStatus(btnBypass, true);

                    DelegateToUI.UIUpdateProcessLabel("BYPASS", Color.Black, "Yellow");
                    //tlpDelay.Visible = true;
                }
            }
            else
            {
                DelegateToUI.PushToListBox("CHECK CONNECTION TO ADAM!");
            }
        }

        private static FormManual manual;
        // ấn nút manual
        private void btnManual_Click(object sender, EventArgs e)
        {
            if (Status.ADAM)
            {
                if (!Status.Camera1 || !Status.Camera2)
                {
                    MessageBox.Show("You need connect to all Camera !");
                    return;
                }
                if (!Status.Lens1 || !Status.Lens2)
                {
                    MessageBox.Show("You need connect to all Lens !");
                    return;
                }
                if (!Status.Scan)
                {
                    MessageBox.Show("You need connect to Scan !");
                    return;
                }
                if (Model.Modelnow != null)
                {
                    if (Warning.IsOpening) return;
                    if (Status.State != STATE.MANUAL)
                    {
                        Adam.SetColorLight(DO.LIGHTGREEN);
                        Status.State = STATE.MANUAL;
                        RefreshColor();
                        UIUpdateStatus(btnManual, true);
                        //tlpDelay.Visible = true;
                        DelegateToUI.UIUpdateProcessLabel("MAN", Color.Black);
                    }
                }
                else
                {
                    MessageBox.Show("You need choose one Model !");
                }
            }
            else
            {
                DelegateToUI.PushToListBox("CHECK CONNECTION TO ADAM!");
            }
        }
        // ấn nút auto
        private void btnAuto_Click(object sender, EventArgs e)
        {
            if (Status.ADAM)
            {
                if (!Status.Camera1 || !Status.Camera2)
                {
                    MessageBox.Show("You need connect to all Camera !");
                    return;
                }
                if (!Status.Lens1 || !Status.Lens2)
                {
                    MessageBox.Show("You need connect to all Lens !");
                    return;
                }
                if (!Status.Scan)
                {
                    MessageBox.Show("You need connect to Scan !");
                    return;
                }
                if (Model.Modelnow != null)
                {
                    if (Warning.IsOpening) return;
                    if (Status.State != STATE.AUTO)
                    {
                        Adam.SetColorLight(DO.LIGHTGREEN);
                        Status.State = STATE.AUTO;
                        RefreshColor();
                        UIUpdateStatus(btnAuto, true);
                        //tlpDelay.Visible = true;
                        DelegateToUI.UIUpdateProcessLabel("AUTO", Color.Black);
                    }
                }
                else
                {
                    MessageBox.Show("You need choose one Model !");
                }

            }
            else
            {
                DelegateToUI.PushToListBox("CHECK CONNECTION TO ADAM!");
            }
        }

        //reset màu
        private void RefreshColor()
        {
            UIUpdateStatus(btnBypass, false);
            UIUpdateStatus(btnManual, false);
            UIUpdateStatus(btnAuto, false);

            //tlpDelay.Visible = false;
            PushToListBox($"START - {Enum.GetName(typeof(STATE), Status.State).ToUpper()} MODE");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //FormGmesResult f = new FormGmesResult();
            //f.ShowDialog();
        }
        // xóa bộ đếm
        private void btnResetCounter_Click(object sender, EventArgs e)
        {
            if (IsPassedLogin())
            {
                btnTotal.Text = "0";
                Constants.Total_Count = btnTotal.Text;
                SaveProductCount();
            }
        }
        // kiểm tra đăng nhập 
        private bool IsPassedLogin()
        {
            FormLogin f = new FormLogin();
            f.ShowDialog();
            return Customer.Passed;
        }
        // xóa danh sách
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lsbProgram.Items.Clear();
        }

        private void lsbProgram_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmsClear.Show();
            }
        }
        // timer kiểm tra trạng thái
        private void tmrCheckConnection_Tick(object sender, EventArgs e)
        {
            // trạng thái camera1
            if (Convert.ToInt32(Status.Camera1) != btnCamera.ImageIndex)
            {
                UIUpdateStatus(btnCamera, Status.Camera1);
            }
            // trạng thái camera2
            if (Convert.ToInt32(Status.Camera2) != btnCamera2.ImageIndex)
            {
                UIUpdateStatus(btnCamera2, Status.Camera2);
            }
            // trạng thái scan
            Scan.CheckConnection();
            if (Convert.ToInt32(Status.Scan) != btnScan.ImageIndex)
            {
                UIUpdateStatus(btnScan, Status.Scan);
            }
            // trạng thái GMES
            if (Convert.ToInt32(Status.GMES) != btnGMES.ImageIndex)
            {
                UIUpdateStatus(btnGMES, Status.GMES);
            }
            // trạng thái ADMAM
            if (Convert.ToInt32(Status.ADAM) != btnADAM.ImageIndex)
            {
                UIUpdateStatus(btnADAM, Status.ADAM);
            }
            // trạng thái LENS1
            if (Convert.ToInt32(Status.Lens1) != btnlens1.ImageIndex)
            {
                UIUpdateStatus(btnlens1, Status.Lens1);
            }
            // trạng thái LENS2
            if (Convert.ToInt32(Status.Lens2) != btnlens2.ImageIndex)
            {
                UIUpdateStatus(btnlens2, Status.Lens2);
            }
            // các trạng thái phần mềm
            if (Status.State == STATE.BYPASS && btnBypass.ImageIndex == 0)
            {
                RefreshColor();
                UIUpdateStatus(btnBypass, true);
            }
            else if (Status.State == STATE.MANUAL && btnManual.ImageIndex == 0)
            {
                RefreshColor();
                UIUpdateStatus(btnManual, true);
            }
            else if (Status.State == STATE.AUTO && btnAuto.ImageIndex == 0)
            {
                RefreshColor();
                UIUpdateStatus(btnAuto, true);
            }


            if (Status.State == STATE.AUTO && manual != null) try { manual.Close(); } catch { } //TESTER

        }

        private void lblResultVisionCheck_Click(object sender, EventArgs e)
        {
            //FormSimulation f = new FormSimulation();
            //f.Show();

            //TEST TAKE IMAGE
            //new Thread(() =>
            //{
            //	DateTime Begin = DateTime.Now;
            //	Bitmap bmpnew = Takeimage(new float[] { 0, 1, 0, 1, 0, 1, 0, 1 });
            //	if (bmpnew != null)
            //	{
            //		bmpnew = Processimage.DrawToImageNoBarcode(bmpnew);
            //                 MessageBox.Show((DateTime.Now - Begin).TotalSeconds.ToString());
            //                 pbImage.Invoke(new Action(() => pbImage.Image = bmpnew));

            //	}
            //})
            //{ IsBackground = true }.Start();

        }



        private void tbDelay_ValueChanged(object sender, EventArgs e)
        {
            Constants.DelayTime = tbDelay.Value.ToString();
            INI.WriteIni(Constants.Common, "Product", "DelayTime", Constants.DelayTime);
        }

        // form kiểm soát mã lỗi
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormErrorControl f = new FormErrorControl();
            f.Show();
        }
        // ấn reset lỗi
        private void btnRserror_Click(object sender, EventArgs e)
        {
            if (Warning.IsOpening)
            {
                Takingimg = false;
                Warning.CloseForm();
            }
        }

        private void btnADAM_Click(object sender, EventArgs e)
        {
            Adam.Disconnect();
            Adam.Connect();
        }

        private void btnlens1_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                if (lens1.Reconnect())
                    DelegateToUI.PushToListBox("Connect to Lens1 successfully");
                else
                    DelegateToUI.PushToListBox("Connect to Lens1 failed");
            })
            { IsBackground = true }.Start();

        }

        private void btnlens2_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                if (lens2.Reconnect())
                    DelegateToUI.PushToListBox("Connect to Lens2 successfully");
                else
                    DelegateToUI.PushToListBox("Connect to Lens2 failed");
            })
            { IsBackground = true }.Start();
        }

        private void btnlens1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (MessageBox.Show("Log In ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    FormLogin f = new FormLogin();
                    f.ShowDialog();
                    if (Customer.Passed)
                    {
                        lens1.ShowController();
                        Customer.Passed = false;
                    }
                }
        }

        private void btnlens2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (MessageBox.Show("Log In ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    FormLogin f = new FormLogin();
                    f.ShowDialog();
                    if (Customer.Passed)
                    {
                        lens2.ShowController();
                        Customer.Passed = false;
                    }
                }
        }

        private void btnmodel_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            if (fc.Cast<Form>().Where(d => d.Name == "frmmodel").Count() == 0)
            {
                frmmodel fmd = new frmmodel();
                fmd.ShowDialog();
            }
        }

        // Take img from 2 CAMERA
        static Bitmap imgleft;
        static Bitmap imgright;
        public static Bitmap Takeimage(float[] cropcf)
        {
            Bitmap bmp = null;
            if (cropcf.Length != 8) return bmp;
            if (Status.Camera1 && Status.Camera2)
            {
                CameraCIC.ErrorTake = false;
                CameraCIC2.ErrorTake = false;

                CameraCIC2.WaitingImage = true;

                Thread TakeIMG = new Thread(() =>
                {
                    Camera1.TriggerCam();
                    DelegateToUI.UpdateProcessBar(20);
                    while (Status.Camera1 && CameraCIC.WaitingImage)
                    {
                        Thread.Sleep(10);
                    }
                    Camera2.TriggerCam();
                    DelegateToUI.UpdateProcessBar(30);
                })
                { IsBackground = true };

                TakeIMG.Start();

                DateTime BeginTime = DateTime.Now;

                while (CameraCIC.WaitingImage || CameraCIC2.WaitingImage)
                {
                    if (!CameraCIC.ErrorTake && !CameraCIC2.ErrorTake)
                    {
                        if ((DateTime.Now - BeginTime).TotalSeconds > Constants.Timeout)
                        {
                            Warning.Show(ERRORCODE.TIME_OUT_TAKE_IMAGE);
                            if (TakeIMG.IsAlive) TakeIMG.Abort();
                            DelegateToUI.UIUpdateProcessLabel("NG", Color.Black, "Red");
                            DelegateToUI.UpdateProcessBar(100);
                            return bmp;
                        }
                    }
                    else
                    {
                        Warning.Show(ERRORCODE.TAKE_IMAGE_ERR);
                        if (TakeIMG.IsAlive) TakeIMG.Abort();
                        DelegateToUI.UIUpdateProcessLabel("NG", Color.Black, "Red");
                        DelegateToUI.UpdateProcessBar(100);
                        return bmp;
                    }

                    Thread.Sleep(10);
                }

                imgleft = null;
                imgright = null;

                DelegateToUI.UpdateProcessBar(70);

                imgleft = Processimage.CropImage(CameraCIC.TrueBitmap, cropcf[0], cropcf[1], cropcf[2], cropcf[3]);
                imgright = Processimage.CropImage(CameraCIC2.TrueBitmap, cropcf[4], cropcf[5], cropcf[6], cropcf[7]);
                if (imgleft != null && imgright != null)
                    bmp = Processimage.MergeImages(imgleft, imgright, 0);
                else if (imgleft != null) bmp = imgleft;
                else if (imgright != null) bmp = imgright;

                DelegateToUI.UpdateProcessBar(90);
            }
            return bmp;
        }

        private void btnManual_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && Status.State != STATE.AUTO)
            {
                if (MessageBox.Show("Log In ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    FormLogin f = new FormLogin();
                    f.ShowDialog();
                    if (Customer.Passed)
                    {
                        FormCollection fc = Application.OpenForms;
                        if (fc.Cast<Form>().Where(d => d.Name == "FormManual").Count() == 0)
                        {
                            manual = new FormManual();
                            manual.Show();
                        }
                        else
                        {
                            if (manual != null) try { manual.Activate(); } catch { }
                        }
                    }
                }
            }
        }

        static DateTime Begin = DateTime.Now;
        public static void MathTakttime(int i)
        {
            if (i == 0)
            {
                DelegateToUI.UIUpdateProcessLabel("BEGIN", Color.Black);
                DelegateToUI.UpdateProcessBar(15);
                DelegateToUI.PushToListBox("Process[] BEGIN . . . ");
                Begin = DateTime.Now;
            }
            if (i == 1)
            {
                double takttime = DateTime.Now.Subtract(Begin).TotalSeconds;
                DelegateToUI.UIUpdateProcessTime(takttime);
                DelegateToUI.UIUpdateProductCount();
                DelegateToUI.PushToListBox("Process[] END . . .");
                if(Status.GMES && GMES_Data.ACK != "0") //GMESTEST
                    DelegateToUI.UIUpdateProcessLabel("NG", Color.Black,"Red");
                else
                    DelegateToUI.UIUpdateProcessLabel("OK", Color.Black);
                DelegateToUI.UpdateProcessBar(100);
            }
        }

        public static void TakeimgAuto()
        {
            new Thread(() =>
            {
                Processimging = true;
                while (Scanner.WaitingBarcode) Thread.Sleep(50);
                Bitmap IMG = Takeimage(Model.Modelcfnowarry);
                if (IMG == null)
                {
                    DelegateToUI.UIUpdateProcessLabel("NG", Color.Black, "Red");
                    DelegateToUI.UpdateProcessBar(100);
                    Processimging = false;
                }
                else
                {
                    DelegateToUI.UpdateProcessBar(90);
                    Processimging = false;
                    IMG = Processimage.DrawToImage(IMG);
                }    
                MathTakttime(1);
                new Thread(() => Processimage.Saveimageauto(IMG)) { IsBackground = true }.Start();
                DelegateToUI.UIUpdatePicture(IMG);
            })
            { IsBackground = true }.Start();


        }

        bool TestSensor = false;

        public void UpProcessMain()
        {
            if (ProcessMain != null && ProcessMain.IsAlive) ProcessMain.Abort();
            ProcessMain = new Thread(new ThreadStart(ProcessM)) { IsBackground = true };
            ProcessMain.Start();
        }

        DateTime checkbegin = DateTime.Now;
        private void ProcessM()
        {
            while (true)
            {
                try
                {
                    if (Status.IsAuto)
                    {
                        if (!Status.ADAM)
                        {
                            Warning.Show(ERRORCODE.DIS_ADAM);
                            btnBypass.Invoke(new Action(() => btnBypass.PerformClick()));
                        }
                        else
                        {
                            if (Adam.bDiData != null)
                            {
                                if ((Model.Modelnow.sensorindex == 0 && Adam.bDiData[(int)DI.SETDETECT1]) || (Model.Modelnow.sensorindex == 1 && Adam.bDiData[(int)DI.SETDETECT2]))
                                {
                                    if (!Takingimg && !Processimging && DateTime.Now.Subtract(checkbegin).TotalSeconds >= double.Parse(Constants.DelayTime))
                                    {
                                        Takingimg = true;
                                        MathTakttime(0);
                                        new Thread(() => { Scan.TriggerBarcodeTimes(); }) { IsBackground = true }.Start();
                                        Thread.Sleep(50);
                                        TakeimgAuto();
                                    }
                                }
                                else
                                {
                                    if (!Processimging)
                                    {
                                        Takingimg = false;
                                        checkbegin = DateTime.Now;
                                    }
                                    else
                                    {
                                        Warning.Show(ERRORCODE.EARLY_PCS_REMOVAL);
                                    }
                                }

                            }
                            else
                            {
                                DelegateToUI.PushToListBox("ADAM []: ADAM waiting Connected !");
                            }
                        }
                    }

                    if (Status.IsManual)
                    {
                        if (!Status.ADAM)
                        {
                            Warning.Show(ERRORCODE.DIS_ADAM);
                            btnBypass.Invoke(new Action(() => btnBypass.PerformClick()));
                        }
                        else
                        {
                            if (Adam.bDiData != null)
                            {
                                if (Adam.bDiData[(int)DI.SETCHECK] && ((Model.Modelnow.sensorindex == 0 && Adam.bDiData[(int)DI.SETDETECT1]) || (Model.Modelnow.sensorindex == 1 && Adam.bDiData[(int)DI.SETDETECT2])))
                                {
                                    if (!Takingimg && !Processimging && DateTime.Now.Subtract(checkbegin).TotalMilliseconds >= 10)
                                    {
                                        Takingimg = true;
                                        MathTakttime(0);
                                        new Thread(() => { Scan.TriggerBarcodeTimes(); }) { IsBackground = true }.Start();
                                        Thread.Sleep(50);
                                        TakeimgAuto();
                                    }
                                }
                                else
                                {

                                    if (!Processimging)
                                    {
                                        Takingimg = false;
                                        checkbegin = DateTime.Now;
                                    }
                                    else
                                    {
                                        if (!Adam.bDiData[(int)DI.SETDETECT1] && !Adam.bDiData[(int)DI.SETDETECT2])
                                            Warning.Show(ERRORCODE.EARLY_PCS_REMOVAL);
                                    }
                                }
                            }
                            else
                            {
                                DelegateToUI.PushToListBox("ADAM []: ADAM waiting Connected !");
                            }
                        }
                    }

                    Thread.Sleep(10);
                }
                catch
                {

                }
            }

        }

        #region GMES
        private void Connect_Disconect_GMES()
        {
            if (Status.GMES == false)
            {
                GMES_Socket.Connect_GMES_Socket();
                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                DelegateToUI.update("Open Connection to GMES", LogTitle.Info);
                DelegateToUI.update("Open Connection to GMES", LogTitle.Gmes);
                Thread.Sleep(500);
                if (Status.GMES)
                {
                    //Update_Status1(btnGMES, true);
                    Application.DoEvents();
                }
            }
            else
            {
                if (MessageBox.Show("Start Program without GMES ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    GMES_Socket.Disconnect_GMES_Socket();
                    GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                    DelegateToUI.update("Close Connection to GMES", LogTitle.Info);
                    DelegateToUI.update("Close Connection to GMES", LogTitle.Gmes);
                }
                if (!Status.GMES)
                {
                    //Update_Status1(btnGMES, false);
                }
            }
        }

        private void btnGMES_Click(object sender, EventArgs e)
        {
            Connect_Disconect_GMES();
        }
        #endregion

        //     private void button3_MouseDown(object sender, MouseEventArgs e)
        //     {
        //TestSensor = true;
        //     }

        //     private void button3_MouseUp(object sender, MouseEventArgs e)
        //     {
        //         TestSensor = false;
        //     }
    }
}
