using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta("Select Id, Descripcion from CATEGORIAS");
                acceso.ejecutarLectura();

                while (acceso.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.Id = (int)acceso.Lector["Id"];
                    aux.Descripcion = (string)acceso.Lector["Descripcion"];

                    lista.Add(aux);
                }

                acceso.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}