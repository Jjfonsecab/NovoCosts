using NovoCosts.Class;
using NovoCosts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            columna.HeaderCell.Style.Font = new Font(columna.DataGridView.Font, FontStyle.Bold);
            columna.HeaderCell.Style.Font = new Font(columna.HeaderCell.Style.Font, FontStyle.Bold);
            columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
