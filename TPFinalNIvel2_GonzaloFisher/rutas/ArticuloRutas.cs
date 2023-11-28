using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using dominio;
using rutas;
using System.Net;


namespace presentacion
{
    public class ArticuloRutas
    {

        public List<Articulo> listar()
        {

            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("SELECT A.Id,Codigo,Nombre, A.Descripcion, M.Descripcion as Marca, C.Descripcion as Area, ImagenUrl, Precio, A.IdMarca,A.IdCategoria FROM ARTICULOS A,MARCAS M,CATEGORIAS C WHERE M.Id = A.IdMarca AND  C.Id = A.IdCategoria AND A.Precio!=0");
                datos.ejecutaLectura();

                while (datos.Lector.Read())
                {
                    Articulo art = new Articulo();

                    art.Id = (int)datos.Lector["Id"];
                    art.Codigo = (string)datos.Lector["Codigo"];
                    art.Nombre = (string)datos.Lector["Nombre"];
                    art.Descripcion = (string)datos.Lector["Descripcion"];
                    art.Marca = new Fabricante();
                    art.Marca.Descripcion = (string)datos.Lector["Marca"];
                    art.Area = new Categoria();
                    art.Area.Descripcion = (string)datos.Lector["Area"];
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl"))))
                    {
                        art.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                    art.Precio = (float)(decimal)datos.Lector["Precio"];
                    art.Marca.IdMarca = (int)datos.Lector["IdMarca"];
                    art.Area.IdCategoria = (int)datos.Lector["IdCategoria"];

                    lista.Add(art);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos info = new AccesoDatos();

            try
            {
                info.setearConsulta("INSERT INTO ARTICULOS(Codigo, Nombre, Descripcion,IdMarca,IdCategoria,ImagenUrl,precio)VALUES(@Codigo,@Nombre,@Descripcion,@Marca,@Area,@ImagenUrl,@Precio )");
                info.setearParametro("@Codigo", nuevo.Codigo);
                info.setearParametro("@Nombre", nuevo.Nombre);
                info.setearParametro("@Descripcion", nuevo.Descripcion);
                info.setearParametro("@Marca", nuevo.Marca.IdMarca);
                info.setearParametro("@Area", nuevo.Area.IdCategoria);
                info.setearParametro("@ImagenUrl", nuevo.ImagenUrl);
                info.setearParametro("@Precio", nuevo.Precio);

                info.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                info.CerrarConexion();
            }
        }

        public void modificar(Articulo modificar)
        {
            AccesoDatos info = new AccesoDatos();

            try
            {
                info.setearConsulta("UPDATE ARTICULOS SET Codigo=@codigo, Nombre=@nombre,Descripcion=@descripcion,IdMarca=@idMarca,IdCategoria=@idCategoria,ImagenUrl=@imagen,Precio =@precio WHERE Id=@id");
                info.setearParametro("@codigo", modificar.Codigo);
                info.setearParametro("@nombre", modificar.Nombre);
                info.setearParametro("@descripcion", modificar.Descripcion);
                info.setearParametro("@idMarca", modificar.Marca.IdMarca);
                info.setearParametro("@idCategoria", modificar.Area.IdCategoria);
                info.setearParametro("@imagen", modificar.ImagenUrl);
                info.setearParametro("@precio", modificar.Precio);
                info.setearParametro("@id", modificar.Id);

                info.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                info.CerrarConexion();
            }

        }

        public void eliminarFisica(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id=@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void eliminarLogica(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("UPDATE ARTICULOS SET Precio=0 WHERE Id=@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string metodo, string combinacion)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT A.Id,Codigo,Nombre, A.Descripcion, M.Descripcion as Marca, C.Descripcion as Area, ImagenUrl, Precio, A.IdMarca,A.IdCategoria FROM ARTICULOS A,MARCAS M,CATEGORIAS C WHERE M.Id = A.IdMarca AND  C.Id = A.IdCategoria AND A.Precio!=0 AND ";
                if (campo == "Precio")
                {
                    switch (metodo)
                    {
                        case "Mayor a":
                            consulta += " Precio > " + combinacion;
                            break;
                        case "Menor a":
                            consulta += " Precio < " + combinacion;
                            break;
                        default:
                            consulta += " Precio = " + combinacion;
                            break;
                    }
                }
                else if (campo == "Area")
                {
                    switch (metodo)
                    {
                        case "Empieza con":
                            consulta += "Area like'"+ combinacion+ "%'";
                            break;
                        case "Termina con":
                            consulta += "Area like'%" + combinacion + "'";
                            break;
                        default:
                            consulta += "Area like'%" + combinacion + "%'";
                            break;
                    }
                }
                else
                {
                    switch (metodo)
                    {
                        case "Empieza con":
                            consulta += "Marca like'" + combinacion + "%'";
                            break;
                        case "Termina con":
                            consulta += "Marca like'%" + combinacion + "'";
                            break;
                        default:
                            consulta += "Marca like'%" + combinacion + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutaLectura();

                while (datos.Lector.Read())
                {
                    Articulo art = new Articulo();

                    art.Id = (int)datos.Lector["Id"];
                    art.Codigo = (string)datos.Lector["Codigo"];
                    art.Nombre = (string)datos.Lector["Nombre"];
                    art.Descripcion = (string)datos.Lector["Descripcion"];
                    art.Marca = new Fabricante();
                    art.Marca.Descripcion = (string)datos.Lector["Marca"];
                    art.Area = new Categoria();
                    art.Area.Descripcion = (string)datos.Lector["Area"];
                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl"))))
                    {
                        art.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                    art.Precio = (float)(decimal)datos.Lector["Precio"];
                    art.Marca.IdMarca = (int)datos.Lector["IdMarca"];
                    art.Area.IdCategoria = (int)datos.Lector["IdCategoria"];

                    lista.Add(art);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }

}
