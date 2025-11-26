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
            this.btnHodKostkou = new System.Windows.Forms.Button();
            this.lblkostka = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHodKostkou
            // 
            this.btnHodKostkou.Location = new System.Drawing.Point(12, 12);
            this.btnHodKostkou.Name = "btnHodKostkou";
            this.btnHodKostkou.Size = new System.Drawing.Size(108, 57);
            this.btnHodKostkou.TabIndex = 0;
            this.btnHodKostkou.Text = "Kostka";
            this.btnHodKostkou.UseVisualStyleBackColor = true;
            this.btnHodKostkou.Click += new System.EventHandler(this.btnHodKostkou_Click);
            // 
            // lblkostka
            // 
            this.lblkostka.AutoSize = true;
            this.lblkostka.Location = new System.Drawing.Point(126, 32);
            this.lblkostka.Name = "lblkostka";
            this.lblkostka.Size = new System.Drawing.Size(0, 16);
            this.lblkostka.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(26, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 51);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1393, 252);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblkostka);
            this.Controls.Add(this.btnHodKostkou);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHodKostkou;
        private System.Windows.Forms.Label lblkostka;
        private System.Windows.Forms.Button button1;
    }
}

