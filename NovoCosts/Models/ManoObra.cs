using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class ManoObra
    {
        public int IdManoObra { get; set; }
        public int IdProducto { get; set; }
        public int IdTipoManoObra { get; set; }
        public decimal Costo { get; set; }
        public DateTime Fecha { get; set; }
        public static bool Guardar(ManoObra manoObra, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_mano_obra", manoObra.IdManoObra),
                new Parametro("@id_producto", manoObra.IdProducto),
                new Parametro("@id_tipo_mano_obra", manoObra.IdTipoManoObra),
                new Parametro("@costo", manoObra.Costo),
                new Parametro("@fecha", manoObra.Fecha),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("ManoObra_Agregar", parametros);
        }
        public static bool Eliminar(int idMateriaPrima)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_mano_obra", idMateriaPrima)
            };
            return DbDatos.Ejecutar("ManoObra_Eliminar", parametros);
        }
        public static DataTable ListarTodo()
        {
            return DbDatos.Listar("ManoObra_Listar");
        }
    }
}
