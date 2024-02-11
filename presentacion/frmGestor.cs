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
    public partial class frmGestor : Form
    {
        List<Articulo> listaArticulos;

        public frmGestor()
        {
            InitializeComponent();
        }

        private void frmGestor_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo art = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(art.ImagenUrl);
                lblCampo.Visible = false;
                cboCampo.Visible = false;
                lblCriterio.Visible = false;
                cboCriterio.Visible = false;
                lblFiltro.Visible = false;
                txtFiltro.Visible = false;
                btnFiltrar.Visible = false;
            }
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            ocultarFiltro();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo frm = new frmAltaArticulo();
            frm.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo art = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmAltaArticulo frm = new frmAltaArticulo(art);
                frm.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad querés eliminarlo?", "Eliminando...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // ----------------------------------------
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulos = negocio.listar();
            dgvArticulos.DataSource = listaArticulos;
            ocultarColumnas();
            cargarImagen(listaArticulos[0].ImagenUrl);

        }

        private void ocultarFiltro()
        {
            if (lblCampo.Visible == true)
            {
                lblCampo.Visible = false;
                cboCampo.Visible = false;
                lblCriterio.Visible = false;
                cboCriterio.Visible = false;
                lblFiltro.Visible = false;
                txtFiltro.Visible = false;
                btnFiltrar.Visible = false;
            }
            else
            {
                lblCampo.Visible = true;
                cboCampo.Visible = true;
                lblCriterio.Visible = true;
                cboCriterio.Visible = true;
                lblFiltro.Visible = true;
                txtFiltro.Visible = true;
                btnFiltrar.Visible = true;
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

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
