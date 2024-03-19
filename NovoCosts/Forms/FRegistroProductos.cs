﻿using NovoCosts.Class;
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
    public partial class FRegistroProductos : Form
    {
        public FRegistroProductos()
        {
            InitializeComponent();
            ToUpperText();
        }
        private void FRegistroProductos_Load(object sender, EventArgs e)
        {
            ListarTodo();
        }

        bool Editar;
        bool Modificar;
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
            if (!ValidarCampos(txtDescripcion, txtReferencia))
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
            if (dgvRegistroProductos.SelectedRows.Count == 0 || dgvRegistroProductos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                return;
            }

            if (dgvRegistroProductos.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvRegistroProductos.CurrentRow.Cells["id_producto"];
                int id_producto = Convert.ToInt32(cell.Value);
                IdProducto = id_producto;
                Eliminar();
                Finalizar();
            }
        }
        private void txtReferencia_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtReferencia.Text;

            BuscarYMostrarResultados("RetornarMateriaPrimaPorReferencia", txtReferencia, listBox1, "@NombreBuscado", "referencia");
        }
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtDescripcion.Text;

            BuscarYMostrarResultados("RetornarMateriaPrimaPorDescripcion", txtDescripcion, listBox1, "@NombreBuscado", "descripcion");
        }
        string opcionSeleccionada;
        private System.Windows.Forms.TextBox ultimoTextBoxModificado = null;
        private void txtReferencia_KeyUp(object sender, KeyEventArgs e)
        {
            ultimoTextBoxModificado = txtReferencia;
        }
        private void txtDescripcion_KeyUp(object sender, KeyEventArgs e)
        {
            ultimoTextBoxModificado = txtDescripcion;
        }
        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBoxBuscar.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione una opción de búsqueda.");
                return;
            }

            string opcionCombo = comboBoxBuscar.SelectedItem.ToString();

            switch (opcionCombo)
            {
                case "REFERENCIA":
                    opcionSeleccionada = "referencia";
                    break;
                case "DESCRIPCION":
                    opcionSeleccionada = "descripcion";
                    break;
            }
            ultimoTextBoxModificado = txtBuscar;
            MostrarResultados("BuscarProducto", txtBuscar, listBox1, opcionSeleccionada, "@ValorBuscado", opcionSeleccionada);
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
        private void MostrarResultados(string nombreProcedimiento, System.Windows.Forms.TextBox textBox, ListBox listBox, string columna, string parametroNombre, string nombreColumna)
        {
            string searchText = textBox.Text;

            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro(parametroNombre, searchText),
                new Parametro("@NombreColumna", columna)  // Asegúrate de agregar este parámetro
            };

            Console.WriteLine($"Valor de {parametroNombre}: {searchText}");
            Console.WriteLine($"Valor de @NombreColumna: {columna}");

            // Llama al método Listar para obtener los resultados de la consulta
            DataTable result = DbDatos.Listar(nombreProcedimiento, parametros);

            // Limpia el ListBox antes de agregar nuevos elementos
            listBox.Items.Clear();

            // Verifica si hay resultados y llena el ListBox
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    // Agrega los elementos al ListBox
                    listBox.Items.Add(row[nombreColumna].ToString());
                }
            }
        }


        //Metodos
        private bool Guardar()
        {
            try
            {
                Producto producto = new Producto()
                {
                    IdProducto = IdProducto,
                    ReferenciaProducto = txtReferencia.Text,
                    DescripcionProducto = txtDescripcion.Text,
                };

                return Producto.Guardar(producto, Editar);
            }
            catch (Exception )
            {
                MessageBox.Show("Error al Guardar!");
                return false;
            }
            
        }
        private bool GuardarEditado()
        {
            if (IdProducto > 0)
            {
                // Obtén la fecha original almacenada
                DateTime fechaOriginal = DateTime.Parse(dgvRegistroProductos.CurrentRow.Cells["fecha"].Value.ToString());

                Producto productoEditado = new Producto()
                {
                    IdProducto = IdProducto,
                    ReferenciaProducto = txtReferencia.Text,
                    DescripcionProducto = txtDescripcion.Text,
                };

                return Producto.Guardar(productoEditado, true);
            }
            else
            {
                MessageBox.Show("Selecciona un producto antes de guardar editado.", "Materia prima no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private bool Eliminar()
        {
            try
            {
                if (IdProducto > 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                        return Producto.Eliminar(IdProducto);
                    return false;
                }
                MessageBox.Show("Selecciona un producto antes de eliminar.", "Producto no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;

            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("No se puede eliminar este producto porque tiene registros relacionados .", "Error de eliminación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            catch (Exception)
            {
                // Mensaje genérico para otras excepciones
                MessageBox.Show($"Se produjo un error al intentar eliminar el producto. Consulta los detalles en la consola.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void ListarTodo()
        {
            dgvRegistroProductos.DataSource = Producto.ListarTodo();
            DbDatos.OcultarIds(dgvRegistroProductos);
            PersonalizarColumnasGrid();
        }
        private void Finalizar()
        {
            ListarTodo();
            Limpiar();
        }
        private void Limpiar()
        {
            txtBuscar.Text = "";
            txtBuscar.Text = "";
            txtDescripcion.Text = "";
            txtDescripcion.Text = "";
            txtReferencia.Text = "";
            txtReferencia.Text = "";
            Editar = false;
        }
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
        {
            txtDescripcion.CharacterCasing = CharacterCasing.Upper;
            txtDescripcion.Click += TextBox_Click;
            txtReferencia.CharacterCasing = CharacterCasing.Upper;
            txtReferencia.Click += TextBox_Click;
            txtBuscar.CharacterCasing = CharacterCasing.Upper;
            txtBuscar.Click += TextBox_Click;

        }
        private void TextBox_Click(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
                textBox.SelectAll();
        }
        private void PersonalizarColumnasGrid()
        {
            foreach (DataGridViewColumn columna in dgvRegistroProductos.Columns)
            {
                if (!string.IsNullOrEmpty(columna.Name))
                {
                    if (columna.Name == "valor_unitario")
                    {
                        dgvRegistroProductos.Columns["valor_unitario"].HeaderText = "Valor Unitario";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        estiloCeldaNumerica.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinea a la derecha
                        estiloCeldaNumerica.Format = "N0";
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvRegistroProductos);
                    }
                    else if (columna.Name == "nombre_materia_prima")
                    {
                        dgvRegistroProductos.Columns["nombre_materia_prima"].HeaderText = "Detalle";
                        DataGridViewCellStyle estiloCeldaNumerica = new DataGridViewCellStyle();
                        columna.DefaultCellStyle = estiloCeldaNumerica;
                        DbDatos.OcultarIds(dgvRegistroProductos);
                    }
                    else if (columna.Name == "fecha")
                    {
                        dgvRegistroProductos.Columns["fecha"].HeaderText = "Fecha";
                        DbDatos.OcultarIds(dgvRegistroProductos);
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
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && ultimoTextBoxModificado != null)
            {
                string selectedText = listBox1.SelectedItem.ToString();

                ultimoTextBoxModificado.Text = selectedText;
            }
            else
            {
                MessageBox.Show("Seleccione un dato valido.");
            }
        }
        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IdProducto = Convert.ToInt32(dgvRegistroProductos.CurrentRow.Cells["id_producto"].Value);
            txtReferencia.Text = dgvRegistroProductos.CurrentRow.Cells["referencia"].Value.ToString();
            txtDescripcion.Text = dgvRegistroProductos.CurrentRow.Cells["descripcion"].Value.ToString();

            Editar = true;
            Modificar = true;
        }

        private void dgvRegistroProductos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvRegistroProductos.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvRegistroProductos.SelectedRows[0];

                    if (selectedRow.Cells.Count >= 5)
                    {
                        IdProducto = Convert.ToInt32(selectedRow.Cells["id_producto"].Value);
                        txtDescripcion.Text = Convert.ToString(selectedRow.Cells["descripcion"].Value);
                        txtReferencia.Text = Convert.ToString(selectedRow.Cells["referencia"].Value);

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

        
    }
}
