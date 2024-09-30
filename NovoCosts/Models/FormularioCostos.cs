using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class FormularioCostos
    {
        public int IdFormularioCostos { get; set; }
        public int IdProducto { get; set; }
        public decimal PorcentajeGanancia { get; set; }
        public decimal Utilidad { get; set; }
        public decimal PrecioFabrica { get; set; }
        public String Anotaciones { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUser { get; set; }

        public static bool Guardar(FormularioCostos fcostos, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_formulario_costos", fcostos.IdFormularioCostos),
                new Parametro("@id_producto", fcostos.IdProducto),
                new Parametro("@porcentaje_ganancia", fcostos.PorcentajeGanancia),
                new Parametro("@utilidad", fcostos.Utilidad),
                new Parametro("@precio_fabrica", fcostos.PrecioFabrica),
                new Parametro("@anotaciones", fcostos.Anotaciones),
                new Parametro("@fecha", fcostos.Fecha),
                new Parametro("@id_usuario", fcostos.IdUser),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("Formulario_Costos_Agregar", parametros);
        }
        public static bool Eliminar(int idFCosto)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_formulario_costos", idFCosto)
            };
            return DbDatos.Ejecutar("Formulario_Costos_Eliminar", parametros);
        }
        public static DataTable ListarTodo()
        {
            return DbDatos.Listar("Formulario_Costos_Listar");
        }
        public static DataTable ListarCostoProducto(int IdFormularioCosto)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_formulario_costo", IdFormularioCosto)
            };
            return DbDatos.Listar("Formulario_Costos_ListarIDFormulario_Costo", parametros);
        }

        
    }
}
