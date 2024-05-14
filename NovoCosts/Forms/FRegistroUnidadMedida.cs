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
    public partial class FRegistroUnidadMedida : Form
    {
        public FRegistroUnidadMedida()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FRegistroUnidadMedida_Load(object sender, EventArgs e)
        {
            Listar();
        }

        bool Editar;
        int IdUnidadMedia;
        bool Modificar;

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(txtNombre, comboBoxP))
                return;
            if (Modificar)
            {
                if (!GuardarEditado())
                    return;
            }
            else
                if (!Guardar()) return;
            MessageBox.Show("Proceso Exitoso.");

            Finalizar();
        }        
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvRegistroUnidades.SelectedRows.Count == 0 || dgvRegistroUnidades.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvRegistroUnidades.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvRegistroUnidades.CurrentRow.Cells["id_unidad_medida"];
                int id_unidades_medida = Convert.ToInt32(cell.Value);
                IdUnidadMedia = id_unidades_medida;
                Eliminar();
                Finalizar();
                MessageBox.Show("Proceso Exitoso.");
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
        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = txtNombre.Text;
            BuscarYMostrarResultados("RetornarUnidadesPorNombre", txtNombre, listBox1, "@NombreBuscado", "nombre");
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdUnidadMedia = Convert.ToInt32(dgvRegistroUnidades.CurrentRow.Cells["id_unidad_medida"].Value);
            txtNombre.Text = dgvRegistroUnidades.CurrentRow.Cells["id_unidad_medida"].Value.ToString();
            comboBoxP.Text = dgvRegistroUnidades.CurrentRow.Cells["cantidad_parametros"].Value.ToString();

            Editar = true;
            Modificar = true;
        }

        //Methods
        private bool Guardar()
        {
            try
            {
                UnidadesMedida unidadesMedida = new UnidadesMedida()
                {
                    IdUnidadMedida = IdUnidadMedia,  // Asegúrate de tener el ID correcto
                    Nombre = txtNombre.Text,
                    CantidadParametros = Convert.ToInt32(comboBoxP.Text),
                };
                
                return UnidadesMedida.Guardar(unidadesMedida, Editar);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar.");
                return false;
            }
                     
        }
        private bool GuardarEditado()
        {
            if (IdUnidadMedia > 0)
            {
                UnidadesMedida unidadesMedida = new UnidadesMedida()
                {
                    IdUnidadMedida = IdUnidadMedia,  
                    Nombre = txtNombre.Text,
                    CantidadParametros = Convert.ToInt32(comboBoxP.Text),                    
                };

                return UnidadesMedida.Guardar(unidadesMedida, Editar); 
            }
            else
            {
                MessageBox.Show("Selecciona una materia prima antes de guardar editado.", "Materia prima no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private bool Eliminar()
        {
            try
            {
                if (IdUnidadMedia > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar esta unidad de medida?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return UnidadesMedida.Eliminar(IdUnidadMedia);
                    else
                        return false;
                }
                else
                {
                    MessageBox.Show("Selecciona una Unidad antes de eliminar.", "Unidad de Medida no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("No se puede eliminar esta Unidad de Medida porque tiene registros relacionados.", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            catch (Exception)
            {
                // Mensaje genérico para otras excepciones
                MessageBox.Show($"Se produjo un error al intentar eliminar la Unidad de Medida. Consulta los detalles en la consola.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        private void Finalizar()
        {
            Listar();
            Limpiar();
        }
        private void Limpiar()
        {
            txtNombre.Text = "";
            comboBoxP.Text = "";

            Editar = false;
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
        private void Listar()
        {
            dgvRegistroUnidades.DataSource = UnidadesMedida.Listar();
            DbDatos.OcultarIds(dgvRegistroUnidades);
            PersonalizarColumnasGrid();
        }
        private void PersonalizarColumnasGrid()
        {
            foreach (DataGridViewColumn columna in dgvRegistroUnidades.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))                {
                    
                    if (columna.Name == "cantidad_parametros")
                    {
                        dgvRegistroUnidades.Columns["cantidad_parametros"].HeaderText = "PARAMETROS";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvRegistroUnidades);
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
