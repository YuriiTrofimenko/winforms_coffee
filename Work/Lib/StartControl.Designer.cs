namespace Work.Lib
{
    partial class StartControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picPB = new System.Windows.Forms.PictureBox();
            this.pointPB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointPB)).BeginInit();
            this.SuspendLayout();
            // 
            // picPB
            // 
            this.picPB.Image = global::Work.Properties.Resources.Start;
            this.picPB.Location = new System.Drawing.Point(3, 186);
            this.picPB.Name = "picPB";
            this.picPB.Size = new System.Drawing.Size(400, 400);
            this.picPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPB.TabIndex = 2;
            this.picPB.TabStop = false;
            // 
            // pointPB
            // 
            this.pointPB.Image = global::Work.Properties.Resources.pointerGIF;
            this.pointPB.Location = new System.Drawing.Point(135, 15);
            this.pointPB.Name = "pointPB";
            this.pointPB.Size = new System.Drawing.Size(132, 165);
            this.pointPB.TabIndex = 1;
            this.pointPB.TabStop = false;
            // 
            // StartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.picPB);
            this.Controls.Add(this.pointPB);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "StartControl";
            this.Size = new System.Drawing.Size(406, 589);
            this.Load += new System.EventHandler(this.StartControl_Load);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.StartControl_ControlRemoved);
            ((System.ComponentModel.ISupportInitialize)(this.picPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointPB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pointPB;
        private System.Windows.Forms.PictureBox picPB;
    }
}
