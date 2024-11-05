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
using SLAB_HID_TO_SMBUS;

namespace LC_Controller2
{
    public partial class Form1 : Form
    {
        public bool waiting = false;
        public int index;
        public List<string> LsMGX = new List<string>();

        public bool withZoom = false;           // The zoom   is installed or not installed
        public bool withFocus = false;          // The focus  is installed or not installed
        public bool withIris = false;           // The iris   is installed or not installed
        public bool withOptFil = false;         // The optfil is installed or not installed
        
        public const int MSGBOX = 0;
        public const int ZOOMTXT = 1;
        public const int FOCUSTXT = 2;
        public const int IRISTXT = 3;

        public const int SUCCESS = 0x00;
        public const int NG = 0x01;

        public const int INIT_COMPLETED = 0;   // for Need Initialize bit (0 is initialized, 1 is need initialized)

        public const bool CONNECT = true;
        public const bool DISCONNECT = false;

        public const float ABS_ZERO_TEMP = 273.15f;

        public const string ZOOM_DEFAULT_STEPS = "100";
        public const string FOCUS_DEFAULT_STEPS = "100";
        public const string IRIS_DEFAULT_STEPS = "50";

        uint deviceNumber = 0;                  // Device Number of use
        uint numDevices = 0;                    // Number of Devices
        bool usbOpen_flag = DISCONNECT;  // Usb connected flag

        public Form1(int index)
        {
            InitializeComponent();
            this.index = index;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Lens{index + 1} Controller";
        }

        public bool LOADFIRST(int index)
        {
            if(usbOpen_flag) UsbDisconnect();
            Thread.Sleep(100);
            ScanUsb();
            BtnConnect_OnOff(DISCONNECT);
            TextClear();
            InputBoxOff();
            BtnProhibit();
            Thread.Sleep(100);
            if (SnComboBox.Items.Count > index)
            {
                SnComboBox.SelectedIndex = index;
                Thread.Sleep(100);
                UsbConnect_btn_Click(null, null);
                Thread.Sleep(200);
            }
            return checkcn();
        }

