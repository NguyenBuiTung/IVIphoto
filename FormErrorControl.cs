using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Controls;
using TakeimgIVI.Function;

namespace TakeimgIVI
{
    public partial class FormErrorControl : Form
    {
        public FormErrorControl()
        {
            InitializeComponent();
        }
        // ấn lưu
        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnEdit.Enabled = true;
            dgvError.ReadOnly = true;
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn column in dgvError.Columns)
            {
                dt.Columns.Add(column.HeaderText);
            }

            for (int i = 0; i < dgvError.Rows.Count; i++)
            {
                dt.Rows.Add();
                for (int j = 0; j < dgvError.Columns.Count; j++)
                {
                    if (dgvError.Rows[i].Cells[0].Value == null)
                    {
                        dt.Rows.RemoveAt(dt.Rows.Count - 1);
                        break;
                    }
                    string str = dgvError.Rows[i].Cells[j].Value != null ? dgvError.Rows[i].Cells[j].Value.ToString() : "";
                    dt.Rows[i][j] = str;
                }
            }
            dt.DefaultView.Sort = dt.Columns[0].ColumnName + " " + "ASC";
            dt = dt.DefaultView.ToTable();

            if (!File.Exists(tbUrl.Text))
            {
                try
                {
                    File.Create(tbUrl.Text).Close();
                }
                catch
                {
                    MessageBox.Show("Cannot create new file!");
                }
            }
            using (ExcelPackage ex = new ExcelPackage(tbUrl.Text))
            {
                ex.Workbook.Worksheets.Delete(0);
                if(ex.Workbook.Worksheets.Count == 0)
                {
                    ex.Workbook.Worksheets.Add("Error");
                }
                ExcelWorksheet ws = ex.Workbook.Worksheets[0];
                
                ws.Cells["A1"].Value = dgvcError.HeaderText;
                ws.Cells["B1"].Value = dgvcCause.HeaderText;
                ws.Cells["C1"].Value = dgvcSolution.HeaderText;

                ws.Cells["A2"].LoadFromDataTable(dt);
                FormMain.ErrorList.Data = dt;
                ws.Columns.AutoFit();
                ex.Save();
            }
            FormMain.ErrorList.ReadExcel();
        }
        // ấn edit
        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            dgvError.ReadOnly = false;
        }
        // ấn lấy đường dẫn
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Constants.Url = tbUrl.Text = ofd.FileName.Replace("\\", "/");
                INI.WriteIni(Constants.Common, "Url", "Url", Constants.Url);
            }
        }
        // ấn thêm
        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
        // ấn xóa
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvError.SelectedRows.Count > 0)
            {
                for(int i = 0;i < dgvError.SelectedRows.Count; i++)
                {
                    dgvError.Rows.Remove(dgvError.SelectedRows[i]);
                }
            }
        }
        // tải ui
        private void FormErrorControl_Load(object sender, EventArgs e)
        {
            tbUrl.Text = Constants.Url;
            string[] code = Enum.GetNames(typeof(ERRORCODE)).ToArray();
            dgvcError.Items.AddRange(code);
            for (int i = 0; i < FormMain.ErrorList.Data.Rows.Count; i++)
            {
                string error = FormMain.ErrorList.Data.Rows[i][0].ToString();
                if(!code.Contains(error))
                {
                    MessageBox.Show("Error code isn't including: " + error);
                    continue;
                }
                string cause = FormMain.ErrorList.Data.Rows[i][1].ToString();
                string solution = FormMain.ErrorList.Data.Rows[i][2].ToString();
                dgvError.Rows.Add(error, cause, solution);
            }
        }
    }
}
