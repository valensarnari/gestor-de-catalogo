using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace presentacion
{
    public partial class frmInformacion : Form
    {
        // atributos
        private Articulo art;

        // constructores y metodos
        public frmInformacion()
        {
            InitializeComponent();
        }

        public frmInformacion(Articulo art)
        {
            InitializeComponent();
            this.art = art;
        }

        private void frmInformacion_Load(object sender, EventArgs e)
        {
            try
            {
                txtId.Text = art.Id.ToString();
                txtCodigo.Text = art.Codigo;
                txtNombre.Text = art.Nombre;
                txtDescripcion.Text = art.Descripcion;
                txtMarca.Text = art.Marca.Descripcion;
                txtIdMarca.Text = art.Marca.Id.ToString();
                txtCategoria.Text = art.Categoria.Descripcion;
                txtIdCategoria.Text = art.Categoria.Id.ToString();
                txtImagenUrl.Text = art.ImagenUrl;
                txtPrecio.Text = art.Precio.ToString();
                cargarImagen(art.ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ----------------------------------------
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
        // ----------------------------------------
    }
}
