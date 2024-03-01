namespace NovoCosts.Forms
{
    partial class FInicio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInicio));
            this.btnProducto = new System.Windows.Forms.PictureBox();
            this.btnMateriaPrima = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMateriaPrima)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProducto
            // 
            this.btnProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProducto.Image = ((System.Drawing.Image)(resources.GetObject("btnProducto.Image")));
            this.btnProducto.Location = new System.Drawing.Point(686, 208);
            this.btnProducto.Name = "btnProducto";
            this.btnProducto.Size = new System.Drawing.Size(66, 61);
            this.btnProducto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnProducto.TabIndex = 0;
            this.btnProducto.TabStop = false;
            this.btnProducto.Click += new System.EventHandler(this.btnProducto_Click);
            // 
            // btnMateriaPrima
            // 
            this.btnMateriaPrima.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMateriaPrima.Image = ((System.Drawing.Image)(resources.GetObject("btnMateriaPrima.Image")));
            this.btnMateriaPrima.Location = new System.Drawing.Point(686, 298);
            this.btnMateriaPrima.Name = "btnMateriaPrima";
            this.btnMateriaPrima.Size = new System.Drawing.Size(66, 61);
            this.btnMateriaPrima.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnMateriaPrima.TabIndex = 1;
            this.btnMateriaPrima.TabStop = false;
            this.btnMateriaPrima.Click += new System.EventHandler(this.btnMateriaPrima_Click);
            // 
            // FInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnMateriaPrima);
            this.Controls.Add(this.btnProducto);
            this.Name = "FInicio";
            this.Text = "FInicio";
            ((System.ComponentModel.ISupportInitialize)(this.btnProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMateriaPrima)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnProducto;
        private System.Windows.Forms.PictureBox btnMateriaPrima;
    }
}