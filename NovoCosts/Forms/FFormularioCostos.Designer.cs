namespace NovoCosts.Forms
{
    partial class FFormularioCostos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFormularioCostos));
            this.btnLimpiar = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnInicio = new System.Windows.Forms.PictureBox();
            this.Costos = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvCosto = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.btnLimpiar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInicio)).BeginInit();
            this.Costos.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCosto)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.Image = ((System.Drawing.Image)(resources.GetObject("btnLimpiar.Image")));
            this.btnLimpiar.Location = new System.Drawing.Point(1027, 97);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(64, 55);
            this.btnLimpiar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnLimpiar.TabIndex = 57;
            this.btnLimpiar.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1010, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 58;
            this.pictureBox1.TabStop = false;
            // 
            // btnInicio
            // 
            this.btnInicio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInicio.Image = ((System.Drawing.Image)(resources.GetObject("btnInicio.Image")));
            this.btnInicio.Location = new System.Drawing.Point(30, 17);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Size = new System.Drawing.Size(52, 50);
            this.btnInicio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnInicio.TabIndex = 59;
            this.btnInicio.TabStop = false;
            // 
            // Costos
            // 
            this.Costos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Costos.Controls.Add(this.tabPage2);
            this.Costos.Controls.Add(this.tabPage4);
            this.Costos.Location = new System.Drawing.Point(30, 97);
            this.Costos.Name = "Costos";
            this.Costos.SelectedIndex = 0;
            this.Costos.Size = new System.Drawing.Size(956, 341);
            this.Costos.TabIndex = 79;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvProductos);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(948, 308);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Productos";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvProductos
            // 
            this.dgvProductos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProductos.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Location = new System.Drawing.Point(6, 5);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.RowHeadersWidth = 62;
            this.dgvProductos.RowTemplate.Height = 28;
            this.dgvProductos.Size = new System.Drawing.Size(936, 299);
            this.dgvProductos.TabIndex = 2;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvCosto);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1316, 316);
            this.tabPage4.TabIndex = 5;
            this.tabPage4.Text = "Costo";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvCosto
            // 
            this.dgvCosto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCosto.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvCosto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCosto.Location = new System.Drawing.Point(6, 5);
            this.dgvCosto.Name = "dgvCosto";
            this.dgvCosto.RowHeadersWidth = 62;
            this.dgvCosto.RowTemplate.Height = 28;
            this.dgvCosto.Size = new System.Drawing.Size(1304, 307);
            this.dgvCosto.TabIndex = 2;
            // 
            // FFormularioCostos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 450);
            this.Controls.Add(this.Costos);
            this.Controls.Add(this.btnInicio);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnLimpiar);
            this.Name = "FFormularioCostos";
            this.Text = "Formulario Costos";
            ((System.ComponentModel.ISupportInitialize)(this.btnLimpiar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInicio)).EndInit();
            this.Costos.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCosto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox btnLimpiar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox btnInicio;
        private System.Windows.Forms.TabControl Costos;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvProductos;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dgvCosto;
    }
}