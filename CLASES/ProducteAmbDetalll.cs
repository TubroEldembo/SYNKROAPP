using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.CLASES
{
    public class ProducteAmbDetall
    {
        public ProducteGeneral Producte { get; set; }
        public string EmpresaID { get; set; }
        public double Preu { get; set; }
        public int Cantidad { get; set; }
        public BitmapImage ImagenProducto { get; set; }
    }

}
