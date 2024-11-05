using TakeimgIVI.Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeimgIVI
{
    public partial class frmmodel : Form
    {
        public frmmodel()
        {
            InitializeComponent();
        }

        private void frmmodel_Load(object sender, EventArgs e)
        {
            cboxmodel.DataSource = Model.LsModelcf;
            cboxmodel.DisplayMember = "Name";
        }

        private void cboxmodel_SelectedIndexChanged(object sender, EventArgs e)
        {
            imagemodel.Invoke(new Action(() => { imagemodel.Image = Model.LsModelcf[cboxmodel.SelectedIndex].Image; }));
        }

        private void btnchoose_Click(object sender, EventArgs e)
        {
            Model.Modelnow = Model.LsModelcf[cboxmodel.SelectedIndex];
            if(Model.UpdateModelnow()) this.Close();
        }
    }
}
