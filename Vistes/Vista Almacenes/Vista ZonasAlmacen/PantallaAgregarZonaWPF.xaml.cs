using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
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

namespace SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen
{
    /// <summary>
    /// Lógica de interacción para PantallaAgregarZonaWPF.xaml
    /// </summary>
    public partial class PantallaAgregarZonaWPF : Window
    {
        private IDAO dao;
        Magatzems almacenSeleccionado;
        private readonly CrearZonaViewModel _viewModel;
        public event Func<object, EventArgs, Task> OnZonaGuardadaEvent;

        public PantallaAgregarZonaWPF(ZonasAlmacenViewModel zonasVM)
        {
            InitializeComponent();
            this.dao = zonasVM._dao; // Asumiendo que tienes dao y magatzem como propiedades públicas
            this.almacenSeleccionado = zonasVM._almacenSeleccionado;
            

            _viewModel = new CrearZonaViewModel(this.dao, this.almacenSeleccionado);
            this.DataContext = _viewModel;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            bool zonaGuardada = await _viewModel.GuardarZona();
            if (zonaGuardada)
            {
                // Invocar evento async si hay suscriptores
                if (OnZonaGuardadaEvent != null)
                    await OnZonaGuardadaEvent.Invoke(this, EventArgs.Empty);

                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
