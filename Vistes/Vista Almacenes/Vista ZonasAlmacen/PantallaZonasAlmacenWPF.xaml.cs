using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
using SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen;
using SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen.Vista_ProductosDe1Zona;
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

namespace SYNKROAPP.Vistes.Vista_Almacenes
{
    /// <summary>
    /// Lógica de interacción para PantallaAgregarZonasWPF.xaml
    /// </summary>
    public partial class PantallaZonasAlmacenWPF : Window
    {
        private IDAO dao;
        Magatzems magatzemSeleccionat;
        private ZonasAlmacenViewModel viewModel;

        public PantallaZonasAlmacenWPF(ZonasAlmacenViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            dao = viewModel._dao;
            this.magatzemSeleccionat = viewModel._almacenSeleccionado;
        }

        private void btnAgregarZona_Click(object sender, RoutedEventArgs e)
        {
            PantallaAgregarZonaWPF agregarZona = new PantallaAgregarZonaWPF(viewModel);

            agregarZona.OnZonaGuardadaEvent += async (s, args) =>
            {
                await viewModel.CargarZonasAsync();
            };

            agregarZona.ShowDialog();
            this.Close();
        }

        private async void dgZonasAlmacen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            ZonaEmmagatzematge zonaSeleccionada = dataGrid.SelectedItem as ZonaEmmagatzematge;

            if (zonaSeleccionada != null)
            {
                // Crear el ViewModel y cargar los datos
                DetalleDe1ZonaViewModel viewModel = new DetalleDe1ZonaViewModel(dao, zonaSeleccionada, magatzemSeleccionat);
                await viewModel.CargarProductos(); // ✅ ESTA LÍNEA ES LA CLAVE

                // Pasar ese ViewModel ya cargado a la ventana
                PantallaDetalleZonaWPF detallesDeLaZona = new PantallaDetalleZonaWPF(viewModel);
                detallesDeLaZona.Show();
                //OnCerrar?.Invoke();
            }
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
