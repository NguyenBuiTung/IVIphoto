using TakeimgIVI.Function;
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
    public partial class FormSimulation : Form
    {
        public static Thread SimulationThread;
        public bool IsOpen = false;
        public static bool IsSimulating = false;
        public FormSimulation()
        {
            InitializeComponent();
        }

        private void cbButtonClick_CheckedChanged(object sender, EventArgs e)
        {
            //MXProtocol.SS_ButtonClick = cbButtonClick.Checked;
        }

        private void cbInJig_CheckedChanged(object sender, EventArgs e)
        {
            //MXProtocol.SS_InJig = cbInJig.Checked;
        }

        private void cbPistolMax_CheckedChanged(object sender, EventArgs e)
        {
            //MXProtocol.SS_PistolMax = cbPistolMax.Checked;
        }

        private void cbPistolMin_CheckedChanged(object sender, EventArgs e)
        {
            //MXProtocol.SS_PistolMin = cbPistolMin.Checked;
        }

        private void cbDoorOpen_CheckedChanged(object sender, EventArgs e)
        {
            //MXProtocol.SS_DoorOpening = cbDoorOpen.Checked;
        }

        private void FormSimulation_Load(object sender, EventArgs e)
        {
            IsOpen = true;
            IsSimulating = false;
            DelegateToUI.updateTextSimulation += UIUpdateTextSimulation;
        }

        private delegate void UpdateTextSimulation();
        private void UIUpdateTextSimulation()
        {
            if (cbInJig.InvokeRequired || cbDoorOpen.InvokeRequired ||
                cbPistolMin.InvokeRequired || cbPistolMax.InvokeRequired ||
                cbButtonClick.InvokeRequired)
            {
                cbInJig.Invoke(new UpdateTextSimulation(UIUpdateTextSimulation));
            }
            else
            {
                //cbInJig.Checked = MXProtocol.SS_InJig;
                //cbButtonClick.Checked = MXProtocol.SS_ButtonClick;
                //cbPistolMax.Checked = MXProtocol.SS_PistolMax;
                //cbPistolMin.Checked = MXProtocol.SS_PistolMin;
                //cbDoorOpen.Checked = MXProtocol.SS_DoorOpening;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            IsSimulating = !IsSimulating;

            if (IsSimulating)
            {
                //FormMain.Mitsu.ThreadStart();
                SimulationThread = new Thread(() =>
                {
                    while (true)
                    {
                        if (IsOpen)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    DelegateToUI.UIUpdateTextSimulation();
                                    //lblProcess.Text = FormMain.Mitsu.GetProcess();
                                }
                                catch { }
                            }));
                        }
                        Thread.Sleep(1000);
                    }
                });
                SimulationThread.IsBackground = true;
                SimulationThread.Start();
                btnStartSimualtion.Text = "Stop";
                btnStartSimualtion.BackColor = Color.Tomato;
            }
            else
            {
                SimulationThread.Abort();
                btnStartSimualtion.Text = "Start";
                btnStartSimualtion.BackColor = Color.LawnGreen;
            }
        }

        private void cbIgnorDoor_CheckedChanged(object sender, EventArgs e)
        {
            //MXProtocol.IgnoreDoor = cbIgnorDoor.Checked;
        }

        private void FormSimulation_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
            if (SimulationThread != null)
            {
                SimulationThread.Abort();
            }
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            //MXProtocol.Process = PROCESS.READY;
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            //MXProtocol.Process = PROCESS.START;
        }

        private void btnMoveIn_Click(object sender, EventArgs e)
        {
            //MXProtocol.Process = PROCESS.MOVING;
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {
            //MXProtocol.Process = PROCESS.TRIGGER;
        }

        private void btnMoveOut_Click(object sender, EventArgs e)
        {
            //MXProtocol.Process = PROCESS.MOVING_OUT;
        }
    }
}
