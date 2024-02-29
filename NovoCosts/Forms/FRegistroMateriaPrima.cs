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

namespace NovoCosts.Forms
{
    public partial class FRegistroMateriaPrima : Form
    {
        public FRegistroMateriaPrima()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void Registro_Load(object sender, EventArgs e)
        {
            ListarTodo();
            monthCalendar.DateChanged += monthCalendar_DateChanged;
        }

        bool Editar;
        int IdMateriaPrimas;
        int ResultadoMedida;
        bool Modificar;
        int IdUnidadMedida;

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(txtDetalle, comboBoxUnidadMedida, txtValorUnitario, txtProveedor, txtFecha))
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
        private void btnEliminar_Click(object sender, EventArgs e)//ToDo
        {

        }
        private void btnRegistroUnidades_Click(object sender, EventArgs e)//ToDo
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
            ActivarControlesSegunParametros();
            RealizarCalculoYEnviar();
        }

        private System.Windows.Forms.TextBox campoSeleccionado;
        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime fechaSeleccionada = monthCalendar.SelectionStart;

            txtFecha.Text = fechaSeleccionada.ToString("yyyy-MM-dd");
        }

        private void monthCalendar_DateChanged_1(object sender, DateRangeEventArgs e)
        {
            DateTime fechaSeleccionada = monthCalendar.SelectionStart;

            txtFecha.Text = fechaSeleccionada.ToString("yyyy-MM-dd");
        }
        //Metodos
        private bool Guardar()
        {
            MateriasPrimas materiasPrimas = new MateriasPrimas()
            {
                IdMateriaPrima = IdMateriaPrimas,
                DetalleMateriaPrima = txtDetalle.Text,
                IdUnidadMedida = IdUnidadMedida,
                Medida = ResultadoMedida,
                Valor = Convert.ToDecimal(txtValorUnitario.Text),
                Proveedor = txtProveedor.Text,
                Fecha = DateTime.Parse(txtFecha.Text),
                Comentarios = txtComentarios.Text,
            };

            return MateriasPrimas.Guardar(materiasPrimas, Editar);
        }
        private bool GuardarEditado()
        {
            if (IdMateriaPrimas > 0)
            {
                // Obtén la fecha original almacenada
                DateTime fechaOriginal = DateTime.Parse(dgvMateriaPrima.CurrentRow.Cells["fecha"].Value.ToString());

                MateriasPrimas materiasPrimasEditado = new MateriasPrimas()
                {
                    IdMateriaPrima = IdMateriaPrimas,
                    DetalleMateriaPrima = txtDetalle.Text,
                    IdUnidadMedida = IdUnidadMedida,
                    Medida = ResultadoMedida,
                    Valor = Convert.ToDecimal(txtValorUnitario.Text),
                    Proveedor = txtProveedor.Text,
                    Fecha = fechaOriginal,
                    Comentarios = txtComentarios.Text,
                };

                return MateriasPrimas.Guardar(materiasPrimasEditado, true);  
            }
            else
            {
                MessageBox.Show("Selecciona una materia prima antes de guardar editado.", "Materia prima no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            txtFecha.Text = "";
            Editar = false;
            MostrarFechaActual();
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
            // Itera sobre todas las columnas del DataGridView
            foreach (DataGridViewColumn columna in dgvMateriaPrima.Columns)
            {
                // Asegúrate de que la columna tenga un nombre
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    ConfigurarCabeceraColumna(columna, columna.HeaderText);
                    // Puedes personalizar las columnas según su nombre o cualquier otra condición necesaria
                    if (columna.Name == "valor_unitario")
                    {
                        dgvMateriaPrima.Columns["valor_unitario"].HeaderText = "Valor Unitario";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvMateriaPrima);
                    }
                    else if (columna.Name == "nombre_materia_prima")
                    {
                        dgvMateriaPrima.Columns["nombre_materia_prima"].HeaderText = "Detalle";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvMateriaPrima);
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvMateriaPrima.Columns["fecha"].HeaderText = "Fecha";
                        DbDatos.OcultarIds(dgvMateriaPrima);
                    }
                }
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

            txtFecha.CharacterCasing = CharacterCasing.Upper;
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }             
        private void ActivarControlesSegunParametros()
        {
            if (comboBoxUnidadMedida.SelectedIndex != -1)
            {
                // Obtener el valor de cantidad_parametros como cadena
                string cantidadParametrosString = comboBoxUnidadMedida.SelectedValue.ToString();

                if (int.TryParse(comboBoxUnidadMedida.SelectedValue.ToString(), out int cantidadParametros))
                {
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
                            // Agrega más casos según sea necesario
                    }

                }
            }
        }
        private void RealizarCalculoYEnviar()
        {
            if(txtOtros.Enabled)
            {
                double.TryParse(txtOtros.Text, out double otro);
                otro = ResultadoMedida;
            }
            else if (txtLargo.Enabled && txtAncho.Enabled)
            {
                if (double.TryParse(txtLargo.Text, out double largo) && double.TryParse(txtAncho.Text, out double ancho))
                {
                    double resultado = largo * ancho;
                    resultado = ResultadoMedida;
                 }
            }
            else if(txtLargo.Enabled && txtAncho.Enabled && txtAlto.Enabled)
            {
                if (double.TryParse(txtLargo.Text, out double largo) && double.TryParse(txtAncho.Text, out double ancho) && double.TryParse(txtAlto.Text, out double alto))
                {
                    double resultado = largo * ancho * alto;
                    resultado = ResultadoMedida; 
                }
            }
        }

    }
}
