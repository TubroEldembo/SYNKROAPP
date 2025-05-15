using Firebase.Auth;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.ViewModel;
using SYNKROAPP.Vistes.Vista_Almacenes.Vista_ZonasAlmacen;
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
    /// Lógica de interacción para PantallaCrearAlmacenWPF.xaml
    /// </summary>
    public partial class PantallaCrearAlmacenWPF : Window
    {
        private IDAO dao;
        private Empreses empresa;
        private ZonasAlmacenViewModel viewModel;
        private Magatzems magatzemCreat;
        private CrearAlmacenViewModel crearAlmacenViewModel;

     

        //public PantallaCrearAlmacenWPF(IDAO dao, Empreses empresa)
        //{
        //    InitializeComponent();
        //    //this.dao = dao;
        //    //this.empresa = empresa;
        //    //txtEmpresaPert.Text = empresa.EmpresaID;
        //    //cmbTipoVia.ItemsSource = Enum.GetValues(typeof(TipusVia));
        //    //cmbTipoVia.SelectedIndex = 0; // O el que quieras como predeterminado

        //    // Inicializa el ViewModel con el DAO aunque todavía no tengas un almacén
        //    this.viewModel = new ZonasAlmacenViewModel(dao, null);

        //    this.DataContext = viewModel;
        //}

        public PantallaCrearAlmacenWPF(CrearAlmacenViewModel crearAlmacenViewModel)
        {
            InitializeComponent();
            this.crearAlmacenViewModel = crearAlmacenViewModel;
            this.DataContext = crearAlmacenViewModel;
        }

        //private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (magatzemCreat == null)
        //    {
        //        // PRIMER CLIC: Crear el almacén
        //        try
        //        {
        //            string nombreAlmacen = txtNombreAlmacen.Text;
        //            //string nombreCalle = txtNombreCalle.Text;
        //            string numero = txtNumero.Text;
        //            string codiPostal = txtCodigoPostal.Text;

        //            int capacitat = int.Parse(txtCapacidadTotal.Text);
                   
        //            //string empresaId = txtEmpresaPert.Text;
        //            //TipusVia tipusSeleccionat = (TipusVia)cmbTipoVia.SelectedItem;

        //            //if (string.IsNullOrEmpty(nombreAlmacen) || string.IsNullOrEmpty(/*empresaId*/))
        //            //{
        //            //    MessageBox.Show("Faltan campos obligatorios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            //    return;
        //            //}

        //            Adreça direccioMagatzem = new Adreça
        //            {
        //                AdreçaID = Math.Abs(Guid.NewGuid().GetHashCode()),
        //                //TipusVia = tipusSeleccionat,
        //                //Nom = nombreCalle,
        //                //Numero = int.Parse(numero),
        //                CodiPostal = codiPostal
        //            };

        //            Magatzems nouMagatzem = new Magatzems
        //            {
        //                NomMagatzem = nombreAlmacen,
        //                Zones = new List<string>(),
        //                MagatzemPerDefecte = false,
        //                Ubicacio = direccioMagatzem
        //            };

        //            magatzemCreat = await dao.CrearAlmacen(empresa, nouMagatzem);

        //            if (magatzemCreat != null)
        //            {
        //                MessageBox.Show("Almacén creado correctamente. Puedes continuar con las zonas o guardar y salir.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

        //                //zonaDerechaPanel.Visibility = Visibility.Visible;

        //                btnGuardar.Content = "Continuar con zonas";
        //                //btnSoloGuardar.Visibility = Visibility.Visible;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error al crear el almacén: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    else
        //    {
        //        var viewModel = new ZonasAlmacenViewModel(dao, magatzemCreat);

        //        // Crear la pantalla sin usar el parámetro 'onZonaGuardada'
        //        var agregarZona = new PantallaAgregarZonaWPF(viewModel);

        //        // Suscribirse al evento personalizado
        //        agregarZona.OnZonaGuardadaEvent += async (s, args) =>
        //        {
        //            await viewModel.CargarZonasAsync();
        //        };

        //        agregarZona.ShowDialog();
        //        this.Close();
        //    }
        //}


      
      
       
        private void btnCancelar_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
