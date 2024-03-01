using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class Producto
    {
        public int IdProducto;
        public string ReferenciaProducto;
        public string DescripcionProducto;

        public static bool Guardar(Producto producto, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_producto", producto.IdProducto),
                new Parametro("@referencia", producto.ReferenciaProducto),
                new Parametro("@descripcion", producto.DescripcionProducto),
                
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("Producto_Agregar", parametros);
        }
        public static bool Eliminar(int idMateriaPrima)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_producto", idMateriaPrima)
            };
            return DbDatos.Ejecutar("Producto_Eliminar", parametros);
        }
        public static DataTable ListarTodo()
        {
            return DbDatos.Listar("Producto_Listar");
        }
    }
}
