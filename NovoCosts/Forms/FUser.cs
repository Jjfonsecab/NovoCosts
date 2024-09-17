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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NovoCosts.Forms
{
    public partial class FUser : Form
    {
        public FUser()
        {
            InitializeComponent();
        }

        private void FUser_Load(object sender, EventArgs e)
        {

        }

        string usuario;
        string sPass;

        private void btnOk_Click(object sender, EventArgs e)
        {
            usuario= txtUsuario.Text;
            sPass = Encrypt.GetSHA256(txtContraseña.Text.Trim());
            try
            {
                if (string.IsNullOrEmpty(usuario))
                {
                    MessageBox.Show("Nombre de usuario vacío.");
                    return;
                }
                if (string.IsNullOrEmpty(sPass))
                {
                    MessageBox.Show("Contraseña vacía.");
                    return;
                }
                string hash = DbDatos.ObtenerPasswordHash(usuario);
                if (sPass == hash)
                {
                    int userId = DbDatos.ObtenerUserId(usuario);
                    CurrentUser.UserId = userId;
                    CurrentUser.Username = usuario;
                    Console.WriteLine("UserId: " + userId);
                    Console.WriteLine("Username: " + usuario);
                    MessageBox.Show("Inicio de sesión exitoso.");
                    FInicio fInicio = new FInicio();
                    fInicio.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de verificacion!. " + ex);
            }

        }
       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "";
            txtContraseña.Text = "";
        }
    }
}
