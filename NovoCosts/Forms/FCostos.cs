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
    public partial class FCostos : Form
    {
        public FCostos()
        {
            InitializeComponent();
            ToUpperText();
        }        
        private void FCostos_Load(object sender, EventArgs e)
        {
            ListarTodo();
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
        Decimal ResultadoDesperdicio;
        Decimal ResultadoTotalCantidad;
        Decimal ResultadoValorTotal;
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
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (!ValidarCamposString(txtReferencia, txtDescripcion, txtDesempeño, txtCantidad, txtCM, txtD1, txtD2, txtD3, comboBoxTC, txtFecha))
                return;
            else if (!ValidarCamposNumericos(txtD1, txtD2, txtD3, txtCantidad))
                return;
            else
            {
                if (txtDesperdicioTotal.Enabled == false)
                {
                    CalcularTotalCantidad(Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtD1.Text), Convert.ToDecimal(txtD2.Text), Convert.ToDecimal(txtD3.Text), Convert.ToDecimal(txtCM.Text));
                    CalcularDesperdicio(ResultadoTotalCantidad, Convert.ToDecimal(txtCantidadDesperdicio.Text));
                    txtDesperdicioTotal.Text = ResultadoDesperdicio.ToString();
                    CalcularValorTotal(Convert.ToDecimal(txtValorU.Text));
                }
                else if (txtDesperdicioTotal.Enabled == false)
                {
                    ResultadoTotalCantidad = Convert.ToDecimal(txtDesperdicioTotal.Text);
                    ResultadoDesperdicio = 0;
                    txtDesperdicioTotal.Text = ResultadoDesperdicio.ToString();
                    CalcularValorTotalManoObra(ResultadoTotalCantidad, Convert.ToDecimal(txtValorU.Text));
                }
            }
            if (Modificar)
            {
                if (!GuardarEditado())
                    return;
                else
                    LimpiarTodo();
            }
            else
                if (!Guardar()) return;

            Finalizar();
        }
        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            DataGridView dgvActual = null;
            
            if (dgvCostos.SelectedRows.Count > 0)
            {
                dgvActual = dgvCostos;
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
            Finalizar();
            
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarTodo();
            Modificar = false;
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdCosto = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_costos"].Value);
            IdProducto = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_producto"].Value);
            IdMateriaPrima = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_materia_prima"].Value);
            txtDesempeño.Text = dgvCostos.CurrentRow.Cells["desempeño"].Value.ToString();
            txtCantidad.Text = dgvCostos.CurrentRow.Cells["cantidad"].Value.ToString();
            txtValorU.Text = dgvCostos.CurrentRow.Cells["valor_unitario"].Value.ToString();
            txtD1.Text = dgvCostos.CurrentRow.Cells["dimension1"].Value.ToString();
            txtD2.Text = dgvCostos.CurrentRow.Cells["dimension2"].Value.ToString();
            txtD3.Text = dgvCostos.CurrentRow.Cells["dimension3"].Value.ToString();
            txtCM.Text = dgvCostos.CurrentRow.Cells["cm"].Value.ToString();
            txtCantidadDesperdicio.Text = dgvCostos.CurrentRow.Cells["cantidad_desperdicio"].Value.ToString();
            txtDesperdicioTotal.Text = dgvCostos.CurrentRow.Cells["total_cantidad"].Value.ToString();
            txtValorU.Text = dgvCostos.CurrentRow.Cells["valor_unitario"].Value.ToString(); 
            IdTipoCosto = Convert.ToInt32(dgvCostos.CurrentRow.Cells["id_tipo_costo"].Value);

            DateTime fechaOriginal = DateTime.Parse(dgvCostos.CurrentRow.Cells["fecha"].Value.ToString());
            txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

            ListarProductoId(IdProducto);
            ListarMaterialId(IdMateriaPrima);
            ListarTipoCostoId(IdTipoCosto);

            Console.WriteLine("El id producto es:" + IdProducto);
            Console.WriteLine("El id material es:" + IdMateriaPrima);
            Console.WriteLine("El id tipo costo es:" + IdTipoCosto);

            Editar = true;

            Modificar = true;            
        }
        private void editarToolStripMenuItem1_Click(object sender, EventArgs e)//Se creo otro metodo copiando el anterior, no encontre como hacerlo mas versatil.
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
            txtValorU.Text = dgvCosto.CurrentRow.Cells["valor_unitario"].Value.ToString();
            IdTipoCosto = Convert.ToInt32(dgvCosto.CurrentRow.Cells["id_tipo_costo"].Value);

            DateTime fechaOriginal = DateTime.Parse(dgvCosto.CurrentRow.Cells["fecha"].Value.ToString());
            txtFecha.Text = fechaOriginal.ToString("yyyy-MM-dd");

            ListarProductoId(IdProducto);
            ListarMaterialId(IdMateriaPrima);
            ListarTipoCostoId(IdTipoCosto);

            Console.WriteLine("El id producto es:" + IdProducto);
            Console.WriteLine("El id material es:" + IdMateriaPrima);
            Console.WriteLine("El id tipo costo es:" + IdTipoCosto);

            Editar = true;
            Modificar = true;
        }
        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            try
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

                    if (selectedRow.Cells.Count >= 4)
                    {
                        IdMateriaPrima = Convert.ToInt32(selectedRow.Cells["id_materia_prima"].Value);
                        txtMaterial.Text = selectedRow.Cells[1].Value.ToString();
                        txtValorU.Text = selectedRow.Cells[4].Value.ToString();
                        txtCM.Text = selectedRow.Cells["medida"].Value.ToString();
                        txtCantidadDesperdicio.Text = selectedRow.Cells[8].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fila vacia");
                return;
            }
            
        }
        private void dgvManoObra_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                txtDesperdicioTotal.Enabled = true;
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
                    Cantidad = Convert.ToInt32(txtCantidad.Text),
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
                DateTime fechaOriginal = DateTime.Parse(dgvCostos.CurrentRow.Cells["fecha"].Value.ToString());

                Costos costos = new Costos()
                {
                    IdCostos = IdCosto,
                    IdProducto = IdProducto,
                    IdMateriaPrima = IdMateriaPrima,
                    Desempeño = txtDesempeño.Text,
                    Cantidad = Convert.ToInt32(txtCantidad.Text),
                    Dimension1 = Convert.ToDecimal(txtD1.Text),
                    Dimension2 = Convert.ToDecimal(txtD2.Text),
                    Dimension3 = Convert.ToDecimal(txtD3.Text),
                    Cm = Convert.ToDecimal(txtCM.Text),
                    CantidadDesperdicio = Convert.ToDecimal(txtCantidadDesperdicio.Text),
                    ValorUnitario = Convert.ToDecimal(txtValorU.Text),
                    IdTipoCosto = IdTipoCosto,

                    Fecha = fechaOriginal,
                };
                Console.WriteLine(IdCosto);
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
            ListarTodo();
            ListarCosto(IdProducto);
            Limpiar();
        }
        private void Limpiar()
        {
            txtDesempeño.Text = "";
            txtCantidad.Text = "";
            txtD1.Text = "";
            txtD2.Text = "";
            txtD3.Text = "";
            comboBoxTC.Text = "";
            txtFecha.Text = "";
            Editar = false;
            
            MostrarFechaActual();
        }
        private void LimpiarTodo()
        {
            txtReferencia.Text = "";
            txtDescripcion.Text = "";
            txtMaterial.Text = "";
            txtDesempeño.Text = "";
            txtCantidad.Text = "";
            txtValorU.Text = "";
            txtD1.Text = "";
            txtD2.Text = "";
            txtD3.Text = "";
            txtCM.Text = "";
            txtCantidadDesperdicio.Text = "";
            txtDesperdicioTotal.Text = "";
            txtValorU.Text = "";
            comboBoxTC.Text = "";

            IdProducto = 0;
            IdMateriaPrima = 0;
            IdTipoCosto = 0;
            MostrarFechaActual();
        }
        private void ListarTodo()
        {
            dgvCostos.DataSource = Models.Costos.ListarTodo();
            DbDatos.OcultarIds(dgvCostos);
            PersonalizarColumnasCostos(dgvCostos);
        }
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
            dgvMateriasPrimas.DataSource = MateriasPrimas.ListarTodo();
            DbDatos.OcultarIds(dgvMateriasPrimas);
            PersonalizarColumnaMateriales();
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
                    if (columna.Name == "dimension1" || columna.Name == "dimension2" || columna.Name == "dimension3" || columna.Name == "cantidad" )
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
                        DbDatos.OcultarIds(dgvCostos);
                    }
                    else if (columna.Name == "descripcion" || columna.Name == "referencia")
                    {
                        dgvActual.Columns[columna.Name].Width = 180;
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
                    else if (columna.Name == "medida" || columna.Name == "valor" || columna.Name == "desperdicio_cantidad" )
                    {
                        dgvMateriasPrimas.Columns["medida"].HeaderText = "MEDIDA";
                        dgvMateriasPrimas.Columns["valor"].HeaderText = "VALOR";
                        dgvMateriasPrimas.Columns["desperdicio_cantidad"].HeaderText = "CANTIDAD DESPERDICIO";
                        //dgvCostos.Columns[columna.Name].Width = 35;
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; 
                        DbDatos.OcultarIds(dgvProductos);
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvMateriasPrimas.Columns["fecha"].HeaderText = "Fecha";
                        dgvMateriasPrimas.Columns[columna.Name].Width = 80;
                        DbDatos.OcultarIds(dgvMateriasPrimas);
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
                    }
                    else if (columna.Name == "fecha")      
                        columna.Visible = false;                        
                    
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
            columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private decimal CalcularTotalCantidad(decimal cantidad, decimal d1, decimal d2, decimal d3, decimal cm)
        {
            int selectedIndex = comboBox1.SelectedIndex;

            if (selectedIndex == 1)
            {
                ResultadoTotalCantidad = (cantidad * d1) / cm;
                Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + " + " + d1 + " / " + cm + " = " + ResultadoTotalCantidad);
                return ResultadoTotalCantidad;
            }
            else if (selectedIndex == 2)
            {
                ResultadoTotalCantidad = (cantidad * d1 * d2) / cm;
                Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + " + " + d1 + " + " + d2 + " / " + cm + " = " + ResultadoTotalCantidad);
                return ResultadoTotalCantidad;
            }
            else if (selectedIndex == 3)
            {
                ResultadoTotalCantidad = (cantidad * d1 * d2 * d3) / cm;
                Console.WriteLine($"ResultadoTotalCantidad :" + cantidad + " + " + d1 + " + " + d2 + " + " + d3 + " / " + cm + " = " + ResultadoTotalCantidad);
                return ResultadoTotalCantidad;
            }
            else
            {
                ResultadoTotalCantidad = 0;
                return ResultadoTotalCantidad;
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
            ResultadoValorTotal = (ResultadoDesperdicio + ResultadoTotalCantidad) * ValorU;
            Console.WriteLine($"ResultadoValorTotal :" + ResultadoDesperdicio + " + " + ResultadoTotalCantidad + " * " + ValorU + " = " + ResultadoValorTotal);

            return ResultadoValorTotal;
        }
        private decimal CalcularValorTotalManoObra(decimal valorU, decimal totalCantidad)
        {
            ResultadoValorTotal = totalCantidad * valorU;
            Console.WriteLine($"ResultadoValorTotal :" + totalCantidad + " * " + valorU + " = " + ResultadoValorTotal);

            return ResultadoValorTotal;
        }
        
    }
}
