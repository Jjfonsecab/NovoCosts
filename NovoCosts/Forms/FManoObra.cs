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
    public partial class FManoObra : Form
    {
        public FManoObra()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FManoObra_Load(object sender, EventArgs e)
        {
            ListarTodo();
            ListarProductos();
            monthCalendar.DateChanged += monthCalendar_DateChanged;
        }

        bool Editar;
        bool Modificar;
        int IdManoObra;
        int IdProducto;
        int IdTipoManoObra;

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
            if (!ValidarCampos(txtReferencia, txtDescripcion, comboBoxTMO, txtCosto, txtFecha))
                return;
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

        private System.Windows.Forms.TextBox campoSeleccionado;
        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime fechaSeleccionada = monthCalendar.SelectionStart;

            txtFecha.Text = fechaSeleccionada.ToString("yyyy-MM-dd");
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

                    if (selectedRow.Cells.Count >= 5)
                    {
                        // Verifica si el valor es DBNull antes de convertir
                        IdManoObra = Convert.ToInt32(selectedRow.Cells["id_mano_obra"].Value);
                        IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                        IdTipoManoObra = Convert.ToInt32(selectedRow.Cells["id_tipo_mano_obra"].Value);
                        txtCosto.Text = Convert.ToString(selectedRow.Cells["costo"].Value);
                        txtDescripcion.Text = Convert.ToString(selectedRow.Cells["Descripcion"].Value);
                        txtReferencia.Text = Convert.ToString(selectedRow.Cells["Referencia"].Value);
                        comboBoxTMO.Text = Convert.ToString(selectedRow.Cells["Nombre"].Value);

                        txtFecha.Text = selectedRow.Cells["fecha"].Value.ToString();

                        Editar = true;
                        Modificar = true;
                    }
                }
            }
            catch (StrongTypingException)
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
            if (dgvProductos.SelectedRows.Count > 0)
            {
                // Obtener la fila seleccionada
                DataGridViewRow selectedRow = dgvProductos.SelectedRows[0];

                // Asegurarse de que la fila tiene al menos dos celdas
                if (selectedRow.Cells.Count >= 2)
                {
                    IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                    txtDescripcion.Text = selectedRow.Cells[2].Value.ToString();
                    txtReferencia.Text = selectedRow.Cells[1].Value.ToString();
                }
            }
        }
        private void comboBoxMO_DropDown(object sender, EventArgs e)
        {
            ListarTipoManoObra();
        }        
        private void comboBoxMO_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView selectedRow = (DataRowView)comboBoxTMO.SelectedItem;

            if (selectedRow != null)
            {
                IdTipoManoObra = Convert.ToInt32(selectedRow["id_tipo_mano_obra"]);

                Console.WriteLine("IdUnidadMedida: " + IdTipoManoObra);

            }
        }

        //Metodos
        private bool Guardar()
        {
            ManoObra manoobra = new ManoObra()
            {
                IdManoObra = IdManoObra,
                IdProducto = IdProducto,
                IdTipoManoObra = IdTipoManoObra,
                Costo = Convert.ToInt32(txtCosto.Text),
                Fecha = DateTime.Parse(txtFecha.Text),
            };


            return ManoObra.Guardar(manoobra, Editar);
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
            ListarTodo();
            Limpiar();
        }
        private void Limpiar()
        {
            txtReferencia.Text = "";
            comboBoxTMO.Text = "";
            txtDescripcion.Text = "";
            txtCosto.Text = "";
            txtFecha.Text = "";
            Editar = false;
        }
        private void ListarTodo()
        {
            dgvManoObra.DataSource = ManoObra.ListarCompleto();
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

            comboBoxTMO.DataSource = dataTable;
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
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }        
        private void PersonalizarColumnasGrid()
        {
            // Itera sobre todas las columnas del DataGridView
            foreach (DataGridViewColumn columna in dgvManoObra.Columns)
            {
                // Asegúrate de que la columna tenga un nombre
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    ConfigurarCabeceraColumna(columna, columna.HeaderText);
                    // Puedes personalizar las columnas según su nombre o cualquier otra condición necesaria
                    if (columna.Name == "valor_unitario")
                    {
                        dgvManoObra.Columns["valor_unitario"].HeaderText = "Valor Unitario";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                    else if (columna.Name == "nombre_materia_prima")
                    {
                        dgvManoObra.Columns["nombre_materia_prima"].HeaderText = "Detalle";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvManoObra.Columns["fecha"].HeaderText = "Fecha";
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                }
            }
        }
        private void ConfigurarCabeceraColumna(DataGridViewColumn columna, string nuevoHeaderText)
        {
            columna.HeaderText = nuevoHeaderText;
            columna.HeaderCell.Style.Font = new Font(columna.DataGridView.Font, FontStyle.Bold);
        }        

        
    }
}
