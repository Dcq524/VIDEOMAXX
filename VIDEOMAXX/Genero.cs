using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videomax
{
    class Genero
    {
        public string Id { get; }

        public string Descripcion { get; }

        public Genero(string id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }
    }
}
