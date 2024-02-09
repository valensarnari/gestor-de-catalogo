using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        // constructor y metodos
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos acceso = new AccesoDatos();
            
            try
            {
                acceso.setearConsulta("Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, M.Descripcion as Marca, IdCategoria, C.Descripcion as Categoria, ImagenUrl, Precio from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdMarca = M.Id and A.IdCategoria = C.Id");
                acceso.ejecutarLectura();

                while(acceso.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)acceso.Lector["Codigo"];
                    aux.Nombre = (string)acceso.Lector["Nombre"];
                    aux.Descripcion = (string)acceso.Lector["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)acceso.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)acceso.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)acceso.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)acceso.Lector["Categoria"];
                    if (!(acceso.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)acceso.Lector["ImagenUrl"];
                    aux.Precio = (Decimal)acceso.Lector["Precio"];

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
