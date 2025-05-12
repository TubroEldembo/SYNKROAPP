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

namespace SYNKROAPP.Vistes.Vista_Mercado.VistaEmpresas.Vista_Catalogo
{
    /// <summary>
    /// Lógica de interacción para PantallaCatalogoProductos.xaml
    /// </summary>
    public partial class PantallaCatalogoProductos : Window
    {
        public PantallaCatalogoProductos(CatalogoProductosViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _ = vm.CargarProductos();
        }

        private void CerrarCarrito_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
