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
using rutas;
using System.IO;
using System.Configuration;

namespace presentacion
{
    public partial class frmAgregarArticulo : Form
    {

        private Articulo articulo = null;
        private OpenFileDialog archivo = null;

        public frmAgregarArticulo()
        {
            InitializeComponent();
        }
        public frmAgregarArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            ArticuloRutas rutas = new ArticuloRutas();

            try
            {
                if (articulo == null)
                    articulo = new Articulo();                        

               
                articulo.Codigo = txtCodigoArt.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Fabricante)cboMarca.SelectedItem;
                articulo.Area = (Categoria)cboArea.SelectedItem;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = int.Parse(txtPrecio.Text);


                if (articulo.Id != 0)
                {
                    rutas.modificar(articulo);
                    MessageBox.Show("MODIFICADO Existosamente");

                }
                else
                {
                    rutas.agregar(articulo);
                    MessageBox.Show("AGREGADO Existosamente");
              
                }

                if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Ariculos"] + archivo.SafeFileName);
                }
                
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCategorias_Click_1(object sender, EventArgs e)
        {
            winformCategoria categorias = new winformCategoria();
            categorias.ShowDialog();
        }

        private void frmAgregarArticulo_Load(object sender, EventArgs e)
        {
            CategoriaRutas categoriaRutas = new CategoriaRutas();
            FabricanteRutas fabricantesRutas = new FabricanteRutas();

            try
            {
                cboMarca.DataSource = fabricantesRutas.listar();
                cboMarca.ValueMember = "IdMarca";
                cboMarca.DisplayMember = "Descripcion";
                cboArea.DataSource = categoriaRutas.listar();
                cboArea.ValueMember = "IdCategoria";
                cboArea.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigoArt.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    cboMarca.SelectedValue = articulo.Marca.IdMarca;
                    cboArea.SelectedValue = articulo.Area.IdCategoria;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                }
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

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxAgregarArticulo.Load(imagen);
            }
            catch (Exception)
            {

                pbxAgregarArticulo.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

               
            }
        }
    }
}
