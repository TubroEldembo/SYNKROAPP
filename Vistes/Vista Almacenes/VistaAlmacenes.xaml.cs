using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SYNKROAPP.Vistes.Vista_Almacenes
{
    /// <summary>
    /// Lógica de interacción para VistaAlmacenes.xaml
    /// </summary>
    public partial class VistaAlmacenes : UserControl
    {
        private IDAO dao;
        private Usuaris loggedUser;
        private Empreses empresa;
        public event Action OnCerrar;

        public VistaAlmacenes(IDAO dao, Empreses empresa)
        {
            InitializeComponent();
            this.dao = dao;
            this.DataContext = new AlmacenesViewModel(dao);
            this.loggedUser = loggedUser;
            this.empresa = empresa;
            DetallesAlmacenes();
        }

        private async void DetallesAlmacenes()
        {
            await ((AlmacenesViewModel)this.DataContext).CargarDetallesAlmacenes(empresa);
        }

        private void btnCrearAlmacen_Click(object sender, RoutedEventArgs e)
        {
            PantallaCrearAlmacenWPF pantallaCrearAlmacen = new PantallaCrearAlmacenWPF(dao, empresa)
            {
                WindowState = WindowState.Maximized, 
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
            };

            pantallaCrearAlmacen.Show();
            OnCerrar?.Invoke();
            
        }

        private async void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var almacenSeleccionado = dataGrid.SelectedItem as Magatzems;

            if (almacenSeleccionado != null)
            {
                // Crear el ViewModel y cargar los datos
                var viewModelZonasAlmacen = new ZonasAlmacenViewModel(dao, almacenSeleccionado);
                await viewModelZonasAlmacen.CargarZonasAsync(); // ✅ ESTA LÍNEA ES LA CLAVE

                // Pasar ese ViewModel ya cargado a la ventana
                var ventanaZonas = new PantallaZonasAlmacenWPF(viewModelZonasAlmacen);
                ventanaZonas.Show();
                OnCerrar?.Invoke();
            }
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
