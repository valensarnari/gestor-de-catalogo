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
                    aux.Id = (int)acceso.Lector["Id"];
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
                    Decimal num = Math.Truncate(aux.Precio * 100) / 100;
                    string num2 = num.ToString();
                    if (tieneComa(num2) == 0)
                        num2 += ",00";
                    else if (tieneComa(num2) == 1)
                        num2 += "0";
                    aux.Precio = Decimal.Parse(num2);

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
        public void agregar(Articulo art)
        {
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                acceso.setearConsulta("Insert into ARTICULOS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio)");
                acceso.setearParametro("@Codigo", art.Codigo);
                acceso.setearParametro("@Nombre", art.Nombre);
                acceso.setearParametro("@Descripcion", art.Descripcion);
                acceso.setearParametro("@IdMarca", art.Marca.Id);
                acceso.setearParametro("@IdCategoria", art.Categoria.Id);
                acceso.setearParametro("@ImagenUrl", art.ImagenUrl);
                acceso.setearParametro("@Precio", art.Precio);
                acceso.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                acceso.cerrarConexion();
            }
        }
        public void modificar(Articulo art)
        {
            AccesoDatos acceso = new AccesoDatos();

            try
            {
                //acceso.setearConsulta("update ARTICULOS set Codigo = '" + art.Codigo + "', Nombre = '" + art.Nombre + "', Descripcion = '" + art.Descripcion + "', IdMarca = '" + art.Marca.Id.ToString() + "', IdCategoria = '" + art.Categoria.Id.ToString() + "', ImagenUrl = '" + art.ImagenUrl + "', Precio = " + art.Precio.ToString() + " where Id = " + art.Id.ToString());
                acceso.setearConsulta("update ARTICULOS set Codigo = @cod, Nombre = @nom, Descripcion = @desc, IdMarca = @idm, IdCategoria = @idc, ImagenUrl = @img, Precio = @prec where Id = @id");
                acceso.setearParametro("@cod", art.Codigo);
                acceso.setearParametro("@nom", art.Nombre);
                acceso.setearParametro("@desc", art.Descripcion);
                acceso.setearParametro("@idm", art.Marca.Id);
                acceso.setearParametro("@idc", art.Categoria.Id);
                acceso.setearParametro("@img", art.ImagenUrl);
                acceso.setearParametro("@prec", art.Precio);
                acceso.setearParametro("@id", art.Id);

                acceso.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                acceso.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {
            try
            {
                AccesoDatos acceso = new AccesoDatos();
                acceso.setearConsulta("Delete from ARTICULOS where Id = " + id);
                acceso.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            try
            {
                AccesoDatos acceso = new AccesoDatos();
                List<Articulo> lista = new List<Articulo>();

                string consulta = "Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, M.Descripcion as Marca, IdCategoria, C.Descripcion as Categoria, ImagenUrl, Precio from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdMarca = M.Id and A.IdCategoria = C.Id ";

                if(campo == "Precio")
                {
                    switch(criterio)
                    {
                        case "Mayor a":
                            consulta += "and Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "and Precio < " + filtro;
                            break;
                        default:
                            consulta += "and Precio = " + filtro;
                            break;
                    }
                }
                else
                {
                    if(campo == "Código")
                    {
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "and Codigo like '" + filtro + "%'";
                                break;
                            case "Termina con":
                                consulta += "and Codigo like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "and Codigo like '%" + filtro + "%'";
                                break;
                        }
                    }
                    else if(campo == "Nombre")
                    {
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "and Nombre like '" + filtro + "%'";
                                break;
                            case "Termina con":
                                consulta += "and Nombre like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "and Nombre like '%" + filtro + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "and A.Descripcion like '" + filtro + "%'";
                                break;
                            case "Termina con":
                                consulta += "and A.Descripcion like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "and A.Descripcion like '%" + filtro + "%'";
                                break;
                        }
                    }
                }

                acceso.setearConsulta(consulta);
                acceso.ejecutarLectura();

                while (acceso.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)acceso.Lector["Id"];
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
                    Decimal num = Math.Truncate(aux.Precio * 100) / 100;
                    string num2 = num.ToString();
                    if (tieneComa(num2) == 0)
                        num2 += ",00";
                    else if (tieneComa(num2) == 1)
                        num2 += "0";
                    aux.Precio = Decimal.Parse(num2);

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

        //----------------------------------------------
        public int tieneComa(string num)
        {
            int cont = 0;
            int flag = 0;

            foreach(char c in num)
            {
                if (flag == 1)
                    cont++;

                if (c == ',')
                    flag = 1;
            }

            if (cont == 1)
                // retorna 1 si tiene un 0 al final de los decimales
                return 1;

            if (cont == 2)
                // retorna 2 si tiene 2 decimales sin 0
                return 2;

            // retorna 0 si no tiene coma
            return 0;
        }
        //----------------------------------------------
    }
}
