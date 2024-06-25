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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FInicio));
            this.btnProducto = new System.Windows.Forms.PictureBox();
            this.btnMateriaPrima = new System.Windows.Forms.PictureBox();
            this.btnManoObra = new System.Windows.Forms.PictureBox();
            this.btnCostos = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnNombre = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.btnProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMateriaPrima)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnManoObra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCostos)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProducto
            // 
            this.btnProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProducto.Image = ((System.Drawing.Image)(resources.GetObject("btnProducto.Image")));
            this.btnProducto.Location = new System.Drawing.Point(111, 76);
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
            this.btnMateriaPrima.Location = new System.Drawing.Point(215, 38);
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
            this.btnManoObra.Location = new System.Drawing.Point(16, 38);
            this.btnManoObra.Name = "btnManoObra";
            this.btnManoObra.Size = new System.Drawing.Size(66, 61);
            this.btnManoObra.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnManoObra.TabIndex = 2;
            this.btnManoObra.TabStop = false;
            this.btnManoObra.Tag = "Mano de Obra";
            this.btnManoObra.Click += new System.EventHandler(this.btnManoObra_Click);
            // 
            // btnCostos
            // 
            this.btnCostos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCostos.Image = ((System.Drawing.Image)(resources.GetObject("btnCostos.Image")));
            this.btnCostos.Location = new System.Drawing.Point(391, 38);
            this.btnCostos.Name = "btnCostos";
            this.btnCostos.Size = new System.Drawing.Size(81, 84);
            this.btnCostos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnCostos.TabIndex = 79;
            this.btnCostos.TabStop = false;
            this.btnCostos.Click += new System.EventHandler(this.btnCostos_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.groupBox1.Controls.Add(this.btnManoObra);
            this.groupBox1.Controls.Add(this.btnCostos);
            this.groupBox1.Controls.Add(this.btnMateriaPrima);
            this.groupBox1.Controls.Add(this.btnProducto);
            this.groupBox1.Location = new System.Drawing.Point(12, 203);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 160);
            this.groupBox1.TabIndex = 80;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registro";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(493, 162);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 81;
            this.pictureBox1.TabStop = false;
            // 
            // FInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(516, 372);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FInicio";
            this.Text = "Inicio";
            this.Load += new System.EventHandler(this.FInicio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMateriaPrima)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnManoObra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCostos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnProducto;
        private System.Windows.Forms.PictureBox btnMateriaPrima;
        private System.Windows.Forms.PictureBox btnManoObra;
        private System.Windows.Forms.PictureBox btnCostos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip btnNombre;
    }
}