using negocio;
using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo art = null;

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo art)
        {
            InitializeComponent();
            this.art = art;
            Text = "Modificar artículo";
            btnAgregar.Text = "Modificar";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (art != null)
                {
                    txtCodigo.Text = art.Codigo;
                    txtNombre.Text = art.Nombre;
                    txtDescripcion.Text = art.Descripcion;
                    cboMarca.SelectedValue = art.Marca.Id;
                    cboCategoria.SelectedValue = art.Categoria.Id;
                    txtImagenUrl.Text = art.ImagenUrl;
                    cargarImagen(txtImagenUrl.Text);
                    txtPrecio.Text = art.Precio.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();

            try
            {
                if (art == null)
                    art = new Articulo();
                    
                art.Codigo = txtCodigo.Text;
                art.Nombre = txtNombre.Text;
                art.Descripcion = txtDescripcion.Text;
                art.Marca = (Marca)cboMarca.SelectedItem;
                art.Categoria = (Categoria)cboCategoria.SelectedItem;
                art.ImagenUrl = txtImagenUrl.Text;

                if(convertirNumero(txtPrecio.Text) == "")
                {
                    MessageBox.Show("Por favor, verifique que todos los campos estén completos.");
                    return;
                }
                else if(convertirNumero(txtPrecio.Text) == null)
                {
                    MessageBox.Show("Por favor, verifique que el campo Precio tenga el formato correcto.");
                    return;
                }
                else
                {
                    if(estaVacio(art.Codigo, art.Nombre, art.Descripcion, art.ImagenUrl) == false)
                    {
                        MessageBox.Show("Por favor, verifique que todos los campos estén completos.");
                        return;
                    }

                    art.Precio = Decimal.Parse(txtPrecio.Text);

                    if (art.Id != 0)
                    {
                        articuloNegocio.modificar(art);
                        MessageBox.Show("Artículo modificado exitosamente");
                    }
                    else
                    {
                        articuloNegocio.agregar(art);
                        MessageBox.Show("Artículo agregado exitosamente");
                    }
                }
                
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

        // ------------------------------------
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {
                pbxImagen.Load("https://www.kurin.com/wp-content/uploads/placeholder-square.jpg");
            }
        }
        private string convertirNumero(string texto)
        {
            int cont = 0;
            string cadena = null;

            // si la cadena esta vacia devolverla para reiniciar dgv
            if (texto == "")
                return texto;

            // chequear si la cadena tiene algo que no sea un numero o ,
            foreach (char caracter in texto)
            {
                if (!(char.IsNumber(caracter)) && !(caracter == ','))
                    return null;
            }

            // si la cadena no tiene comas, le agrego el ,00
            if (texto.Contains(',') == false)
                texto += ",00";

            // si la cadena tiene una coma, lo reemplaza por un punto para la consulta
            foreach (char caracter in texto)
            {
                if (caracter == ',')
                    cont++;

                if (cont < 1)
                    cadena = texto.Replace(',', '.');
            }

            // verifico si la cadena tiene mas de dos comas
            if (cont > 1)
                return null;

            return cadena;
        }
        private bool estaVacio(string cod, string nom, string desc, string url)
        {
            if (cod == "" || nom == "" || desc == "" || url == "")
                return false;
            return true;
        }
        // ------------------------------------
    }
}
