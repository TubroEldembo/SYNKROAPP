using SYNKROAPP.ViewModel;
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

namespace SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen.Vista_ProductosDe1Zona
{
    /// <summary>
    /// Lógica de interacción para PantallaCrearProductoGeneral.xaml
    /// </summary>
    public partial class PantallaCrearProductoGeneral : Window
    {
        private CrearProductoEn1ZonaViewModel viewModelCrearProducto;
       

        public PantallaCrearProductoGeneral(CrearProductoEn1ZonaViewModel viewModelCrearProducto)
        {
            InitializeComponent();
            this.viewModelCrearProducto = viewModelCrearProducto;
            this.DataContext = viewModelCrearProducto;
            Loaded += PantallaCrearProductoGeneral_Loaded; ;
        }

        private async void PantallaCrearProductoGeneral_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModelCrearProducto.InicializarCamposAsync();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
