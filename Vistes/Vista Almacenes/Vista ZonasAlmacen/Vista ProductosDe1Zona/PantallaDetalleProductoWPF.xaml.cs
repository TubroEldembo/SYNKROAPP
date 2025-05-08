using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
using SYNKROAPP.VIEWMODEL;
using SYNKROAPP.Vistes.Vista_Movimientos;

using System.Windows;


namespace SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen.Vista_ProductosDe1Zona
{
    /// <summary>
    /// Lógica de interacción para PantallaDetalleProductoWPF.xaml
    /// </summary>
    public partial class PantallaDetalleProductoWPF : Window
    {
        private DetalleDelProductoViewModel viewModel;

        public PantallaDetalleProductoWPF(DetalleDelProductoViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            _ = this.viewModel.CargarDetallesProducto();
            
        }

        private void btnMoverProducto_Click(object sender, RoutedEventArgs e)
        {
            MoverProductoViewModel moverProductoViewModel = new MoverProductoViewModel(viewModel.dao, viewModel.ProductoSeleccionado, viewModel.Zona, viewModel.Magatzem);

            PantallaMoverProductosWPF moverProductoVentana = new PantallaMoverProductosWPF(moverProductoViewModel); // le pasas el producto o VM
            moverProductoVentana.ShowDialog();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
 