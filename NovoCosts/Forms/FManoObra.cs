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
    public partial class FManoObra : Form
    {
        public FManoObra()
        {
            InitializeComponent();
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
    }
}
