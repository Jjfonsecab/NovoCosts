﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Configuration.ConfigurationManager;


namespace NovoCosts.Class
{
    class DbDatos
    {
        public static string stringConnection = ConnectionStrings["stringConnection"].ConnectionString;
        static SqlConnection connection = new SqlConnection(stringConnection);//public conecction

        static void AbrirConexion()
        {
            if (connection.State == System.Data.ConnectionState.Closed) connection.Open();
        }
        static void CerrarConexion()
        {
            if (connection.State == System.Data.ConnectionState.Open) connection.Close();
        }
        public static bool Ejecutar(string nombreProcedimiento, List<Parametro> parametros = null)
        {
            try
            {
                AbrirConexion();
                SqlCommand cmd = new SqlCommand(nombreProcedimiento, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        if (!parametro.Salida)
                        {
                            cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                        }
                        else
                        {
                            cmd.Parameters.Add(parametro.Nombre, SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                        }
                    }
                }
                Console.WriteLine("Consulta SQL generada: " + cmd.CommandText);
                int e = cmd.ExecuteNonQuery();

                for (int i = 0; i < parametros.Count; i++)
                {
                    if (cmd.Parameters[i].Direction == ParameterDirection.Output)
                    {
                        string mensaje = cmd.Parameters[i].Value.ToString();
                        if (!string.IsNullOrEmpty(mensaje))
                        {
                            MessageBox.Show(mensaje);
                        }
                    }

                }
                return e > 0 ? true : false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error imprevisto, contacte a su tecnico.!");
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                CerrarConexion();
            }
        }        
        public static DataTable Listar(string nombreProcedimiento, List<Parametro> parametros = null)
        {

            try
            {
                AbrirConexion();
                SqlCommand cmd = new SqlCommand(nombreProcedimiento, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                    }
                }
                DataTable dt = new DataTable();//Data set
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error imprevisto, contacte a su tecnico.!");
                return null;
            }
            finally { CerrarConexion(); }
        }
        public static string ObtenerPasswordHash(string nombreUsuario)
        {
            try
            {
                AbrirConexion();
                SqlCommand cmd = new SqlCommand("ObtenerPasswordHash", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                object resultado = cmd.ExecuteScalar();
                return resultado != null ? resultado.ToString() : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el hash de la contraseña. " + ex.Message);
                throw;
            }
            finally
            {
                CerrarConexion();
            }
        }
        internal static int ObtenerUserId(string usuario)
        {
            try
            {
                AbrirConexion();
                SqlCommand cmd = new SqlCommand("ObtenerUsuarioId", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null && int.TryParse(resultado.ToString(), out int userId))
                {
                    return userId;
                }
                else
                {
                    throw new Exception("No se encontró el ID del usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el hash de la contraseña. " + ex.Message);
                throw;
            }
            finally
            {
                CerrarConexion();
            }
        }
        public static void OcultarIds(DataGridView dataGrid)
        {
            foreach (DataGridViewColumn column in dataGrid.Columns)
            {
                if (column.Name.Substring(0, 2).ToUpper().Equals("ID"))
                {
                    dataGrid.Columns[column.Index].Visible = false;
                }
            }
        }

       
    }
}
