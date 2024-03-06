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
            this.btnManoObra = new System.Windows.Forms.PictureBox();
            this.btnTapiceria = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMateriaPrima)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnManoObra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTapiceria)).BeginInit();
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
            this.btnMateriaPrima.Location = new System.Drawing.Point(82, 298);
            this.btnMateriaPrima.Name = "btnMateriaPrima";
            this.btnMateriaPrima.Size = new System.Drawing.Size(66, 61);
            this.btnMateriaPrima.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnMateriaPrima.TabIndex = 1;
            this.btnMateriaPrima.TabStop = false;
            this.btnMateriaPrima.Click += new System.EventHandler(this.btnMateriaPrima_Click);
            // 
            // btnManoObra
            // 
            this.btnManoObra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManoObra.Image = ((System.Drawing.Image)(resources.GetObject("btnManoObra.Image")));
            this.btnManoObra.Location = new System.Drawing.Point(686, 114);
            this.btnManoObra.Name = "btnManoObra";
            this.btnManoObra.Size = new System.Drawing.Size(66, 61);
            this.btnManoObra.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnManoObra.TabIndex = 2;
            this.btnManoObra.TabStop = false;
            this.btnManoObra.Click += new System.EventHandler(this.btnManoObra_Click);
            // 
            // btnTapiceria
            // 
            this.btnTapiceria.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTapiceria.Image = ((System.Drawing.Image)(resources.GetObject("btnTapiceria.Image")));
            this.btnTapiceria.Location = new System.Drawing.Point(686, 298);
            this.btnTapiceria.Name = "btnTapiceria";
            this.btnTapiceria.Size = new System.Drawing.Size(66, 61);
            this.btnTapiceria.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnTapiceria.TabIndex = 3;
            this.btnTapiceria.TabStop = false;
            this.btnTapiceria.Click += new System.EventHandler(this.btnTapiceria_Click);
            // 
            // FInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTapiceria);
            this.Controls.Add(this.btnManoObra);
            this.Controls.Add(this.btnMateriaPrima);
            this.Controls.Add(this.btnProducto);
            this.Name = "FInicio";
            this.Text = "FInicio";
            ((System.ComponentModel.ISupportInitialize)(this.btnProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMateriaPrima)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnManoObra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTapiceria)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnProducto;
        private System.Windows.Forms.PictureBox btnMateriaPrima;
        private System.Windows.Forms.PictureBox btnManoObra;
        private System.Windows.Forms.PictureBox btnTapiceria;
    }
}