using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class TipoCosto
    {
        public int IdTipoCosto { get; set; }
        public string Nombre { get; set; }
        
        public static bool Guardar(TipoCosto tipocostos, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_tipo_costo", tipocostos.IdTipoCosto),
                new Parametro("@nombre", tipocostos.Nombre),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("TipoCosto_Agregar", parametros);
        }
        public static bool Eliminar(int idMateriaPrima)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_tipo_costo", idMateriaPrima)
            };
            return DbDatos.Ejecutar("TipoCosto_Eliminar", parametros);
        }
        public static DataTable ListarTodo()
        {
            return DbDatos.Listar("TipoCosto_Listar");
        }
        public static DataTable ListarTipoCosto(int tipoCosto)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_tipo_costo", tipoCosto)
            };
            return DbDatos.Listar("Listar_TipoCostoId", parametros);
        }

        public static DataTable ListarTipoPorId(int idTCosto)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_tipo_costo", idTCosto)
            };
            return DbDatos.Listar("ObtenerTipoCostoPorId", parametros);
        }
    }
}
