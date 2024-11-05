namespace TakeimgIVI
{
    partial class frmmodel
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cboxmodel = new System.Windows.Forms.ComboBox();
            this.btnchoose = new System.Windows.Forms.Button();
            this.imagemodel = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagemodel)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.18239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.81761F));
            this.tableLayoutPanel1.Controls.Add(this.cboxmodel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnchoose, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.imagemodel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(582, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cboxmodel
            // 
            this.cboxmodel.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.cboxmodel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboxmodel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxmodel.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxmodel.ForeColor = System.Drawing.SystemColors.Window;
            this.cboxmodel.FormattingEnabled = true;
            this.cboxmodel.Location = new System.Drawing.Point(3, 3);
            this.cboxmodel.Name = "cboxmodel";
            this.cboxmodel.Size = new System.Drawing.Size(396, 39);
            this.cboxmodel.TabIndex = 0;
            this.cboxmodel.Text = "Model Name";
            this.cboxmodel.SelectedIndexChanged += new System.EventHandler(this.cboxmodel_SelectedIndexChanged);
            // 
            // btnchoose
            // 
            this.btnchoose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnchoose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnchoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnchoose.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnchoose.Location = new System.Drawing.Point(405, 3);
            this.btnchoose.Name = "btnchoose";
            this.btnchoose.Size = new System.Drawing.Size(174, 40);
            this.btnchoose.TabIndex = 1;
            this.btnchoose.Text = "Choose";
            this.btnchoose.UseVisualStyleBackColor = false;
            this.btnchoose.Click += new System.EventHandler(this.btnchoose_Click);
            // 
            // imagemodel
            // 
            this.imagemodel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.imagemodel, 2);
            this.imagemodel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagemodel.Location = new System.Drawing.Point(3, 49);
            this.imagemodel.Name = "imagemodel";
            this.imagemodel.Size = new System.Drawing.Size(576, 409);
            this.imagemodel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imagemodel.TabIndex = 2;
            this.imagemodel.TabStop = false;
            // 
            // frmmodel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(582, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmmodel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MODEL";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmmodel_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagemodel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cboxmodel;
        private System.Windows.Forms.Button btnchoose;
        private System.Windows.Forms.PictureBox imagemodel;
    }
}