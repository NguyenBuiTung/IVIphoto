namespace TakeimgIVI
{
    partial class FormManual
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManual));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnScanDisconnect = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnCloseDevice = new System.Windows.Forms.Button();
            this.manTriggerCam = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnckIO = new System.Windows.Forms.Button();
            this.btnOffBuzzer = new System.Windows.Forms.Button();
            this.btnResetError = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TimerSignal = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btndiscam2 = new System.Windows.Forms.Button();
            this.manTriggerCam2 = new System.Windows.Forms.Button();
            this.btnEditmodel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnCameratest = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnScanDisconnect);
            this.groupBox2.Controls.Add(this.btnScan);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(9, 258);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 77);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Send Command To Barcode";
            // 
            // btnScanDisconnect
            // 
            this.btnScanDisconnect.BackColor = System.Drawing.Color.DarkRed;
            this.btnScanDisconnect.Location = new System.Drawing.Point(173, 25);
            this.btnScanDisconnect.Name = "btnScanDisconnect";
            this.btnScanDisconnect.Size = new System.Drawing.Size(122, 40);
            this.btnScanDisconnect.TabIndex = 1;
            this.btnScanDisconnect.Text = "Disconnect";
            this.btnScanDisconnect.UseVisualStyleBackColor = false;
            this.btnScanDisconnect.Click += new System.EventHandler(this.btnScanDisconnect_Click);
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnScan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnScan.Location = new System.Drawing.Point(32, 25);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(122, 40);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Trigger Barcode";
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnCloseDevice
            // 
            this.btnCloseDevice.BackColor = System.Drawing.Color.DarkRed;
            this.btnCloseDevice.Location = new System.Drawing.Point(173, 24);
            this.btnCloseDevice.Name = "btnCloseDevice";
            this.btnCloseDevice.Size = new System.Drawing.Size(122, 40);
            this.btnCloseDevice.TabIndex = 0;
            this.btnCloseDevice.Text = "Disconnect";
            this.btnCloseDevice.UseVisualStyleBackColor = false;
            this.btnCloseDevice.Click += new System.EventHandler(this.btnCloseDevice_Click);
            // 
            // manTriggerCam
            // 
            this.manTriggerCam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.manTriggerCam.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.manTriggerCam.Location = new System.Drawing.Point(32, 25);
            this.manTriggerCam.Name = "manTriggerCam";
            this.manTriggerCam.Size = new System.Drawing.Size(122, 39);
            this.manTriggerCam.TabIndex = 50;
            this.manTriggerCam.Text = "Trigger Camera";
            this.manTriggerCam.UseVisualStyleBackColor = false;
            this.manTriggerCam.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnckIO);
            this.groupBox1.Controls.Add(this.btnOffBuzzer);
            this.groupBox1.Controls.Add(this.btnResetError);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(9, 337);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 135);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send Command To ADAM";
            // 
            // btnckIO
            // 
            this.btnckIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnckIO.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnckIO.Location = new System.Drawing.Point(32, 79);
            this.btnckIO.Name = "btnckIO";
            this.btnckIO.Size = new System.Drawing.Size(122, 40);
            this.btnckIO.TabIndex = 52;
            this.btnckIO.Text = "Check IO";
            this.btnckIO.UseVisualStyleBackColor = false;
            this.btnckIO.Click += new System.EventHandler(this.btnckIO_Click);
            // 
            // btnOffBuzzer
            // 
            this.btnOffBuzzer.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btnOffBuzzer.BackColor = System.Drawing.Color.DarkGreen;
            this.btnOffBuzzer.Location = new System.Drawing.Point(32, 29);
            this.btnOffBuzzer.Name = "btnOffBuzzer";
            this.btnOffBuzzer.Size = new System.Drawing.Size(122, 40);
            this.btnOffBuzzer.TabIndex = 8;
            this.btnOffBuzzer.Text = "Off Buzzer";
            this.btnOffBuzzer.UseVisualStyleBackColor = false;
            this.btnOffBuzzer.Click += new System.EventHandler(this.btnOffBuzzer_Click);
            // 
            // btnResetError
            // 
            this.btnResetError.BackColor = System.Drawing.Color.Olive;
            this.btnResetError.Location = new System.Drawing.Point(173, 29);
            this.btnResetError.Name = "btnResetError";
            this.btnResetError.Size = new System.Drawing.Size(122, 40);
            this.btnResetError.TabIndex = 7;
            this.btnResetError.Text = "Reset Error";
            this.btnResetError.UseVisualStyleBackColor = false;
            this.btnResetError.Click += new System.EventHandler(this.btnResetError_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCloseDevice);
            this.groupBox3.Controls.Add(this.manTriggerCam);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox3.Location = new System.Drawing.Point(9, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(319, 76);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Send Command To Camera 1";
            // 
            // TimerSignal
            // 
            this.TimerSignal.Enabled = true;
            this.TimerSignal.Interval = 300;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btndiscam2);
            this.groupBox4.Controls.Add(this.manTriggerCam2);
            this.groupBox4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox4.Location = new System.Drawing.Point(9, 91);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(319, 76);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Send Command To Camera 2";
            // 
            // btndiscam2
            // 
            this.btndiscam2.BackColor = System.Drawing.Color.DarkRed;
            this.btndiscam2.Location = new System.Drawing.Point(173, 24);
            this.btndiscam2.Name = "btndiscam2";
            this.btndiscam2.Size = new System.Drawing.Size(122, 40);
            this.btndiscam2.TabIndex = 0;
            this.btndiscam2.Text = "Disconnect";
            this.btndiscam2.UseVisualStyleBackColor = false;
            this.btndiscam2.Click += new System.EventHandler(this.btndiscam2_Click);
            // 
            // manTriggerCam2
            // 
            this.manTriggerCam2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.manTriggerCam2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.manTriggerCam2.Location = new System.Drawing.Point(32, 25);
            this.manTriggerCam2.Name = "manTriggerCam2";
            this.manTriggerCam2.Size = new System.Drawing.Size(122, 39);
            this.manTriggerCam2.TabIndex = 50;
            this.manTriggerCam2.Text = "Trigger Camera";
            this.manTriggerCam2.UseVisualStyleBackColor = false;
            this.manTriggerCam2.Click += new System.EventHandler(this.manTriggerCam2_Click);
            // 
            // btnEditmodel
            // 
            this.btnEditmodel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnEditmodel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditmodel.Location = new System.Drawing.Point(96, 484);
            this.btnEditmodel.Name = "btnEditmodel";
            this.btnEditmodel.Size = new System.Drawing.Size(146, 40);
            this.btnEditmodel.TabIndex = 2;
            this.btnEditmodel.Text = "Edit Model Config";
            this.btnEditmodel.UseVisualStyleBackColor = false;
            this.btnEditmodel.Click += new System.EventHandler(this.btnEditmodel_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.btnEditmodel);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 537);
            this.panel1.TabIndex = 7;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnCameratest);
            this.groupBox5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox5.Location = new System.Drawing.Point(9, 176);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(319, 76);
            this.groupBox5.TabIndex = 51;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Send Command To 2 Camera ";
            // 
            // btnCameratest
            // 
            this.btnCameratest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCameratest.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCameratest.Location = new System.Drawing.Point(32, 25);
            this.btnCameratest.Name = "btnCameratest";
            this.btnCameratest.Size = new System.Drawing.Size(122, 39);
            this.btnCameratest.TabIndex = 50;
            this.btnCameratest.Text = "Trigger Camera";
            this.btnCameratest.UseVisualStyleBackColor = false;
            this.btnCameratest.Click += new System.EventHandler(this.btnCameratest_Click);
            // 
            // FormManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(340, 537);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(360, 580);
            this.MinimumSize = new System.Drawing.Size(360, 444);
            this.Name = "FormManual";
            this.Text = "Manual";
            this.Load += new System.EventHandler(this.FormManual_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnCloseDevice;
        private System.Windows.Forms.Button manTriggerCam;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Timer TimerSignal;
        private System.Windows.Forms.Button btnScanDisconnect;
        private System.Windows.Forms.Button btnResetError;
        private System.Windows.Forms.Button btnOffBuzzer;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btndiscam2;
        private System.Windows.Forms.Button manTriggerCam2;
        private System.Windows.Forms.Button btnEditmodel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnCameratest;
        private System.Windows.Forms.Button btnckIO;
    }
}