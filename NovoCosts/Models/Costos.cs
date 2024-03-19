using NovoCosts.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Models
{
    class Costos
    {
        public int IdCostos { get; set; }
        public int IdProducto { get; set; }
        public int IdMateriaPrima { get; set; }
        public string Desempeño { get; set; }
        public int Cantidad { get; set; }
        public decimal? Dimension1 { get; set; }
        public decimal? Dimension2 { get; set; }
        public decimal? Dimension3 { get; set; }
        public decimal Cm { get; set; }
        public decimal CantidadDesperdicio { get; set; }
        public decimal Desperdicio { get; set; }
        public decimal TotalCantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public int IdTipoCosto { get; set; }
        public DateTime Fecha { get; set; }

        public static bool Guardar(Costos costos, bool editar)
        {
            List<Parametro> parametros = new List<Parametro>()
            {
                new Parametro("@id_costos", costos.IdCostos),
                new Parametro("@id_producto", costos.IdProducto),
                new Parametro("@id_materia_prima", costos.IdMateriaPrima),
                new Parametro("@desempeño", costos.Desempeño),
                new Parametro("@cantidad", costos.Cantidad),
                new Parametro("@dimension1", costos.Dimension1),
                new Parametro("@dimension2", costos.Dimension2),
                new Parametro("@dimension3", costos.Dimension3),
                new Parametro("@cm", costos.Cm),
                new Parametro("@cantidad_desperdicio", costos.CantidadDesperdicio),
                new Parametro("@desperdicio", costos.Desperdicio),
                new Parametro("@total_cantidad", costos.TotalCantidad),
                new Parametro("@valor_unitario", costos.ValorUnitario),
                new Parametro("@valor_total", costos.ValorTotal),
                new Parametro("@id_tipo_costo", costos.IdTipoCosto),
                new Parametro("@fecha", costos.Fecha),
                new Parametro("@Editar", editar)
            };
            return DbDatos.Ejecutar("Costos_Agregar", parametros);
        }
        public static bool Eliminar(int idMateriaPrima)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_costos", idMateriaPrima)
            };
            return DbDatos.Ejecutar("Costos_Eliminar", parametros);
        }
        public static DataTable ListarTodo()
        {
            return DbDatos.Listar("Costos_Listar");
        }
        public static DataTable ListarCostoProducto(int IdProducto)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@id_producto", IdProducto)
            };
            return DbDatos.Listar("Costos_ListarPorIDProducto", parametros);
        }
    }
}
