using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listar()
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta("Select Id, Descripcion from MARCAS");
                acceso.ejecutarLectura();

                while(acceso.Lector.Read())
                {
                    Marca aux = new Marca();
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
