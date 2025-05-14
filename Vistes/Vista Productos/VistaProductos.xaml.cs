using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.VIEWMODEL;
using SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen.Vista_ProductosDe1Zona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace SYNKROAPP.Vistes.Vista_Productos
{
    /// <summary>
    /// Lógica de interacción para VistaProductos.xaml
    /// </summary>
    public partial class VistaProductos : UserControl
    {
        private IDAO dao;
        private Empreses empresa;
        private ProductosViewModel _viewModel;

        public VistaProductos(IDAO dao, Empreses empresa)
        {
            InitializeComponent();
            this.dao = dao;
            this.empresa = empresa;
            _viewModel = new ProductosViewModel(dao, empresa);
            DataContext = _viewModel;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
           _viewModel.GuardarProducto();
        }

        private void txtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SeleccionarImagen();
        }

        private void dgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem is ProducteAmbDetall seleccion)
            {
                if (seleccion != null)
                {
                    CrearEntradaProductoExistenteViewModel viewModel = new CrearEntradaProductoExistenteViewModel(dao, seleccion, empresa.EmpresaID);

                    PantallaAgregarProductoA1ZonaWPF pantallaAgregarProducto = new PantallaAgregarProductoA1ZonaWPF(viewModel);
                    pantallaAgregarProducto.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No se pudo cargar la información del producto.");
                }
            }
        }
    }
}
