using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;


namespace rutas
{
    public class FabricanteRutas
    {
        public List<Fabricante> listar()
        {
			List<Fabricante> lista = new List<Fabricante>();
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.setearConsulta("SELECT Id, Descripcion FROM MARCAS");
				datos.ejecutaLectura();

				while (datos.Lector.Read())
				{
					Fabricante marca = new Fabricante();
					marca.IdMarca = (int)datos.Lector["Id"];
					marca.Descripcion = (string)datos.Lector["Descripcion"];

					lista.Add(marca);
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
