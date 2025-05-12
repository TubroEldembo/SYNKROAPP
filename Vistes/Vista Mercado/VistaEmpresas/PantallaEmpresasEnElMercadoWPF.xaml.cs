using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
using SYNKROAPP.VIEWMODEL;
using SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen.Vista_ProductosDe1Zona;
using SYNKROAPP.Vistes.Vista_Mercado.VistaEmpresas.Vista_Catalogo;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SYNKROAPP.Vistes.Vista_Mercado.VistaEmpresas
{
    /// <summary>
    /// Lógica de interacción para PantallaEmpresasEnElMercadoWPF.xaml
    /// </summary>
    public partial class PantallaEmpresasEnElMercadoWPF : UserControl
    {
        private IDAO dao;
        private Empreses empresa;
        private MercadoEmpresasViewModel viewModel;

        public PantallaEmpresasEnElMercadoWPF(IDAO dao, Empreses empresa)
        {
            InitializeComponent();
            this.dao = dao;
            this.empresa = empresa;

            viewModel = new MercadoEmpresasViewModel(dao, empresa);
            this.DataContext = viewModel;

            this.Loaded += PantallaEmpresasEnElMercadoWPF_Loaded;
        }

        private async void PantallaEmpresasEnElMercadoWPF_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.CargarEmpresas();
        }

        private void btnVerProductosEmpresa_Click(object sender, RoutedEventArgs e)
        {
            //if (zonaSeleccionada != null)
            //{
            //    // Crear el ViewModel y cargar los datos
            //    DetalleDe1ZonaViewModel viewModel = new DetalleDe1ZonaViewModel(dao, zonaSeleccionada, magatzemSeleccionat);
            //    await viewModel.CargarProductos(); // ✅ ESTA LÍNEA ES LA CLAVE

            //    // Pasar ese ViewModel ya cargado a la ventana
            //    PantallaDetalleZonaWPF detallesDeLaZona = new PantallaDetalleZonaWPF(viewModel);
            //    detallesDeLaZona.Show();
            //    //OnCerrar?.Invoke();
            //}

            if (sender is Button btnVerProductosEmpresa && btnVerProductosEmpresa.DataContext  is Empreses empresaSeleccionada)
            {
                
                CatalogoProductosViewModel catalogoVM = new CatalogoProductosViewModel(dao, empresaSeleccionada);

                PantallaCatalogoProductos pantallaCatalogo = new PantallaCatalogoProductos(catalogoVM);

                pantallaCatalogo.Show(); // o ShowDialog() si quieres que sea modal
            }
        }


        private void btnUbicacion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAplicarUbicacion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelarUbicacion_Click(object sender, RoutedEventArgs e)
        {

        }

      
    }
}
