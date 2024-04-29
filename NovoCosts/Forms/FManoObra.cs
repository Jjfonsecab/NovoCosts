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
    public partial class FManoObra : Form
    {
        public FManoObra()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FManoObra_Load(object sender, EventArgs e)
        {
            //ListarTodo();
            ListarProductos();
            MostrarFechaActual();
        }

        bool Editar;
        bool Modificar = false;
        int IdManoObra;
        int IdProducto;
        int IdTipoManoObra;
        decimal ResultadoValorTotal;

        private void btnInicio_Click(object sender, EventArgs e)
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
        private void btnTipo_Click(object sender, EventArgs e)
        {
            FRegistroTipoManoObra ftipo = Application.OpenForms.OfType<FRegistroTipoManoObra>().FirstOrDefault();

            if (ftipo == null)
            {
                ftipo = new FRegistroTipoManoObra();
                ftipo.Show();
            }
            else
                ftipo.BringToFront();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(txtReferencia, txtDescripcion, comboBoxTMO, txtCosto, txtFecha, txtCantidad))
                return;
            else
            {
                CalcularValorTotal(Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtCosto.Text));
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
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvManoObra.SelectedRows.Count == 0 || dgvManoObra.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvManoObra.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvManoObra.CurrentRow.Cells["id_mano_obra"];
                int id_mano_obra = Convert.ToInt32(cell.Value);
                IdManoObra = id_mano_obra;
                Eliminar();
                Finalizar();
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
            Modificar = false;
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdManoObra = Convert.ToInt32(dgvManoObra.CurrentRow.Cells["id_mano_obra"].Value);
            IdProducto = Convert.ToInt32(dgvManoObra.CurrentRow.Cells["id_producto"].Value);
            IdTipoManoObra = Convert.ToInt32(dgvManoObra.CurrentRow.Cells["id_tipo_mano_obra"].Value);
            txtCosto.Text = dgvManoObra.CurrentRow.Cells["costo"].Value.ToString();

            DateTime fechaOriginal = DateTime.Parse(dgvManoObra.CurrentRow.Cells["fecha"].Value.ToString());
            txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

            Editar = true;
            Modificar = true;
        }
        private void dgvManoObra_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvManoObra.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvManoObra.SelectedRows[0];
                    if (selectedRow != null)
                    {
                        IdManoObra = Convert.ToInt32(selectedRow.Cells["id_mano_obra"].Value);
                        IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                        IdTipoManoObra = Convert.ToInt32(selectedRow.Cells["id_tipo_mano_obra"].Value);
                        txtCosto.Text = Convert.ToString(selectedRow.Cells["costo"].Value);
                        txtDescripcion.Text = Convert.ToString(selectedRow.Cells["Descripcion"].Value);
                        txtReferencia.Text = Convert.ToString(selectedRow.Cells["Referencia"].Value);
                        txtCantidad.Text = Convert.ToString(selectedRow.Cells["Cantidad"].Value);
                        comboBoxTMO.Text = Convert.ToString(selectedRow.Cells["Nombre"].Value);

                        DataGridViewCell fechaCell = selectedRow.Cells["fecha"];
                        if (fechaCell != null && fechaCell.Value != null)
                            txtFecha.Text = fechaCell.Value.ToString();
                        else
                        {
                            MessageBox.Show("Valor nulo!");
                            return;
                        }

                        Editar = true;
                        Modificar = true;

                    }

                }
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Fila vacia");
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar");
                return;
            }
        }
        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvProductos.SelectedRows[0];

                    if (selectedRow != null)
                    {
                        DataGridViewCell idProductoCell = selectedRow.Cells["id_producto"];
                        DataGridViewCell descripcionCell = selectedRow.Cells["descripcion"];
                        DataGridViewCell referenciaCell = selectedRow.Cells["referencia"];
                        if (idProductoCell != null && idProductoCell.Value != null)
                        {
                            IdProducto = Convert.ToInt32(idProductoCell.Value);
                            ListarTodoPorProducto(IdProducto);
                        }                      
                        else
                        {
                            MessageBox.Show("Valor nulo!");
                            return;
                        }
                            
                        if (descripcionCell != null && descripcionCell.Value != null)
                            txtDescripcion.Text = descripcionCell.Value.ToString();
                        else
                        {
                            MessageBox.Show("Valor nulo!");
                            return;
                        }
                        if (referenciaCell != null && referenciaCell.Value != null)
                            txtReferencia.Text = referenciaCell.Value.ToString();
                        else
                        {
                            MessageBox.Show("Valor nulo!");
                            return;
                        }

                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }
        }
        private void comboBoxMO_DropDown(object sender, EventArgs e)
        {
            ListarTipoManoObra();
        }
        private void comboBoxMO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)comboBoxTMO.SelectedItem;

                if (selectedRow != null)
                {
                    IdTipoManoObra = Convert.ToInt32(selectedRow["id_tipo_mano_obra"]);

                    Console.WriteLine("IdUnidadMedida: " + IdTipoManoObra);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }
        }

        //Metodos
        private bool Guardar()
        {
            try
            {
                ManoObra manoobra = new ManoObra()
                {
                    IdManoObra = IdManoObra,
                    IdProducto = IdProducto,
                    IdTipoManoObra = IdTipoManoObra,
                    Costo = Convert.ToDecimal(txtCosto.Text),
                    Fecha = DateTime.Parse(txtFecha.Text),
                    TotalCantidad = Convert.ToDecimal(txtCantidad.Text),
                    ValorTotal = ResultadoValorTotal,
                };

                return ManoObra.Guardar(manoobra, Editar);
            }
            catch (Exception)
            {
                MessageBox.Show("Error !.");
                return false;
            }

        }
        private bool GuardarEditado()
        {
            if (IdManoObra > 0)
            {
                DateTime fechaOriginal = DateTime.Parse(dgvManoObra.CurrentRow.Cells["fecha"].Value.ToString());

                ManoObra manoobra = new ManoObra()
                {
                    IdManoObra = IdManoObra,
                    IdProducto = IdProducto,
                    IdTipoManoObra = IdTipoManoObra,
                    Costo = Convert.ToInt32(txtCosto.Text),
                    Fecha = fechaOriginal,
                };

                return ManoObra.Guardar(manoobra, true);
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
                if (IdManoObra > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este dato?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return ManoObra.Eliminar(IdManoObra);
                    return false;
                }
                MessageBox.Show("Selecciona una mano de obra antes de eliminar.", "Mano de Obra no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private void Finalizar()
        {
            //ListarTodo();
            Limpiar();
        }
        private void Limpiar()
        {
            txtReferencia.Text = "";
            comboBoxTMO.Text = "";
            txtDescripcion.Text = "";
            txtCosto.Text = "";
            txtFecha.Text = "";
            txtCantidad.Text = "";
            Editar = false;
            MostrarFechaActual();
        }
        private void ListarTodo()
        {
            dgvManoObra.DataSource = ManoObra.ListarCompleto();
            DbDatos.OcultarIds(dgvManoObra);
            PersonalizarColumnasGrid();
        }
        private void ListarTodoPorProducto(int IdProducto)
        {
            dgvManoObra.DataSource = ManoObra.ListarPorProducto(IdProducto);
            DbDatos.OcultarIds(dgvManoObra);
            PersonalizarColumnasGrid();
        }
        private void ListarProductos()
        {
            dgvProductos.DataSource = Producto.ListarTodo();
            DbDatos.OcultarIds(dgvProductos);
            PersonalizarColumnasGrid();
        }
        private void ListarTipoManoObra()
        {
            DataTable dataTable = TipoManoObra.ListarTodo();

            var filteredRows = dataTable.AsEnumerable()
                                .Where(row => row.Field<string>("nombre_tipo") != "PORCENTAJE")
                                .CopyToDataTable();

            comboBoxTMO.DataSource = filteredRows;
            comboBoxTMO.DisplayMember = "nombre_tipo";
            comboBoxTMO.ValueMember = "id_tipo_mano_obra";
        }
        private bool ValidarCampos(params Control[] controles)
        {
            foreach (var control in controles)
            {
                if (control is System.Windows.Forms.TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos antes de guardar.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
        {
            txtReferencia.CharacterCasing = CharacterCasing.Upper;
            txtReferencia.Click += TextBox_Click;
            txtDescripcion.CharacterCasing = CharacterCasing.Upper;
            txtDescripcion.Click += TextBox_Click;

            txtFecha.CharacterCasing = CharacterCasing.Upper;
        }
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }
        private void PersonalizarColumnasGrid()
        {
            foreach (DataGridViewColumn columna in dgvManoObra.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {

                    if (columna.Name == "costo")
                    {
                        dgvManoObra.Columns["costo"].HeaderText = "COSTO UNITARIO";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvManoObra.Columns["fecha"].HeaderText = "FECHA";
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                    ConfigurarCabeceraColumna(columna, columna.HeaderText);
                }
            }
            foreach (DataGridViewColumn columna in dgvProductos.Columns)
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
        }        
        private decimal CalcularValorTotal(decimal vCantidad, decimal vUnitario)
        {
            ResultadoValorTotal = vCantidad * vUnitario;

            return ResultadoValorTotal;
        }
    }
}
