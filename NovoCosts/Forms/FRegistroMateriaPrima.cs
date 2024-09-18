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
using static NovoCosts.Forms.FRegistroMateriaPrima;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace NovoCosts.Forms
{
    public partial class FRegistroMateriaPrima : Form
    {
        int usuarioIdActual = CurrentUser.UserId;
        private System.Windows.Forms.ToolTip toolTip;
        public FRegistroMateriaPrima()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void Registro_Load(object sender, EventArgs e)
        {
            ListarTodo();
            monthCalendar.DateChanged += monthCalendar_DateChanged;
            ToolTipInitialicer();
        }

        bool Editar;
        bool Modificar;
        int IdMateriaPrima;
        int IdUnidadMedida;
        int NumeroParametros;
        decimal ResultadoMedida;

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
            RealizarCalculo();
            if (!ValidarCamposString(txtDetalle, comboBoxUnidadMedida, txtProveedor, txtFecha))
                return;
            else if (!ValidarCamposNumericos(txtValorUnitario, txtCantidadDesperdicio))
                return;

            if (Modificar)
            {
                if (!GuardarEditado())
                    return;
            }
            else
                if (!Guardar()) return;

            Modificar = false;
            MessageBox.Show("Proceso Exitoso.");
            Finalizar();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMateriaPrima.SelectedRows.Count == 0 || dgvMateriaPrima.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvMateriaPrima.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvMateriaPrima.CurrentRow.Cells["id_materia_prima"];//Antes id_producto
                int id_materia_prima = Convert.ToInt32(cell.Value);
                IdMateriaPrima = id_materia_prima;
                Eliminar();
                MessageBox.Show("Eliminacion Exitosa.");
                Finalizar();
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
            txtComentarios.Text = "N.A.";
            Modificar = false;
        }
        private void btnRegistroUnidades_Click(object sender, EventArgs e)
        {
            FRegistroUnidadMedida fregistro = Application.OpenForms.OfType<FRegistroUnidadMedida>().FirstOrDefault();

            if (fregistro == null)
            {
                fregistro = new FRegistroUnidadMedida();
                fregistro.Show();
            }
            else
                fregistro.BringToFront();
        }
        private void comboBoxUnidadMedida_DropDown(object sender, EventArgs e)
        {
            ListarUnidadesMedia();
        }
        private void comboBoxUnidadMedida_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)comboBoxUnidadMedida.SelectedItem;

                if (selectedRow != null)
                {
                    IdUnidadMedida = Convert.ToInt32(selectedRow["id_unidad_medida"]);
                    NumeroParametros = Convert.ToInt32(selectedRow["cantidad_parametros"]);

                    Console.WriteLine("IdUnidadMedida: " + IdUnidadMedida);
                    Console.WriteLine("NumeroParametros: " + NumeroParametros);

                    string unidadMedida = selectedRow["nombre"].ToString();

                    if (unidadMedida.Contains("TABLA"))
                    {
                        txtOtros.Enabled = true;
                        txtDividir.Enabled = true;
                    }
                    else                    
                        ActivarControlesSegunParametros();                    
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
            IdMateriaPrima = Convert.ToInt32(dgvMateriaPrima.CurrentRow.Cells["id_materia_prima"].Value);
            txtDetalle.Text = dgvMateriaPrima.CurrentRow.Cells["detalle_mp"].Value.ToString();
            IdUnidadMedida = Convert.ToInt32(dgvMateriaPrima.CurrentRow.Cells["id_unidad_medida"].Value);
            txtOtros.Text = dgvMateriaPrima.CurrentRow.Cells["medida"].Value.ToString();
            txtValorUnitario.Text = dgvMateriaPrima.CurrentRow.Cells["valor"].Value.ToString();
            txtProveedor.Text = dgvMateriaPrima.CurrentRow.Cells["proveedor"].Value.ToString();
            txtCantidadDesperdicio.Text = dgvMateriaPrima.CurrentRow.Cells["desperdicio_cantidad"].Value.ToString();
            txtComentarios.Text = dgvMateriaPrima.CurrentRow.Cells["comentarios"].Value.ToString();

            DateTime fechaOriginal = DateTime.Parse(dgvMateriaPrima.CurrentRow.Cells["fecha"].Value.ToString());
            txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

            Editar = true;
            Modificar = true;
        }
        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime fechaSeleccionada = monthCalendar.SelectionStart;

            txtFecha.Text = fechaSeleccionada.ToString("yyyy-MM-dd");
        }
        string searchText;
        private void txtDetalle_TextChanged(object sender, EventArgs e)
        {
            searchText = txtDetalle.Text;

            BuscarYMostrarResultados("RetornarMateriaPrimaPorDetalle", txtDetalle, listBox1, "@NombreBuscado", "detalle_mp");
        }
        private void txtProveedor_TextChanged(object sender, EventArgs e)
        {
           searchText = txtProveedor.Text;

            BuscarYMostrarResultados("RetornarMateriaPrimaPorProveedor", txtProveedor, listBox1, "@NombreBuscado", "proveedor");
        }

        private System.Windows.Forms.TextBox ultimoTextBoxModificado = null;
        private void txtDetalle_KeyUp(object sender, KeyEventArgs e)
        {
            ultimoTextBoxModificado = txtDetalle;
        }
        private void txtProveedor_KeyUp(object sender, KeyEventArgs e)
        {
            ultimoTextBoxModificado = txtProveedor;
        }
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
                    MessageBox.Show("No se pudo determinar el TextBox correspondiente.");
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
                MateriasPrimas materiasPrimas = new MateriasPrimas()
                {
                    DetalleMateriaPrima = txtDetalle.Text,
                    IdUnidadMedida = IdUnidadMedida,
                    Medida = ResultadoMedida,
                    Valor = Convert.ToDecimal(txtValorUnitario.Text),
                    Proveedor = txtProveedor.Text,
                    Desperdicio = Convert.ToDecimal(txtCantidadDesperdicio.Text),
                    Fecha = DateTime.Parse(txtFecha.Text),
                    Comentarios = txtComentarios.Text,
                    IdMateriaPrima = IdMateriaPrima,
                    IdUser = usuarioIdActual,
                };

                return MateriasPrimas.Guardar(materiasPrimas, Editar);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar.");
                return false;
            }            
        }
        private bool GuardarEditado()
        {
            if (IdMateriaPrima > 0)
            {
                DateTime fechaOriginal = DateTime.Parse(dgvMateriaPrima.CurrentRow.Cells["fecha"].Value.ToString());

                MateriasPrimas materiasPrimasEditado = new MateriasPrimas()
                {
                    IdMateriaPrima = IdMateriaPrima,
                    DetalleMateriaPrima = txtDetalle.Text,
                    IdUnidadMedida = IdUnidadMedida,
                    Medida = ResultadoMedida,
                    Valor = Convert.ToDecimal(txtValorUnitario.Text),
                    Proveedor = txtProveedor.Text,
                    Desperdicio = Convert.ToDecimal(txtCantidadDesperdicio.Text),
                    Fecha = fechaOriginal,
                    Comentarios = txtComentarios.Text,
                    IdUser = usuarioIdActual,
                };
                MessageBox.Show("Actualizando costos.!");
                return MateriasPrimas.Guardar(materiasPrimasEditado, true);
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
                if (IdMateriaPrima > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar esta materia prima?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return MateriasPrimas.Eliminar(IdMateriaPrima);
                    else
                        return false;
                }
                else
                {
                    MessageBox.Show("Selecciona una materia prima antes de eliminar.", "Materia prima no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("No se puede eliminar esta materia prima porque tiene registros relacionados .", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            txtDetalle.Text = "";
            comboBoxUnidadMedida.Text = "";
            txtLargo.Text = "";
            txtAlto.Text = "";
            txtAncho.Text = "";
            txtOtros.Text = "";
            txtValorUnitario.Text = "";
            txtProveedor.Text = "";
            txtCantidadDesperdicio.Text = "";
            txtFecha.Text = "";
            txtDividir.Text = "";
            txtComentarios.Text = "N.A.";
            listBox1.Text = null;
            ultimoTextBoxModificado = null;
            searchText = ""; 
            Editar = false;
            MostrarFechaActual();
        }
        private void ListarTodo()
        {
            dgvMateriaPrima.DataSource = MateriasPrimas.ListarTodo();
            DbDatos.OcultarIds(dgvMateriaPrima);
            PersonalizarColumnasGrid();
        }
        private void ListarUnidadesMedia()
        {
            DataTable dataTable = UnidadesMedida.Listar();

            comboBoxUnidadMedida.DataSource = dataTable;
            comboBoxUnidadMedida.DisplayMember = "nombre";
            comboBoxUnidadMedida.ValueMember = "cantidad_parametros";
        }
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void PersonalizarColumnasGrid()
        {
            foreach (DataGridViewColumn columna in dgvMateriaPrima.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    DbDatos.OcultarIds(dgvMateriaPrima);
                    if (columna.Name == "detalle_mp" || columna.Name == "proveedor" || columna.Name == "comentarios")
                    {
                        dgvMateriaPrima.Columns["detalle_mp"].HeaderText = "DETALLE";
                        dgvMateriaPrima.Columns["proveedor"].HeaderText = "PROVEEDOR";
                        dgvMateriaPrima.Columns["comentarios"].HeaderText = "COMENTARIOS";
                        dgvMateriaPrima.Columns[columna.Name].Width = 170;
                    }
                    else if (columna.Name == "medida" || columna.Name == "desperdicio_cantidad" )
                    {
                        dgvMateriaPrima.Columns["desperdicio_cantidad"].HeaderText = "DESPER";
                        dgvMateriaPrima.Columns["medida"].HeaderText = "MEDIDA";                     
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight;
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        dgvMateriaPrima.Columns[columna.Name].Width = 80;
                    }
                    else if (columna.Name == "fecha")
                    {                        
                        dgvMateriaPrima.Columns["fecha"].Visible = false;
                    }
                    else if (columna.Name == "valor")
                    {
                        dgvMateriaPrima.Columns["valor"].HeaderText = "VALOR";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight;
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        dgvMateriaPrima.Columns[columna.Name].Width = 100;
                    }
                }            
                ConfigurarCabeceraColumna(columna, columna.HeaderText);
            }
            
        }
        private void ConfigurarCabeceraColumna(DataGridViewColumn columna, string nuevoHeaderText)
        {
            columna.HeaderText = nuevoHeaderText;
            columna.HeaderCell.Style.Font = new Font(columna.DataGridView.Font, FontStyle.Bold);
        }
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
        {
            txtDetalle.CharacterCasing = CharacterCasing.Upper;
            txtDetalle.Click += TextBox_Click;
            txtProveedor.CharacterCasing = CharacterCasing.Upper;
            txtProveedor.Click += TextBox_Click;
            txtComentarios.CharacterCasing = CharacterCasing.Upper;
            txtComentarios.Click += TextBox_Click;
            txtCantidadDesperdicio.Click += TextBox_Click;

            txtFecha.CharacterCasing = CharacterCasing.Upper;
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }
        private void ActivarControlesSegunParametros()
        {
            int cantidadParametros = NumeroParametros;

            txtOtros.Enabled = false;
            txtLargo.Enabled = false;
            txtAncho.Enabled = false;
            txtAlto.Enabled = false;

            switch (cantidadParametros)
            {
                case 1:
                    txtOtros.Enabled = true;
                    break;
                case 2:
                    txtLargo.Enabled = true;
                    txtAncho.Enabled = true;
                    break;
                case 3:
                    txtLargo.Enabled = true;
                    txtAncho.Enabled = true;
                    txtAlto.Enabled = true;
                    break;
            }
        }
        private void RealizarCalculo()
        {
            if (txtOtros.Enabled && txtDividir.Enabled)
            {
                if (decimal.TryParse(txtOtros.Text, out decimal otros) && decimal.TryParse(txtDividir.Text, out decimal divisor))
                {
                    ResultadoMedida = otros / divisor;
                }
            }
            else if (txtOtros.Enabled)
            {
                if (decimal.TryParse(txtOtros.Text, out decimal otro))
                {
                    ResultadoMedida = otro;
                }
            }
            else if (txtLargo.Enabled && txtAncho.Enabled && txtAlto.Enabled)
            {
                if (decimal.TryParse(txtLargo.Text, out decimal largo) && decimal.TryParse(txtAncho.Text, out decimal ancho) && decimal.TryParse(txtAlto.Text, out decimal alto))
                {
                    ResultadoMedida = largo * ancho * alto;
                }
            }
            else if (txtLargo.Enabled && txtAncho.Enabled)
            {
                if (decimal.TryParse(txtLargo.Text, out decimal largo) && decimal.TryParse(txtAncho.Text, out decimal ancho))
                {
                    ResultadoMedida = largo * ancho;
                }
            }
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
        string opcionSeleccionada;
        private bool EsNumero(string texto)
        {
            return double.TryParse(texto, out _);
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
        private void ToolTipInitialicer()
        {
            toolTip = new System.Windows.Forms.ToolTip();

            toolTip.SetToolTip(btnLimpiar, "LIMPIAR");
            toolTip.SetToolTip(btnInicio, "INICIO");
            toolTip.SetToolTip(btnRegistroUnidades, "UNIDADES DE MEDIDA");
            toolTip.SetToolTip(btnGuardar, "GUARDAR");
            toolTip.SetToolTip(btnEliminar, "ELIMINAR");
        }
    }
}
