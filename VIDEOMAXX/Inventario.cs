using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videomax
{
    class Inventario
    {
        public int PeliculaId { get; }

        public int FormatoId { get; }

        public int Cantidad { get; set; }

        public Inventario(int peliculaId, int formatoId, int cantidad)
        {
            PeliculaId = peliculaId;
            FormatoId = formatoId;
            Cantidad = cantidad;
        }
    }
}

