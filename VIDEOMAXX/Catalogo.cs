using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;  //ESTO ES PARA USAR EL EASY FILE

namespace Videomax

{//Esto estaba en el main, pero lo necesita catalogo para la función findPeliculas
    public enum OpcionBusqueda { Todas = 1, PorGenero, PorFormato, PorIntervaloDeAños }
    //Para el compilador, la enumeracion es un entero, cada dato empieza con una posición que es un número

    class Catalogo
    {



        //LISTAS
        private List<Formato> formatos;
        private List<Genero> generos;
        private List<Pelicula> peliculas;
        private List<Inventario> inventarios;

        public Catalogo() //Constructor
        {
            //Lo que hace el easyfil es 
            /* formatos =
                 EasyFile<Formato>.LoadDataFromFile("formatos.txt", 
                       tokens =>                          //formo una lista y devueve una lista
                              new Formato(Convert.ToInt32(tokens[0]), //ASD (esto es una función lamba)
                 tokens[1],
                 Convert.ToDecimal(tokens[2])));           //segundo parametro-recibe un string y devuleve un formato
                 */
            formatos =
            EasyFile<Formato>.LoadDataFromFile("formatos.txt", FormatosCallback);

            generos =
           EasyFile<Genero>.LoadDataFromFile("generos.txt",
           tokens =>
           new Genero(tokens[0], tokens[1]));

            peliculas =
          EasyFile<Pelicula>.LoadDataFromFile("peliculas.txt",
          tokens =>
          new Pelicula(Convert.ToInt32(tokens[0]),
                                       tokens[1],
                                       tokens[2],
                                       Convert.ToInt32(tokens[3])));

            inventarios =
                EasyFile<Inventario>.LoadDataFromFile("inventario.txt",
                tokens =>
                new Inventario(Convert.ToInt32(tokens[0]),
                Convert.ToInt32(tokens[1]),
                Convert.ToInt32(tokens[2])));
        }

        //ESTO ES LO MISMO QUE ASD
        private Formato FormatosCallback(string[] tokens) //Es una función que necesita el easy fil para que convierta la linea de texto
                                                          //En un formato. Callback es una funcion que se llama por un tercero
        {
            return new Formato(Convert.ToInt32(tokens[0]),
                tokens[1],
                Convert.ToDecimal(tokens[2]));
            //El tokens es un arreglo, vemos que en el archivo "formato", existen tres parametros, los cuales necesitamos convertir
            //a los tipos de datos que necesitan, por eso el convert, 
        }


        public List<PeliculaEnInventario> FindPeliculas(OpcionBusqueda opcion,
                                                         string generoId = " ",
                                                         int formatoId = 0,
                                                         int añoMinimo = -1,
                                                         int añoMaximo = -1)
        {
            switch (opcion)
            {

                case OpcionBusqueda.PorIntervaloDeAños:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                              new List<PeliculaEnInventario>();

                        añoMinimo = añoMinimo != -1 ? añoMinimo : 0;
                        añoMaximo = añoMaximo != -1 ? añoMaximo : 3000;

                        foreach (Pelicula p in peliculas.FindAll(p => p.Año >= añoMinimo && p.Año <= añoMaximo))   //listas a Pelicula inventario
                        {
                            peliculasEnInventario.Add(
                                new PeliculaEnInventario(
                                    p.Id, p.GeneroId, p.Titulo, p.Año,
                                    generos.Find(g => g.Id == p.GeneroId).Descripcion,     //En la lista de genero encuentra un generp qie contenga el ID 
                                                                                           //Voy a buscar el genero con el idgenero, tengo que mandarle una función que devuelva
                                                                                           //Un booleano para que me diga cuando coincide el genero con el idgenero
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad, //0 Corresponde a DVD
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad, //1 corresponde a HD
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad//2 corresponde a FULLHD
                                    )
                                );
                        }
                        return peliculasEnInventario;
                    }

                case OpcionBusqueda.PorFormato:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                                 new List<PeliculaEnInventario>();
                        foreach (Inventario i in inventarios.FindAll(i => i.FormatoId == formatoId))
                        {
                            Pelicula pelicula = peliculas.Find(p => p.Id == i.PeliculaId);
                            peliculasEnInventario.Add(
                              new PeliculaEnInventario(
                                  pelicula.Id, pelicula.GeneroId, pelicula.Titulo, pelicula.Año,
                                  generos.Find(g => g.Id == pelicula.GeneroId).Descripcion,     //En la lista de genero encuentra un generp qie contenga el ID 
                                      formatoId == 0 ? i.Cantidad : 0,
                                      formatoId == 1 ? i.Cantidad : 0,
                                      formatoId == 2 ? i.Cantidad : 0)


                              );
                        }


                        return peliculasEnInventario;
                    }

                case OpcionBusqueda.PorGenero:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                              new List<PeliculaEnInventario>();

                        foreach (Pelicula p in peliculas.FindAll(p => p.GeneroId == generoId))   //listas a Pelicula inventario
                        {
                            peliculasEnInventario.Add(
                                new PeliculaEnInventario(
                                    p.Id, p.GeneroId, p.Titulo, p.Año,
                                    generos.Find(g => g.Id == p.GeneroId).Descripcion,     //En la lista de genero encuentra un generp qie contenga el ID 
                                                                                           //Voy a buscar el genero con el idgenero, tengo que mandarle una función que devuelva
                                                                                           //Un booleano para que me diga cuando coincide el genero con el idgenero
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad, //0 Corresponde a DVD
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad, //1 corresponde a HD
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad//2 corresponde a FULLHD
                                    )
                                );
                        }
                        return peliculasEnInventario;
                    }

                case OpcionBusqueda.Todas:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                          new List<PeliculaEnInventario>();

                        foreach (Pelicula p in peliculas)   //listas a Pelicula inventario
                        {
                            peliculasEnInventario.Add(
                                new PeliculaEnInventario(
                                    p.Id, p.GeneroId, p.Titulo, p.Año,
                                    generos.Find(g => g.Id == p.GeneroId).Descripcion, //En la lista de genero encuentra un generp qie contenga el ID 
                                                                                       //Voy a buscar el genero con el idgenero, tengo que mandarle una función que devuelva
                                                                                       //Un booleano para que me diga cuando coincide el genero con el idgenero
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad, //0 Corresponde a DVD
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad, //1 corresponde a HD
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad//2 corresponde a FULLHD
                                    )
                                );
                        }

                        return peliculasEnInventario;
                    }
                default:
                    throw new InvalidOperationException("Opcion invalida");

            }



        }
        public List<Genero> GetAllGeneros() => new List<Genero>(generos);

        //Copia de la lista
        public List<Formato> GetAllFormatos() => new List<Formato>(formatos);

        public bool VerifyPelicula(int peliculaId)
        {
            return peliculas.Exists(p => p.Id == peliculaId);
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return peliculas.Find(p => p.Id == peliculaId);
        }

        public void AgregarInventario(Pelicula pelicula, int dvd, int blueray, int uhdBlueray)
        {
            inventarios.Find(p => pelicula.Id == pelicula.Id && p.FormatoId == 0).Cantidad += dvd;

            inventarios.Find(p => pelicula.Id == pelicula.Id && p.FormatoId == 1).Cantidad += blueray;
            inventarios.Find(p => pelicula.Id == pelicula.Id && p.FormatoId == 2).Cantidad += uhdBlueray;

            EasyFile<Inventario>.SaveDataToFile("inventario.txt", new string[] { "PeliculaId", "FormatoId", "Cantidad" }, inventarios);
        }

    }
}
//funcion lamda es una manera de escribir una función lo más corta posible
