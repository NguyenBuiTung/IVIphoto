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

namespace TakeimgIVI
{
	public partial class FormAccount : Form
	{
		public FormAccount()
		{
			InitializeComponent();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if ((txtAccount.Text != "" && txtPassword.Text != "" && cbbRole.Text != "") && (MessageBox.Show("Do you want to add " + this.txtAccount.Text + " Account ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
			{
				if (Customer._position == "Engineer")
				{
					if (!Customer.AccountExistChk(txtAccount.Text))
					{
						MessageBox.Show("This account is already Registered.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
					else
					{
						Customer.Add_Account(txtAccount.Text, txtPassword.Text, cbbRole.Text);
						this.Initialize_AccountList();
					}
				}
				else
				{
					MessageBox.Show("You Can't Add Account");
				}
			}
			txtAccount.Text = "";
			txtPassword.Text = "";
			this.txtAccount.Focus();
		}

		private void FormAccount_Load(object sender, EventArgs e)
		{
			DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
			col1.DataPropertyName = "username";
			col1.HeaderText = "User Name";
			col1.Width = 100;
			DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
			col2.DataPropertyName = "password";
			col2.HeaderText = "Password";
			col2.Width = 100;
			DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
			col3.DataPropertyName = "position";
			col3.HeaderText = "Position";
			col3.Width = 100;
			dtgvAcc.Columns.Add(col1);
			dtgvAcc.Columns.Add(col2);
			dtgvAcc.Columns.Add(col3);
			Initialize_AccountList();
		}

		private void Initialize_AccountList()
		{
			try
			{
				if (!File.Exists(Application.StartupPath + @"\DataPackage\LogIn.txt"))
				{
					MessageBox.Show("Registered Account is not exist.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{

					StreamReader reader = new StreamReader(Application.StartupPath + @"\DataPackage\LogIn.txt", Encoding.Default);
					string str = "";
					string[] strArray = new string[3];
					List<Customer> customers = new List<Customer>();
					str = reader.ReadLine();
					while (true)
					{
						if (str == "" || str is null)
						{
							reader.Close();
							break;
						}
						char[] separator = new char[] { ',' };
						strArray = str.Split(separator);
						customers.Add(new Customer(strArray[0], strArray[1], strArray[2]));
						str = reader.ReadLine();
					}
					dtgvAcc.DataSource = customers;
				}
			}
			catch (Exception exception1)
			{
				Log.LogEvent(exception1.Message, 3);
				MessageBox.Show("Fail to Initialize Account List.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Do you want to Delete Seleted Account ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Customer.Delete_Account(dtgvAcc.CurrentRow.Cells[0].Value.ToString());
				this.Initialize_AccountList();
			}
		}

        private void btnClose_Click(object sender, EventArgs e)
        {
			Close();
        }
    }
}
