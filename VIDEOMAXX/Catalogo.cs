using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;  //ESTO ES PARA USAR EL EASY FILE

namespace Videomax

{
    public enum searchmovie { todo = 1, PorGenero, PorFormato, PorIntervaloDeAños, Missing }


    class Catalogo

    {
        
        private List<Formato> formatos;
        private List<Genero> generos;
        private List<Pelicula> peliculas;
        private List<Inventario> inventarios;

        public Catalogo()
        {
         
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

       
        private Formato FormatosCallback(string[] tokens)
        {
            return new Formato(Convert.ToInt32(tokens[0]),
                tokens[1],
                Convert.ToDecimal(tokens[2]));
        }


        public List<PeliculaEnInventario> FindPeliculas(searchmovie opcion,
                                                         string generoId = " ",
                                                         int formatoId = 0,
                                                         int añoMinimo = -1,
                                                         int añoMaximo = -1,
                                                         int cantidad  = 0)
        {
            switch (opcion)
            {
                case searchmovie.todo:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                          new List<PeliculaEnInventario>();

                        foreach (Pelicula p in peliculas)   //listas a Pelicula inventario
                        {
                            peliculasEnInventario.Add(
                                new PeliculaEnInventario(
                                    p.Id, p.GeneroId, p.Titulo, p.Año,
                                    generos.Find(g => g.Id == p.GeneroId).Descripcion,
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad,//0 Corresponde a DVD
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad,//1 corresponde a HD
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad//2 corresponde a FULLHD
                                    )
                                );
                        }
                        return peliculasEnInventario;
                    }

                case searchmovie.PorGenero:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                              new List<PeliculaEnInventario>();

                        foreach (Pelicula p in peliculas.FindAll(p => p.GeneroId == generoId))
                        {
                            peliculasEnInventario.Add(
                                new PeliculaEnInventario(
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

                case searchmovie.PorFormato:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                                 new List<PeliculaEnInventario>();
                        foreach (Inventario i in inventarios.FindAll(i => i.FormatoId == formatoId))
                        {
                            if (i.Cantidad != 0)
                            {
                                Pelicula pelicula = peliculas.Find(p => p.Id == i.PeliculaId);
                                peliculasEnInventario.Add(
                                  new PeliculaEnInventario(
                                      pelicula.Id, pelicula.GeneroId, pelicula.Titulo, pelicula.Año,
                                      generos.Find(g => g.Id == pelicula.GeneroId).Descripcion,
                                          formatoId == 0 ? i.Cantidad : 0,
                                          formatoId == 1 ? i.Cantidad : 0,
                                          formatoId == 2 ? i.Cantidad : 0)
                                  );
                            }
                        }
                        return peliculasEnInventario;
                    }

                case searchmovie.PorIntervaloDeAños:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario =
                              new List<PeliculaEnInventario>();

                        añoMinimo = añoMinimo != -1 ? añoMinimo : 0;
                        añoMaximo = añoMaximo != -1 ? añoMaximo : 3000;

                        foreach (Pelicula p in peliculas.FindAll(p => p.Año >= añoMinimo && p.Año <= añoMaximo))   
                        {
                            peliculasEnInventario.Add(
                                new PeliculaEnInventario(
                                    p.Id, p.GeneroId, p.Titulo, p.Año,
                                    generos.Find(g => g.Id == p.GeneroId).Descripcion,     
                                    inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 0).Cantidad, //DVD
                                     inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 1).Cantidad, //HD
                                      inventarios.Find(i => i.PeliculaId == p.Id && i.FormatoId == 2).Cantidad//FULLHD
                                    )
                                );
                        }
                        return peliculasEnInventario;
                    }

                case searchmovie.Missing:
                    {
                        List<PeliculaEnInventario> peliculasEnInventario = new List<PeliculaEnInventario>();

                        foreach (Inventario i in inventarios.FindAll(i => i.Cantidad == cantidad))
                        {
                            
                            Pelicula pelicula = peliculas.Find(p => i.PeliculaId == p.Id);

                            peliculasEnInventario.Add(new PeliculaEnInventario(
                                  pelicula.Id, pelicula.GeneroId, pelicula.Titulo, pelicula.Año,
                                  generos.Find(g => g.Id == pelicula.GeneroId).Descripcion,
                                       
                                       inventarios.Find(d => d.PeliculaId == pelicula.Id && d.FormatoId == 0).Cantidad,
                                       inventarios.Find(b => b.PeliculaId == pelicula.Id && b.FormatoId == 1).Cantidad,
                                       inventarios.Find(u => u.PeliculaId == pelicula.Id && u.FormatoId == 2).Cantidad)
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

        
        
        public Pelicula GetPelicula(int peliculaId)
        {
            return peliculas.Find(p => p.Id == peliculaId);
        }

        public bool VerifyTitulo(string titulo)
        {

            return peliculas.Exists(p => p.Titulo.Contains(titulo));
        }

        public Pelicula GetTitulo(string titulo)
        {

            Pelicula match = new Pelicula(0, "0", "0", 0);
            
            for(int i = 0; i< peliculas.Count; i++)
            {
                if (peliculas[i].Titulo.Contains(titulo))
                {
                    match = new Pelicula(peliculas[i].Id,
                        peliculas[i].GeneroId,peliculas[i].Titulo,peliculas[i].Año);
                }
            }
            return match ;

        }

       
       
        public void AgregarInventario(Pelicula pelicula, int dvd, int blueray, int uhdBlueray)
        {
            inventarios.Find(p => pelicula.Id == pelicula.Id && p.FormatoId == 0).Cantidad += dvd;
            inventarios.Find(p => pelicula.Id == pelicula.Id && p.FormatoId == 1).Cantidad += blueray;
            inventarios.Find(p => pelicula.Id == pelicula.Id && p.FormatoId == 2).Cantidad += uhdBlueray;

            EasyFile<Inventario>.SaveDataToFile("inventario.txt", new string[] { "PeliculaId", "FormatoId", "Cantidad" }, inventarios);
        }

        public bool ValidarCodigo(int peliculaId) =>
           inventarios.Exists(p => p.PeliculaId == peliculaId);

        public bool ValidarFormato(string peliculaf) =>
            formatos.Exists(f => f.Descripcion == peliculaf);

        public Compras AgregarPelicula(int peliculaId, string peliculaf, int cantidad)
        {
            Compras compras = new Compras(formatos.Find(f => f.Descripcion == peliculaf),
                peliculas.Find(p => p.Id == peliculaId), cantidad);

            return compras;
        }
        public Inventario AgregarPelicula1(int peliculaId, string peliculaf, int cantidad)
        {
            Inventario inv = new Inventario(peliculaId, formatos.Find(f => f.Descripcion == peliculaf).Id, cantidad);
            return inv;
        }
        public bool ValidarCantidad(int peliculaId, string peliculaf, int cantidad)
        {
            bool x = true;

            Formato aux1 = formatos.Find(f => f.Descripcion == peliculaf);
            Inventario aux2 = inventarios.Find(p => p.PeliculaId == peliculaId && p.FormatoId == aux1.Id);

            if (aux2.Cantidad < cantidad) x = false;

            return x;
        }

        public void ActualizarInventario(List<Inventario> listaux)
        {
            var inv = new List<Inventario>();
            for (int i = 0; i < listaux.Count; i++)
            {
                inv.Add(inventarios.Find(p => p.PeliculaId == listaux[i].PeliculaId
                && p.FormatoId == listaux[i].FormatoId));

                inventarios.RemoveAll(p => p.PeliculaId == listaux[i].PeliculaId
                && p.FormatoId == listaux[i].FormatoId);

                inv[i].Cantidad -= listaux[i].Cantidad;

                inventarios.Add(inv[i]);
            }
            EasyFile<Inventario>.SaveDataToFile("inventario.txt",
                new string[] { "PeliculaId", "FormatoId", "Cantidad" },
                inventarios);
        }
    }
}

