namespace Hra_člověče_nezlob_se
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnHodit = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pictureBoxDeska = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDeska)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHodit
            // 
            this.btnHodit.Location = new System.Drawing.Point(900, 220);
            this.btnHodit.Name = "btnHodit";
            this.btnHodit.Size = new System.Drawing.Size(300, 150);
            this.btnHodit.TabIndex = 1;
            this.btnHodit.Text = "Kostka";
            this.btnHodit.UseVisualStyleBackColor = true;
            this.btnHodit.Click += new System.EventHandler(this.btnHodit_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblInfo.Location = new System.Drawing.Point(700, 10);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(122, 32);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Na řadě:";
            // 
            // pictureBoxDeska
            // 
            this.pictureBoxDeska.BackColor = System.Drawing.Color.White;
            this.pictureBoxDeska.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxDeska.Location = new System.Drawing.Point(10, 10);
            this.pictureBoxDeska.Name = "pictureBoxDeska";
            this.pictureBoxDeska.Size = new System.Drawing.Size(600, 600);
            this.pictureBoxDeska.TabIndex = 2;
            this.pictureBoxDeska.TabStop = false;
            this.pictureBoxDeska.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxDeska_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 653);
            this.Controls.Add(this.pictureBoxDeska);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnHodit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Člověče nezlob se";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDeska)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHodit;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.PictureBox pictureBoxDeska;
    }
}

