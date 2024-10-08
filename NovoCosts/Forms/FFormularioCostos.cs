﻿using NovoCosts.Class;
using NovoCosts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Globalization;

namespace NovoCosts.Forms
{
    public partial class FFormularioCostos : Form
    {
        int usuarioIdActual = CurrentUser.UserId;
        private System.Windows.Forms.ToolTip toolTip;
        public FFormularioCostos()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FFormularioCostos_Load(object sender, EventArgs e)
        {
            MostrarFechaActual();
            ToolTipInitialicer();
        }

        bool Editar;
        int IdFCosto;
        int IdProducto;
        decimal Porcentaje;
        decimal Costo;
        decimal CostoProducto;
        decimal CostoManoObra;
        decimal Utilidad;
        decimal PrecioFabrica;
        string NombreProdcucto;
        bool Calcualdo = false;
        private void btnCostos_Click(object sender, EventArgs e)
        {
            FCostos fcostos = Application.OpenForms.OfType<FCostos>().FirstOrDefault();

            if (fcostos == null)
            {
                fcostos = new FCostos();
                fcostos.Show();
            }
            else
                fcostos.BringToFront();

            this.Close();
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            txtAnotaciones.Text = "N.A.";
            txtPorcentaje.Text = "";
            Calcualdo = false;
        }
        private void btnPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCamposString(comboBox1, txtAnotaciones))
                    return;
                else if (!ValidarCamposNumericos(txtPorcentaje))
                    return;

                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione un producto.");
                    return;
                }
                if (!Calcualdo)
                {
                    MessageBox.Show("Calcule los datos.");
                    return;
                }
                else
                {
                    GenerarPdf();                    
                    Guardar();
                    MessageBox.Show("PDF creado con exito.");
                    Calcualdo = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el PDF:");
                Console.WriteLine(ex.Message);
                return;
            }        
        }
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                if (IdProducto != 0)
                    calcularCosto();
                if (txtPorcentaje != null)
                    calcularPorcentajeGanancia();
                Calcualdo = true;

                MessageBox.Show("Calculo exitoso.");
            }
            catch (Exception)
            {
                MessageBox.Show("El porcentaje no ha sido definido.");
                return;
            }
        }    
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            ListarNombreProductos();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView selectedRow = (DataRowView)comboBox1.SelectedItem;

            if (selectedRow != null)
            {
                IdProducto = Convert.ToInt32(selectedRow["id_producto"]);
                NombreProdcucto = Convert.ToString(selectedRow["nombre"]);
            }
            ListarCostosProductoPorId();
            ListarManoObraProductoPorId();
            ListarTipoCostoPorId();            
            ConsultarProductoId(IdProducto);
        }

        //Metodos:
        private bool Guardar()
        {
            try
            {
                FormularioCostos fcostos = new FormularioCostos()
                {
                    IdFormularioCostos = IdFCosto,
                    IdProducto = IdProducto,
                    PorcentajeGanancia = Convert.ToDecimal(txtPorcentaje.Text),
                    Utilidad = Convert.ToDecimal(txtUtilidad.Text),
                    PrecioFabrica = Convert.ToDecimal(txtPrecioFabrica.Text),                    
                    Anotaciones = txtAnotaciones.Text,
                    IdUser = usuarioIdActual,

                    Fecha = DateTime.Parse(txtFecha.Text),
                };
                return Models.FormularioCostos.Guardar(fcostos, Editar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar." + ex);
                return false;
            }
        }
        private void GenerarPdf()
        {
            string defaultFileName = $"{NombreProdcucto}_{DateTime.Now.ToString("dd-MM-yyyy")}.pdf";

            SaveFileDialog savePdf = new SaveFileDialog();
            savePdf.FileName = defaultFileName;
            savePdf.Filter = "Archivos PDF (*.pdf)|*.pdf";

            string paginaHtml_texto = Properties.Resources.plantilla.ToString();
            paginaHtml_texto = paginaHtml_texto.Replace("@nombre", comboBox1.Text);
            paginaHtml_texto = paginaHtml_texto.Replace("@fecha", txtFecha.Text);
            paginaHtml_texto = paginaHtml_texto.Replace("@anotaciones", txtAnotaciones.Text);
            paginaHtml_texto = paginaHtml_texto.Replace("@costo", Convert.ToDouble(txtCosto.Text).ToString("#,##0", CultureInfo.InvariantCulture));
            paginaHtml_texto = paginaHtml_texto.Replace("@porcentaje", Convert.ToDouble(txtPorcentaje.Text).ToString());
            paginaHtml_texto = paginaHtml_texto.Replace("@precioFabrica", Convert.ToDouble(txtPrecioFabrica.Text).ToString("#,##0", CultureInfo.InvariantCulture));
            paginaHtml_texto = paginaHtml_texto.Replace("@utilidad", Convert.ToDouble(txtUtilidad.Text).ToString("#,##0", CultureInfo.InvariantCulture));

            string filasTipoCosto = string.Empty;

            foreach (DataGridViewRow row in dgvTipos.Rows)
            {
                if (row.Cells["nombre"].Value != null && row.Cells["suma_total"].Value != null)
                {
                    string nombre = row.Cells["nombre"].Value.ToString();
                    double valorTotal = Convert.ToDouble(row.Cells["suma_total"].Value);

                    filasTipoCosto += "<tr>";
                    filasTipoCosto += $"<td>{nombre}</td>";
                    filasTipoCosto += $"<td>{valorTotal.ToString("#,##0", CultureInfo.InvariantCulture)}</td>";
                    filasTipoCosto += "</tr>";
                }
            }

            paginaHtml_texto = paginaHtml_texto.Replace("@tipoCosto", filasTipoCosto);

            string filasCostos = string.Empty;
            string filasManoObra = string.Empty;

            foreach (DataGridViewRow row in dgvCostos.Rows)
            {
                filasCostos += "<tr>";
                filasCostos += "<td>" + (row.Cells["MateriaPrima"].Value ?? "").ToString() + "</td>";
                filasCostos += "<td>" + (row.Cells["desempeño"].Value ?? "").ToString() + "</td>";
                filasCostos += "<td>" + (row.Cells["cantidad"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["cantidad"].Value)).ToString() : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["dimension1"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["dimension1"].Value)).ToString() : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["dimension2"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["dimension2"].Value)).ToString() : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["dimension3"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["dimension3"].Value)).ToString() : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["cm"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["cm"].Value)).ToString("#,##0", CultureInfo.InvariantCulture) : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["desperdicio"].Value != null ? Convert.ToDouble(row.Cells["desperdicio"].Value).ToString("#,##0.##", CultureInfo.InvariantCulture) : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["total_cantidad"].Value != null ? Convert.ToDouble(row.Cells["total_cantidad"].Value).ToString("#,##0.##", CultureInfo.InvariantCulture) : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["valor_unitario"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["valor_unitario"].Value)).ToString("#,##0", CultureInfo.InvariantCulture) : "") + "</td>";
                filasCostos += "<td>" + (row.Cells["valor_total"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["valor_total"].Value)).ToString("#,##0", CultureInfo.InvariantCulture) : "") + "</td>";
                filasCostos += "</tr>";
            }
            paginaHtml_texto = paginaHtml_texto.Replace("@filasCostos", filasCostos);

            foreach (DataGridViewRow row in dgvManoObra.Rows)
            {
                filasManoObra += "<tr>";
                filasManoObra += "<td>" + (row.Cells["nombre_tipo"].Value ?? "").ToString() + "</td>";
                filasManoObra += "<td>" + (row.Cells["costo"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["costo"].Value)).ToString("#,##0", CultureInfo.InvariantCulture) : "") + "</td>";
                filasManoObra += "<td>" + (row.Cells["total_cantidad"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["total_cantidad"].Value)).ToString("#,##0", CultureInfo.InvariantCulture) : "") + "</td>";
                filasManoObra += "<td>" + (row.Cells["valor_total"].Value != null ? Math.Round(Convert.ToDouble(row.Cells["valor_total"].Value)).ToString("#,##0", CultureInfo.InvariantCulture) : "") + "</td>";
                filasManoObra += "</tr>";
            }

            paginaHtml_texto = paginaHtml_texto.Replace("@filasManoObra", filasManoObra);

            if (savePdf.ShowDialog() == DialogResult.OK)
            {
                string imagenSeleccionada = string.Empty;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Archivos de Imagen (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imagenSeleccionada = openFileDialog.FileName;
                }

                using (FileStream stream = new FileStream(savePdf.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.LETTER, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(Properties.Resources.LogoNovo, System.Drawing.Imaging.ImageFormat.Png);
                    imgLogo.ScaleToFit(80, 100);
                    imgLogo.Alignment = iTextSharp.text.Image.UNDERLYING;
                    imgLogo.SetAbsolutePosition(pdfDoc.LeftMargin, pdfDoc.Top - 30);
                    pdfDoc.Add(imgLogo);

                    if (!string.IsNullOrEmpty(imagenSeleccionada))
                    {
                        iTextSharp.text.Image imgSeleccionada = iTextSharp.text.Image.GetInstance(imagenSeleccionada);
                        imgSeleccionada.ScaleToFit(70, 110);
                        imgSeleccionada.Alignment = iTextSharp.text.Image.UNDERLYING;
                        float imgWidth = imgSeleccionada.ScaledWidth;


                        float xPosition = pdfDoc.PageSize.Width - pdfDoc.RightMargin - imgWidth; // Margen derecho menos el ancho de la imagen
                        imgSeleccionada.SetAbsolutePosition(xPosition, pdfDoc.Top - 50); // Ajusta la posición vertical según sea necesario

                        pdfDoc.Add(imgSeleccionada);
                    }

                    using (StringReader sr = new StringReader(paginaHtml_texto))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }
                    pdfDoc.Close();
                    stream.Close();
                }
            }
        }
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
        {
            txtAnotaciones.CharacterCasing = CharacterCasing.Upper;
            txtAnotaciones.Click += TextBox_Click;
            txtPorcentaje.Click += TextBox_Click;
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void ListarNombreProductos()
        {
            DataTable dataTable = Producto.ListarProductos();

            comboBox1.DataSource = dataTable;
            comboBox1.DisplayMember = "nombre";
            comboBox1.ValueMember = "id_producto";
        }
        private void ListarCostosProductoPorId()
        {
            if (IdProducto != 0)
            {
                dgvCostos.DataSource = Models.Costos.ListarCostoProducto(IdProducto);
                DbDatos.OcultarIds(dgvCostos);
                PersonalizarColumnasCostos(dgvCostos);               
            }
            ColorearFilas(dgvCostos, "id_tipo_costo");
        }
        private void ListarManoObraProductoPorId()
        {
            if (IdProducto != 0)
            {
                dgvManoObra.DataSource = ManoObra.ListarPorProducto(IdProducto);
                DbDatos.OcultarIds(dgvManoObra);
                PersonalizarColumnasManoObra();
            }            
        }
        private void ListarTipoCostoPorId()
        {
            if (IdProducto != 0)
            {
                dgvTipos.DataSource = TipoCosto.ListarSumadoPorProducto(IdProducto);
                DbDatos.OcultarIds(dgvTipos);
                PersonalizarColumnasTipoCosto();
            }
            ColorearFilas(dgvTipos, "id_tipo_costo");
        }
        private void ConsultarProductoId(int idProducto)
        {
            DataTable dataTable = Producto.ListarProducto(idProducto);

            if (dataTable.Rows.Count > 0)
            {
                if (dataTable.Rows[0]["costo"] != DBNull.Value)
                {
                    CostoProducto = Convert.ToDecimal(dataTable.Rows[0]["costo"]);
                }
            }
        }
        private void PersonalizarColumnasCostos(DataGridView dgvActual)
        {
            foreach (DataGridViewColumn columna in dgvActual.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    DbDatos.OcultarIds(dgvActual);
                    if (columna.Name == "dimension1" || columna.Name == "dimension2" || columna.Name == "dimension3" || columna.Name == "cantidad")
                    {
                        dgvActual.Columns["dimension1"].HeaderText = "D1";
                        dgvActual.Columns["dimension2"].HeaderText = "D2";
                        dgvActual.Columns["dimension3"].HeaderText = "D3";
                        dgvActual.Columns["cantidad"].HeaderText = "CAN";
                        dgvActual.Columns[columna.Name].Width = 35;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                    }
                    else if (columna.Name == "total_cantidad")
                    {
                        dgvActual.Columns["total_cantidad"].HeaderText = "TOTAL CANT.";
                        dgvActual.Columns[columna.Name].Width = 62;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                    }
                    else if (columna.Name == "cantidad_desperdicio")
                    {
                        columna.Visible = false;
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvActual.Columns["fecha"].Visible = false;
                    }
                    else if (columna.Name == "desempeño")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;
                    }
                    else if (columna.Name == "desperdicio")
                    {
                        dgvActual.Columns[columna.Name].Width = 100;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        columna.DefaultCellStyle = estiloCeldaNumerica;

                    }
                    else if (columna.Name == "cm" || columna.Name == "valor_unitario")
                    {
                        dgvActual.Columns["valor_unitario"].HeaderText = "VALOR UNIT.";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;

                    }
                    else if (columna.Name == "valor_total")
                    {
                        dgvActual.Columns["valor_total"].HeaderText = "VALOR TOTAL";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;

                    }
                    else if (columna.Name == "descripcion" || columna.Name == "referencia")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;

                    }
                    else if (columna.Name == "MateriaPrima")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;
                        dgvActual.Columns[columna.Name].DisplayIndex = 0;
                    }
                }
                ConfigurarCabeceraColumna(columna, columna.HeaderText);
            }
        }
        private void PersonalizarColumnasManoObra()
        {
            foreach (DataGridViewColumn columna in dgvManoObra.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "nombre_tipo")
                    {
                        dgvManoObra.Columns["nombre_tipo"].HeaderText = "NOMBRE";
                        dgvManoObra.Columns["nombre_tipo"].DisplayIndex = 0;
                    }   
                    else if (columna.Name == "fecha")
                        columna.Visible = false;
                    else if (columna.Name == "costo"  || columna.Name == "valor_total")
                    {
                        dgvManoObra.Columns["total_cantidad"].HeaderText = "CANTIDAD TOTAL";
                        dgvManoObra.Columns["valor_total"].HeaderText = "VALOR TOTAL";
                        columna.Width = 107;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight;
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                    }                    
                }
                ConfigurarCabeceraColumna(columna, columna.HeaderText);
            }
        }
        private void PersonalizarColumnasTipoCosto()
        {
            foreach (DataGridViewColumn columna in dgvTipos.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "nombre")
                    {
                        dgvTipos.Columns["nombre"].HeaderText = "NOMBRE";
                        dgvTipos.Columns["nombre"].DisplayIndex = 0;
                    }
                    else if (columna.Name == "suma_total")
                    {
                        dgvTipos.Columns["suma_total"].HeaderText = "TOTAL";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        dgvTipos.Columns[columna.Name].Width = 80;
                    }
                }
            }
        }
        private void ConfigurarCabeceraColumna(DataGridViewColumn columna, string nuevoHeaderText)
        {
            string nuevoHeaderTextMayusculas = nuevoHeaderText.ToUpper();

            columna.HeaderText = nuevoHeaderTextMayusculas;
            columna.HeaderCell.Style.Font = new System.Drawing.Font(columna.DataGridView.Font, FontStyle.Bold);
            columna.HeaderCell.Style.Font = new System.Drawing.Font(columna.HeaderCell.Style.Font, FontStyle.Bold);
            columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void ColorearFilas(DataGridView dataGridView, string nombreColumnaTipoCosto)
        {
            foreach (DataGridViewRow fila in dataGridView.Rows)
            {                
                var tipoCosto = Convert.ToInt32(fila.Cells[nombreColumnaTipoCosto].Value);

                if (tipoCosto == 1)                  
                    fila.DefaultCellStyle.BackColor = Color.LightBlue;                
                else if (tipoCosto == 2)                  
                    fila.DefaultCellStyle.BackColor = Color.LightSeaGreen;                
                else if (tipoCosto == 4)                 
                    fila.DefaultCellStyle.BackColor = Color.LightSkyBlue;                
                else if (tipoCosto == 5)                
                    fila.DefaultCellStyle.BackColor = Color.LightSalmon;                
                else                                    
                    fila.DefaultCellStyle.BackColor = Color.White;                
            }
        }

        //Definir costo, utilidad y precio de fabrica:
        private void calcularCosto()
        {
            CostoProducto = SumarColumna(dgvCostos, "valor_total");
            CostoManoObra = SumarColumna(dgvManoObra, "valor_total");
            Costo = CostoManoObra + CostoProducto;
            
            
            txtCosto.Text = Convert.ToString(Costo);
        }
        private void calcularPorcentajeGanancia()
        {
            Porcentaje = Convert.ToDecimal(txtPorcentaje.Text) / 100;
            PrecioFabrica = Costo * (1 + Porcentaje);
            Utilidad = PrecioFabrica - Costo;

            txtUtilidad.Text = Convert.ToString(Utilidad);
            txtPrecioFabrica.Text = Convert.ToString(PrecioFabrica);
        }
        private decimal SumarColumna(DataGridView dataGridView, string nombreColumna)
        {
            decimal suma = 0;

            if (dataGridView.Rows.Count > 0 && dataGridView.Columns.Contains(nombreColumna))
            {
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    suma += Convert.ToDecimal(fila.Cells[nombreColumna].Value);
                }
            }
            return suma;
        }
        private decimal SumarTipoCosto(DataGridView dataGridView, string nombreColumna)
        {
            decimal suma = 0;

            if (dataGridView.Rows.Count > 0 && dataGridView.Columns.Contains(nombreColumna))
            {
                foreach (DataGridViewRow fila in dataGridView.Rows)
                {
                    suma += Convert.ToDecimal(fila.Cells[nombreColumna].Value);
                }
            }
            return suma;
        }
        private bool EsNumero(string texto)
        {
            return double.TryParse(texto, out _);
        }
        private bool ValidarCamposString(params Control[] controles)
        {
            foreach (var control in controles)
            {
                if (control is System.Windows.Forms.TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    MessageBox.Show("Por favor, complete o verifique todos los campos antes de guardar.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }
        private bool ValidarCamposNumericos(params Control[] controles)
        {
            foreach (var control in controles)
            {
                if (control is System.Windows.Forms.TextBox textBox)
                {
                    if (!EsNumero(textBox.Text))
                    {
                        MessageBox.Show("Por favor, verifique los campos numericos antes de seguir.", "Formato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return true;
        }
        private void ToolTipInitialicer()
        {
            toolTip = new System.Windows.Forms.ToolTip();

            toolTip.SetToolTip(btnLimpiar, "LIMPIAR");
            toolTip.SetToolTip(btnCostos, "COSTOS");
            toolTip.SetToolTip(btnPdf, "GENERAR PDF");
            toolTip.SetToolTip(btnCalcular, "CALCULAR");
        }

        
    }
}
