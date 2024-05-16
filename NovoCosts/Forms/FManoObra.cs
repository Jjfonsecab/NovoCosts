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
            ListarProductos();
            MostrarFechaActual();
        }

        bool Editar;
        bool Modificar = false;
        int IdManoObra;
        int IdProducto;
        int IdTipoManoObra;
        decimal Porcentaje = 0.12m;
        decimal ResultadoValorTotal;
        decimal CostoPorcentaje;
        decimal ValorTotalPorcentaje;

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
            try
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
                    MessageBox.Show("Editado con Exito.!");
                }
                else
                    if (!Guardar()) return;
                else

                    ListarTodoPorProducto(IdProducto);
                Finalizar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar.");
                Console.WriteLine(ex.Message);
                return;
            }

        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
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
                MessageBox.Show("Eliminado con Exito.!");
                ListarTodoPorProducto(IdProducto);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar.");
                return;
            }

        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
            Modificar = false;
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IdManoObra = Convert.ToInt32(dgvManoObra.CurrentRow.Cells["id_mano_obra"].Value);
                IdProducto = Convert.ToInt32(dgvManoObra.CurrentRow.Cells["id_producto"].Value);
                IdTipoManoObra = Convert.ToInt32(dgvManoObra.CurrentRow.Cells["id_tipo_mano_obra"].Value);
                txtCantidad.Text = dgvManoObra.CurrentRow.Cells["total_cantidad"].Value.ToString();
                txtCosto.Text = dgvManoObra.CurrentRow.Cells["costo"].Value.ToString();
                comboBoxTMO.Text = ObtenerNombreTipoManoObra(IdTipoManoObra);

                txtFecha.Text = dgvManoObra.CurrentRow.Cells["fecha"].Value.ToString();

                Editar = true;
                Modificar = true;

                ListarProductoId(IdProducto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Editar.!");
                Console.WriteLine(ex.Message);
                return;
            }

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
                MessageBox.Show("Guardado con Exito.!");
                return ManoObra.Guardar(manoobra, Editar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar!.");
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        private bool GuardarEditado()
        {
            try
            {
                if (IdManoObra > 0)
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
                    EliminarPorcentaje();
                    CostoPorcentaje = 0;
                    return ManoObra.Guardar(manoobra, true);
                }
                else
                {
                    MessageBox.Show("Selecciona una mano de obra antes de guardar editado.", "Mano de Obra no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar Editado!.");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private bool GuardarPorcentaje()
        {
            try
            {
                ManoObra manoobra = new ManoObra()
                {
                    IdManoObra = IdManoObra,
                    IdProducto = IdProducto,
                    IdTipoManoObra = 4,
                    Costo = CostoPorcentaje,
                    Fecha = DateTime.Parse(txtFecha.Text),
                    TotalCantidad = Porcentaje,
                    ValorTotal = ValorTotalPorcentaje,
                };
                ListarProductoId(IdProducto);

                return ManoObra.Guardar(manoobra, Editar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar Porcentaje!.");
                Console.WriteLine(ex.Message);
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
        private bool EliminarPorcentaje()
        {
            try
            {
                MessageBox.Show("Eliminando Porcentaje");
                int idTipoManoObra = 4;

                return ManoObra.EliminarPorIdProductoYTipoManoObra(IdProducto, idTipoManoObra);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error al intentar eliminar. Detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Finalizar()
        {
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
            ListarTodoPorProducto(IdProducto);
        }
        private void ListarTodoPorProducto(int IdProducto)
        {
            dgvManoObra.DataSource = ManoObra.ListarPorProducto(IdProducto);
            DbDatos.OcultarIds(dgvManoObra);
            PersonalizarColumnasGrid();
            BuscarPorcentajeEnTabla();
        }
        private void ListarProductos()
        {
            dgvProductos.DataSource = Producto.ListarTodo();
            DbDatos.OcultarIds(dgvProductos);
            PersonalizarColumnasGrid();
        }
        private void ListarTipoManoObra()
        {
            try
            {
                DataTable dataTable = TipoManoObra.ListarTodo();

                var filteredRows = dataTable.AsEnumerable()
                                    .Where(row => row.Field<string>("nombre_tipo") != "PORCENTAJE")//Ocultamos los datos del combobox
                                    .CopyToDataTable();

                comboBoxTMO.DataSource = filteredRows;
                comboBoxTMO.DisplayMember = "nombre_tipo";
                comboBoxTMO.ValueMember = "id_tipo_mano_obra";

                IdTipoManoObra = filteredRows.Rows[0].Field<int>("id_tipo_mano_obra");
            }
            catch (Exception)
            {
                MessageBox.Show("Lista vacia.!");
                return;
            }

        }
        private void ListarProductoId(int idProducto)
        {
            DataTable dataTable = Producto.ListarProducto(idProducto);

            if (dataTable.Rows.Count > 0)
            {
                txtReferencia.Text = dataTable.Rows[0]["referencia"].ToString();
                txtDescripcion.Text = dataTable.Rows[0]["descripcion"].ToString();
            }
        }
        private string ObtenerNombreTipoManoObra(int idTipoManoObra)
        {
            DataTable dataTable = TipoManoObra.ListarTipoPorId(idTipoManoObra);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0]["nombre_tipo"].ToString();
            }
            return "Error al cargar los datos de tipo de mano de obra";

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
            txtCantidad.Click += TextBox_Click;

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

                    if (columna.Name == "costo" || columna.Name == "valor_total")
                    {
                        dgvManoObra.Columns["costo"].HeaderText = "COSTO";
                        dgvManoObra.Columns["valor_total"].HeaderText = "TOTAL";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        dgvManoObra.Columns[columna.Name].Width = 170;
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvManoObra.Columns["fecha"].HeaderText = "FECHA";
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                    else if (columna.Name == "total_cantidad")
                    {
                        dgvManoObra.Columns["total_cantidad"].HeaderText = "CANTIDAD";

                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        //estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvManoObra);
                        dgvManoObra.Columns[columna.Name].Width = 100;

                    }
                    else if (columna.Name == "nombre_tipo")
                    {
                        dgvManoObra.Columns["nombre_tipo"].HeaderText = "TIPO";
                        dgvManoObra.Columns[columna.Name].Width = 152;
                        dgvManoObra.Columns["nombre_tipo"].DisplayIndex = 0;
                        DbDatos.OcultarIds(dgvManoObra);
                    }
                    ConfigurarCabeceraColumna(columna, columna.HeaderText);

                }
            }
            foreach (DataGridViewColumn columna in dgvProductos.Columns)
            {
                if (columna.Name == "costo")
                {
                    dgvProductos.Columns["costo"].HeaderText = "COSTO";
                    DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                    estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                    estiloCeldaNumerica.Format = "N0";
                    columna.DefaultCellStyle = estiloCeldaNumerica;
                    DbDatos.OcultarIds(dgvProductos);
                }
                else if (columna.Name == "descripcion")
                {
                    dgvProductos.Columns["descripcion"].HeaderText = "DESCRIPCION";
                    DbDatos.OcultarIds(dgvProductos);
                    dgvProductos.Columns[columna.Name].Width = 170;
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
        private decimal CalcularValorTotal(decimal vCantidad, decimal vUnitario)
        {
            ResultadoValorTotal = vCantidad * vUnitario;

            return ResultadoValorTotal;
        }
        private decimal CalcularCostoPorcentaje()
        {
            foreach (DataGridViewRow fila in dgvManoObra.Rows)
            {
                if (!fila.IsNewRow && fila.Cells["costo"].Value != null)
                {
                    if (fila.Cells["nombre_tipo"].Value != null && fila.Cells["nombre_tipo"].Value.ToString() == "PINTURA")
                    {
                        continue;
                    }
                    decimal costo = Convert.ToDecimal(fila.Cells["costo"].Value);
                    Console.WriteLine("Costo :" + costo);
                    CostoPorcentaje += costo;
                }
            }
            return CostoPorcentaje;
        }
        private decimal CalcularValorTotalPorcentaje()
        {
            ValorTotalPorcentaje = Porcentaje * CostoPorcentaje;
            Console.WriteLine("Porcentaje :" + Porcentaje);
            Console.WriteLine("CostoPorcentaje :" + CostoPorcentaje);
            Console.WriteLine("ValorTotalPorcentaje :" + ValorTotalPorcentaje);
            return ValorTotalPorcentaje;
        }
        private void BuscarPorcentajeEnTabla()
        {
            string valorBuscado = "PORCENTAJE";
            bool encontrado = false;

            foreach (DataGridViewRow fila in dgvManoObra.Rows)
            {
                if (!fila.IsNewRow && fila.Cells[0].Value != null)
                {
                    string nombreTipo = fila.Cells["nombre_tipo"].Value.ToString();

                    if (nombreTipo.Equals(valorBuscado))
                    {
                        encontrado = true;
                        break;
                    }
                }
            }
            if (!encontrado)
            {
                CrearPorcentaje();                   
            }
        }

        private void CrearPorcentaje()
        {
            CalcularCostoPorcentaje();
            CalcularValorTotalPorcentaje();
            GuardarPorcentaje();
        }
    }
}