        public bool checkcn()
        {
            if (waiting) return true;
            bool check = LensCtrl.Instance.UsbRead(DevAddr.TEMPERATURE_VAL, 2) == 0;
            return check;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        public int ErrorChk(int returnCode, int motorNumbers)   // motorNumbers = 0:MsgBox, 1:Zoom, 2:Focus, 3:Iris
        {
            if (returnCode != SUCCESS)
            {
                switch (motorNumbers)
                {
                    case 0:
                        Addlist(LensCtrl.Instance.ErrorTxt(returnCode));
                        break;
                    case 1:
                        ZoomStatusTxt.Text = LensCtrl.Instance.ErrorTxt(returnCode);
                        break;
                    case 2:
                        FocusStatusTxt.Text = LensCtrl.Instance.ErrorTxt(returnCode);
                        break;
                    case 3:
                        IrisStatusTxt.Text = LensCtrl.Instance.ErrorTxt(returnCode);
                        break;
                }
            }
            return returnCode;
        }
        private void Addlist(string _MSG)
        {
            while(LsMGX.Count >= 100) LsMGX.RemoveAt(LsMGX.Count-1);
            LsMGX.Insert(0, _MSG);
        }


        private void ScanUsb()
        {
            int indexCount = 0;
            SnComboBox.Items.Clear();
            ErrorChk(LensCtrl.Instance.UsbGetNumDevices(out numDevices), MSGBOX);
            if (numDevices >= 1)
            {
                LensInfoClear();
                for (uint i = 0; i < numDevices; i++)
                {
                    int retval = ErrorChk(LensCtrl.Instance.UsbGetSnDevice(i, out string snString), MSGBOX);
                    SnComboBox.Items.Add(snString);
                    if (retval != SUCCESS)
                        indexCount++;
                }
                if (indexCount >= numDevices)
                    indexCount = 0;
                SnComboBox.SelectedIndex = indexCount;   // SnComboBox_SelectedIndexChanged is called
                UsbConnect_btn.Enabled = true;
            }
            else
            {
                LensInfoClear();
                SnComboBox.ResetText();
                Addlist("No LensConnect is connected.");
                UsbConnect_btn.Enabled = false;
            }
        }

        private void LensInfoClear()
        {
            LensModelTxt.Text = "";
            FwVersionTxt.Text = "";
            UserAreaVal.Text = "";
        }
        private void SnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (numDevices != 0)
            {
                deviceNumber = (uint)SnComboBox.SelectedIndex;
                if (SUCCESS != ErrorChk(LensCtrl.Instance.UsbOpen(deviceNumber), MSGBOX))
                    return;

                ErrorChk(LensCtrl.Instance.ModelName(out string model), MSGBOX);
                LensModelTxt.Text = model;

                ErrorChk(LensCtrl.Instance.FWVersion(out string version), MSGBOX);
                FwVersionTxt.Text = version;

                ErrorChk(LensCtrl.Instance.UserAreaRead(out string userName), MSGBOX);
                UserAreaVal.Text = userName;

                LensCtrl.Instance.UsbClose();
            }
            else
            {
                LensInfoClear();
            }
        }
        private void DeviceScan_btn_Click(object sender, EventArgs e)
        {
            ScanUsb();
        }
        public void UsbConnect_btn_Click(object sender, EventArgs e)
        {
            if (usbOpen_flag == DISCONNECT)
            {
                if (SUCCESS != ErrorChk(LensCtrl.Instance.UsbOpen(deviceNumber), MSGBOX))
                    return;
                if (SUCCESS != ErrorChk(LensCtrl.Instance.UsbSetConfig(), MSGBOX))
                    return;

                ErrorChk(LensCtrl.Instance.CapabilitiesRead(out ushort Capabilities), MSGBOX);
                ErrorChk(LensCtrl.Instance.Status2ReadSet(), MSGBOX);
                ushort InitializeStatus = LensCtrl.Instance.status2;

                if ((Capabilities & ConfigVal.ZOOM_MASK) == ConfigVal.ZOOM_MASK)                  // Determine if Zoom is installed
                {
                    LensCtrl.Instance.ZoomParameterReadSet();
                    if ((InitializeStatus & ConfigVal.ZOOM_MASK) == INIT_COMPLETED)        // Determine if it has been Zoom Initialized
                    {
                        ZoomParameterSet();
                        ZoomInputEnable(true);
                    }
                    ZoomInit_btn.Enabled = true;
                    withZoom = true;
                }
                if ((Capabilities & ConfigVal.FOCUS_MASK) == ConfigVal.FOCUS_MASK)                // Determine if Focus is installed
                {
                    LensCtrl.Instance.FocusParameterReadSet();
                    if ((InitializeStatus & ConfigVal.FOCUS_MASK) == INIT_COMPLETED)       // Determine if it has been Focus Initialized
                    {
                        FocusParameterSet();
                        FocusInputEnable(true);
                    }
                    FocusInit_btn.Enabled = true;
                    withFocus = true;
                }
                if ((Capabilities & ConfigVal.IRIS_MASK) == ConfigVal.IRIS_MASK)                  // Determine if Iris is installed
                {
                    LensCtrl.Instance.IrisParameterReadSet();
                    if ((InitializeStatus & ConfigVal.IRIS_MASK) == INIT_COMPLETED)        // Determine if it has been Iris Initialized
                    {
                        IrisParameterSet();
                        IrisInputEnable(true);
                    }
                    IrisInit_btn.Enabled = true;
                    withIris = true;
                }
                if ((Capabilities & ConfigVal.OPT_FILTER_MASK) == ConfigVal.OPT_FILTER_MASK)      // Determine if OptFilter is installed
                {
                    LensCtrl.Instance.OptFilterParameterReadSet();
                    if ((InitializeStatus & ConfigVal.OPT_FILTER_MASK) == INIT_COMPLETED)  // Determine if it has been OptFilter Initialized
                    {
                        OptFilNumTxt.Text = (LensCtrl.Instance.optFilMaxAddr+1).ToString();
                        OptFilCurrTxt.Text = LensCtrl.Instance.optCurrentAddr.ToString();
                    }
                    OptInit_btn.Enabled = true;
                    withOptFil = true;
                }
                TempInfoGet();
                usbOpen_flag = CONNECT;
                BtnConnect_OnOff(CONNECT);
                BtnPermition(SUCCESS);
            }
            else
            {
                UsbDisconnect();
            }
        }
        private void UsbDisconnect()
        {
            LensCtrl.Instance.UsbClose();
            usbOpen_flag = DISCONNECT;
            BtnConnect_OnOff(DISCONNECT);
            withZoom = false;
            withFocus = false;
            withIris = false;
            withOptFil = false;
            TextClear();
            InputBoxOff();
            BtnProhibit();
        }
        private void BtnConnect_OnOff(bool flag)
        {
            DeviceScan_btn.Enabled = !flag;
            SnComboBox.Enabled = !flag;
            UsbConnect_btn.Text = "Connect";
            if (flag == CONNECT)
                UsbConnect_btn.Text = "Disconnect";
        }
        private void TextClear()
        {
            ZoomStatusTxt.Text = ZoomMinTxt.Text = ZoomMaxTxt.Text = ZoomAddrTxt.Text = ZoomStepVal.Text = ZoomGotoVal.Text = "";
            FocusStatusTxt.Text = FocusMinTxt.Text = FocusMaxTxt.Text = FocusAddrTxt.Text = FocusStepVal.Text = FocusGotoVal.Text = "";
            IrisStatusTxt.Text = IrisMinTxt.Text = IrisMaxTxt.Text = IrisAddrTxt.Text = IrisStepVal.Text = IrisGotoVal.Text = "";
            OptFilNumTxt.Text = OptFilCurrTxt.Text = "";
            TempCelsTxt.Text = TempFahrTxt.Text = "";
        }
        private void InputBoxOff()
        {
            ZoomStepVal.Enabled = false;
            ZoomGotoVal.Enabled = false;
            FocusStepVal.Enabled = false;
            FocusGotoVal.Enabled = false;
            IrisStepVal.Enabled = false;
            IrisGotoVal.Enabled = false;
        }
        public void BtnProhibit()     // Device operation button prohibited
        {
            ZoomInit_btn.Enabled =  false;
            ZoomBtnEnable(false);

            FocusInit_btn.Enabled = false;
            FocusBtnEnable(false);
            
            IrisInit_btn.Enabled = false;
            IrisBtnEnable(false);
            
            OptInit_btn.Enabled = false;
            OptFilterBtnEnable(false);
            
            PresetOpen_btn.Enabled = false;
            OtherBtnsEnable(false);
        }
        private void OtherBtnsEnable(bool flag)
        {
            TempUpdate_btn.Enabled = flag;
            InfoMore_btn.Enabled = flag;
            UserAreaWrite_btn.Enabled = flag;
        }
        public void BtnPermition(int retval)    // Device operation button permition with Error code
        {
            if (retval >= ConfigVal.LOWHI_ERROR | retval == SUCCESS)    // Error code check
            {
                ushort InitializeStatus = LensCtrl.Instance.status2;
                if (withZoom == true)
                {
                    ZoomInit_btn.Enabled = true;
                    if ((InitializeStatus & ConfigVal.ZOOM_MASK) == INIT_COMPLETED)
                        ZoomBtnEnable(true);
                }
                if (withFocus == true)
                {
                    FocusInit_btn.Enabled = true;
                    if ((InitializeStatus & ConfigVal.FOCUS_MASK) == INIT_COMPLETED)
                        FocusBtnEnable(true);
                }
                if (withIris == true)
                {
                    IrisInit_btn.Enabled = true;
                    if ((InitializeStatus & ConfigVal.IRIS_MASK) == INIT_COMPLETED)
                        IrisBtnEnable(true);
                }
                if (withOptFil == true)
                {
                    OptInit_btn.Enabled = true;
                    if ((InitializeStatus & ConfigVal.OPT_FILTER_MASK) == INIT_COMPLETED)
                        OptFilterBtnEnable(true);
                }
                LensCtrl.Instance.Status1Read(out ushort status1);
                if ((status1 & ConfigVal.NEED_INIT_BIT) == INIT_COMPLETED)
                    PresetOpen_btn.Enabled = true;

                OtherBtnsEnable(true);
            }
            else
            {
                UsbDisconnect();
            }
        }
        private void TempUpdate_btn_Click(object sender, EventArgs e)
        {
            TempInfoGet();
        }
        private void TempInfoGet()
        {
            ErrorChk(LensCtrl.Instance.TempKelvin(out int KelvinValue), MSGBOX);
            TempCelsTxt.Text = (Math.Round(KelvinValue - ABS_ZERO_TEMP, MidpointRounding.AwayFromZero)).ToString();                 // Celcius Calculation.
            //TempFahrTxt.Text = (Math.Round(KelvinVal * 1.8 - 459.67, MidpointRounding.AwayFromZero)).ToString();   
            TempFahrTxt.Text = (Math.Round( (KelvinValue - ABS_ZERO_TEMP) * 9 / 5 + 32, MidpointRounding.AwayFromZero)).ToString(); // Fahrenheit Calculation.
        }
        private void UserAreaWrite_btn_Click(object sender, EventArgs e)
        {
            byte[] sendUserName;
            sendUserName = Encoding.ASCII.GetBytes(UserAreaVal.Text);
            int retval = ErrorChk(LensCtrl.Instance.UserAreaWrite(sendUserName), MSGBOX);
            if (retval != SUCCESS)
                UsbDisconnect();
        }
        private void ZoomBtnEnable(bool flag)
        {
            ZoomWide_btn.Enabled = flag;
            ZoomTele_btn.Enabled = flag;
            ZoomGoto_btn.Enabled = flag;
            ZoomBar.Enabled = flag;
        }
        private void ZoomInputEnable(bool flag)
        {
            ZoomStepVal.Enabled = flag;
            ZoomGotoVal.Enabled = flag;
        }
        public void ZoomParameterSet()
        {
            ZoomMinTxt.Text = LensCtrl.Instance.zoomMinAddr.ToString();
            ZoomBar.Minimum = LensCtrl.Instance.zoomMinAddr;
            ZoomMaxTxt.Text = LensCtrl.Instance.zoomMaxAddr.ToString();
            ZoomBar.Maximum = LensCtrl.Instance.zoomMaxAddr;
            LensCtrl.Instance.ZoomCurrentAddrReadSet();
            ZoomAddrTxt.Text = LensCtrl.Instance.zoomCurrentAddr.ToString();
            try { ZoomBar.Value = LensCtrl.Instance.zoomCurrentAddr; }
            catch { }// Set after defining Min/Max
            ZoomStepVal.Text = ZOOM_DEFAULT_STEPS;
            ZoomStatusTxt.Text = "";
        }
        public void ZoomValueUpdate()
        {
            ZoomAddrTxt.Text = LensCtrl.Instance.zoomCurrentAddr.ToString();
            ZoomBar.Value = LensCtrl.Instance.zoomCurrentAddr;
            ZoomStatusTxt.Text = "";
        }
        public void ZoomOperation(ushort addrData)
        {
            if (addrData != LensCtrl.Instance.zoomCurrentAddr)
            {
                if (LensCtrl.Instance.zoomMaxAddr >= addrData & addrData >= LensCtrl.Instance.zoomMinAddr)
                {
                    BtnProhibit();
                    ZoomStatusTxt.Text = "In operation.";
                    int retval = ErrorChk(LensCtrl.Instance.ZoomMove(addrData), ZOOMTXT);
                    if (retval == SUCCESS)
                        ZoomValueUpdate();
                    BtnPermition(retval);
                    return;
                }
                ZoomStatusTxt.Text = "Out of Address";
                return;
            }
            ZoomStatusTxt.Text = "Same Address";
        }
        private void ZoomInit_btn_Click(object sender, EventArgs e)
        {
            BtnProhibit();
            ZoomStatusTxt.Text = "Initializing.";
            int retval = ErrorChk(LensCtrl.Instance.ZoomInit(), ZOOMTXT);
            ZoomParameterSet();
            ZoomInputEnable(true);
            BtnPermition(retval);
        }
        private void ZoomWide_btn_Click(object sender, EventArgs e)
        {
            if (ZoomStepVal.Text != "")
            {
                ushort moveValue = (ushort)Convert.ToInt32(ZoomStepVal.Text);
                ushort addrData = (ushort)(LensCtrl.Instance.zoomCurrentAddr - moveValue);
                ZoomOperation(addrData);
            }
            else
            {
                ZoomStatusTxt.Text = "No step value.";
            }
        }
        private void ZoomTele_btn_Click(object sender, EventArgs e)
        {
            if (ZoomStepVal.Text != "")
            {
                ushort moveValue = (ushort)Convert.ToInt32(ZoomStepVal.Text);
                ushort addrData = (ushort)(LensCtrl.Instance.zoomCurrentAddr + moveValue);
                ZoomOperation(addrData);
            }
            else
            {
                ZoomStatusTxt.Text = "No step value";
            }
        }
        private void ZoomGoto_btn_Click(object sender, EventArgs e)
        {
            if (ZoomGotoVal.Text != "")
            {
                ushort addrData = (ushort)Convert.ToInt32(ZoomGotoVal.Text);
                ZoomOperation(addrData);
            }
            else
            {
                ZoomStatusTxt.Text = "No address value";
            }
        }
        private void ZoomBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ZoomOperation((ushort)ZoomBar.Value);
        }
        private void FocusBtnEnable(bool flag)
        {
            FocusNear_btn.Enabled = flag;
            FocusFar_btn.Enabled = flag;
            FocusGoto_btn.Enabled = flag;
            FocusBar.Enabled = flag;

        }
        private void FocusInputEnable(bool flag)
        {
            FocusStepVal.Enabled = flag;
            FocusGotoVal.Enabled = flag;
        }
        public void FocusParameterSet()
        {
            FocusMinTxt.Text = LensCtrl.Instance.focusMinAddr.ToString();
            FocusBar.Minimum = LensCtrl.Instance.focusMinAddr;
            FocusMaxTxt.Text = LensCtrl.Instance.focusMaxAddr.ToString();
            FocusBar.Maximum = LensCtrl.Instance.focusMaxAddr;
            LensCtrl.Instance.FocusCurrentAddrReadSet();
            FocusAddrTxt.Text = LensCtrl.Instance.focusCurrentAddr.ToString();
            FocusBar.Value = LensCtrl.Instance.focusCurrentAddr;            // Set after defining Min/Max
            FocusStepVal.Text = FOCUS_DEFAULT_STEPS;
            FocusStatusTxt.Text = "";
        }
        public void FocusValueUpdate()
        {
            FocusAddrTxt.Text = LensCtrl.Instance.focusCurrentAddr.ToString();
            FocusBar.Value = LensCtrl.Instance.focusCurrentAddr;
            FocusStatusTxt.Text = "";
        }
        bool focusok = false;
        public void FocusOperation(ushort addrData)
        {
            waiting = true;
            try
            {
                if (addrData != LensCtrl.Instance.focusCurrentAddr)
                {
                    if (LensCtrl.Instance.focusMaxAddr >= addrData & addrData >= LensCtrl.Instance.focusMinAddr)
                    {
                        BtnProhibit();
                        FocusStatusTxt.Text = "In Operation";
                        int retval = ErrorChk(LensCtrl.Instance.FocusMove(addrData), FOCUSTXT);
                        if (retval == SUCCESS)
                        {
                            focusok = true;
                            FocusValueUpdate();
                        }

                        BtnPermition(retval);
                        return;
                    }
                    FocusStatusTxt.Text = "Out of Address";
                    focusok = false;
                    return;
                }
                FocusStatusTxt.Text = "Same Address";
                focusok = true;
            }
            catch { focusok = false; }
            waiting = false;
        }
        private void FocusInit_btn_Click(object sender, EventArgs e)
        {
            waiting = true;
            BtnProhibit();
            FocusStatusTxt.Text = "Initializing";
            int retval = ErrorChk(LensCtrl.Instance.FocusInit(), FOCUSTXT);
            FocusParameterSet();
            FocusInputEnable(true);
            BtnPermition(retval);
            waiting = false;
        }
        private void FocusNear_btn_Click(object sender, EventArgs e)
        {
            if (FocusStepVal.Text != "")
            {
                ushort moveValue = (ushort)Convert.ToInt32(FocusStepVal.Text);
                ushort addrData = (ushort)(LensCtrl.Instance.focusCurrentAddr + moveValue);
                FocusOperation(addrData);
            }
            else
            {
                FocusStatusTxt.Text = "No step value";
            }
        }
        private void FocusFar_btn_Click(object sender, EventArgs e)
        {
            if (FocusStepVal.Text != "")
            {
                ushort moveValue = (ushort)Convert.ToInt32(FocusStepVal.Text);
                ushort addrData = (ushort)(LensCtrl.Instance.focusCurrentAddr - moveValue);
                FocusOperation(addrData);
            }
            else
            {
                FocusStatusTxt.Text = "No step value";
            }
        }

