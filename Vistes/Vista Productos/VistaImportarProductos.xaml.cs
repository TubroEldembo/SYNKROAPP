using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SYNKROAPP.Vistes.Vista_Productos
{
    /// <summary>
    /// Lógica de interacción para VistaImportarProductos.xaml
    /// </summary>
    public partial class VistaImportarProductos : UserControl
    {
        private IDAO dao;
        private Empreses empresa;
        private ImportarProductosViewModel _viewModel;

        public VistaImportarProductos(IDAO dao, Empreses empresa)
        {
            InitializeComponent();
            this.dao = dao;
            this.empresa = empresa;

            _viewModel = new ImportarProductosViewModel(dao, empresa);
            DataContext = _viewModel;
            //await _viewModel.InicializarAsync();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.GuardarProductosImportadosConInventarioAsync();
        }

        private async void btnSeleccionarArchivo_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SeleccionarArchivoYLeerAsync();
            txtRutaArchivo.Text = _viewModel.RutaArchivo;
        }

        private void cmbAlmacen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
