using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class TipoManoObra
    {
        public int IdTipoManoObra { get; set; }
        public string NombreTipo {  get; set; }
        public static bool Guardar(TipoManoObra tipo, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_tipo_mano_obra", tipo.IdTipoManoObra),
                new Parametro("nombre_tipo", tipo.NombreTipo),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("TipoManoObra_Agregar", parametros);
        }
        public static bool Eliminar(int idManoObra)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_tipo_mano_obra", idManoObra)
            };
            return DbDatos.Ejecutar("TipoManoObra_Eliminar", parametros);
        }
        public static DataTable ListarTodo()
        {
            return DbDatos.Listar("TipoManoObra_Listar");
        }
    }
}
