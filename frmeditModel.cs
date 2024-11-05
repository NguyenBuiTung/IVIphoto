using TakeimgIVI.Function;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TakeimgIVI
{
    public partial class frmeditModel : Form
    {
        private static List<Modelcf> Lsmodeledit = new List<Modelcf>(Model.LsModelcf);
        BindingSource bindingSource1 = new BindingSource { DataSource = Lsmodeledit };

        public frmeditModel()
        {
            InitializeComponent();
        }

        private void Updateindex(int indexstart)
        {
            for (int i = indexstart; i < Lsmodeledit.Count; i++)
            {
                Lsmodeledit[i].Index = i + 1;
            }
            dgvmodel.Refresh();
        }

        private void frmeditModel_Shown(object sender, EventArgs e)
        {
            dgvmodel.DataSource = bindingSource1;
            dgvmodel.Columns[0].ReadOnly = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dgvmodel.ReadOnly = false;
            btnAdd.Enabled = btndel.Enabled = btnsave.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Modelcf modelcf = new Modelcf() { Index = dgvmodel.Rows.Count + 1 };
            bindingSource1.Add(modelcf);
            dgvmodel.Refresh();
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            if (dgvmodel.SelectedCells.Count > 0)
            {
                int indexrow = dgvmodel.SelectedCells[0].RowIndex;
                if (indexrow >= 0)
                {
                    bindingSource1.RemoveAt(indexrow);
                }
                Updateindex(indexrow);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            dgvmodel.Refresh();
            dgvmodel.ReadOnly = true;
            btnAdd.Enabled = btndel.Enabled = btnsave.Enabled = false;
            Model.Savemodelinto(Lsmodeledit);
            Model.Readmodelinto();
            Lsmodeledit = new List<Modelcf>(Model.LsModelcf);
            if(Model.Modelnow != null)
            Model.Modelnow = Model.LsModelcf[Model.Modelnow.Index-1];
            dgvmodel.Refresh();
            Close();
        }

        private void dgvmodel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                imageboxmodel.Invoke(new Action(() => { imageboxmodel.Image = Lsmodeledit[e.RowIndex].Image; }));
                if (e.ColumnIndex == 2)
                {
                    if (openImage.ShowDialog() == DialogResult.OK)
                    {
                        string pathimage = openImage.FileName;
                        (bindingSource1[e.RowIndex] as Modelcf).patchImage = pathimage;
                        (bindingSource1[e.RowIndex] as Modelcf).Image = new Bitmap(pathimage);
                        dgvmodel.Refresh();
                    }
                }
            }
        }

        static Bitmap Testbmp;
        private void btnTest_Click(object sender, EventArgs e)
        {
            if (dgvmodel.SelectedCells.Count > 0)
            {
                Constants.Barcode = "";
                Modelcf now = Lsmodeledit[dgvmodel.SelectedCells[0].RowIndex];

                Model.UpdateModelcf(now);

                new Thread(() =>
                {
                    btnTest.Invoke(new Action(() => btnTest.Enabled = false));
                    Testbmp = FormMain.Takeimage(new float[8] { now.widthbegin1, now.widthend1, now.heightbegin1, now.heightend1, now.widthbegin2, now.widthend2, now.heightbegin2, now.heightend2 });

                    if (Testbmp != null)
                    {
                        Bitmap show = new Bitmap(Testbmp);
                        FormTestImage fts = new FormTestImage(show);
                        new Thread(() => Processimage.SaveImageManual(Testbmp)) { IsBackground = true }.Start();
                        fts.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Error Take Image Camera !");
                    }
                    btnTest.Invoke(new Action(() => btnTest.Enabled = true));
                })
                { IsBackground = true }.Start();

            }
            else
            {
                MessageBox.Show("You need choose one Model");
            }

        }

        private void frmeditModel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Model.UpdateModelnow();
        }
    }
}
