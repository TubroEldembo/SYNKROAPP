using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen.Vista_ProductosDe1Zona
{
    /// <summary>
    /// Lógica de interacción para PantallaDetalleZonaWPF.xaml
    /// </summary>
    public partial class PantallaDetalleZonaWPF : Window
    {
        public IDAO _dao;
        ZonaEmmagatzematge _zona;
        public PantallaDetalleZonaWPF(DetalleDe1ZonaViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            _zona = viewModel._zonaSeleccionada;
            _dao = viewModel._dao;
        }

        private async void dgProductosDeLaZona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (dgProductosDeLaZona.SelectedItem is ZonaEmmagatzematge zona)
           {
                var viewModel = new DetalleDe1ZonaViewModel(_dao, _zona);
                await viewModel.CargarProductos();
           }
        }

        private void btnAgregarZona_Click(object sender, RoutedEventArgs e)
        {
            var viewModelAddProducts = new CrearProductoEn1ZonaViewModel(_dao, _zona);

            PantallaAgregarProductoA1ZonaWPF pAgregarProducto = new PantallaAgregarProductoA1ZonaWPF(viewModelAddProducts);
            pAgregarProducto.Show();

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
