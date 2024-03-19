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

namespace NovoCosts.Forms
{
    public partial class FCostos : Form
    {
        public FCostos()
        {
            InitializeComponent();
            ToUpperText();
        }        
        private void FCostos_Load(object sender, EventArgs e)
        {
            ListarTodo();
            ListarProductos();
            ListarMaterial();
            MostrarFechaActual();
        }

        bool Editar;
        bool Modificar;
        int IdCosto;
        int IdProducto;
        int IdMateriaPrima;
        int IdTipoCosto;
        int IdManoObra;
        Decimal ResultadoDesperdicio;
        Decimal ResultadoTotalCantidad;
        Decimal ResultadoValorTotal;
        private void btnInicio_Click_1(object sender, EventArgs e)
        {
            FInicio fInicio = Application.OpenForms.OfType<FInicio>().FirstOrDefault();

            if (fInicio == null)
            {
                fInicio = new FInicio();
                fInicio.Show();
            }
            else
                fInicio.BringToFront();

            if (Application.OpenForms.Count > 1)
                this.Close();
            else
                this.Hide();
        }
        private void btnTipoCostos_Click(object sender, EventArgs e)
        {
            FRegistroTipoCosto fregistrotp = Application.OpenForms.OfType<FRegistroTipoCosto>().FirstOrDefault();

            if (fregistrotp == null)
            {
                fregistrotp = new FRegistroTipoCosto();
                fregistrotp.Show();
            }
            else
                fregistrotp.BringToFront();
        }
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (!ValidarCamposString(txtReferencia, txtDescripcion, txtDesempeño, txtCantidad, txtCM, txtD1, txtD2, txtD3, comboBoxTC, txtFecha))
                return;
            else if (!ValidarCamposNumericos(txtD1, txtD2, txtD3, txtCantidad))
                return;
            else
            {
                if (txtDesperdicioTotal.Enabled == false)
                {
                    CalcularTotalCantidad(Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtD1.Text), Convert.ToDecimal(txtD2.Text), Convert.ToDecimal(txtD3.Text), Convert.ToDecimal(txtCM.Text));
                    CalcularDesperdicio(ResultadoTotalCantidad, Convert.ToDecimal(txtCantidadDesperdicio.Text));
                    txtDesperdicioTotal.Text = ResultadoDesperdicio.ToString();
                    CalcularValorTotal(Convert.ToDecimal(txtValorU.Text));
                }
                else if (txtDesperdicioTotal.Enabled == false)
                {
                    ResultadoTotalCantidad = Convert.ToDecimal(txtDesperdicioTotal.Text);
                    ResultadoDesperdicio = 0;
                    txtDesperdicioTotal.Text = ResultadoDesperdicio.ToString();
                    CalcularValorTotalManoObra(ResultadoTotalCantidad, Convert.ToDecimal(txtValorU.Text));
                }

            }
            if (Modificar)
            {
                if (!GuardarEditado())
                    return;
            }
            else
                if (!Guardar()) return;

