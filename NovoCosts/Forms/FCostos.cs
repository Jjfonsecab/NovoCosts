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
    public partial class FCostos : Form
    {
        public FCostos()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FCostos_Load(object sender, EventArgs e)
        {
            //ListarTodo();
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
        decimal ResultadoDesperdicio;
        decimal ResultadoTotalCantidad;
        decimal ResultadoValorTotal;

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
        private void btnFCostos_Click(object sender, EventArgs e)
        {
            FFormularioCostos fFormulario = Application.OpenForms.OfType<FFormularioCostos>().FirstOrDefault();
            if (fFormulario == null)
            {
                fFormulario = new FFormularioCostos();
                fFormulario.Show();
            }
            else
                fFormulario.BringToFront();
        }
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCamposString(txtReferencia, txtDescripcion, txtDesempeño, txtCantidad, txtD1, txtD2, txtD3, comboBoxTC, txtFecha))
                    return;
                else if (!ValidarCamposNumericos(txtD1, txtD2, txtD3, txtCantidad))
                    return;

                CalcularTotalCantidad(Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtD1.Text), Convert.ToDecimal(txtD2.Text), Convert.ToDecimal(txtD3.Text), Convert.ToDecimal(txtCM.Text));
                CalcularDesperdicio(ResultadoTotalCantidad, Convert.ToDecimal(txtCantidadDesperdicio.Text));
                txtDesperdicioTotal.Text = ResultadoDesperdicio.ToString();
                CalcularValorTotal(Convert.ToDecimal(txtValorU.Text));


                if (Modificar == true)
                {
                    if (!GuardarEditado())
                        return;
                }
                else
                    if (!Guardar()) return;

                ListarProductos();
                Finalizar();
                MessageBox.Show("Guardado con Exito.!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar.!!");
                Console.WriteLine(ex.Message);
                return;
            }

        }
        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgvActual = null;

                if (dgvCosto.SelectedRows.Count > 0)
                {
                    dgvActual = dgvCosto;
                }
                else if (dgvCosto.SelectedRows.Count > 0)
                {
                    dgvActual = dgvCosto;
                }
                else if (dgvActual == null)
                {
                    MessageBox.Show("Configuracion no disponible");
                    return;
                }
                if (dgvActual == null || dgvActual.SelectedRows.Count == 0 || dgvActual.CurrentRow == null)
                {
                    MessageBox.Show("Selecciona una fila antes de eliminar.");
                    return;
                }
                DataGridViewCell cell = dgvActual.CurrentRow.Cells["id_costos"];
                int id_costo = Convert.ToInt32(cell.Value);
                IdCosto = id_costo;
                Eliminar();
                MessageBox.Show("Eliminado con exito.!");
                Finalizar();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Eliminar.!");
                return;
            }


        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarTodo();
            ListarMaterial();
            Modificar = false;
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IdCosto = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_costos"].Value);
                IdProducto = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_producto"].Value);
                IdMateriaPrima = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_materia_prima"].Value);
                txtDesempeño.Text = dgvCosto.CurrentRow.Cells["desempeño"].Value.ToString();
                txtCantidad.Text = dgvCosto.CurrentRow.Cells["cantidad"].Value.ToString();
                txtValorU.Text = dgvCosto.CurrentRow.Cells["valor_unitario"].Value.ToString();
                txtD1.Text = dgvCosto.CurrentRow.Cells["dimension1"].Value.ToString();
                txtD2.Text = dgvCosto.CurrentRow.Cells["dimension2"].Value.ToString();
                txtD3.Text = dgvCosto.CurrentRow.Cells["dimension3"].Value.ToString();
                txtCM.Text = dgvCosto.CurrentRow.Cells["cm"].Value.ToString();
                txtCantidadDesperdicio.Text = dgvCosto.CurrentRow.Cells["cantidad_desperdicio"].Value.ToString();
                txtDesperdicioTotal.Text = dgvCosto.CurrentRow.Cells["total_cantidad"].Value.ToString();
                IdTipoCosto = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_tipo_costo"].Value);

                DateTime fechaOriginal = DateTime.Parse(dgvCosto.CurrentRow.Cells["fecha"].Value.ToString());
                txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

                ListarProductoId(IdProducto);
                ListarMaterialId(IdMateriaPrima);
                ListarTipoCostoId(IdTipoCosto);
                ListarCosto(IdProducto);

                Editar = true;
                Modificar = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Editar 1.!");
                return;
            }

        }
        private void editarToolStripMenuItem1_Click(object sender, EventArgs e)//Se creo otro metodo copiando el anterior, no encontre como hacerlo mas versatil.
        {
            try
            {
                IdCosto = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_costos"].Value);
                IdProducto = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_producto"].Value);
                IdMateriaPrima = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_materia_prima"].Value);
                txtDesempeño.Text = dgvCosto.CurrentRow.Cells["desempeño"].Value.ToString();
                txtCantidad.Text = dgvCosto.CurrentRow.Cells["cantidad"].Value.ToString();
                txtValorU.Text = dgvCosto.CurrentRow.Cells["valor_unitario"].Value.ToString();
                txtD1.Text = dgvCosto.CurrentRow.Cells["dimension1"].Value.ToString();
                txtD2.Text = dgvCosto.CurrentRow.Cells["dimension2"].Value.ToString();
                txtD3.Text = dgvCosto.CurrentRow.Cells["dimension3"].Value.ToString();
                txtCM.Text = dgvCosto.CurrentRow.Cells["cm"].Value.ToString();
                txtCantidadDesperdicio.Text = dgvCosto.CurrentRow.Cells["cantidad_desperdicio"].Value.ToString();
                txtDesperdicioTotal.Text = dgvCosto.CurrentRow.Cells["total_cantidad"].Value.ToString();
                IdTipoCosto = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_tipo_costo"].Value);

                DateTime fechaOriginal = DateTime.Parse(dgvCosto.CurrentRow.Cells["fecha"].Value.ToString());
                txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

                ListarProductoId(IdProducto);
                ListarMaterialId(IdMateriaPrima);
                ListarTipoCostoId(IdTipoCosto);
                ListarCosto(IdProducto);

                Editar = true;
                Modificar = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Editar 2.!");
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
                        if (idProductoCell != null && idProductoCell.Value != null)
                            IdProducto = Convert.ToInt32(idProductoCell.Value);

                        else
                        {
                            MessageBox.Show("Valor nulo!");
                            return;
                        }

                        DataGridViewCell descripcionCell = selectedRow.Cells["descripcion"];
                        if (descripcionCell != null && descripcionCell.Value != null)
                            txtDescripcion.Text = descripcionCell.Value.ToString();
                        else
                        {
                            MessageBox.Show("Valor nulo!");
                            return;
                        }

                        DataGridViewCell referenciaCell = selectedRow.Cells["referencia"];
                        if (referenciaCell != null && referenciaCell.Value != null)
                            txtReferencia.Text = referenciaCell.Value.ToString();

                        else
                        {
                            MessageBox.Show("Valor nulo!");
                            return;
                        }
                    }
                }
                ListarManoObra(IdProducto);
                ListarCosto(IdProducto);
            }
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }

        }
        private void dgvMateriasPrimas_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                txtDesperdicioTotal.Enabled = false;
                if (dgvMateriasPrimas.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvMateriasPrimas.SelectedRows[0];

                    if (selectedRow != null)
                    {
                        DataGridViewCell idMateriaPrimaCell = selectedRow.Cells["id_materia_prima"];
                        if (idMateriaPrimaCell != null && idMateriaPrimaCell.Value != null)
                            IdMateriaPrima = Convert.ToInt32(idMateriaPrimaCell.Value);
                        DataGridViewCell detalleCell = selectedRow.Cells["detalle_mp"];
                        if (detalleCell != null && detalleCell.Value != null)
                            txtMaterial.Text = detalleCell.Value.ToString();
                        DataGridViewCell cmCell = selectedRow.Cells["medida"];
                        if (cmCell != null && cmCell.Value != null)
                            txtCM.Text = cmCell.Value.ToString();
                        DataGridViewCell valorUcell = selectedRow.Cells["valor"];
                        if (valorUcell != null && valorUcell.Value != null)
                            txtValorU.Text = valorUcell.Value.ToString();
                        DataGridViewCell desperdicioCell = selectedRow.Cells["desperdicio_cantidad"];
                        if (desperdicioCell != null && desperdicioCell.Value != null)
                            txtCantidadDesperdicio.Text = desperdicioCell.Value.ToString();
                        DataGridViewCell fechaCell = selectedRow.Cells["fecha"];
                        if (fechaCell != null && fechaCell.Value != null)
                            txtFecha.Text = fechaCell.Value.ToString();

                        Console.WriteLine("IdMateriaPrima: " + IdMateriaPrima);
                        Console.WriteLine("Material: " + txtMaterial.Text);
                        Console.WriteLine("CM: " + txtCM.Text);
                        Console.WriteLine("Valor Unitario: " + txtValorU.Text);
                        Console.WriteLine("Cantidad de Desperdicio: " + txtCantidadDesperdicio.Text);
                        Console.WriteLine("Fecha: " + txtFecha.Text);

                        if (idMateriaPrimaCell == null || detalleCell == null || idMateriaPrimaCell.Value == null || detalleCell.Value == null || cmCell.Value == null || valorUcell == null || fechaCell == null || desperdicioCell == null)
                        {
                            mensaje();
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
        private void mensaje()
        {
            MessageBox.Show("Valor nulo!");
            return;
        }
        private void dgvManoObra_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
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
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }
        }
        private void comboBoxTC_DropDown(object sender, EventArgs e)
        {
            ListarTipoCosto();
        }
        private void comboBoxTC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)comboBoxTC.SelectedItem;

                if (selectedRow != null)
                {
                    IdTipoCosto = Convert.ToInt32(selectedRow["id_tipo_costo"]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }

        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBuscar.Text;
            BuscarMaterial("BuscarCostosPorDetalleMP", txtBuscar, dgvMateriasPrimas, "@detalleMP");
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
                    Cantidad = Convert.ToDecimal(txtCantidad.Text),
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
                Modificar = false;
                Console.WriteLine($"ResultadoValorTotal :" + ResultadoValorTotal);
                return Models.Costos.Guardar(costos, Editar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Guardar." + ex);
                return false;
            }
        }
        private bool GuardarEditado()
        {

            if (IdCosto > 0)
            {
                Costos costos = new Costos()
                {
                    IdCostos = IdCosto,
                    IdProducto = IdProducto,
                    IdMateriaPrima = IdMateriaPrima,
                    Desempeño = txtDesempeño.Text,
                    Cantidad = Convert.ToDecimal(txtCantidad.Text),
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
                Modificar = true;
                Console.WriteLine($"ResultadoValorTotal :" +  ResultadoValorTotal);

                MessageBox.Show("Editado con Exito.!");
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
            //ListarTodo();
            ListarCosto(IdProducto);
            Limpiar();
        }
        private void Limpiar()
        {
            txtDesempeño.Text = "";
            txtCantidad.Text = "0";
            txtD1.Text = "0";
            txtD2.Text = "0";
            txtD3.Text = "0";
            comboBoxTC.Text = "";
            txtFecha.Text = "";
            Editar = false;
            Modificar = false;


            MostrarFechaActual();
        }
        private void LimpiarTodo()
        {
            txtReferencia.Text = "";
            txtDescripcion.Text = "";
            txtMaterial.Text = "";
            txtDesempeño.Text = "";
            txtCantidad.Text = "0";
            txtValorU.Text = "0";
            txtD1.Text = "0";
            txtD2.Text = "0";
            txtD3.Text = "0";
            txtCM.Text = "0";
            txtCantidadDesperdicio.Text = "0";
            txtDesperdicioTotal.Text = "0";
            txtValorU.Text = "0";
            comboBoxTC.Text = "";

            IdProducto = 0;
            IdMateriaPrima = 0;
            IdTipoCosto = 0;
            MostrarFechaActual();
        }
        /*
        private void ListarTodo()
        {
            dgvCostos.DataSource = Models.Costos.ListarTodo();
            DbDatos.OcultarIds(dgvCostos);
            PersonalizarColumnasCostos(dgvCostos);
        }*/
        private void ListarProductos()
        {
            dgvProductos.DataSource = Producto.ListarTodo();
            DbDatos.OcultarIds(dgvProductos);
            PersonalizarColumnasProductos();
        }
        private void ListarProductoId(int idProducto)
        {
            DataTable dataTable = Producto.ListarProducto(idProducto);

            if (dataTable.Rows.Count > 0)
            {
                txtReferencia.Text = dataTable.Rows[0]["referencia"].ToString();
                txtDescripcion.Text = dataTable.Rows[0]["descripcion"].ToString();
            }
            PersonalizarColumnasProductos();
        }
        private void ListarMaterialId(int idMateriaPrima)
        {
            DataTable dataTable = MateriasPrimas.ListarMateriaPrima(idMateriaPrima);

            if (dataTable.Rows.Count > 0)
            {
                txtMaterial.Text = dataTable.Rows[0]["detalle_mp"].ToString();
            }
            PersonalizarColumnaMateriales();
        }
        private void ListarTipoCostoId(int idTipoCosto)
        {
            DataTable dataTable = TipoCosto.ListarTipoCosto(idTipoCosto);

            if (dataTable.Rows.Count > 0)
            {
                comboBoxTC.Text = dataTable.Rows[0]["nombre"].ToString();
            }
            DbDatos.OcultarIds(dgvCosto);
        }
        private void ListarMaterial()
        {
            dgvMateriasPrimas.DataSource = MateriasPrimas.ListarTodoActualizado();
            DbDatos.OcultarIds(dgvMateriasPrimas);
            PersonalizarColumnaMateriales();
        }
        private void BuscarMaterial(string nombreProcedimiento, System.Windows.Forms.TextBox textBox, DataGridView dgv, string parametroNombre)
        {
            string searchText = textBox.Text;

            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro(parametroNombre, searchText)
            };

            DataTable result = DbDatos.Listar(nombreProcedimiento, parametros);

            dgv.DataSource = result;
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
            PersonalizarColumnasCostos(dgvCosto);

        }
        private void ListarManoObra(int IdProducto)
        {
            dgvManoObra.DataSource = ManoObra.ListarPorProducto(IdProducto);
            DbDatos.OcultarIds(dgvManoObra);
            PersonalizarColumnasManoObra();
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
            txtValorU.Click += TextBox_Click;
            txtCantidad.Click += TextBox_Click;
            txtBuscar.CharacterCasing = CharacterCasing.Upper;
            txtBuscar.Click += TextBox_Click;

            txtFecha.CharacterCasing = CharacterCasing.Upper;
        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }
        private void PersonalizarColumnasCostos(DataGridView dgvActual)
        {
            foreach (DataGridViewColumn columna in dgvActual.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "dimension1" || columna.Name == "dimension2" || columna.Name == "dimension3" || columna.Name == "cantidad")
                    {
                        dgvActual.Columns["dimension1"].HeaderText = "D1";
                        dgvActual.Columns["dimension2"].HeaderText = "D2";
                        dgvActual.Columns["dimension3"].HeaderText = "D3";
                        dgvActual.Columns["cantidad"].HeaderText = "CAN";
                        dgvActual.Columns[columna.Name].Width = 35;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        //estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "total_cantidad" || columna.Name == "valor_unitario")
                    {
                        dgvActual.Columns["total_cantidad"].HeaderText = "TOTAL CANT.";
                        dgvActual.Columns["valor_unitario"].HeaderText = "VALOR UNIT.";
                        dgvActual.Columns[columna.Name].Width = 62;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "cantidad_desperdicio")
                    {
                        columna.Visible = false;
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvActual.Columns["fecha"].HeaderText = "Fecha";
                        dgvActual.Columns[columna.Name].Width = 80;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "desempeño")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "desper")
                    {
                        dgvActual.Columns[columna.Name].Width = 60;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "cm")
                    {
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "valor_total")
                    {
                        dgvActual.Columns["valor_total"].HeaderText = "VALOR TOTAL";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alinea aL CENTRO
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvCosto);
                    }
                    else if (columna.Name == "descripcion" || columna.Name == "referencia")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;
                        DbDatos.OcultarIds(dgvActual);
                    }
                    else if (columna.Name == "MateriaPrima")
                    {
                        dgvActual.Columns["MateriaPrima"].DisplayIndex = 0;
                        dgvActual.Columns["MateriaPrima"].HeaderText = "MATERIA PRIMA";
                        dgvActual.Columns[columna.Name].Width = 220;
                        DbDatos.OcultarIds(dgvActual);
                    }

                }
                ConfigurarCabeceraColumna(columna, columna.HeaderText);
            }
        }
        private void PersonalizarColumnasProductos()
        {
            foreach (DataGridViewColumn columna in dgvProductos.Columns)
            {

                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "descripcion" || columna.Name == "referencia")
                    {
                        dgvProductos.Columns["descripcion"].HeaderText = "DESCRIPCION";
                        dgvProductos.Columns["referencia"].HeaderText = "REFERENCIA";

                        dgvProductos.Columns["descripcion"].Width = 155;
                    }
                    ConfigurarCabeceraColumna(columna, columna.HeaderText);
                }
            }
        }
        private void PersonalizarColumnaMateriales()
        {
            foreach (DataGridViewColumn columna in dgvMateriasPrimas.Columns)
            {

                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "detalle_mp" || columna.Name == "proveedor" || columna.Name == "comentarios")
                    {
                        dgvMateriasPrimas.Columns["detalle_mp"].HeaderText = "DETALLE";
                        dgvMateriasPrimas.Columns["proveedor"].HeaderText = "PROVEEDOR";
                        dgvMateriasPrimas.Columns["comentarios"].HeaderText = "COMENTARIOS";
                        dgvMateriasPrimas.Columns[columna.Name].Width = 250;
                    }
                    else if (columna.Name == "medida" || columna.Name == "valor" || columna.Name == "desperdicio_cantidad")
                    {
                        dgvMateriasPrimas.Columns["medida"].HeaderText = "MEDIDA";
                        dgvMateriasPrimas.Columns["valor"].HeaderText = "VALOR";
                        dgvMateriasPrimas.Columns["desperdicio_cantidad"].HeaderText = "DESPERDICIO";
                        //dgvCostos.Columns[columna.Name].Width = 35;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight;
                        DbDatos.OcultarIds(dgvMateriasPrimas);
                    }
                    else if (columna.Name == "fecha")
                    {
                        columna.Visible = false;
                        //dgvMateriasPrimas.Columns["fecha"].HeaderText = "Fecha";
                        //dgvMateriasPrimas.Columns[columna.Name].Width = 80;
                        //DbDatos.OcultarIds(dgvMateriasPrimas);
                    }
                    ConfigurarCabeceraColumna(columna, columna.HeaderText);
                }
            }
        }
        private void PersonalizarColumnasManoObra()
        {
            foreach (DataGridViewColumn columna in dgvManoObra.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "nombre_tipo")
                    {
                        dgvManoObra.Columns["nombre_tipo"].HeaderText = "NOMBRE";
                        dgvManoObra.Columns["nombre_tipo"].DisplayIndex = 0;
                    }
                    else if (columna.Name == "fecha")
                        columna.Visible = false;
                    else if (columna.Name == "costo" || columna.Name == "total_cantidad" || columna.Name == "valor_total")
                    {
                        dgvManoObra.Columns["total_cantidad"].HeaderText = "TOTAL CANTIDAD";
                        dgvManoObra.Columns["valor_total"].HeaderText = "VALOR TOTAL";
                        columna.Width = 107;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight;
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
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
            columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private decimal CalcularTotalCantidad(decimal cantidad, decimal d1, decimal d2, decimal d3, decimal cm)
        {
            int selectedIndex = comboBox1.SelectedIndex;
            if (Modificar == true)
            {
                ResultadoTotalCantidad = (cantidad * d1 * d2 * d3) / cm;
                return ResultadoTotalCantidad;
            }
            else
            {
                if (selectedIndex == 1)
                {
                    ResultadoTotalCantidad = (cantidad * d1) / cm;
                    Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + "*" + d1 + "/" + cm + ":" + ResultadoTotalCantidad);
                    return ResultadoTotalCantidad;
                }
                else if (selectedIndex == 2)
                {
                    ResultadoTotalCantidad = (cantidad * d1 * d2) / cm;
                    Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + "*" + d1 + "*" + d2 + "/" + cm + ResultadoTotalCantidad);
                    return ResultadoTotalCantidad;
                }
                else if (selectedIndex == 3)
                {
                    ResultadoTotalCantidad = (cantidad * d1 * d2 * d3) / cm;
                    Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + "*" + d1 + "*" + d2 + "*" + d3 + "/" + cm + ":" + ResultadoTotalCantidad);
                    return ResultadoTotalCantidad;
                }
                else
                {
                    ResultadoTotalCantidad = Convert.ToDecimal(txtCantidad.Text);
                    Console.WriteLine($"ResultadoTotalCantidad :" + ResultadoTotalCantidad);
                    return ResultadoTotalCantidad;
                }
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
            if (ResultadoDesperdicio != 0 && ResultadoTotalCantidad != 0)
            {
                ResultadoValorTotal = (ResultadoDesperdicio + ResultadoTotalCantidad) * ValorU;
                ResultadoValorTotal = Math.Round(ResultadoValorTotal, 2);
                Console.WriteLine($"ResultadoValorTotal :" + ResultadoDesperdicio + " + " + ResultadoTotalCantidad + " * " + ValorU + " = " + ResultadoValorTotal);
            }
            else
            {
                ResultadoValorTotal = 0;
            }

            return ResultadoValorTotal;
        }
    }
}
