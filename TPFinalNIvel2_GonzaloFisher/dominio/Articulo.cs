using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulo
    {
        public int Id { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public Fabricante Marca { get; set; }

        [DisplayName("Área")]
        public Categoria Area { get; set; }

        public string ImagenUrl { get; set; }

        public float Precio { get; set; }
      
        
       
    }
}
