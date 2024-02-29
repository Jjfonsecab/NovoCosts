using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovoCosts.Class
{
    class Parametro
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }
        public bool Salida { get; set; }

        public Parametro(string nombre, object valor)
        {
            Nombre = nombre;
            Valor = valor;
            Salida = false;
        }

        public Parametro(string nombre)
        {
            Nombre = nombre;
            Salida = true;
        }
    }
}
