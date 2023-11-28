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
using dominio;

namespace presentacion
{
    public partial class winformCategoria : Form
    {
        private List<Categoria> ListaCategoria = new List<Categoria>();
        public winformCategoria()
        {
            InitializeComponent();
        }

        private void winformCategoria_Load(object sender, EventArgs e)
        {
            CategoriaRutas ruta = new CategoriaRutas();
            ListaCategoria = ruta.listar();
            dgvCategorias.DataSource = ListaCategoria;
        }
    }
}
