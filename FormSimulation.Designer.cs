namespace TakeimgIVI
{
    partial class FormSimulation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSimulation));
            this.cbInJig = new System.Windows.Forms.CheckBox();
            this.cbPistolMax = new System.Windows.Forms.CheckBox();
            this.cbPistolMin = new System.Windows.Forms.CheckBox();
            this.cbDoorOpen = new System.Windows.Forms.CheckBox();
            this.cbButtonClick = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStartSimualtion = new System.Windows.Forms.Button();
            this.cbIgnorDoor = new System.Windows.Forms.CheckBox();
            this.lblProcess = new System.Windows.Forms.Label();
            this.btnReady = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnMoveIn = new System.Windows.Forms.Button();
            this.btnTrigger = new System.Windows.Forms.Button();
            this.btnMoveOut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbInJig
            // 
            this.cbInJig.AutoSize = true;
            this.cbInJig.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbInJig.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.cbInJig.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbInJig.Location = new System.Drawing.Point(108, 324);
            this.cbInJig.Name = "cbInJig";
            this.cbInJig.Size = new System.Drawing.Size(119, 30);
            this.cbInJig.TabIndex = 0;
            this.cbInJig.Text = "SS_InJig";
            this.cbInJig.UseVisualStyleBackColor = true;
            this.cbInJig.CheckedChanged += new System.EventHandler(this.cbInJig_CheckedChanged);
            // 
            // cbPistolMax
            // 
            this.cbPistolMax.AutoSize = true;
            this.cbPistolMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.cbPistolMax.Location = new System.Drawing.Point(421, 304);
            this.cbPistolMax.Name = "cbPistolMax";
            this.cbPistolMax.Size = new System.Drawing.Size(130, 30);
            this.cbPistolMax.TabIndex = 0;
            this.cbPistolMax.Text = "SS_Pmax";
            this.cbPistolMax.UseVisualStyleBackColor = true;
            this.cbPistolMax.CheckedChanged += new System.EventHandler(this.cbPistolMax_CheckedChanged);
            // 
            // cbPistolMin
            // 
            this.cbPistolMin.AutoSize = true;
            this.cbPistolMin.BackColor = System.Drawing.Color.Transparent;
            this.cbPistolMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.cbPistolMin.Location = new System.Drawing.Point(589, 304);
            this.cbPistolMin.Name = "cbPistolMin";
            this.cbPistolMin.Size = new System.Drawing.Size(123, 30);
            this.cbPistolMin.TabIndex = 0;
            this.cbPistolMin.Text = "SS_PMin";
            this.cbPistolMin.UseVisualStyleBackColor = false;
            this.cbPistolMin.CheckedChanged += new System.EventHandler(this.cbPistolMin_CheckedChanged);
            // 
            // cbDoorOpen
            // 
            this.cbDoorOpen.AutoSize = true;
            this.cbDoorOpen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbDoorOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.cbDoorOpen.Location = new System.Drawing.Point(773, 86);
            this.cbDoorOpen.Name = "cbDoorOpen";
            this.cbDoorOpen.Size = new System.Drawing.Size(173, 30);
            this.cbDoorOpen.TabIndex = 0;
            this.cbDoorOpen.Text = "SS_DoorOpen";
            this.cbDoorOpen.UseVisualStyleBackColor = true;
            this.cbDoorOpen.CheckedChanged += new System.EventHandler(this.cbDoorOpen_CheckedChanged);
            // 
            // cbButtonClick
            // 
            this.cbButtonClick.AutoSize = true;
            this.cbButtonClick.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbButtonClick.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.cbButtonClick.Location = new System.Drawing.Point(43, 462);
            this.cbButtonClick.Name = "cbButtonClick";
            this.cbButtonClick.Size = new System.Drawing.Size(184, 30);
            this.cbButtonClick.TabIndex = 0;
            this.cbButtonClick.Text = "SS_ButtonClick";
            this.cbButtonClick.UseVisualStyleBackColor = true;
            this.cbButtonClick.CheckedChanged += new System.EventHandler(this.cbButtonClick_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1052, 729);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnStartSimualtion
            // 
            this.btnStartSimualtion.Location = new System.Drawing.Point(6, 12);
            this.btnStartSimualtion.Name = "btnStartSimualtion";
            this.btnStartSimualtion.Size = new System.Drawing.Size(120, 40);
            this.btnStartSimualtion.TabIndex = 2;
            this.btnStartSimualtion.Text = "Start";
            this.btnStartSimualtion.UseVisualStyleBackColor = true;
            this.btnStartSimualtion.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cbIgnorDoor
            // 
            this.cbIgnorDoor.AutoSize = true;
            this.cbIgnorDoor.Checked = true;
            this.cbIgnorDoor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIgnorDoor.Location = new System.Drawing.Point(132, 25);
            this.cbIgnorDoor.Name = "cbIgnorDoor";
            this.cbIgnorDoor.Size = new System.Drawing.Size(82, 17);
            this.cbIgnorDoor.TabIndex = 3;
            this.cbIgnorDoor.Text = "Ignore Door";
            this.cbIgnorDoor.UseVisualStyleBackColor = true;
            this.cbIgnorDoor.CheckedChanged += new System.EventHandler(this.cbIgnorDoor_CheckedChanged);
            // 
            // lblProcess
            // 
            this.lblProcess.BackColor = System.Drawing.Color.LightGreen;
            this.lblProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.lblProcess.Location = new System.Drawing.Point(485, 25);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(258, 40);
            this.lblProcess.TabIndex = 4;
            this.lblProcess.Text = "WAITING";
            this.lblProcess.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReady
            // 
            this.btnReady.BackColor = System.Drawing.Color.LawnGreen;
            this.btnReady.Location = new System.Drawing.Point(6, 67);
            this.btnReady.Name = "btnReady";
            this.btnReady.Size = new System.Drawing.Size(120, 40);
            this.btnReady.TabIndex = 5;
            this.btnReady.Text = "READY";
            this.btnReady.UseVisualStyleBackColor = false;
            this.btnReady.Click += new System.EventHandler(this.btnReady_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LawnGreen;
            this.btnStart.Location = new System.Drawing.Point(6, 113);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 40);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click_1);
            // 
            // btnMoveIn
            // 
            this.btnMoveIn.BackColor = System.Drawing.Color.LawnGreen;
            this.btnMoveIn.Location = new System.Drawing.Point(6, 159);
            this.btnMoveIn.Name = "btnMoveIn";
            this.btnMoveIn.Size = new System.Drawing.Size(120, 40);
            this.btnMoveIn.TabIndex = 5;
            this.btnMoveIn.Text = "MOVE IN";
            this.btnMoveIn.UseVisualStyleBackColor = false;
            this.btnMoveIn.Click += new System.EventHandler(this.btnMoveIn_Click);
            // 
            // btnTrigger
            // 
            this.btnTrigger.BackColor = System.Drawing.Color.LawnGreen;
            this.btnTrigger.Location = new System.Drawing.Point(6, 205);
            this.btnTrigger.Name = "btnTrigger";
            this.btnTrigger.Size = new System.Drawing.Size(120, 40);
            this.btnTrigger.TabIndex = 5;
            this.btnTrigger.Text = "TRIGGER";
            this.btnTrigger.UseVisualStyleBackColor = false;
            this.btnTrigger.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // btnMoveOut
            // 
            this.btnMoveOut.BackColor = System.Drawing.Color.LawnGreen;
            this.btnMoveOut.Location = new System.Drawing.Point(6, 251);
            this.btnMoveOut.Name = "btnMoveOut";
            this.btnMoveOut.Size = new System.Drawing.Size(120, 40);
            this.btnMoveOut.TabIndex = 5;
            this.btnMoveOut.Text = "MOVE OUT";
            this.btnMoveOut.UseVisualStyleBackColor = false;
            this.btnMoveOut.Click += new System.EventHandler(this.btnMoveOut_Click);
            // 
            // FormSimulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 729);
            this.Controls.Add(this.btnMoveOut);
            this.Controls.Add(this.btnTrigger);
            this.Controls.Add(this.btnMoveIn);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnReady);
            this.Controls.Add(this.lblProcess);
            this.Controls.Add(this.cbIgnorDoor);
            this.Controls.Add(this.btnStartSimualtion);
            this.Controls.Add(this.cbButtonClick);
            this.Controls.Add(this.cbDoorOpen);
            this.Controls.Add(this.cbPistolMin);
            this.Controls.Add(this.cbPistolMax);
            this.Controls.Add(this.cbInJig);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSimulation";
            this.Text = "Simulation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSimulation_FormClosing);
            this.Load += new System.EventHandler(this.FormSimulation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbInJig;
        private System.Windows.Forms.CheckBox cbPistolMax;
        private System.Windows.Forms.CheckBox cbPistolMin;
        private System.Windows.Forms.CheckBox cbDoorOpen;
        private System.Windows.Forms.CheckBox cbButtonClick;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStartSimualtion;
        private System.Windows.Forms.CheckBox cbIgnorDoor;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.Button btnReady;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnMoveIn;
        private System.Windows.Forms.Button btnTrigger;
        private System.Windows.Forms.Button btnMoveOut;
    }
}