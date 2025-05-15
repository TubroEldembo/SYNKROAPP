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
    /// Lógica de interacción para PantallaAgregarProductoA1ZonaWPF.xaml
    /// </summary>
    public partial class PantallaAgregarProductoA1ZonaWPF : Window
    {
        private CrearProductoEn1ZonaViewModel viewModelAddProducts;
        private CrearEntradaProductoExistenteViewModel viewModelProductosExistentes;


        public PantallaAgregarProductoA1ZonaWPF(CrearEntradaProductoExistenteViewModel viewModelProductosExistentes)
        {
            InitializeComponent();
            this.viewModelProductosExistentes = viewModelProductosExistentes;
            this.DataContext = viewModelProductosExistentes;
            Loaded += PantallaAgregarProductoA1ZonaWPF_Loaded;
        }

        private async void PantallaAgregarProductoA1ZonaWPF_Loaded(object sender, RoutedEventArgs e)
        {
            if (viewModelAddProducts != null)
            {
                await viewModelAddProducts.InicializarCamposAsync();
            }
            else if (viewModelProductosExistentes != null)
            {
                await viewModelProductosExistentes.InicializarAsync();
            }
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
