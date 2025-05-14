using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
using SYNKROAPP.VIEWMODEL;
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
        Magatzems _magatzem;


        public  PantallaDetalleZonaWPF(DetalleDe1ZonaViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            _zona = viewModel.ZonaSeleccionada;
            _dao = viewModel.Dao;
            _magatzem = viewModel.MagatzemSeleccionat;
            CargarListas();
        }

        private async void dgProductosDeLaZona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductosDeLaZona.SelectedItem is ProducteInventariAmbNom seleccion)
            {
                // Cargar el producto general desde Firestore usando el ID
                DocumentSnapshot docSnap = await _dao.GetProducteGeneralPorID(seleccion.ProducteID);
                var producteGeneral = docSnap.Exists ? docSnap.ConvertTo<ProducteGeneral>() : null;

                string estado = seleccion.Estat;
                string descripcion = seleccion.Descripcio;
                string categoria = seleccion.Categoria;
                string subcategoria = seleccion.SubCategoria;

                if (producteGeneral != null)
                {
                    DetalleDelProductoViewModel viewModel = new DetalleDelProductoViewModel(_dao, producteGeneral, estado, descripcion, categoria, subcategoria, _magatzem, _zona);
                    PantallaDetalleProductoWPF pantallaDetalleProducto = new PantallaDetalleProductoWPF(viewModel);
                    pantallaDetalleProducto.ShowDialog();

                }
                else
                {
                    MessageBox.Show("No se pudo cargar la información del producto.");
                }
            }
        }

        private async void CargarListas()
        {
            var viewModel = (DetalleDe1ZonaViewModel)this.DataContext;
            await viewModel.CargarListasParaCombosAsync();
        }

        private void btnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            var viewModelAddProducts = new CrearProductoEn1ZonaViewModel(_dao, _zona);

            PantallaCrearProductoGeneral pCrearProducto = new PantallaCrearProductoGeneral(viewModelAddProducts);
            pCrearProducto.Show();

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
