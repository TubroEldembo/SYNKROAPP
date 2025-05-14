using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
using SYNKROAPP.VIEWMODEL;
using SYNKROAPP.Vistes.Vista_Productos;
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

namespace SYNKROAPP.Vistes.Vista_Mercado.VistaEmpresas.Vista_Catalogo
{
    /// <summary>
    /// Lógica de interacción para VistaGestionarCatalogo.xaml
    /// </summary>
    public partial class VistaGestionarCatalogo : UserControl
    {
        private IDAO dao;
        private Empreses empresa;
        private GestionarCatalogoViewModel viewModel;


        public VistaGestionarCatalogo(IDAO dao, Empreses empresa)
        {
            InitializeComponent();
            this.dao = dao;
            this.empresa = empresa;

            viewModel = new GestionarCatalogoViewModel(dao, empresa);
            this.DataContext = viewModel;
            viewModel.PropertyChanged += ViewModel_PropertyChanged; ;
            this.Loaded += VistaGestionarCatalogo_Loaded;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.ProductoSeleccionadoParaMenu) &&
                viewModel.ProductoSeleccionadoParaMenu != null)
            {
                // Muestra el Popup en la posición del ratón
                popupMenuProducto.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
                popupMenuProducto.IsOpen = true;
            }
        }

        private void VistaGestionarCatalogo_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

        private void txtBusqueda_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Obtener el producto asociado al control que se hizo clic
            if (sender is Border border && border.DataContext is ProducteAmbDetall producto)
            {
                // Ejecutar el comando MostrarMenuContextual con el producto como parámetro
                viewModel.MostrarMenuContextualCommand.Execute(producto);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Para el botón de menú en cada producto (los tres puntos)
            if (sender is Button button && button.DataContext is ProducteAmbDetall producto)
            {
                viewModel.MostrarMenuContextualCommand.Execute(producto);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AgregarProductoACatalogoVM agregarProductoACatalogo = new AgregarProductoACatalogoVM(dao, empresa.EmpresaID);

            PantallaAgregarProductoACatalago pantallaAgregarProdCatalogo = new PantallaAgregarProductoACatalago(agregarProductoACatalogo);

            pantallaAgregarProdCatalogo.Show(); // o ShowDialog() si quieres que sea modal
        }

        private void btnDuplicar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            EditarProductoViewModel editarProdViewModel = new EditarProductoViewModel(dao, viewModel.ProductoSeleccionadoParaMenu.Producte);
            PantallaEditarProducto editarProducto = new PantallaEditarProducto(editarProdViewModel);
            editarProducto.Show();
        }

        private void btnVerDetalles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            AgregarProductoACatalogoVM agregarProductoACatalogo = new AgregarProductoACatalogoVM(dao, empresa.EmpresaID);

            PantallaAgregarProductoACatalago pantallaAgregarProdCatalogo = new PantallaAgregarProductoACatalago(agregarProductoACatalogo);

            pantallaAgregarProdCatalogo.Show(); // o ShowDialog() si quieres que sea modal
        }

        private void btnConfirmarEliminar_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
