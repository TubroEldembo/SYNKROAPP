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
            this.empresa = empresa;

            var vm = new AlmacenesViewModel(dao, empresa);

            vm.AbrirPantallaZonasAlmacen = async (almacen) =>
            {
                ZonasAlmacenViewModel viewModelZonasAlmacen = new ZonasAlmacenViewModel(dao, almacen);
                await viewModelZonasAlmacen.CargarZonasAsync();

                PantallaZonasAlmacenWPF ventanaZonas = new PantallaZonasAlmacenWPF(viewModelZonasAlmacen);
                ventanaZonas.Show();
                OnCerrar?.Invoke();
            };

            vm.AbrirPantallaCrearAlmacen = () =>
            {
                PantallaCrearAlmacenWPF pantallaCrearAlmacen = new PantallaCrearAlmacenWPF(dao, empresa)
                {
                    WindowState = WindowState.Maximized,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                };

                pantallaCrearAlmacen.Show();
                OnCerrar?.Invoke(); // cerrar la vista actual si hace falta
            };
            this.DataContext = vm;

            DetallesAlmacenes();
        }


        private async void DetallesAlmacenes()
        {
            await ((AlmacenesViewModel)this.DataContext).CargarDetallesAlmacenes();
        }
    }
}
