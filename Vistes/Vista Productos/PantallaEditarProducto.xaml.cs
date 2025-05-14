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

namespace SYNKROAPP.Vistes.Vista_Productos
{
    /// <summary>
    /// Lógica de interacción para PantallaEditarProducto.xaml
    /// </summary>
    public partial class PantallaEditarProducto : Window
    {
        private EditarProductoViewModel editarProdViewModel;

        public PantallaEditarProducto(EditarProductoViewModel editarProdViewModel)
        {
            InitializeComponent();
            this.editarProdViewModel = editarProdViewModel;
            DataContext = editarProdViewModel;

            Loaded += async (s, e) => await editarProdViewModel.CargarDetallProducte(); // Carga los datos aquí
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
