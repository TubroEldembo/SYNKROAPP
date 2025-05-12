using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
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

namespace SYNKROAPP.Vistes.Vista_Movimientos
{
    /// <summary>
    /// Lógica de interacción para VistaMovimientos.xaml
    /// </summary>
    public partial class VistaMovimientos : UserControl
    {
        private IDAO dao;
        private Empreses empresa;
        private VistaMovimientosViewModel _viewModel;
        public VistaMovimientos(IDAO dao, Empreses empresa)
        {
            InitializeComponent();
            this.dao = dao;
            this.empresa = empresa;

            _viewModel = new VistaMovimientosViewModel(dao, empresa);
            DataContext = _viewModel;
            _ = _viewModel.CargarMovimientosAsync();

        }

        private void cmbTipoMovimiento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnLimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LimpiarFiltros();
        }

        private void dpFechaDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dpFechaHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnNuevoMovimiento_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
