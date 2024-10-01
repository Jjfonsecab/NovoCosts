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
        private ToolTip toolTip;
        public FInicio()
        {
            InitializeComponent();
            ToolTipInitialicer();
            this.FormClosing += new FormClosingEventHandler(FInicio_FormClosing);
        }
        private void FInicio_Load(object sender, EventArgs e)
        {

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
        private void btnManoObra_Click(object sender, EventArgs e)
        {
            FManoObra fmanoobra = Application.OpenForms.OfType<FManoObra>().FirstOrDefault();

            if (fmanoobra == null)
            {
                fmanoobra = new FManoObra();
                fmanoobra.Show();
            }
            else
                fmanoobra.BringToFront();
        }
        private void btnCostos_Click(object sender, EventArgs e)
        {
            FCostos fcostos = Application.OpenForms.OfType<FCostos>().FirstOrDefault();

            if (fcostos == null)
            {
                fcostos = new FCostos();
                fcostos.Show();
            }
            else
                fcostos.BringToFront();
        }
        private void ToolTipInitialicer()
        {
            toolTip = new ToolTip();

            toolTip.SetToolTip(btnManoObra, "MANO DED OBRA");
            toolTip.SetToolTip(btnProducto, "PRODUCTO");
            toolTip.SetToolTip(btnMateriaPrima, "MATERIA PRIMA");
            toolTip.SetToolTip(btnCostos, "COSTOS");
        }
        private bool isExiting = false;
        private void FInicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isExiting)
            {
                var result = MessageBox.Show("¿Deseas salir de la aplicación?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true; 
                }
                else
                {
                    isExiting = true; 
                    Application.Exit(); 
                }
            }
        }
    }
}
