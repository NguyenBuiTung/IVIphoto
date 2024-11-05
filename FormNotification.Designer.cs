namespace TakeimgIVI
{
	partial class FormNotification
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNotification));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOffBuzzer = new System.Windows.Forms.Button();
            this.btnRecheck = new System.Windows.Forms.Button();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.tmrSplash = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblContent = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSolution = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(606, 522);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnOffBuzzer
            // 
            this.btnOffBuzzer.BackColor = System.Drawing.Color.Orange;
            this.btnOffBuzzer.Font = new System.Drawing.Font("Times New Roman", 20F);
            this.btnOffBuzzer.Location = new System.Drawing.Point(50, 396);
            this.btnOffBuzzer.Name = "btnOffBuzzer";
            this.btnOffBuzzer.Size = new System.Drawing.Size(160, 71);
            this.btnOffBuzzer.TabIndex = 11;
            this.btnOffBuzzer.Text = "Buzzer Off";
            this.btnOffBuzzer.UseVisualStyleBackColor = false;
            this.btnOffBuzzer.Click += new System.EventHandler(this.btnOffBuzzer_Click);
            // 
            // btnRecheck
            // 
            this.btnRecheck.BackColor = System.Drawing.Color.PaleGreen;
            this.btnRecheck.Font = new System.Drawing.Font("Times New Roman", 20F);
            this.btnRecheck.Location = new System.Drawing.Point(403, 396);
            this.btnRecheck.Name = "btnRecheck";
            this.btnRecheck.Size = new System.Drawing.Size(148, 71);
            this.btnRecheck.TabIndex = 15;
            this.btnRecheck.Text = "Confirm";
            this.btnRecheck.UseVisualStyleBackColor = false;
            this.btnRecheck.Visible = false;
            this.btnRecheck.Click += new System.EventHandler(this.btnRecheck_Click);
            // 
            // btnReconnect
            // 
            this.btnReconnect.BackColor = System.Drawing.Color.PaleGreen;
            this.btnReconnect.Font = new System.Drawing.Font("Times New Roman", 20F);
            this.btnReconnect.Location = new System.Drawing.Point(225, 396);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(160, 71);
            this.btnReconnect.TabIndex = 14;
            this.btnReconnect.Text = "Reconnect";
            this.btnReconnect.UseVisualStyleBackColor = false;
            this.btnReconnect.Visible = false;
            this.btnReconnect.Click += new System.EventHandler(this.btnReconnect_Click);
            // 
            // tmrSplash
            // 
            this.tmrSplash.Enabled = true;
            this.tmrSplash.Interval = 200;
            this.tmrSplash.Tick += new System.EventHandler(this.tmrSplash_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Crimson;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblContent, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(38, 202);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(529, 176);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // lblContent
            // 
            this.lblContent.BackColor = System.Drawing.Color.White;
            this.lblContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblContent.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContent.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblContent.Location = new System.Drawing.Point(3, 3);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(523, 29);
            this.lblContent.TabIndex = 0;
            this.lblContent.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblContent.UseVisualStyleBackColor = false;
            this.lblContent.Click += new System.EventHandler(this.lblContent_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblSolution);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(523, 135);
            this.panel1.TabIndex = 1;
            // 
            // lblSolution
            // 
            this.lblSolution.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolution.Location = new System.Drawing.Point(0, 0);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(523, 135);
            this.lblSolution.TabIndex = 2;
            this.lblSolution.Text = "";
            // 
            // FormNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(606, 522);
            this.Controls.Add(this.btnRecheck);
            this.Controls.Add(this.btnReconnect);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnOffBuzzer);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNotification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNotification_FormClosing);
            this.Load += new System.EventHandler(this.FormNotification_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnOffBuzzer;
        private System.Windows.Forms.Button btnRecheck;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.Timer tmrSplash;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox lblSolution;
        private System.Windows.Forms.Button lblContent;
    }
}