using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class MateriasPrimas
    {
        public int IdMateriaPrima { get; set; }
        public string DetalleMateriaPrima { get; set; }
        public int IdUnidadMedida { get; set; }
        public decimal Medida { get; set; }
        public decimal Valor { get; set; }
        public string Proveedor { get; set; }
        public decimal Desperdicio {  get; set; }
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }


        public static bool Guardar(MateriasPrimas materiasPrimas, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_materia_prima", materiasPrimas.IdMateriaPrima),
                new Parametro("@detalle_mp", materiasPrimas.DetalleMateriaPrima),
                new Parametro("@id_unidad_medida", materiasPrimas.IdUnidadMedida),
                new Parametro("@medida", materiasPrimas.Medida),
                new Parametro("@valor", materiasPrimas.Valor),
                new Parametro("@proveedor", materiasPrimas.Proveedor),
                new Parametro("@desperdicio_cantidad", materiasPrimas.Desperdicio),
                new Parametro("@fecha", materiasPrimas.Fecha),
                new Parametro("@Comentarios", materiasPrimas.Comentarios),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("MateriaPrima_Agregar", parametros);
        }
        public static bool Eliminar(int idMateriaPrima)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_materia_prima", idMateriaPrima)
            };
            return DbDatos.Ejecutar("MateriaPrima_Eliminar", parametros);
        }
        public static DataTable ListarMateriaPrima(int idMateriaPrima)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_material", idMateriaPrima)
            };
            return DbDatos.Listar("Listar_MaterialId", parametros);
        }
        public static DataTable ListarTodo()
        {
            return DbDatos.Listar("MateriaPrima_Listar");
        }
        

    }
}
