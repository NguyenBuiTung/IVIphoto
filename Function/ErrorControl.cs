using OfficeOpenXml;
using TakeimgIVI.Function;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeimgIVI
{
    public class ErrorControl
    {
        public List<ErrorObj> Errors;
        public DataTable Data;

        public ErrorControl()
        {
            Errors = new List<ErrorObj>();
            Data = new DataTable();
            ReadExcel();
        }
        // đọc dữ liệu từ excel
        public void ReadExcel()
        {
            Errors = new List<ErrorObj>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage ex = new ExcelPackage(Constants.Url))
            {
                if (ex.Workbook.Worksheets.Count == 0)
                {
                    ex.Workbook.Worksheets.Add("ERROR");
                    ex.Workbook.Worksheets[0].Cells["A1"].Value = "Mã lỗi";
                    ex.Workbook.Worksheets[0].Cells["B1"].Value = "Nguyên nhân";
                    ex.Workbook.Worksheets[0].Cells["C1"].Value = "Giải pháp";
                    ex.Save();
                }

                ExcelWorksheet ws = ex.Workbook.Worksheets[0];
                try
                {   if(ws.Dimension.Rows>1)
                    Data = ws.Cells[1, 1, ws.Dimension.Rows, 3].ToDataTable();

                }
                catch { Data = null; }
            }
            if (Data != null)
                for (int i = 0; i < Data.Rows.Count; i++)
                {
                    string error = Data.Rows[i][0].ToString();
                    string cause = Data.Rows[i][1].ToString();
                    string solution = Data.Rows[i][2].ToString();
                    if (Errors.Where(d => d.Code == error).Count() == 0)
                    {
                        Errors.Add(new ErrorObj(error));
                    }
                    Errors.Last().Add(cause, solution);
                }
        }
    }

    public class ErrorObj
    {
        public string Code;
        public Dictionary<string, string> ErrorDetails;

        public ErrorObj(string code)
        {
            Code = code;
            ErrorDetails = new Dictionary<string, string>();
        }
        // thêm mô tả
        public void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ErrorDetails[key] = value;
            }
        }
        // xóa mô tả
        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ErrorDetails.Remove(key);
            }
        }
        // sửa mô tả
        public void Edit(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ErrorDetails[key] = value;
            }
        }
        // khởi tạo chuỗi nguyên nhân
        public string GenerateCause()
        {
            return string.Join("\n", ErrorDetails.Keys.ToList());
        }
        // khởi tạo chuỗi giải pháp
        public string GenerateSolution()
        {
            return string.Join("\n", ErrorDetails.Values.ToList());
        }
    }
}
