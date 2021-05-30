using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;  

namespace Videomax

{
    public enum searchmovie { 
todo = 1,
xgenero,
xformato, 
xintervaloaños
}
    class Catalogo
    {
        private List<Formato> formatos;
        private List<Genero> generos;
        private List<Pelicula> peliculas;
        private List<Inventario> inventarios;

        public Catalogo() 
        {
            
            formatos = EasyFile<Formato>.LoadDataFromFile("formatos.txt", 
                tokens => new Formato(Convert.ToInt32(tokens[0]),
                                                       tokens[1],
                                   Convert.ToDecimal(tokens[2])));

            generos = EasyFile<Genero>.LoadDataFromFile("generos.txt",
           tokens => new Genero(tokens[0], tokens[1]));

            peliculas = EasyFile<Pelicula>.LoadDataFromFile("peliculas.txt",
          tokens => new Pelicula(Convert.ToInt32(tokens[0]),
                                                  tokens[1],
                                                  tokens[2],
                               Convert.ToInt32(tokens[3])));

            inventarios =EasyFile<Inventario>.LoadDataFromFile("inventario.txt",
                             tokens =>new Inventario(Convert.ToInt32(tokens[0]),
                                                    Convert.ToInt32(tokens[1]),
                                                  Convert.ToInt32(tokens[2])));
        }

        public List<PeliculaEnInventario> FindPeliculas(searchmovie opcion,
                                                         string generoId =" ",
                                                         int formatoId=0,
                                                         int añoMinimo=-1,
                                                         int añoMaximo=-1)
        {
            switch (opcion)
            {
                case searchmovie.todo:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario = new List<PeliculaEnInventario>();

                        foreach (Pelicula p in peliculas)   
                        {
                            peliculasEnInventario.Add( new PeliculaEnInventario(p.Id, p.GeneroId, p.Titulo, p.Año,
                             generos.Find(g => g.Id == p.GeneroId).Descripcion,
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad, 
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad, 
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad
                                    )
                                );
                        }

                        return peliculasEnInventario;
                    }
                     case searchmovie.xgenero:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                              new List<PeliculaEnInventario>();

                        foreach (Pelicula p in peliculas.FindAll(p => p.GeneroId == generoId))   //listas a Pelicula inventario
                        {
                            peliculasEnInventario.Add(new PeliculaEnInventario(
                                    p.Id, p.GeneroId, p.Titulo, p.Año,
                                    generos.Find(g => g.Id == p.GeneroId).Descripcion,     
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad, //0 Corresponde a DVD
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad, //1 corresponde a HD
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad)//2 corresponde a FULLHD
                                );
                        }
                        return peliculasEnInventario;
                    }
                    case searchmovie.xformato:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                                 new List<PeliculaEnInventario>();
                        foreach (Inventario i in inventarios.FindAll(i => i.FormatoId == formatoId))
                        {
                            Pelicula pelicula = peliculas.Find(p => p.Id == i.PeliculaId);
                            peliculasEnInventario.Add( new PeliculaEnInventario(
                                  pelicula.Id, pelicula.GeneroId, pelicula.Titulo, pelicula.Año,
                                  generos.Find(g => g.Id == pelicula.GeneroId).Descripcion,     //En la lista de genero encuentra un generp qie contenga el ID 
                                      formatoId == 0 ? i.Cantidad : 0,
                                      formatoId == 1 ? i.Cantidad : 0,
                                      formatoId == 2 ? i.Cantidad : 0)
                              );
                        }
                        return peliculasEnInventario;
                    }
                case searchmovie.xintervaloaños:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario = new List<PeliculaEnInventario>();

                        añoMinimo = añoMinimo != -1 ? añoMinimo : 0;
                        añoMaximo = añoMaximo != -1 ? añoMaximo : 2500;

                        foreach (Pelicula p in peliculas.FindAll(p => p.Año >= añoMinimo && p.Año <= añoMaximo))   
                        {
                            peliculasEnInventario.Add(new PeliculaEnInventario(
                                    p.Id, p.GeneroId, p.Titulo, p.Año,
                                    generos.Find(g => g.Id == p.GeneroId).Descripcion,     
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad, 
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad, 
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad
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

        public List<Formato> GetAllFormatos() => new List<Formato>(formatos);

        public bool VerifyPelicula(int peliculaId)
        {
            return peliculas.Exists(p => p.Id == peliculaId);
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return peliculas.Find(p => p.Id == peliculaId);
        } 
    }
}
