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
    public partial class FRegistroTipoManoObra : Form
    {
        public FRegistroTipoManoObra()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FRegistroTipoManoObra_Load(object sender, EventArgs e)
        {
            ListarTodo();
        }

        bool Editar;
        bool Modificar;
        int IdTipoManoObra;
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(txtNombre))
                return;
            if (Modificar)
            {
                if (!GuardarEditado())
                    return;
            }
            else
                if (!Guardar())
                {
                    MessageBox.Show("ERROR!. Dato Existente.");
                    return;
                }
            Finalizar();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTipoMO.SelectedRows.Count == 0 || dgvTipoMO.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvTipoMO.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvTipoMO.CurrentRow.Cells["id_tipo_mano_obra"];
                int id_tipo = Convert.ToInt32(cell.Value);
                IdTipoManoObra = id_tipo;
                Eliminar();
                Finalizar();
            }
        }
        private System.Windows.Forms.TextBox ultimoTextBoxModificado = null;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex != -1 && ultimoTextBoxModificado != null)
                {
                    string selectedText = listBox1.SelectedItem.ToString();
                    ultimoTextBoxModificado.Text = selectedText;
                }
                else
                {
                    MessageBox.Show("No se pudo determinar el TextBox correspondiente.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }            
        }
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtNombre.Text;

            BuscarYMostrarResultados("RetornarNombreTipo", txtNombre, listBox1, "@NombreBuscado", "nombre_tipo");
        }
        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = txtNombre.Text;

            BuscarYMostrarResultados("RetornarNombreTipo", txtNombre, listBox1, "@NombreBuscado", "nombre_tipo");
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdTipoManoObra = Convert.ToInt32(dgvTipoMO.CurrentRow.Cells["id_tipo_mano_obra"].Value);
            txtNombre.Text = dgvTipoMO.CurrentRow.Cells["nombre_tipo"].Value.ToString();

            Editar = true;
            Modificar = true;
        }
        private void dgvTipoMO_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvTipoMO.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvTipoMO.SelectedRows[0];

                    if (selectedRow.Cells.Count >= 5)
                    {
                        IdTipoManoObra = Convert.ToInt32(selectedRow.Cells["id_tipo_mano_obra"].Value);
                        txtNombre.Text = Convert.ToString(selectedRow.Cells["nombre_tipo"].Value);

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
        private void ToUpperText()
        {
            txtNombre.CharacterCasing = CharacterCasing.Upper;
            txtNombre.Click += TextBox_Click;
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }
        private void ListarTodo()
        {
            dgvTipoMO.DataSource = TipoManoObra.ListarTodo();
            DbDatos.OcultarIds(dgvTipoMO);
            PersonalizarColumnasGrid();
        }
        private void PersonalizarColumnasGrid()
        {
            foreach (DataGridViewColumn columna in dgvTipoMO.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "nombre_tipo")
                    {
                        dgvTipoMO.Columns["nombre_tipo"].HeaderText = "Tipo";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        DbDatos.OcultarIds(dgvTipoMO);
                        dgvTipoMO.Columns[columna.Name].Width = 150;
                    }
                    ConfigurarCabeceraColumna(columna, columna.HeaderText);
                }
            }
        }
        private void ConfigurarCabeceraColumna(DataGridViewColumn columna, string nuevoHeaderText)
        {
            string nuevoHeaderTextMayusculas = nuevoHeaderText.ToUpper();

            columna.HeaderText = nuevoHeaderTextMayusculas;
            columna.HeaderCell.Style.Font = new Font(columna.DataGridView.Font, FontStyle.Bold);
            columna.HeaderCell.Style.Font = new Font(columna.HeaderCell.Style.Font, FontStyle.Bold);
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
        private bool Guardar()
        {
            try
            {
                TipoManoObra tipo = new TipoManoObra()
                {
                    IdTipoManoObra = IdTipoManoObra,
                    NombreTipo = txtNombre.Text,
                };
                return TipoManoObra.Guardar(tipo, Editar);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar.");
                return false;
            }            
        }
        private bool GuardarEditado()
        {
            if (IdTipoManoObra > 0)
            {
                TipoManoObra tipo = new TipoManoObra()
                {
                    IdTipoManoObra = IdTipoManoObra,
                    NombreTipo = txtNombre.Text,
                };
                return TipoManoObra.Guardar(tipo, true);
            }
            else
            {
                MessageBox.Show("Selecciona un dato antes de guardar editado.", "Materia prima no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private bool Eliminar()
        {
            try
            {
                if (IdTipoManoObra > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este dato?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return TipoManoObra.Eliminar(IdTipoManoObra);
                    else
                        return false;
                }
                else
                {
                    MessageBox.Show("Selecciona un dato antes de eliminar.", "Materia prima no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("No se puede eliminar este dato porque tiene registros relacionados .", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            catch (Exception)
            {
                MessageBox.Show($"Se produjo un error al intentar eliminar la amteria prima. Consulta los detalles en la consola.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtNombre.Text = "";
        }       
        private void BuscarYMostrarResultados(string nombreProcedimiento, System.Windows.Forms.TextBox textBox, ListBox listBox, string parametroNombre, string nombreColumna)
        {
            string searchText = textBox.Text;

            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro(parametroNombre, searchText)
            };

            DataTable result = DbDatos.Listar(nombreProcedimiento, parametros);
            listBox.Items.Clear();

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                    listBox.Items.Add(row[nombreColumna].ToString());
            }
        }            
    }
}
