using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TakeimgIVI.Function;

namespace TakeimgIVI
{
    public partial class FormGmesResult : Form
    {
        static Thread LOAD;
        public static bool OKGMES = false;
        bool confirm = false;

        public FormGmesResult()
        {
            InitializeComponent();
        }

        public void SendOKGmes()
        {
            if (!confirm)
            {
                confirm = true;
                GMES_Pakage.SendValueCheck("OK");
                OKGMES = true;
                try
                {
                    btntop.Invoke(new Action(() => { btntop.Text += " : OK"; btntop.BackColor = Color.DarkGreen; }));
                    btnOK.Invoke(new Action(() => { btnOK.BackColor = Color.DarkBlue; }));
                }
                catch { }
                EndSend();
            }
        }

        public void SendNGGmes()
        {
            if (!confirm)
            {
                confirm = true;
                GMES_Pakage.SendValueCheck("NG");
                OKGMES = false;
                try
                {
                    btntop.Invoke(new Action(() => { btntop.Text += " : NG"; btntop.BackColor = Color.DarkRed; }));
                    btnOK.Invoke(new Action(() => { btnNG.BackColor = Color.DarkBlue; }));
                }
                catch { }
                EndSend();
                new Thread(() =>
                {
                    Thread.Sleep(400);
                    DelegateToUI.UIUpdateProcessLabel("NG", Color.Black, "Red");
                    DelegateToUI.UpdateProcessBar(100);
                })
                { IsBackground = true }.Start();
            }
        }

        public void EndSend()
        {
            new Thread(() =>
            {
                Thread.Sleep(300);
                this.Invoke(new Action(() => this.Close()));
            })
            { IsBackground = true }.Start();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SendOKGmes();
        }

        private void btnNG_Click(object sender, EventArgs e)
        {
            SendNGGmes();
        }

        private void FormGmesResult_Shown(object sender, EventArgs e)
        {
            OKGMES = false;
            LOAD = new Thread(() =>
            {
                bool Done = false;
                DateTime Beginpress = DateTime.Now;
                while (Status.ADAM)
                {
                    if (FormMain.Adam.bDiData[(int)DI.OK_GMES])
                    {
                        if (!Done && DateTime.Now.Subtract(Beginpress).TotalMilliseconds > 50)
                        {
                            Done = true;
                            SendOKGmes();
                        }
                    }
                    else if (FormMain.Adam.bDiData[(int)DI.NG_GMES])
                    {
                        if (!Done && DateTime.Now.Subtract(Beginpress).TotalMilliseconds > 50)
                        {
                            Done = true;
                            SendNGGmes();
                        }
                    }
                    else
                    {
                        Beginpress = DateTime.Now;
                    }
                }
            })
            { IsBackground = true };
            LOAD.Start();
        }

        private void FormGmesResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LOAD != null && LOAD.IsAlive) LOAD.Abort();
        }
    }
}
