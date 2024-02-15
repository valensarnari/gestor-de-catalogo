using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            cboCampo.Items.Clear();
            cboCampo.Items.Add("Código");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
            cboCampo.Items.Add("Precio");
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
                lblFormato.Visible = false;
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
                MessageBox.Show("Por favor, seleccione un Articulo para modificar.");
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
                MessageBox.Show("Por favor, seleccione un Articulo para eliminar.");
            }
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                Articulo select = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmInformacion frmInformacion = new frmInformacion(select);
                frmInformacion.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Por favor, seleccione un Articulo para ver.");
            }
        }

        private void cboCampo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboCampo.Text == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                string campo = "";
                string criterio = "";
                string filtro = "";

                // verifico si hay algun campo vacio
                if (cboCampo.SelectedItem != null)
                {
                    campo = cboCampo.SelectedItem.ToString();
                    
                    if (cboCriterio.SelectedItem != null)
                    {
                        criterio = cboCriterio.SelectedItem.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Verifique que Criterio no esté vacío.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Verifique que Campo y criterio no estén vacíos.");
                    return;
                }

                
                filtro = txtFiltro.Text;

                // verifico que si esta seleccionado Precio, el formato sea el correcto
                if (campo == "Precio")
                {
                    if (convertirNumero(filtro) == null)
                    {
                        MessageBox.Show("Por favor, ingrese un formato válido si selecciona Precio como campo.");
                        return;
                    }

                    filtro = convertirNumero(filtro);
                }

                // agrego que reinicie la lista si el filtro esta vacio y el campo es Precio
                if(filtro == "" && campo == "Precio")
                {
                    dgvArticulos.DataSource = negocio.listar();
                    ocultarFiltro();
                }
                else
                {
                    dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);

                    cargarImagen(listaArticulos[0].ImagenUrl);
                    ocultarColumnas();
                    ocultarFiltro();
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
                lblFormato.Visible = false;
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
                lblFormato.Visible = true;
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

            // verifico si la cadena tiene mas de dos numeros despues del punto
            int flag = 0;
            cont = 0;
            foreach (char caracter in cadena)
            {
                if(flag == 1)
                    cont++;

                if(caracter == '.')
                    flag = 1;
                    
                if (cont > 2)
                    return null;
            }

            return cadena;
        }
        // ----------------------------------------

    }
}
