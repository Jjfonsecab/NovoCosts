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
    public partial class FInicio : Form
    {
        public FInicio()
        {
            InitializeComponent();
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            FRegistroProductos fproductos = Application.OpenForms.OfType<FRegistroProductos>().FirstOrDefault();

            if (fproductos == null)
            {
                fproductos = new FRegistroProductos();
                fproductos.Show();
            }
            else
                fproductos.BringToFront();
            
        }

        private void btnMateriaPrima_Click(object sender, EventArgs e)
        {
            FRegistroMateriaPrima fmateriaprima = Application.OpenForms.OfType<FRegistroMateriaPrima>().FirstOrDefault();

            if (fmateriaprima == null)
            {
                fmateriaprima = new FRegistroMateriaPrima();
                fmateriaprima.Show();
            }
            else
                fmateriaprima.BringToFront();
        }
    }
}