            Finalizar();
        }
        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dgvCostos.SelectedRows.Count == 0 || dgvCostos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvCostos.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvCostos.CurrentRow.Cells["id_costos"];
                int id_costo = Convert.ToInt32(cell.Value);
                IdCosto = id_costo;
                Eliminar();
                Finalizar();
            }
        }        
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)//faltan campos
        {
            IdCosto = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_costos"].Value);
            IdProducto = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_producto"].Value);
            IdMateriaPrima = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_materia_prima"].Value);
            txtDesempeño.Text = dgvCostos.CurrentRow.Cells["desempeño"].Value.ToString();
            txtCantidad.Text = dgvCostos.CurrentRow.Cells["cantidad"].Value.ToString();
            txtD1.Text = dgvCostos.CurrentRow.Cells["dimension1"].Value.ToString();
            txtD2.Text = dgvCostos.CurrentRow.Cells["dimension2"].Value.ToString();
            txtD3.Text = dgvCostos.CurrentRow.Cells["dimension3"].Value.ToString();
            txtCM.Text = dgvCostos.CurrentRow.Cells["cm"].Value.ToString();
            txtCantidadDesperdicio.Text = dgvCostos.CurrentRow.Cells["cm"].Value.ToString();
            IdTipoCosto = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_tipo_costo"].Value);

            DateTime fechaOriginal = DateTime.Parse(dgvCostos.CurrentRow.Cells["fecha"].Value.ToString());
            txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

            Editar = true;
            Modificar = true;
        }
        private void dgvCostos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvCostos.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvCostos.SelectedRows[0];

                    if (selectedRow.Cells.Count >= 5)
                    {
                        IdCosto = Convert.ToInt32(selectedRow.Cells["id_costos"].Value);
                        IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                        IdMateriaPrima = Convert.ToInt32(selectedRow.Cells["id_materia_prima"].Value);
                        txtDesempeño.Text = Convert.ToString(selectedRow.Cells["desempeño"].Value);
                        txtCantidad.Text = Convert.ToString(selectedRow.Cells["cantidad"].Value);
                        txtD1.Text = Convert.ToString(selectedRow.Cells["dimension1"].Value);
                        txtD2.Text = Convert.ToString(selectedRow.Cells["dimension2"].Value);
                        txtD3.Text = Convert.ToString(selectedRow.Cells["dimension3"].Value);
                        txtCM.Text = Convert.ToString(selectedRow.Cells["cm"].Value);
                        txtCantidadDesperdicio.Text = Convert.ToString(selectedRow.Cells["cantidad_desperdicio"].Value);
                        txtDesperdicioTotal.Text = Convert.ToString(selectedRow.Cells["cm"].Value);
                        IdTipoCosto = Convert.ToInt32(selectedRow.Cells["id_tipo_costo"].Value);

                        txtFecha.Text = selectedRow.Cells["fecha"].Value.ToString();                       

                        Editar = true;
                        Modificar = true;
                    }
                    
                }
                else
                {
                    Limpiar(); 
                    Editar = false;
                    Modificar = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar" + ex);
                return;
            }
        }
        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvProductos.SelectedRows[0];

                if (selectedRow.Cells.Count >= 2)
                {
                    IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                    txtDescripcion.Text = selectedRow.Cells[2].Value.ToString();
                    txtReferencia.Text = selectedRow.Cells[1].Value.ToString();
                }
            }
            ListarManoObra(IdProducto);
            ListarCosto(IdProducto);
            Console.WriteLine(IdProducto);

        }
        private void dgvMateriasPrimas_SelectionChanged(object sender, EventArgs e)
        {
            txtDesperdicioTotal.Enabled = false;
            if (dgvMateriasPrimas.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvMateriasPrimas.SelectedRows[0];

                if (selectedRow.Cells.Count >= 4)
                {
                    IdMateriaPrima = Convert.ToInt32(selectedRow.Cells["id_materia_prima"].Value);
                    txtMaterial.Text = selectedRow.Cells[1].Value.ToString();
                    txtValorU.Text = selectedRow.Cells[4].Value.ToString();
                    txtCM.Text = selectedRow.Cells["medida"].Value.ToString();
                    txtCantidadDesperdicio.Text = selectedRow.Cells[8].Value.ToString();
                }
            }
        }
        private void dgvManoObra_SelectionChanged(object sender, EventArgs e)
        {
            txtDesperdicioTotal.Enabled = true;
            if (dgvManoObra.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvManoObra.SelectedRows[0];

                if (selectedRow.Cells.Count >= 3)
                {
                    txtMaterial.Text = "MANO DE OBRA";
                    txtDesempeño.Text = selectedRow.Cells["nombre_tipo"].Value.ToString();
                    txtValorU.Text = selectedRow.Cells["costo"].Value.ToString();
                }
            }
        }
        private void comboBoxTC_DropDown(object sender, EventArgs e)
        {
            ListarTipoCosto();
        }
        private void comboBoxTC_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView selectedRow = (DataRowView)comboBoxTC.SelectedItem;

            if (selectedRow != null)
            {
                IdTipoCosto = Convert.ToInt32(selectedRow["id_tipo_costo"]);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtD1.Enabled = false;
            txtD2.Enabled = false;
            txtD3.Enabled = false;

            int selectedIndex = comboBox1.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    txtD1.Enabled = false;
                    txtD2.Enabled = false;
                    txtD3.Enabled = false;
                    break;
                case 1:
                    txtD1.Enabled = true;
                    break;
                case 2:
                    txtD1.Enabled = true;
                    txtD2.Enabled = true;
                    break;
                case 3:
                    txtD1.Enabled = true;
                    txtD2.Enabled = true;
                    txtD3.Enabled = true;
                    break;
            }
        }

        //Metodos
        private bool Guardar()
        {
            try
            {
                Costos costos = new Costos()
                {
                    IdCostos = IdCosto,
                    IdProducto = IdProducto,
                    IdMateriaPrima = IdMateriaPrima,
                    Desempeño = txtDesempeño.Text,
                    Cantidad = Convert.ToInt32(txtCantidad.Text),
                    Dimension1 = Convert.ToDecimal(txtD1.Text),
                    Dimension2 = Convert.ToDecimal(txtD2.Text),
                    Dimension3 = Convert.ToDecimal(txtD3.Text),
                    Cm = Convert.ToDecimal(txtCM.Text),
                    CantidadDesperdicio = Convert.ToDecimal(txtCantidadDesperdicio.Text),
                    Desperdicio = ResultadoDesperdicio,
                    TotalCantidad = ResultadoTotalCantidad,
                    ValorUnitario = Convert.ToDecimal(txtValorU.Text),
                    ValorTotal = ResultadoValorTotal,

                    IdTipoCosto = IdTipoCosto,

                    Fecha = DateTime.Parse(txtFecha.Text),
                };
                return Models.Costos.Guardar(costos, Editar);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar.");
                return false;
            }
            
        }
        private bool GuardarEditado()
        {
            if (IdCosto > 0)
            {
                DateTime fechaOriginal = DateTime.Parse(dgvCostos.CurrentRow.Cells["fecha"].Value.ToString());

                Costos costos = new Costos()
                {
                    IdCostos = IdCosto,
                    IdProducto = IdProducto,
                    IdMateriaPrima = IdMateriaPrima,
                    Desempeño = txtDesempeño.Text,
                    Cantidad = Convert.ToInt32(txtCantidad.Text),
                    Dimension1 = Convert.ToDecimal(txtD1.Text),
                    Dimension2 = Convert.ToDecimal(txtD2.Text),
                    Dimension3 = Convert.ToDecimal(txtD3.Text),
                    Cm = Convert.ToDecimal(txtCM.Text),
                    CantidadDesperdicio = Convert.ToDecimal(txtCantidadDesperdicio.Text),
                    ValorUnitario = Convert.ToDecimal(txtValorU.Text),
                    IdTipoCosto = IdTipoCosto,

                    Fecha = fechaOriginal,
                };

                return Models.Costos.Guardar(costos, true);
            }
            else
            {
                MessageBox.Show("Selecciona una mano de obra antes de guardar editado.", "Mano de Obra no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private bool Eliminar()
        {
            try
            {
                if (IdCosto > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este dato?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return Models.Costos.Eliminar(IdCosto);
                    return false;
                }
                MessageBox.Show("Selecciona un costo antes de eliminar.", "Costo no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("No se puede eliminar este dato porque tiene registros relacionados.", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error al intentar eliminar. Detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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
                        MessageBox.Show("Por favor, ingrese solo números en todos los campos antes de guardar.", "Formato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return true;
        }
        private bool EsNumero(string texto)
        {
            return double.TryParse(texto, out _);
        }
        private void Finalizar()
        {
            ListarTodo();
            Limpiar();
        }
        private void Limpiar()
        {
            txtDesempeño.Text = "";
            txtCantidad.Text = "";
            txtD1.Text = "";
            txtD2.Text = "";
            txtD3.Text = "";
            comboBoxTC.Text = "";
            txtFecha.Text = "";
            Editar = false;

            MostrarFechaActual();
        }
        private void ListarTodo()
        {
            dgvCostos.DataSource = Models.Costos.ListarTodo();
            DbDatos.OcultarIds(dgvCostos);
            PersonalizarColumnasGrid();
        }
        private void ListarProductos()
        {
            dgvProductos.DataSource = Producto.ListarTodo();
            DbDatos.OcultarIds(dgvProductos);
            PersonalizarColumnasGrid();
        }
        private void ListarMaterial()
        {
            dgvMateriasPrimas.DataSource = MateriasPrimas.ListarTodo();
            DbDatos.OcultarIds(dgvMateriasPrimas);
            PersonalizarColumnasGrid();
        }
        private void ListarTipoCosto()
        {
            DataTable dataTable = TipoCosto.ListarTodo();

            comboBoxTC.DataSource = dataTable;
            comboBoxTC.DisplayMember = "nombre";
            comboBoxTC.ValueMember = "id_tipo_costo";
        }
        private void ListarCosto(int IdProducto)
        {
            dgvCosto.DataSource = Models.Costos.ListarCostoProducto(IdProducto);
            DbDatos.OcultarIds(dgvCosto);
            PersonalizarColumnasGrid();
        }
        private void ListarManoObra(int IdProducto)
        {
            dgvManoObra.DataSource = ManoObra.ListarPorProducto(IdProducto);
            DbDatos.OcultarIds(dgvManoObra);
            PersonalizarColumnasGrid();
        }
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
        {
            txtReferencia.CharacterCasing = CharacterCasing.Upper;
            txtReferencia.Click += TextBox_Click;
            txtDescripcion.CharacterCasing = CharacterCasing.Upper;
            txtDescripcion.Click += TextBox_Click;
            txtDesempeño.CharacterCasing = CharacterCasing.Upper;
            txtDesempeño.Click += TextBox_Click;
            comboBoxTC.Click += TextBox_Click;
            txtD1.Click += TextBox_Click;
            txtD2.Click += TextBox_Click;
            txtD3.Click += TextBox_Click;

            txtFecha.CharacterCasing = CharacterCasing.Upper;
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }
        private void PersonalizarColumnasGrid()
        {
            foreach (DataGridViewColumn columna in dgvCostos.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "dimension1" || columna.Name == "dimension2" || columna.Name == "dimension3" || columna.Name == "cantidad")
                    {
                        dgvCostos.Columns["dimension1"].HeaderText = "D1";
                        dgvCostos.Columns["dimension2"].HeaderText = "D2";
                        dgvCostos.Columns["dimension3"].HeaderText = "D3";
                        dgvCostos.Columns["cantidad"].HeaderText = "CAN";
                        dgvCostos.Columns[columna.Name].Width = 35;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        //estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "total_cantidad" || columna.Name == "valor_unitario")
                    {
                        dgvCostos.Columns["total_cantidad"].HeaderText = "TOTAL CANT.";
                        dgvCostos.Columns["valor_unitario"].HeaderText = "VALOR UNIT.";
                        dgvCostos.Columns[columna.Name].Width = 62;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "cantidad_desperdicio")
                    {
                        columna.Visible = false;
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvCostos.Columns["fecha"].HeaderText = "Fecha";
                        dgvCostos.Columns[columna.Name].Width = 80;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "desempeño")
                    {
                        dgvCostos.Columns[columna.Name].Width = 111;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "desper")
                    {
                        dgvCostos.Columns[columna.Name].Width = 60;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "cm")
                    {
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "valor_total")
                    {
                        dgvCostos.Columns["valor_total"].HeaderText = "VALOR TOTAL";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvCostos);
                    }
                }
                ConfigurarCabeceraColumna(columna, columna.HeaderText);
            }
            foreach (DataGridViewColumn columna in dgvMateriasPrimas.Columns)
            {
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
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private decimal CalcularTotalCantidad(decimal cantidad, decimal d1, decimal d2, decimal d3, decimal cm)
        {
            int selectedIndex = comboBox1.SelectedIndex;

            if (selectedIndex == 3)
            {
                ResultadoTotalCantidad = (cantidad * d1 * d2 * d3) / cm;
                Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + " + " + d1 + " + " + d2 + " + " + d3 + " / " + cm + " = " + ResultadoTotalCantidad);
                return ResultadoTotalCantidad;
            }
            else if (selectedIndex == 2)
            {
                ResultadoTotalCantidad = (cantidad * d1 * d2) / cm;
                Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + " + " + d1 + " + " + d2 + " / " + cm + " = " + ResultadoTotalCantidad);
                return ResultadoTotalCantidad;
            }
            else if (selectedIndex == 1)
            {
                ResultadoTotalCantidad = (cantidad * d1) / cm;
                Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + " + " + d1 + " / " + cm + " = " + ResultadoTotalCantidad);
                return ResultadoTotalCantidad;
            }
            else
            {
                ResultadoTotalCantidad = 0;
                return ResultadoTotalCantidad;
            }
        }
        private decimal CalcularDesperdicio(decimal totalDesperdicio, decimal totalCantidad)
        {
            ResultadoDesperdicio = totalDesperdicio * totalCantidad;
            Console.WriteLine($"ResultadoDesperdicio :" + totalDesperdicio + " * " + totalCantidad + " = " + ResultadoDesperdicio);
            return ResultadoDesperdicio;
        }
        private decimal CalcularValorTotal(decimal ValorU)
        {
            ResultadoValorTotal = (ResultadoDesperdicio + ResultadoTotalCantidad) * ValorU;
            Console.WriteLine($"ResultadoValorTotal :" + ResultadoDesperdicio + " + " + ResultadoTotalCantidad + " * " + ValorU + " = " + ResultadoValorTotal);

            return ResultadoValorTotal;
        }
        private decimal CalcularValorTotalManoObra(decimal valorU, decimal totalCantidad)
        {
            ResultadoValorTotal = totalCantidad * valorU;
            Console.WriteLine($"ResultadoValorTotal :" + totalCantidad + " * " + valorU + " = " + ResultadoValorTotal);

            return ResultadoValorTotal;
        }

        
    }
}
