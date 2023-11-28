using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using rutas;

namespace presentacion
{
    public partial class frmArticulos : Form
    {

        private List<Articulo> ListaArticulo;
        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargar();

            cbxCampo.Items.Add("Marca");
            cbxCampo.Items.Add("Area");
            cbxCampo.Items.Add("Precio");
                    
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {

            if (dgvArticulos.CurrentRow != null) 
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
          

        }

        public void cargar()
        {

            ArticuloRutas ruta = new ArticuloRutas();


            try
            {
                ListaArticulo = ruta.listar();
                dgvArticulos.DataSource = ListaArticulo;
                ocultarColumnas();
                cargarImagen(ListaArticulo[0].ImagenUrl);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        public void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticulo.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }

        public void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarArticulo alta = new frmAgregarArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado = new Articulo();
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAgregarArticulo modificar = new frmAgregarArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar(bool logico = false)
        {
            ArticuloRutas datos = new ArticuloRutas();
            Articulo artseleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿Queres eliminar el Articulo?", "Eliminado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    artseleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    DialogResult respuesta2 = MessageBox.Show("¿Queres hacer una eliminacion Visual?", "Eliminacion Visual o Definitiva", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (respuesta2 == DialogResult.Yes || logico)
                    {
                        datos.eliminarLogica(artseleccionado.Id);
                        cargar();

                    }
                    else if (respuesta2 == DialogResult.No)
                    {
                        DialogResult respuesta3 = MessageBox.Show("VAS ELIMINAR UN REGISTRO DEFINITIVAMENTE¿CONTINUAMOS?", "ULTIMO AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (respuesta3 == DialogResult.Yes)
                        {
                            datos.eliminarFisica(artseleccionado.Id);
                            cargar();
                        }
                        else
                        {
                            return;
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private bool validarFiltro()
        {
            if (cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo");
                return true;
            }
            if(cbxMetodo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el metodo");
                return true;
            }
            if(cbxCampo.SelectedIndex.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro2.Text))
                {
                    MessageBox.Show("Debes cargar un número....");
                    return true;
                }
                if (!(soloNumeros(txtFiltro2.Text)))
                {
                    MessageBox.Show("Solo Numeros Por favor");
                    return true;

                }
            }

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void btnFiltroRapido_Click(object sender, EventArgs e)
        {
            ArticuloRutas rutas = new ArticuloRutas();

            try
            {
                if (validarFiltro())
                    return;

                string campo = cbxCampo.SelectedItem.ToString();
                string metodo = cbxMetodo.SelectedItem.ToString();
                string combinacion = txtFiltro2.Text;

                dgvArticulos.DataSource=rutas.filtrar(campo, metodo, combinacion);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
          


        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltro;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltro = ListaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) ||
                x.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltro = ListaArticulo;

            }


            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltro;
            ocultarColumnas();
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();
            if(opcion == "Marca" || opcion == "Area")
            {
                cbxMetodo.Items.Clear();
                cbxMetodo.Items.Add("Empieza con:");
                cbxMetodo.Items.Add("Termina con:");
                cbxMetodo.Items.Add("Contiene:");
            }
            else
            {
                cbxMetodo.Items.Clear();
                cbxMetodo.Items.Add("Mayor a");
                cbxMetodo.Items.Add("Menor a");
                cbxMetodo.Items.Add("Igual a");

            }

        }

       
    }
}
