namespace Work.Lib
{
    partial class Finish
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPrintCheck = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbPrintCheck = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrintCheck
            // 
            this.btnPrintCheck.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrintCheck.BackgroundImage = global::Work.Properties.Resources.printer;
            this.btnPrintCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrintCheck.Location = new System.Drawing.Point(435, 214);
            this.btnPrintCheck.Name = "btnPrintCheck";
            this.btnPrintCheck.Size = new System.Drawing.Size(54, 54);
            this.btnPrintCheck.TabIndex = 1;
            this.btnPrintCheck.UseVisualStyleBackColor = true;
            this.btnPrintCheck.Click += new System.EventHandler(this.btnPrintCheck_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Work.Properties.Resources.Thanks;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 156);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lbPrintCheck
            // 
            this.lbPrintCheck.AutoSize = true;
            this.lbPrintCheck.Location = new System.Drawing.Point(428, 198);
            this.lbPrintCheck.Name = "lbPrintCheck";
            this.lbPrintCheck.Size = new System.Drawing.Size(69, 13);
            this.lbPrintCheck.TabIndex = 2;
            this.lbPrintCheck.Text = "Печать чека";
            // 
            // Finish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbPrintCheck);
            this.Controls.Add(this.btnPrintCheck);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Finish";
            this.Size = new System.Drawing.Size(500, 271);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnPrintCheck;
        private System.Windows.Forms.Label lbPrintCheck;
    }
}
