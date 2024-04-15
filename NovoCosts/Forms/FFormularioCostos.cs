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
    public partial class FFormularioCostos : Form
    {
        public FFormularioCostos()
        {
            InitializeComponent();
            ToUpperText();
        }

        private void FFormularioCostos_Load(object sender, EventArgs e)
        {
            ListarCostosProducto();
            ListarManoObraProducto();
            MostrarFechaActual();
        }
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            ListarNombreProductos();
        }

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
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
        }
        private void btnImprimir_Click(object sender, EventArgs e)
        {

        }

        int IdProducto;

        //Metodos:
        private void ToUpperText()//El upperText para los comboBox esta en comboBox_TextChanged
        {
            txtProducto.CharacterCasing = CharacterCasing.Upper;       

            txtFecha.CharacterCasing = CharacterCasing.Upper;
        }
        private void MostrarFechaActual()
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }       

        private void ListarNombreProductos()
        {
            DataTable dataTable = Producto.ListarProductos();

            comboBox1.DataSource = dataTable;
            comboBox1.DisplayMember = "nombre";
            comboBox1.ValueMember = "id_producto";
        }

        private void ListarManoObraProducto()
        {
            //toDo
        }

        private void ListarCostosProducto()
        {
            //toDo
        }


    }
}
