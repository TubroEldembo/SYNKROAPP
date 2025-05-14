using SYNKROAPP.VIEWMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SYNKROAPP.Vistes.Vista_Mercado.VistaEmpresas.Vista_Catalogo
{
    /// <summary>
    /// Lógica de interacción para PantallaAgregarProductoACatalago.xaml
    /// </summary>
    public partial class PantallaAgregarProductoACatalago : Window
    {
        private AgregarProductoACatalogoVM agregarProductoACatalogo;

        public PantallaAgregarProductoACatalago(AgregarProductoACatalogoVM agregarProductoACatalogo)
        {
            InitializeComponent();
            this.agregarProductoACatalogo = agregarProductoACatalogo;
            DataContext = agregarProductoACatalogo;
            agregarProductoACatalogo.InicializarAsync();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRegresar_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