        public bool UpdateFocus(ushort focus)
        {
            focusok = false;
            if (!FocusBar.Enabled)
            {
                FocusInit_btn_Click(null, null);
            }
            DateTime Begin = DateTime.Now;
            while (!FocusBar.Enabled && DateTime.Now.Subtract(Begin).TotalSeconds > 8) { Thread.Sleep(500); }
            if (FocusBar.Enabled) FocusOperation(focus);
            return focusok;
        }

        private void FocusGoto_btn_Click(object sender, EventArgs e)
        {
            if (FocusGotoVal.Text != "")
            {
                ushort addrData = (ushort)Convert.ToInt32(FocusGotoVal.Text);
                FocusOperation(addrData);
            }
            else
            {
                FocusStatusTxt.Text = "No address value";
            }
        }
        private void FocusBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                FocusOperation((ushort)FocusBar.Value);
        }
        private void IrisBtnEnable(bool flag)
        {
            IrisOpen_btn.Enabled = flag;
            IrisClose_btn.Enabled = flag;
            IrisGoto_btn.Enabled = flag;
            IrisBar.Enabled = flag;

        }
        private void IrisInputEnable(bool flag)
        {
            IrisStepVal.Enabled = flag;
            IrisGotoVal.Enabled = flag;
        }
        public void IrisParameterSet()
        {
            IrisMinTxt.Text = LensCtrl.Instance.irisMinAddr.ToString();
            IrisBar.Minimum = LensCtrl.Instance.irisMinAddr;
            IrisMaxTxt.Text = LensCtrl.Instance.irisMaxAddr.ToString();
            IrisBar.Maximum = LensCtrl.Instance.irisMaxAddr;
            LensCtrl.Instance.IrisCurrentAddrReadSet();
            IrisAddrTxt.Text = LensCtrl.Instance.irisCurrentAddr.ToString();
            try { IrisBar.Value = LensCtrl.Instance.irisCurrentAddr; }
            catch { }// Set after defining Min/Max
            IrisStepVal.Text = IRIS_DEFAULT_STEPS;
            IrisStatusTxt.Text = "";
        }
        public void IrisValueUpdate()
        {
            IrisAddrTxt.Text = LensCtrl.Instance.irisCurrentAddr.ToString();
            IrisBar.Value = LensCtrl.Instance.irisCurrentAddr;
            IrisStatusTxt.Text = "";
        }
        public void IrisOperation(ushort addrData)
        {
            waiting = true;
            try
            {
                if (addrData != LensCtrl.Instance.irisCurrentAddr)
                {
                    if (LensCtrl.Instance.irisMaxAddr >= addrData & addrData >= LensCtrl.Instance.irisMinAddr)
                    {
                        BtnProhibit();
                        IrisStatusTxt.Text = "In Operation.";
                        int retval = ErrorChk(LensCtrl.Instance.IrisMove(addrData), IRISTXT);
                        if (retval == SUCCESS)
                        {
                            irisok = true;
                            IrisValueUpdate();
                        }

                        BtnPermition(retval);
                        return;
                    }
                    IrisStatusTxt.Text = "Out of Address";
                    irisok = false;
                    return;
                }
                IrisStatusTxt.Text = "Same Address";
                irisok = true;
            }
            catch { irisok = false; }

            waiting = false;
        }
        private void IrisInit_btn_Click(object sender, EventArgs e)
        {
            waiting = true;
            BtnProhibit();
            IrisStatusTxt.Text = "Initializing";
            int retval = ErrorChk(LensCtrl.Instance.IrisInit(), IRISTXT);
            IrisParameterSet();
            IrisInputEnable(true);
            BtnPermition(retval);
            waiting = false;
        }
        private void IrisOpen_btn_Click(object sender, EventArgs e)
        {
            if (IrisStepVal.Text != "")
            {
                ushort moveValue = (ushort)Convert.ToInt32(IrisStepVal.Text);
                ushort addrData = (ushort)(LensCtrl.Instance.irisCurrentAddr - moveValue);
                IrisOperation(addrData);
            }
            else
            {
                IrisStatusTxt.Text = "No step value";
            }
        }
        private void IrisClose_btn_Click(object sender, EventArgs e)
        {
            if (IrisStepVal.Text != "")
            {
                ushort moveValue = (ushort)Convert.ToInt32(IrisStepVal.Text);
                ushort addrData = (ushort)(LensCtrl.Instance.irisCurrentAddr + moveValue);
                IrisOperation(addrData);
            }
            else
            {
                IrisStatusTxt.Text = "No step value";
            }
        }

        bool irisok = false;
        public bool UpdateIris(ushort iris)
        {
            waiting = true;
            irisok = false;
            if (!IrisBar.Enabled)
            {
                IrisInit_btn_Click(null, null);
            }
            DateTime Begin = DateTime.Now;
            while (!IrisBar.Enabled && DateTime.Now.Subtract(Begin).TotalSeconds > 8) { Thread.Sleep(500); }
            if (IrisBar.Enabled) IrisOperation(iris);
            waiting = false;
            return irisok;
        }


        private void IrisGoto_btn_Click(object sender, EventArgs e)
        {
            if (IrisGotoVal.Text != "")
            {
                ushort addrData = (ushort)Convert.ToInt32(IrisGotoVal.Text);
                IrisOperation(addrData);
            }
            else
            {
                IrisStatusTxt.Text = "No address value";
            }
        }
        private void IrisBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                IrisOperation((ushort)IrisBar.Value);
        }
        private void OptFilterBtnEnable(bool flag)
        {
            if (flag == true)
            {
                if (LensCtrl.Instance.optCurrentAddr != 0)
                    Opt0_btn.Enabled = true;
                if ((LensCtrl.Instance.optFilMaxAddr >= 1) && (LensCtrl.Instance.optCurrentAddr != 1))
                    Opt1_btn.Enabled = true;
                if ((LensCtrl.Instance.optFilMaxAddr >= 2) && (LensCtrl.Instance.optCurrentAddr != 2))
                    Opt2_btn.Enabled = true;
                if ((LensCtrl.Instance.optFilMaxAddr >= 3) && (LensCtrl.Instance.optCurrentAddr != 3))
                    Opt3_btn.Enabled = true;
            }
            else
            {
                    Opt0_btn.Enabled = false;
                    Opt1_btn.Enabled = false;
                    Opt2_btn.Enabled = false;
                    Opt3_btn.Enabled = false;
            }
        }
        public void OptFilterOperation(ushort FilterNum)
        {
            BtnProhibit();
            int retval = ErrorChk(LensCtrl.Instance.OptFilterMove(FilterNum), MSGBOX);
            OptFilCurrTxt.Text = LensCtrl.Instance.optCurrentAddr.ToString();
            BtnPermition(retval);
        }
        private void OptInit_btn_Click(object sender, EventArgs e)
        {
            BtnProhibit();
            int retval = ErrorChk(LensCtrl.Instance.OptFilterInit(), MSGBOX);
            OptFilNumTxt.Text = LensCtrl.Instance.optFilMaxAddr.ToString();
            OptFilCurrTxt.Text = LensCtrl.Instance.optCurrentAddr.ToString();
            BtnPermition(retval);
        }
        private void Opt0_btn_Click(object sender, EventArgs e)
        {
            OptFilterOperation(0);
        }
        private void Opt1_btn_Click(object sender, EventArgs e)
        {
            OptFilterOperation(1);
        }
        private void Opt2_btn_Click(object sender, EventArgs e)
        {
            OptFilterOperation(2);
        }
        private void Opt3_btn_Click(object sender, EventArgs e)
        {
            OptFilterOperation(3);
        }

        private void InfoMore_btn_Click(object sender, EventArgs e)
        {

        }
        private void PresetOpen_btn_Click_1(object sender, EventArgs e)
        {

        }

        public Form1 Instance = null;

        private void btnckcnt_Click(object sender, EventArgs e)
        {
            int retval = LensCtrl.Instance.UsbRead(DevAddr.TEMPERATURE_VAL, 2);
            if(retval != 0) { Addlist("Disconnect Lens!"); }
            else { Addlist("Connect to Lens OK!"); }
        }
    }
}
