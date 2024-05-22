using NovoCosts.Class;
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
        public FFormularioCostos()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FFormularioCostos_Load(object sender, EventArgs e)
        {
            MostrarFechaActual();
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
        private void btnLimpiar_Click(object sender, EventArgs e)//toDos
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
                        using (FileStream stream = new FileStream(savePdf.FileName, FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.LETTER, 25, 25, 25, 25);

                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                            pdfDoc.Open();

                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Properties.Resources.LogoNovo, System.Drawing.Imaging.ImageFormat.Png);
                            img.ScaleToFit(80, 100);
                            img.Alignment = iTextSharp.text.Image.UNDERLYING;
                            img.SetAbsolutePosition(pdfDoc.LeftMargin, pdfDoc.Top - 30);
                            pdfDoc.Add(img);

                            using (StringReader sr = new StringReader(paginaHtml_texto))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            }
                            pdfDoc.Close();
                            stream.Close();
                        }
                    }
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
        }
        private void ListarManoObraProductoPorId()
        {
            if (IdProducto != 0)
            {
                dgvManoObra.DataSource = ManoObra.ListarPorProducto(IdProducto);
                DbDatos.OcultarIds(dgvManoObra);
                PersonalizarColumnasManoObra();
            }            
            CostoManoObra = SumarColumna(dgvManoObra, "valor_total");
        }
        private void ConsultarProductoId(int idProducto)
        {
            DataTable dataTable = Producto.ListarProducto(idProducto);

            if (dataTable.Rows.Count > 0)
            {
                // Verificar si la columna "costo" existe y no es nula antes de asignarla a la variable global
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
                        //estiloCeldaNumerica.Format = "N0";
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
        private void ConfigurarCabeceraColumna(DataGridViewColumn columna, string nuevoHeaderText)
        {
            string nuevoHeaderTextMayusculas = nuevoHeaderText.ToUpper();

            columna.HeaderText = nuevoHeaderTextMayusculas;
            columna.HeaderCell.Style.Font = new System.Drawing.Font(columna.DataGridView.Font, FontStyle.Bold);
            columna.HeaderCell.Style.Font = new System.Drawing.Font(columna.HeaderCell.Style.Font, FontStyle.Bold);
            columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }
        

        //Definir costo, utilidad y precio de fabrica:
        private void calcularCosto()
        {
            Costo = CostoManoObra + CostoProducto;
            Console.WriteLine("CostoManoObra : " + CostoManoObra);
            Console.WriteLine("Costo : " + Costo);
            txtCosto.Text = Convert.ToString(Costo);
            Console.WriteLine("Costo : " + Costo);
            Console.WriteLine("-----");
        }
        private void calcularPorcentajeGanancia()
        {
            Porcentaje = Convert.ToDecimal(txtPorcentaje.Text) / 100;
            Console.WriteLine("Porcentaje : " + Porcentaje);
            PrecioFabrica = Costo * (1 + Porcentaje);
            Console.WriteLine("PrecioFabrica : " + PrecioFabrica);
            Utilidad = PrecioFabrica - Costo;
            Console.WriteLine("Utilidad : " + Utilidad);

            txtUtilidad.Text = Convert.ToString(Utilidad);
            txtPrecioFabrica.Text = Convert.ToString(PrecioFabrica);
            Console.WriteLine("Costo : " + Costo);
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

    }
}
