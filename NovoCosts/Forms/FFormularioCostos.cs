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

        int IdProducto;
        decimal Porcentaje;
        decimal Costo;
        decimal CostoProducto;
        decimal CostoManoObra;
        decimal Utilidad;
        decimal PrecioFabrica;
        string NombreProdcucto;
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
        }
        private void btnLimpiar_Click(object sender, EventArgs e)//toDos
        {
            comboBox1.Text = "";
        }
        private void btnImprimir_Click(object sender, EventArgs e)//toDo
        {
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            printDocument1.PrinterSettings = ps;
            printDocument1.PrintPage += Imprimir;
            printDocument1.Print();
        }
        private void btnPdf_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }
            else
            {
                string defaultFileName = $"{NombreProdcucto}_{DateTime.Now.ToString("dd-MM-yyyy")}.pdf";

                SaveFileDialog savePdf = new SaveFileDialog();
                savePdf.FileName = defaultFileName;
                savePdf.Filter = "Archivos PDF (*.pdf)|*.pdf"; // Filtro para mostrar solo archivos PDF

                string paginaHtml_texto = $"<table border=1><tr><td>{NombreProdcucto}</td></tr></table>";

               if(savePdf.ShowDialog() == DialogResult.OK)
                {
                    using(FileStream stream = new FileStream(savePdf.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4,25,25,25,25);

                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);//este creo que es el que permite actualizar

                        pdfDoc.Open();
                        pdfDoc.Add(new Phrase("Novo Arte!"));

                        using (StringReader sr = new StringReader(paginaHtml_texto))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        }

                        pdfDoc.Close();
                        stream.Close();
                    }
                }


            }
            
        }
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if(IdProducto != 0)
                calcularCosto();
            
            if (txtPorcentaje != null)            
                calcularPorcentajeGanancia();            
            else            
                MessageBox.Show("El porcentaje no ha sido definido.");
            
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
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
        {
            txtFecha.CharacterCasing = CharacterCasing.Upper;
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

            DataTable dataTable = ManoObra.CalcularCostoManoObra(IdProducto);
            if (dataTable.Rows.Count > 0)
            {
                // Verificar si la columna "costo" existe y no es nula antes de asignarla a la variable global
                if (dataTable.Rows[0]["total_costo"] != DBNull.Value)
                {
                    CostoManoObra = Convert.ToDecimal(dataTable.Rows[0]["total_costo"]);
                }

            }
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
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "total_cantidad" || columna.Name == "valor_unitario")
                    {
                        dgvActual.Columns["total_cantidad"].HeaderText = "TOTAL CANT.";
                        dgvActual.Columns["valor_unitario"].HeaderText = "VALOR UNIT.";
                        dgvActual.Columns[columna.Name].Width = 62;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "cantidad_desperdicio")
                    {
                        columna.Visible = false;
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvActual.Columns["fecha"].HeaderText = "Fecha";
                        dgvActual.Columns[columna.Name].Width = 80;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "desempeño")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "desper")
                    {
                        dgvActual.Columns[columna.Name].Width = 60;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "cm")
                    {
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "valor_total")
                    {
                        dgvActual.Columns["valor_total"].HeaderText = "VALOR TOTAL";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "descripcion" || columna.Name == "referencia")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;
                        DbDatos.OcultarIds(dgvActual);
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
                    else if (columna.Name == "costo")
                    {
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
        private void Imprimir(object sender, PrintPageEventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string textoComboBox = comboBox1.SelectedItem.ToString();

                // Definir la fuente y el pincel para dibujar el texto
                System.Drawing.Font fuente = new System.Drawing.Font("Arial", 12);
                SolidBrush pincel = new SolidBrush(Color.Black);

                PointF posicion = new PointF(100, 100);

                e.Graphics.DrawString(textoComboBox, fuente, pincel, posicion);
            }
                
        }

        //Definir costo, utilidad y precio de fabrica:
        private void calcularCosto()
        {
            Costo = CostoManoObra + CostoProducto;
            txtCosto.Text = Convert.ToString(Costo);
        }
        private void calcularPorcentajeGanancia()
        {
            Porcentaje = Convert.ToDecimal(txtPorcentaje.Text)/100;

            PrecioFabrica = Costo * (1 + Porcentaje);
            Utilidad =  PrecioFabrica - Costo ;

            txtUtilidad.Text = Convert.ToString(Utilidad);
            txtPrecioFabrica.Text = Convert.ToString(PrecioFabrica);
        }

        
    }
}
