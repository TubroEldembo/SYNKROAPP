using SYNKROAPP.ViewModel;
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

namespace SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen.Vista_ProductosDe1Zona
{
    /// <summary>
    /// Lógica de interacción para PantallaAgregarProductoA1ZonaWPF.xaml
    /// </summary>
    public partial class PantallaAgregarProductoA1ZonaWPF : Window
    {
        private CrearProductoEn1ZonaViewModel viewModelAddProducts;

        public PantallaAgregarProductoA1ZonaWPF(CrearProductoEn1ZonaViewModel viewModelAddProducts)
        {
            InitializeComponent();
            this.viewModelAddProducts = viewModelAddProducts;
            this.DataContext = viewModelAddProducts;
            Loaded += PantallaAgregarProductoA1ZonaWPF_Loaded;
        }

        private async void PantallaAgregarProductoA1ZonaWPF_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModelAddProducts.InicializarCamposAsync();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAgregarCategoria_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnGenerarSKU_Click(object sender, RoutedEventArgs e)
        {
            viewModelAddProducts.GenerarSKU();
        }


        private void btnCancelar_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async void btnGuardarProducto_Click(object sender, RoutedEventArgs e)
        {
            bool productoGuardado = await viewModelAddProducts.GuardarProductoEnZona();
            if (productoGuardado)
            {
                // Puedes cerrar la ventana o mostrar algo adicional
                this.Close();
            }
        }

        private void cmbAlmacen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
