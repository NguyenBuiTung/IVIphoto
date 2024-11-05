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
    public partial class FormCheckIO : Form
    {
        Thread CheckIO;
        public FormCheckIO()
        {
            InitializeComponent();
        }

        private void FormCheckIO_Load(object sender, EventArgs e)
        {
            CheckIO = new Thread(() =>
            {
                while (true)
                {
                    if (Status.ADAM)
                    {
                        bool[] DIT = FormMain.Adam.bDiData;
                        if (DIT != null)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                CheckBox cki = (tbmain.Controls.Find($"ckdi{i}", false))[0] as CheckBox;
                                cki.Invoke(new Action(() => { cki.Checked = DIT[i]; }));
                            }
                        }
                        bool[] DOT = FormMain.Adam.bDoData;
                        if (DOT != null)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                CheckBox cko = (tbmain.Controls.Find($"ckdo{i}", false))[0] as CheckBox;
                                cko.Invoke(new Action(() => { cko.Checked = DOT[i]; }));
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            })
            { IsBackground = true };
            CheckIO.Start();
        }

        private void FormCheckIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckIO != null && CheckIO.IsAlive)
            {
                CheckIO.Abort();
            }
        }
    }
}
