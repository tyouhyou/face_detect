
namespace zb.FaceDetect
{
    partial class FmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbMain = new System.Windows.Forms.PictureBox();
            this.btnPicture = new System.Windows.Forms.Button();
            this.btnCamera = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbDeviceList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(12, 12);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(1027, 600);
            this.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMain.TabIndex = 0;
            this.pbMain.TabStop = false;
            // 
            // btnPicture
            // 
            this.btnPicture.Location = new System.Drawing.Point(1045, 12);
            this.btnPicture.Name = "btnPicture";
            this.btnPicture.Size = new System.Drawing.Size(160, 52);
            this.btnPicture.TabIndex = 1;
            this.btnPicture.Text = "Browse Picture";
            this.btnPicture.UseVisualStyleBackColor = true;
            this.btnPicture.Click += new System.EventHandler(this.btnPicture_Click);
            // 
            // btnCamera
            // 
            this.btnCamera.Location = new System.Drawing.Point(1045, 171);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(160, 52);
            this.btnCamera.TabIndex = 2;
            this.btnCamera.Text = "Open Camera";
            this.btnCamera.UseVisualStyleBackColor = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1045, 560);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(160, 52);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbDeviceList
            // 
            this.cmbDeviceList.FormattingEnabled = true;
            this.cmbDeviceList.Location = new System.Drawing.Point(1045, 137);
            this.cmbDeviceList.Name = "cmbDeviceList";
            this.cmbDeviceList.Size = new System.Drawing.Size(160, 28);
            this.cmbDeviceList.TabIndex = 4;
            // 
            // FmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 626);
            this.Controls.Add(this.cmbDeviceList);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCamera);
            this.Controls.Add(this.btnPicture);
            this.Controls.Add(this.pbMain);
            this.Name = "FmMain";
            this.Text = ":)";
            this.Load += new System.EventHandler(this.FmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbMain;
        private System.Windows.Forms.Button btnPicture;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cmbDeviceList;
    }
}

