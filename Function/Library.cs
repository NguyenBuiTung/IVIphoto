using System;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Security.Cryptography;
namespace TakeimgIVI.Function
{
	public static class Library
	{
		// dùng lại
		public static string DauMay = "";

		public static string TenMay = "";

		public static bool isQuanLy = false;

		public static string IPAddress = "";

		public static string key = "p@ssw0rd";


		public static void ShowBaoLoi(string NoiDung)
		{
			MessageBox.Show(NoiDung, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void ShowCanhBao(string NoiDung)
		{
			MessageBox.Show(NoiDung, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static void ShowThongBao(string NoiDung)
		{
			MessageBox.Show(NoiDung, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public static void ShowCauHoi(string NoiDung)
		{
			MessageBox.Show(NoiDung, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Question);
		}

		public static String CatChuoi_ViTri(string DSFile = "", string FieldName = "File", char _char = ';', int _ViTri = 0)
		{
			DataTable tb = new DataTable();
			tb.Columns.Add(FieldName);
			if (DSFile != "")
			{
				string[] listFile = DSFile.ToString().Split(new char[] { _char });
				foreach (var file in listFile)
				{
					if (file != "")
					{
						tb.Rows.Add(tb.NewRow());
						tb.Rows[tb.Rows.Count - 1][FieldName] = file;
					}
				}
			}
			if (tb.Rows.Count > _ViTri)
			{
				return (string)tb.Rows[_ViTri][FieldName];
			}
			else
			{
				return "";
			}
		}

		public static string MaHoaStr(string strText, string strEncrKey)
		{
			byte[] IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
			try
			{
				byte[] bykey = System.Text.Encoding.UTF8.GetBytes(strEncrKey);
				byte[] InputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write);
				cs.Write(InputByteArray, 0, InputByteArray.Length);
				cs.FlushFinalBlock();
				return Convert.ToBase64String(ms.ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public static string GiaiMaMaStr(string strText, string sDecrKey)
		{
			byte[] IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
			byte[] inputByteArray = new byte[strText.Length + 1];
			try
			{
				byte[] byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey);
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				inputByteArray = Convert.FromBase64String(strText);
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0, inputByteArray.Length);
				cs.FlushFinalBlock();
				System.Text.Encoding encoding = System.Text.Encoding.UTF8;
				return encoding.GetString(ms.ToArray());
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

	}
}
