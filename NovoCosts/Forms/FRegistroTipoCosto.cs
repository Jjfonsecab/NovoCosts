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
    public partial class FRegistroTipoCosto : Form
    {
        public FRegistroTipoCosto()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FRegistroTipoCosto_Load(object sender, EventArgs e)
        {
            ListarTodo();
        }

        bool Editar;
        bool Modificar;
        int IdTipoCosto;

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
            if (dgvTipoCosto.SelectedRows.Count == 0 || dgvTipoCosto.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvTipoCosto.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvTipoCosto.CurrentRow.Cells["id_tipo_costo"];
                int id_tipocosto = Convert.ToInt32(cell.Value);
                IdTipoCosto = id_tipocosto;
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
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdTipoCosto = Convert.ToInt32(dgvTipoCosto.CurrentRow.Cells["id_tipo_costo"].Value);
            txtNombre.Text = dgvTipoCosto.CurrentRow.Cells["nombre"].Value.ToString();

            Editar = true;
            Modificar = true;
        }
        private void dgvTipoCosto_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvTipoCosto.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvTipoCosto.SelectedRows[0];

                    if (selectedRow.Cells.Count >= 5)
                    {
                        IdTipoCosto = Convert.ToInt32(selectedRow.Cells["id_tipo_costo"].Value);
                        txtNombre.Text = Convert.ToString(selectedRow.Cells["nombre"].Value);

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

        //Metodos

        private bool Guardar()
        {
            try
            {
                TipoCosto tipo = new TipoCosto()
                {
                    IdTipoCosto = IdTipoCosto,
                    Nombre = txtNombre.Text,
                };

                return TipoCosto.Guardar(tipo, Editar);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar.");
                return false;
            }            
        }
        private bool GuardarEditado()
        {
            if (IdTipoCosto > 0)
            {
                TipoCosto tipo = new TipoCosto()
                {
                    IdTipoCosto = IdTipoCosto,
                    Nombre = txtNombre.Text,
                };

                return TipoCosto.Guardar(tipo, true);
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
                if (IdTipoCosto > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este dato?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return TipoCosto.Eliminar(IdTipoCosto);
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
                // Mensaje genérico para otras excepciones
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
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
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
            dgvTipoCosto.DataSource = TipoCosto.ListarTodo();
            DbDatos.OcultarIds(dgvTipoCosto);
            PersonalizarColumnasGrid();
        }
        private void PersonalizarColumnasGrid()
        {
            foreach (DataGridViewColumn columna in dgvTipoCosto.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
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
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtNombre.Text;

            BuscarYMostrarResultados("RetornarNombreTipoCosto", txtNombre, listBox1, "@NombreBuscado", "nombre");

        }
        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = txtNombre.Text;

            BuscarYMostrarResultados("RetornarNombreTipoCosto", txtNombre, listBox1, "@NombreBuscado", "nombre");

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
