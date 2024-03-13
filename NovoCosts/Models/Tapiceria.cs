using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class Tapiceria
    {
        public int IdTapiceria { get; set; }
        public int IdProducto { get; set; }
        public decimal CorteAlistado { get; set; }
        public decimal Blanco { get; set; }
        public decimal Costura { get; set; }
        public decimal Forrado { get; set; }
        public DateTime Fecha { get; set; }
        public static bool Guardar(Tapiceria tapiceria, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_costo_tapiceria", tapiceria.IdTapiceria),
                new Parametro("@id_producto", tapiceria.IdProducto),
                new Parametro("@corte_alistado", tapiceria.CorteAlistado),
                new Parametro("@blanco", tapiceria.Blanco),
                new Parametro("@costura", tapiceria.Costura),
                new Parametro("@forrado", tapiceria.Forrado),
                new Parametro("@fecha", tapiceria.Fecha),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("Tapiceria_Agregar", parametros);
        }
        public static bool Eliminar(int idCostoTapiceria)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_costo_tapiceria", idCostoTapiceria)
            };
            return DbDatos.Ejecutar("Tapiceria_Eliminar", parametros);
        }
        public static DataTable ListarTodo()//Aca esta un inner
        {
            return DbDatos.Listar("Tapiceria_Listar");
        }
        public static DataTable ListarPorProducto(int IdProducto)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_producto", IdProducto)
            };
            return DbDatos.Listar("ObtenerCostoProductoTapiceria",parametros);
        }
    }
}
