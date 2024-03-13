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
    public partial class FRegistroTapiceriaCostos : Form
    {
        public FRegistroTapiceriaCostos()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FRegistroTapiceriaCostos_Load(object sender, EventArgs e)
        {
            ListarTodo();
            ListarProductos();
            MostrarFechaActual();
        }

        bool Editar;
        bool Modificar;
        int IdTapiceria;
        int IdProducto;

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
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(txtReferencia, txtDescripcion, txtCorte, txtCostura, txtForrado, txtBlanco, txtFecha))
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
            if (dgvTapiceria.SelectedRows.Count == 0 || dgvTapiceria.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvTapiceria.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvTapiceria.CurrentRow.Cells["id_costo_tapiceria"];
                int id_costo_tapiceria = Convert.ToInt32(cell.Value);
                IdTapiceria = id_costo_tapiceria;
                Eliminar();
                Finalizar();
            }
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdTapiceria = Convert.ToInt32(dgvTapiceria.CurrentRow.Cells["id_costo_tapiceria"].Value);
            IdProducto = Convert.ToInt32(dgvTapiceria.CurrentRow.Cells["id_producto"].Value);
            txtCorte.Text = dgvTapiceria.CurrentRow.Cells["corte_alistado"].Value.ToString();
            txtBlanco.Text = dgvTapiceria.CurrentRow.Cells["blanco"].Value.ToString();
            txtCostura.Text = dgvTapiceria.CurrentRow.Cells["costura"].Value.ToString();
            txtForrado.Text = dgvTapiceria.CurrentRow.Cells["forrado"].Value.ToString();

            DateTime fechaOriginal = DateTime.Parse(dgvTapiceria.CurrentRow.Cells["fecha"].Value.ToString());
            txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

            Editar = true;
            Modificar = true;
        }
        private void dgvTapiceria_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvTapiceria.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvTapiceria.SelectedRows[0];

                    if (selectedRow.Cells.Count >= 5)
                    {
                        // Verifica si el valor es DBNull antes de convertir
                        IdTapiceria = Convert.ToInt32(selectedRow.Cells["id_costo_tapiceria"].Value);
                        IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                        txtCorte.Text = Convert.ToString(selectedRow.Cells["corte_alistado"].Value);
                        txtBlanco.Text = Convert.ToString(selectedRow.Cells["blanco"].Value);
                        txtCostura.Text = Convert.ToString(selectedRow.Cells["costura"].Value);
                        txtForrado.Text = Convert.ToString(selectedRow.Cells["forrado"].Value);

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
                DataGridViewRow selectedRow = dgvProductos.SelectedRows[0];

                if (selectedRow.Cells.Count >= 2)
                {
                    IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                    txtDescripcion.Text = selectedRow.Cells[2].Value.ToString();
                    txtReferencia.Text = selectedRow.Cells[1].Value.ToString();
                }
            }
        }
        //Metodos
        private bool Guardar()
        {
            Tapiceria tapiceria = new Tapiceria()
            {
                IdTapiceria = IdTapiceria,
                IdProducto = IdProducto,
                CorteAlistado = Convert.ToInt32(txtCorte.Text),
                Blanco = Convert.ToInt32(txtBlanco.Text),
                Costura = Convert.ToInt32(txtCostura.Text),
                Forrado = Convert.ToInt32(txtForrado.Text),
                Fecha = DateTime.Parse(txtFecha.Text),
            };


            return Tapiceria.Guardar(tapiceria, Editar);
        }
        private bool GuardarEditado()
        {
            if (IdTapiceria > 0)
            {
                DateTime fechaOriginal = DateTime.Parse(dgvTapiceria.CurrentRow.Cells["fecha"].Value.ToString());

                Tapiceria tapiceria = new Tapiceria()
                {
                    IdTapiceria = IdTapiceria,
                    IdProducto = IdProducto,
                    CorteAlistado = Convert.ToInt32(txtCorte.Text),
                    Blanco = Convert.ToInt32(txtBlanco.Text),
                    Costura = Convert.ToInt32(txtCostura.Text),
                    Forrado = Convert.ToInt32(txtForrado.Text),
                    Fecha = fechaOriginal,
                };

                return Tapiceria.Guardar(tapiceria, true);
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
                if (IdTapiceria > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este dato?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return Tapiceria.Eliminar(IdTapiceria);
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
        private void Finalizar()
        {
            ListarTodo();
            Limpiar();
        }
        private void Limpiar()
        {
            txtReferencia.Text = "";
            txtDescripcion.Text = "";
            txtBlanco.Text = "";
            txtCorte.Text = "";
            txtCostura.Text = "";
            txtForrado.Text = "";
            txtFecha.Text = "";
            Editar = false;
        }
        private void ListarTodo()
        {
            dgvTapiceria.DataSource = Tapiceria.ListarTodo();
            DbDatos.OcultarIds(dgvTapiceria);
            PersonalizarColumnasGrid();
        }
        private void ListarProductos()
        {
            dgvProductos.DataSource = Producto.ListarTodo();
            DbDatos.OcultarIds(dgvProductos);
            PersonalizarColumnasGrid();
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
            foreach (DataGridViewColumn columna in dgvTapiceria.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "valor_unitario")
                    {
                        dgvTapiceria.Columns["valor_unitario"].HeaderText = "Valor Unitario";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvTapiceria);
                    }
                    else if (columna.Name == "nombre_materia_prima")
                    {
                        dgvTapiceria.Columns["nombre_materia_prima"].HeaderText = "Detalle";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvTapiceria);
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvTapiceria.Columns["fecha"].HeaderText = "Fecha";
                        DbDatos.OcultarIds(dgvTapiceria);
                    }
                }
                ConfigurarCabeceraColumna(columna, columna.HeaderText);
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
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }


    }
}
