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
        public VistaAlmacenes()
        {
            InitializeComponent();
            this.DataContext = new AlmacenesViewModel
            {
                TotalAlmacenes = "2",
                MovimientosRecientes = "1",
                ProductosEnAlmacenes = "134"
            };
        }

        private void btnCrearAlmacen_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
