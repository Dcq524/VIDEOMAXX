using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videomax
{

    class Formato
    {
        public int Id { get; } //Propiedad, Solo lectura

        public string Descripcion { get; } //Propiedad, Solo lectura

        public decimal Precio { get; } //Propiedad,Solo lectura

        public Formato(int id, string descripcion, decimal precio) //Constructor
        {
            Id = id;
            Descripcion = descripcion;
            Precio = precio;
        }
    }
}
