using NovoCosts.Class;
using NovoCosts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NovoCosts.Forms
{
    public partial class FUser : Form
    {
        private System.Windows.Forms.ToolTip toolTip;
        public FUser()
        {
            InitializeComponent();
            ToolTipInitialicer();
            ToUpperText();
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
                    MessageBox.Show("Inicio de sesión exitoso. Hola " + usuario + "!");
                    FInicio fInicio = new FInicio();
                    fInicio.Show();
                    this.Hide();
                }
                else
                {
                    // Si la contraseña no coincide, mostrar un mensaje de error
                    MessageBox.Show("La contraseña es incorrecta.");
                }

            }
            catch (SqlException ex) when (ex.Number == -1) // Error de conexión con la base de datos
            {
                MessageBox.Show("No se pudo conectar con la base de datos. Prueba nuevamente.");
            }
            catch (SqlException ex) when (ex.Number == 18456)
            {
                MessageBox.Show("Acceso no autorizado. La IP desde la que intentas conectarte no está habilitada.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "";
            txtContraseña.Text = "";
        }
        private void ToolTipInitialicer()
        {
            toolTip = new System.Windows.Forms.ToolTip();

            toolTip.SetToolTip(BtnOk, "OK");
            toolTip.SetToolTip(BtnCancel, "CANCELAR");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtContraseña.PasswordChar = '\0'; 
            }
            else
            {
                txtContraseña.PasswordChar = '*';
            }
        }
        private void ToUpperText()
        {
            txtUsuario.CharacterCasing = CharacterCasing.Upper;
        }
    }
}
