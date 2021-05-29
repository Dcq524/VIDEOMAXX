using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videomax
{
    class PeliculaEnInventario
    {
        public int Id { get; }

        public string GeneroId { get; }

        public string Titulo { get; }

        public int Año { get; }

        public string Genero { get; }

        public int DVD { get; }

        public int BlueRay { get; }
        public int UHDBlueRay { get; }

        public PeliculaEnInventario(int id, string generoId, string titulo, int año, string genero, int dVD, int blueRay, int uHDBlueRay)
        {
            Id = id;
            GeneroId = generoId;
            Titulo = titulo;
            Año = año;
            Genero = genero;
            DVD = dVD;
            BlueRay = blueRay;
            UHDBlueRay = uHDBlueRay;
        }
    }
}

