using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class UnidadesMedida
    {
        public int IdUnidadMedida { get; set; }
        public string Nombre { get; set; }
        public int CantidadParametros { get; set; }

        //Methods
        public static bool Guardar(UnidadesMedida unidades, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_unidad_medida", unidades.IdUnidadMedida),
                new Parametro("@nombre", unidades.Nombre),
                new Parametro("@cantidad_parametros", unidades.CantidadParametros),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("UnidadMedida_Agregar", parametros);
        }
        public static bool Eliminar(int idUnidadMedida)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_unidad_medida", idUnidadMedida)
            };
            return DbDatos.Ejecutar("UnidadMedida_Eliminar", parametros);
        }
        public static DataTable Listar()
        {
            return DbDatos.Listar("UnidadesMedida_Listar");
        }

        internal static DataTable ListarTipoPorId(int idUnidadMedida)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_unidad_medida", idUnidadMedida)
            };
            return DbDatos.Listar("ObtenerUnidadesPorId", parametros);
        }
    }
}
