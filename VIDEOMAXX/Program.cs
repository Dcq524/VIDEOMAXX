using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace Videomax
{


    class Program
    {
        static Catalogo catalogo; 

        static void Main(string[] args)
        {
            catalogo = new Catalogo();

            int opcion = 0;
            do
            {
                Clear();
                WriteLine("\t\t\t\t\t--------------------------------------------");
                WriteLine("\t\t\t\t\t||             V I D E O M A X            ||");
                WriteLine("\t\t\t\t\t--------------------------------------------");
                WriteLine("\n\t\t\t\t\tOpciones: ");
                WriteLine("\t\t\t\t\t1. Buscar peliculas");
                WriteLine("\t\t\t\t\t2. Inventario");
                WriteLine("\t\t\t\t\t3. Ventas");
                WriteLine("\t\t\t\t\t0. Salir");

                Write("\n\t\t\t\t\tElige una opción: ");
                opcion = Convert.ToInt32(ReadLine());

                switch (opcion)
                {
                    case 0:
                        WriteLine("\n\t\t\t\t\t¡Gracias por utilizar el programa!");
                        break;

                    case 1:
                        BuscarPeliculas();
                        break;
                    case 2:
                        SubMenuInventario();
                        break;
                    case 3:
                        SubmenuVentas();
                        break;
                    default:
                        WriteLine("\n\t\t\t\t\t¡OPCIÓN NO IMPLEMENTADA!");

                        break;
                }
                WriteLine("\nPresione cualquier tecla para regresar al menú principal");
                WriteLine("Presione 0 para salir del programa");
                ReadKey();
            } while (opcion != 0);

        }

        static void BuscarPeliculas()
        {
            Clear();
            WriteLine("\t\t\t\t\t-------------------------------------");
            WriteLine("\t\t\t\t\t||             VIDEOMAX            ||");
            WriteLine("\t\t\t\t\t-------------------------------------");
            WriteLine("\t\t\t\t\t          BUSCAR PELÍCULAS         ");
            WriteLine("\n\t\t\t\t\tOpciones de filtrado: ");
            WriteLine("\t\t\t\t\t1. Todo");
            WriteLine("\t\t\t\t\t2. Por género");
            WriteLine("\t\t\t\t\t3. Por formato");
            WriteLine("\t\t\t\t\t4. Por intervalo de años");


            Write("\n\t\t\t\t\tElige una opción: ");
            searchmovie opcion = (searchmovie)Convert.ToInt32(ReadLine());

            switch (opcion)
            {
                case searchmovie.todo:

                    Write("\nLas películas en existencia son las siguientes: \n");
                    OpcionMostrarTodo(catalogo.FindPeliculas(searchmovie.todo));
                    break;

                case searchmovie.PorGenero:

                    List<Genero> generos = catalogo.GetAllGeneros();
                    OpcionGenero(generos);

                    Write("\n¿De que género le gustaría?:");
                    int generoIndice = Convert.ToInt32(ReadLine());


                    if (generoIndice > (generos.Count - 1))
                    {
                        WriteLine("\n¡OPCIÓN NO VÁLIDA!");
                    }
                    else
                    {
                        OpcionMostrarTodo(catalogo.FindPeliculas(searchmovie.PorGenero, generos[generoIndice].Id));
                    }

                    break;

                case searchmovie.PorIntervaloDeAños:
                INTERVAL: 
                    int añoMinimo = -1;
                    Write("\nDesea especificar un año minimo [s/n]");
                    if (ReadLine().Trim().ToUpper()[0] == 'S')
                    {
                        Write("\nDigite el año mínimo:\n");
                        añoMinimo = Convert.ToInt32(ReadLine());
                    }

                    int añoMaximo = -1;
                    Write("\nDesea especificar un año máximo [s/n]");
                    if (ReadLine().Trim().ToUpper()[0] == 'S')
                    {
                        Write("\nDigite el año máximo:\n");
                        añoMaximo = Convert.ToInt32(ReadLine());
                    }
                    else { añoMaximo = 3000; }
                    if (añoMinimo > añoMaximo)
                    {
                        Write("\n¡DATOS ERRÓNEOS, INTÉNTELO DE NUEVO!");
                        ReadKey();
                        goto INTERVAL;
                    }
                    
                    OpcionMostrarTodo(catalogo.FindPeliculas(searchmovie.PorIntervaloDeAños, añoMinimo: añoMinimo, añoMaximo: añoMaximo));
                    ReadKey();

                    break;

                case searchmovie.PorFormato:
                    List<Formato> formatos = catalogo.GetAllFormatos();
                    OpcionFormato(formatos);

                    Write("\n¿Qué tipo de formato desea?\n");
                    int formatoIndice = Convert.ToInt32(ReadLine());


                    if (formatoIndice > (formatos.Count - 1))
                    {
                        WriteLine("\n¡OPCIÓN NO VÁLIDA!");
                    }
                    else
                    {
                        OpcionMostrarFormato(catalogo.FindPeliculas(searchmovie.PorFormato, formatoId: formatos[formatoIndice].Id), formatoIndice);
                    }
                        

                    break;

                default:
                    WriteLine("\n¡OPCIÓN NO VÁLIDA!");
                    break;

            }

        }

        static void OpcionMostrarTodo(List<PeliculaEnInventario> peliculas)
        {
            WriteLine("\n**** RESULTADO DE BÚSQUEDA ****");

            if (peliculas.Count > 0)
            {
                Clear();
                WriteLine("\t\t\t\t\t-------------------------------------");
                WriteLine("\t\t\t\t\t||             VIDEOMAX            ||");
                WriteLine("\t\t\t\t\t-------------------------------------");
                WriteLine("\t\t\t\t\t          BUSCAR PELÍCULAS\n         ");
                foreach (var p in peliculas)
                {
                    WriteLine($"{p.Id} - {p.Titulo} - {p.Genero}.({p.Año}).   [DVD:{p.DVD}, BR:{p.BlueRay}, UHDBR:{p.UHDBlueRay}].");
                }

            }
            else
            {
                WriteLine(" ¡No se encontraron resultados!");
            }                    


        }

        static void OpcionMostrarFormato(List<PeliculaEnInventario> peliculas, int formatoIndice)
        {
            WriteLine("\n**** RESULTADO DE BÚSQUEDA ****");

            if (peliculas.Count > 0)
            {
                switch (formatoIndice)
                {
                    case 0:
                        foreach (var p in peliculas)
                        {
                            WriteLine($"{p.Id} - {p.Titulo} - {p.Genero}.({p.Año}).   [DVD Disponibles:{p.DVD}].");
                        }
                        break;
                    case 1:
                        foreach (var p in peliculas)
                        {
                            WriteLine($"{p.Id} - {p.Titulo} - {p.Genero}.({p.Año}).   [BR Disponibles:{p.BlueRay}].");
                        }
                        break;
                    case 2:
                        foreach (var p in peliculas)
                        {
                            WriteLine($"{p.Id} - {p.Titulo} - {p.Genero}.({p.Año}).   [UHDBR Disponibles:{p.UHDBlueRay}].");
                        }
                        break;
                    default:
                        WriteLine("\n¡OPCIÓN NO VÁLIDA!");
                        break;
                }


            }
            else
            {
                WriteLine(" ¡No se encontraron resultados!");
            }

            ReadKey();


        }

        static void OpcionGenero(List<Genero> generos)
        {

            //Clear();
            WriteLine("\n**** GENEROS ****\n");

            for (int i = 0; i < generos.Count; ++i)
            {
                WriteLine($"{i}.{generos[i].Descripcion}");
            }
        }

        static void OpcionFormato(List<Formato> formatos)
        {

            //Clear();
            WriteLine("\n**** FORMATOS ****");

            for (int i = 0; i < formatos.Count; ++i)
            {
                WriteLine($"{i}.{formatos[i].Descripcion}");
            }

        }

        static void SubMenuInventario()
        {
            catalogo = new Catalogo();

            int opcion = 0;
            do
            {
                Clear();
                WriteLine("\t\t\t\t\t-------------------------------------");
                WriteLine("\t\t\t\t\t||             VIDEOMAX            ||");
                WriteLine("\t\t\t\t\t-------------------------------------");
                WriteLine("\t\t\t\t\t             INVENTARIO            ");
                WriteLine("\n\t\t\t\t\tEliga una opción: ");
                WriteLine("\t\t\t\t\t1. Arribo de unidades");
                WriteLine("\t\t\t\t\t2. Reporte de faltantes");
                WriteLine("\t\t\t\t\t0. Salir");

                Write("\nElige una opción: ");
                opcion = Convert.ToInt32(ReadLine());

                switch (opcion)
                {

                    case 0:
                        WriteLine("\n\t\t\t\t\t¡Gracias por utilizar el programa!");
                        break;

                    case 1:

                        WriteLine("Arribo de unidades por medio de (Escriba la letra):");
                        WriteLine(" a) ID de película");
                        WriteLine(" b) Texto o parte del texto");
                        char opcion1 = ReadLine().Trim().ToUpper()[0];

                        if (opcion1 == 'A')
                        {
                            int peliculaID = -1;
                            Write("Escribe el ID (6 dígitos): ");
                            peliculaID = Convert.ToInt32(ReadLine());
                            if (catalogo.ValidarCodigo(peliculaID))
                            {
                                Pelicula pelicula = catalogo.GetPelicula(peliculaID);
                                //Preguntar el formato
                                WriteLine("\nEscoge el formato de la película");
                                WriteLine(" 1. DVD \n 2. BlueRay \n 3. hdBlueray");
                                int EscogerFormato = Convert.ToInt32(ReadLine());

                                switch (EscogerFormato)
                                {
                                    case 1:
                                        WriteLine("Cantidad de artículos: "); //preguntar la cantidad 
                                        int dvd = Convert.ToInt32(ReadLine());
                                        catalogo.AgregarInventario(pelicula, dvd, 0, 0); //Enviar la cantidad 
                                        break;
                                    case 2:
                                        WriteLine("Cantidad de artículos: "); //preguntar la cantidad 
                                        int blueray = Convert.ToInt32(ReadLine());
                                        catalogo.AgregarInventario(pelicula, 0, blueray, 0); //Enviar la cantidad 
                                        break;
                                    case 3:
                                        WriteLine("Cantidad de artículos: "); //preguntar la cantidad 
                                        int uhdBlueray = Convert.ToInt32(ReadLine());
                                        catalogo.AgregarInventario(pelicula, 0, 0, uhdBlueray); //Enviar la cantidad 
                                        break;
                                    default:
                                        WriteLine("***FORMATO INVÁLIDO***");
                                        ReadKey();
                                        break;
                                }



                            }
                            else
                            {
                                WriteLine("\n***ID DE PELICULA INCORRECTO***");
                                ReadKey();
                            }
                        }
                        else if (opcion1 == 'B')
                        {

                            WriteLine("Escriba el Titulo de la pelicula o parte de ella: ");
                            string nombrepelicula = ReadLine();
                            
                            if (catalogo.VerifyTitulo(nombrepelicula))
                            {
                                Pelicula titulo = catalogo.GetTitulo(nombrepelicula);
                                WriteLine($"La película encontrada es {titulo.Titulo}");
                                WriteLine("\nEscoge el formato de la película");
                                WriteLine(" 1. DVD \n 2. BlueRay \n 3. hdBlueray");
                                int EscogerFormato = Convert.ToInt32(ReadLine());
                                switch (EscogerFormato)
                                {
                                    case 1:
                                        WriteLine("Cantidad de artículos: "); //preguntar la cantidad 
                                        int dvd = Convert.ToInt32(ReadLine());
                                        catalogo.AgregarInventario(titulo, dvd, 0, 0); //Enviar la cantidad 
                                        break;
                                    case 2:
                                        WriteLine("Cantidad de artículos: "); //preguntar la cantidad 
                                        int blueray = Convert.ToInt32(ReadLine());
                                        catalogo.AgregarInventario(titulo, 0, blueray, 0); //Enviar la cantidad 
                                        break;
                                    case 3:
                                        WriteLine("Cantidad de artículos: "); //preguntar la cantidad 
                                        int uhdBlueray = Convert.ToInt32(ReadLine());
                                        catalogo.AgregarInventario(titulo, 0, 0, uhdBlueray); //Enviar la cantidad 
                                        break;
                                    default:
                                        WriteLine("***FORMATO INVÁLIDO***");
                                        ReadKey();
                                        break;
                                }
                            }
                            else
                            {
                                WriteLine("\n***PELÍCULA NO ENCONTRADA***");
                                ReadKey();
                            }
                        }
                        else
                        {
                            WriteLine("***OPCIÓN NO IMPLEMENTADA***");
                        }

                        break;


                    case 2:
                        WriteLine("LISTA DE PELÍCULAS FALTANTES");
                        PrintMissing(catalogo.FindPeliculas(searchmovie.Missing));
                        WriteLine("Presione cualquier tecla para regresar");
                        ReadKey();
                        break;
                    default:

                        WriteLine("\n¡OPCIÓN NO IMPLEMENTADA!");
                        ReadKey();
                        break;


                }

            } while (opcion != 0);

        }

        static void PrintMissing(List<PeliculaEnInventario> missing)
        {
            
            foreach (PeliculaEnInventario p in missing)
            {
                WriteLine($"{p.Id} - {p.Titulo} - {p.Genero}.({p.Año}).   [VD:{p.DVD}, BR:{p.BlueRay}, UHDBR:{p.UHDBlueRay}].");
            }
            ReadKey();
        }

        static void SubmenuVentas()
        {
            Clear();
            WriteLine("\t\t\t\t\t-------------------------------------");
            WriteLine("\t\t\t\t\t||             VIDEOMAX            ||");
            WriteLine("\t\t\t\t\t-------------------------------------");
            WriteLine("\t\t\t\t\t               VENTAS                 ");
            int op = -1;
            List<Compras> pelis = new List<Compras>();
            List<Inventario> listAux = new List<Inventario>();
            do
            {
                //Clear();
                WriteLine("\n\t\t\t\t\tLista de compras actual:");
                MostrarListadeCompras(pelis);
                
                int peliculaId = ObtenerPeliId();
                string peliculaf = ObtenerPeliFormato();
                int cantidad = ObtenerCantidad();
                
                if (!catalogo.ValidarCantidad(peliculaId, peliculaf, cantidad))
                {
                    //Clear();
                    MostrarListadeCompras(pelis);
                comodin: WriteLine("La cantidad escogida supera el inventarios");
                    WriteLine("\n\t\t\t\t\t1. Corregir cantidad");
                    WriteLine("\t\t\t\t\t2. Cambiar formato");
                    WriteLine("\t\t\t\t\t0. Cancelar compra");
                    WriteLine("\tElige una opcion: ");
                    op = Convert.ToInt32(ReadLine());

                    switch (op)
                    {
                        case 1:
                            cantidad = ObtenerCantidad();
                            if (!catalogo.ValidarCantidad(peliculaId, peliculaf, cantidad))
                            {
                                goto comodin;
                            }
                            break;
                        case 2:
                            peliculaf = ObtenerPeliFormato();
                            if (!catalogo.ValidarCantidad(peliculaId, peliculaf, cantidad))
                            {
                                goto comodin;
                            }
                            break;
                        case 0:
                            break;
                        default:
                            WriteLine("\nOpción no válida");
                            ReadKey();
                            break;
                    }
                }
                if (op != 0)
                {
                    listAux.Add(catalogo.AgregarPelicula1(peliculaId, peliculaf, cantidad));
                    pelis.Add(catalogo.AgregarPelicula(peliculaId, peliculaf, cantidad));
                    MostrarListadeCompras(pelis);
                    WriteLine();
                    WriteLine("\t\t\t\t\t1. Seguir agragando peliculas a la lista actual");
                    WriteLine("\t\t\t\t\t2. Finalizar compra");
                    WriteLine("\t\t\t\t\t0. Cancelar compra");
                    Write("Elige una opcion: ");
                    op = Convert.ToInt32(ReadLine());

                    switch (op)
                    {
                        case 1:
                            break;
                        case 2:
                            FinalizarCompra(pelis, listAux);
                            op = 0;
                            break;
                        case 0:
                            break;
                        default:
                            WriteLine("\nOpcion no valida");
                            ReadKey();
                            break;
                    }
                }

            } while (op != 0);
        }

        static int ObtenerPeliId()
        {
            WriteLine("\nIngrese el codigo de la pelicula que desea comprar: ");
            int peliId = Convert.ToInt32(ReadLine());            
            while (!catalogo.ValidarCodigo(peliId))
            {
                //Clear();
                WriteLine("\n***Codigo invalido. Por favor ingrese un codigo valido:***");
                peliId = Convert.ToInt32(ReadLine());
            }

            return peliId;
        }

        static string ObtenerPeliFormato()
        {
            WriteLine("\nIngrese el formato de la pelicula que desea comprar: (DVD, Blue-Ray, Ultra HD Blu-Ray)");
            string pelif = ReadLine();

            while (!catalogo.ValidarFormato(pelif))
            {
                //Clear();
                WriteLine("\n***Formato invalido. Por favor ingrese un formato valido:*** ");
                pelif = ReadLine();
            }

            return pelif;
        }

        static int ObtenerCantidad()
        {
            WriteLine("\nIngrese la cantidad de peliculas que desea comprar: ");
            int c = Convert.ToInt32(ReadLine());

            while (c <= 0)
            {
               // Clear();
                WriteLine("\n***Cantidad invalida. Por favor ingrese una cantidad correcta:*** ");
                c = Convert.ToInt32(ReadLine());
            }

            return c;
        }

        static void MostrarListadeCompras(List<Compras> pelis)
        {
           
            if (pelis.Count == 0)
            {
                WriteLine("\n\t\t\t\t\t***Su lista de compras esta vacia***");
            }
            else
            {
                Clear();
                WriteLine("\t\t\t\t\t-------------------------------------");
                WriteLine("\t\t\t\t\t||             VIDEOMAX            ||");
                WriteLine("\t\t\t\t\t-------------------------------------");
                WriteLine("\t\t\t\t\t           LISTA DE COMPRAS          ");
                pelis.ForEach(p => WriteLine($"\n\t\t\t\t\t{p.cantidad} - {p.pelicula.Titulo} - {p.formato.Descripcion}"));
                WriteLine("\t\t\t\t\t-------------------------------------");
            }
        }
        static void FinalizarCompra(List<Compras> pelis, List<Inventario> listAux)
        {
            Clear();
            WriteLine("\t\t\t\t\t-------------------------------------");
            WriteLine("\t\t\t\t\t||             VIDEOMAX            ||");
            WriteLine("\t\t\t\t\t-------------------------------------");
            WriteLine("\t\t\t\t\t                NOTA                 ");
            Write("\n\t\t\t\t\tPeliculas seleccionadas\n\n");
            pelis.ForEach(p => WriteLine($"\t\t\t\t\t{p.pelicula.Titulo} - {p.formato.Descripcion} - {p.cantidad}"));
            

            decimal Total = 0;

            pelis.ForEach(p =>
            Total += Convert.ToDecimal(p.cantidad) * p.formato.Precio);

            WriteLine();
            WriteLine($"\n\t\t\t\t\tEl total de su compra es: {Total}");
            WriteLine("\t\t\t\t\t-------------------------------------");
            Write("\n\t\t\t\t\tPulse cualquier tecla para confirmar: ");
            ReadKey();

            catalogo.ActualizarInventario(listAux);

            Write("\n\t\t\t\t\t ¡Su compra ha sido realizada con exito!");
            ReadKey();
        }

    }
}