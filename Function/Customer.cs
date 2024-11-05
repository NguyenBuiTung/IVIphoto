using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeimgIVI.Function
{
	public class Customer
	{
		// dùng lại
		public Customer(string username, string password, string role)
		{
			Username = username;
			Password = password;
			Position = role;
		}
		private static string INIPath = Application.StartupPath + @"\DataPackage\Program.ini";
		public string Username { get; set; }
		public string Password { get; set; }
		public string Position { get; set; }
		public static bool Passed { set; get; }
		public static string Name { set; get; }
		public static string _position { set; get; }
		public static bool AccountExistChk(string _account)
		{
			bool flag;
			if (!File.Exists(Application.StartupPath + @"\DataPackage\Login.txt"))
			{
				flag = true;
			}
			else
			{
				int index = 0;
				int num2 = Convert.ToInt32(INI.ReadIni(INIPath, "Account", "Count", ""));
				if (num2 <= 0)
				{
					flag = true;
				}
				else
				{
					StreamReader reader = new StreamReader(Application.StartupPath + @"\DataPackage\Login.txt", Encoding.Default);
					string[] strArray = new string[num2 + 1];
					strArray[index] = reader.ReadLine();
					while (true)
					{
						if (strArray[index] != "")
						{
							index++;
							if (num2 > index)
							{
								strArray[index] = reader.ReadLine();
								continue;
							}
						}
						reader.Close();
						StreamWriter writer = new StreamWriter(Application.StartupPath + @"\DataPackage\Login.txt", true, Encoding.Default);
						string[] strArray2 = new string[3];
						int num3 = num2 - 1;
						int num4 = 0;
						while (true)
						{
							if (num4 > num3)
							{
								writer.Close();
								flag = true;
							}
							else
							{
								char[] separator = new char[] { ',' };
								strArray2 = strArray[num4].Split(separator);
								if (strArray2[0] != _account)
								{
									num4++;
									continue;
								}
								writer.Close();
								flag = false;
							}
							break;
						}
						break;
					}
				}
			}
			return flag;
		}

		public static void Add_Account(string _account, string _pwd, string _position)
		{
			int index = 0;
			int num2 = Convert.ToInt32(INI.ReadIni(INIPath, "Account", "Count", ""));
			string[] strArray = new string[num2 + 1];
			if (!File.Exists(Application.StartupPath + @"\DataPackage\Login.txt"))
			{
				num2 = 0;
			}
			else
			{
				StreamReader reader = new StreamReader(Application.StartupPath + @"\DataPackage\Login.txt", Encoding.Default);
				strArray[index] = reader.ReadLine();
				while (true)
				{
					if (strArray[index] != "")
					{
						index++;
						if (num2 > index)
						{
							strArray[index] = reader.ReadLine();
							continue;
						}
					}
					reader.Close();
					File.Delete(Application.StartupPath + @"\DataPackage\Login.txt");
					break;
				}
			}
			StreamWriter writer = new StreamWriter(Application.StartupPath + @"\DataPackage\Login.txt", true, Encoding.Default);
			int num3 = num2 - 1;
			for (int i = 0; i <= num3; i++)
			{
				if (strArray[i] != null)
				{
					writer.WriteLine(strArray[i]);
				}
			}
			writer.WriteLine(_account + "," + _pwd + "," + _position);
			writer.Close();
			num2++;
			INI.WriteIni(INIPath, "Account", "Count", Convert.ToString(num2));
			MessageBox.Show("Adding '" + _account + "' account is success", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		public static void Change_Password(string _account, string _pwd)
		{
			int index = 0;
			int num2 = Convert.ToInt32(INI.ReadIni(INIPath, "Account", "Count", ""));
			StreamReader reader = new StreamReader(Application.StartupPath + @"\DataPackage\Login.txt", Encoding.Default);
			string[] strArray = new string[num2 + 1];
			strArray[index] = reader.ReadLine();
			while (true)
			{
				if (strArray[index] != "")
				{
					index++;
					if (num2 > index)
					{
						strArray[index] = reader.ReadLine();
						continue;
					}
				}
				reader.Close();
				File.Delete(Application.StartupPath + @"\Data Pakage\Login.txt");
				StreamWriter writer = new StreamWriter(Application.StartupPath + @"\DataPackage\Login.txt", true, Encoding.Default);
				string[] strArray2 = new string[3];
				int num3 = num2 - 1;
				for (int i = 0; i <= num3; i++)
				{
					char[] separator = new char[] { ',' };
					strArray2 = strArray[i].Split(separator);
					if (strArray2[0] == _account)
					{
						writer.WriteLine(_account + "," + _pwd);
					}
				}
				writer.Close();

				MessageBox.Show("Changed Password '" + _account + "' account is success", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
		}

		public static void Delete_Account(string _account)
		{
			if (File.Exists(Application.StartupPath + @"\DataPackage\Login.txt"))
			{
				int index = 0;
				int num2 = Convert.ToInt32(INI.ReadIni(INIPath, "Account", "Count", ""));
				if (num2 > 0)
				{
					StreamReader reader = new StreamReader(Application.StartupPath + @"\DataPackage\Login.txt", Encoding.Default);
					string[] strArray = new string[num2 + 1];
					strArray[index] = reader.ReadLine();
					while (true)
					{
						if (strArray[index] != "")
						{
							index++;
							if (num2 > index)
							{
								strArray[index] = reader.ReadLine();
								continue;
							}
						}
						reader.Close();
						File.Delete(Application.StartupPath + @"\DataPackage\Login.txt");
						StreamWriter writer = new StreamWriter(Application.StartupPath + @"\DataPackage\Login.txt", true, Encoding.Default);
						string[] strArray2 = new string[3];
						int num3 = num2 - 1;
						int num4 = 0;
						while (true)
						{
							if (num4 > num3)
							{
								writer.Close();
								num2--;
								INI.WriteIni(INIPath, "Account", "Count", Convert.ToString(num2));
								MessageBox.Show("Delete '" + _account + "' account is success", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								break;
							}
							char[] separator = new char[] { ',' };
							strArray2 = strArray[num4].Split(separator);
							if (strArray2[0] != _account)
							{
								writer.WriteLine(strArray[num4]);
							}
							num4++;
						}
						break;
					}
				}
			}
		}
	}
}

