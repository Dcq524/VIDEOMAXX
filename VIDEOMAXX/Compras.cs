using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videomax
{
    class Compras
    {
        public Formato formato { get; }
        public Pelicula pelicula { get; }
        public int cantidad { get; }

        public Compras(Formato formato, Pelicula pelicula, int cantidad)
        {
            this.formato = formato;
            this.pelicula = pelicula;
            this.cantidad = cantidad;
        }
    }
}
