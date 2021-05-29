using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videomax
{
    class Pelicula
    {
        public int Id { get; }

        public string GeneroId { get; }

        public string Titulo { get; }

        public int Año { get; }

        public Pelicula(int id, string generoId, string titulo, int año)
        {
            Id = id;
            GeneroId = generoId;
            Titulo = titulo;
            Año = año;
        }
    }
}
