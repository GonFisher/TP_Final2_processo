﻿using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rutas
{
    public class CategoriaRutas
    {
        public List<Categoria> listar()
        {
			List<Categoria> lista = new List<Categoria>();
			AccesoDatos datos = new AccesoDatos();
			try
			{

				datos.setearConsulta("SELECT Id, Descripcion FROM CATEGORIAS");
				datos.ejecutaLectura();

				while (datos.Lector.Read())
				{
					Categoria aux = new Categoria();
					aux.IdCategoria=(int)datos.Lector["Id"];
					aux.Descripcion = (string)datos.Lector["Descripcion"];

					lista.Add(aux);
				}
				return lista;
				
			}
			catch (Exception)
			{

				throw;
			}
			finally
			{
				datos.CerrarConexion();
			}
		}
         
    }
}
