using TakeimgIVI.Function;
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
using System.Xml;

namespace TakeimgIVI
{
	public partial class FormLogin : Form
	{
		private XmlDocument xmlDoc = new XmlDocument();

		public FormLogin()
		{
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			if(bgW.IsBusy == false)
			{
				bgW.RunWorkerAsync();
			}
		}

		private int LoginChk(string _id, string _pwd)
		{

			int num;
			if ((_id == "ADMIN") | (_id == "admin"))
			{
				num = !((_pwd == "admin") | (_pwd == "ADMIN")) ? 1 : 0;
			}
			else if (!File.Exists(Application.StartupPath + @"\DataPackage\Login.txt"))
			{
				num = 3;
			}
			else
			{
				StreamReader reader = new StreamReader(Application.StartupPath + @"\DataPackage\Login.txt", Encoding.Default);
				string[] strArray = new string[3];
				string str = reader.ReadLine();
				while (true)
				{
					if (str == "" || str is null)
					{
						num = 2;
					}
					else
					{
						char[] separator = new char[] { ',' };
						strArray = str.Split(separator);
						if (strArray[0] != _id)
						{
							str = reader.ReadLine();
							continue;
						}
						if (strArray[1] == _pwd)
						{
							Customer._position = strArray[2];
							reader.Close();
							num = 0;
						}
						else
						{
							reader.Close();
							num = 1;
						}
					}
					break;
				}
			}
			return num;

		}

		private void FormLogin_Load(object sender, EventArgs e)
		{
			ktGhiNhoMK();
			LayThongTinDangNhap();
			btnLogin.Focus();
		}

		private void LayThongTinDangNhap()
		{
			try
			{

				xmlDoc.Load(Application.StartupPath + @"\Config.xml");
				txtUsername.Text = xmlDoc.DocumentElement.SelectSingleNode("User").Attributes["Value"].Value;
				if (ckcRemember.Checked)
				{
					txtPassword.Text = Library.GiaiMaMaStr(xmlDoc.DocumentElement.SelectSingleNode("Pw").Attributes["Value"].Value, Library.key);
				}
			}
			catch (Exception ex)
			{
				//Library.ShowBaoLoi(ex.Message);
			}
		}

		private void ktGhiNhoMK()
		{
			xmlDoc.Load(Application.StartupPath + @"\Config.xml");

			if (xmlDoc.DocumentElement.SelectSingleNode("SavePw").Attributes["Value"].Value == "True")
			{
				ckcRemember.Checked = true;
				btnLogin.Focus();
			}
			else
			{
				ckcRemember.Checked = false;
				txtUsername.Focus();
			}

		}

		private void bgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (txtUsername.Text == "")
			{
				MessageBox.Show("ID was not entered.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (txtPassword.Text == "")
			{
				MessageBox.Show("Password was not entered.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				int i = LoginChk(txtUsername.Text, txtPassword.Text);
				{
					if (i == 0)
					{
						Customer.Name = txtUsername.Text;
						Customer.Passed = true;
						this.Close();
					}
					else if (i == 1)
					{
						MessageBox.Show("Password is wrong", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this.txtUsername.Text = "";
						this.txtPassword.Text = "";
					}
					else if (i == 2)
					{
						MessageBox.Show("Input ID doesn't exist", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this.txtUsername.Text = "";
						this.txtPassword.Text = "";
					}
					else if (i == 3)
					{
						MessageBox.Show("Registered Account is not exist. Add your account to your manager account.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						this.txtUsername.Text = "";
						this.txtPassword.Text = "";
					}
				}
			}
			//Luu thong tin mat khau
			if (File.Exists(Application.StartupPath + @"\Config.xml"))
			{
				try
				{
					xmlDoc.Load(Application.StartupPath + @"\Config.xml");
					if (ckcRemember.Checked)
					{
						//(xmlDoc.DocumentElement.SelectSingleNode("Pw")).SetAttribute("Value", txtMatKhau.Text);
						xmlDoc.DocumentElement.SelectSingleNode("Pw").Attributes["Value"].InnerText = Library.MaHoaStr(txtPassword.Text, Library.key);
						xmlDoc.DocumentElement.SelectSingleNode("User").Attributes["Value"].InnerText = txtUsername.Text;
					}
					else
					{
						xmlDoc.DocumentElement.SelectSingleNode("Pw").Attributes["Value"].InnerText = Library.MaHoaStr("", Library.key);
						xmlDoc.DocumentElement.SelectSingleNode("User").Attributes["Value"].InnerText = "";
					}
					xmlDoc.DocumentElement.SelectSingleNode("SavePw").Attributes["Value"].InnerText = ckcRemember.Checked.ToString();
					xmlDoc.Save(Application.StartupPath + @"\Config.xml");
				}
				catch (Exception ex)
				{
					Library.ShowBaoLoi(ex.Message);
				}
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
