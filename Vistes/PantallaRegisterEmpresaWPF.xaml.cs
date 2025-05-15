using Microsoft.Win32;
using SYNKROAPP.AUTH;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.Vistes.Vista_Home;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Google.Rpc.Context.AttributeContext.Types;
using SYNKROAPP.GOOGLE_PLACES_SERVICE;

namespace SYNKROAPP.Vistes
{
    /// <summary>
    /// Lógica de interacción para PantallaRegisterEmpresaWPF.xaml
    /// </summary>
    public partial class PantallaRegisterEmpresaWPF : Window
    {
        private Usuaris usuari;
        IDAO? dao;
        private string rutaImagenEmpresa = null;
        private string codigoPais;
        private GooglePlacesService placesService = new GooglePlacesService();

        public PantallaRegisterEmpresaWPF(Usuaris usuari)
        {
            InitializeComponent();
            this.usuari = usuari;
            //cmbTipoVia.ItemsSource = Enum.GetValues(typeof(TipusVia));
            //cmbTipoVia.SelectedIndex = 0; // O el que quieras como predeterminado
        }

        private async void btnRegistrarEmpresa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var auth = await Autentication.SignUp(usuari.Email, usuari.Password);
                dao = DAOFactory.CreateDAO(auth.FirebaseToken, "projecto-synkroapp");

                string direccionCompleta = txtBuscarDireccion.Text;

                // Obtener coordenadas de la dirección
                var (lat, lng) = await placesService.ObtenerCoordenadasAsync(direccionCompleta);

                // Crear objeto dirección
                Adreça direccioEmpresa = new Adreça
                {
                    AdreçaID = Math.Abs(Guid.NewGuid().GetHashCode()),
                    Direccio = direccionCompleta,
                    Latitud = lat,
                    Longitud = lng,
                    CodiPostal = txtCodigoPostal.Text,
                    Pais = txtBuscarPais.Text,

                };

                string urlImagen = "";
                if (!string.IsNullOrWhiteSpace(rutaImagenEmpresa) && File.Exists(rutaImagenEmpresa))
                {
                    string nombreFirebase = $"{txtNombreEmpresa.Text.Replace(" ", "_")}_{Guid.NewGuid()}.jpg";
                    urlImagen = await dao.StoreImage(rutaImagenEmpresa, nombreFirebase);
                }

                Empreses novaEmpresa = new Empreses
                {
                    EstatVerificacio = false,
                    NomEmpresa = txtNombreEmpresa.Text,
                    Tipus = cmbSector.Text, 
                    Magatzems = new List<string>(),
                    Usuaris = new List<string>(),
                    Ubicacio = direccioEmpresa,
                    FotoEmpresalUrl = urlImagen
                };

                await dao.RegistrarUsuariAmbEmpresa(usuari, novaEmpresa);
                MessageBox.Show("Empresa y usuario registrados correctamente!");

                PantallaHomeWPF homeWindow = new PantallaHomeWPF(usuari, auth, dao, novaEmpresa);
                homeWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnUploadImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                rutaImagenEmpresa = openFileDialog.FileName;

                BitmapImage bitmap = new BitmapImage();
                using (FileStream stream = new FileStream(rutaImagenEmpresa, FileMode.Open, FileAccess.Read))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // Carga todo en memoria y no bloquea el archivo
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }

                imgEmpresa.Source = bitmap;
            }
        }

        private void lstSugerenciasDireccion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstSugerenciasDireccion.SelectedItem is string seleccion)
            {
                txtBuscarDireccion.Text = seleccion;
                lstSugerenciasDireccion.Visibility = Visibility.Collapsed;
            }
        }

        private async void txtBuscarDireccion_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtBuscarDireccion.Text.Length < 3)
            {
                lstSugerenciasDireccion.Visibility = Visibility.Collapsed;
                return;
            }

            List<string> sugerencias = await placesService.AutocompletarCiudadAsync(txtBuscarDireccion.Text, codigoPais);
            if (sugerencias.Any())
            {
                lstSugerenciasDireccion.ItemsSource = sugerencias;
                lstSugerenciasDireccion.Visibility = Visibility.Visible;
            }
            else
            {
                lstSugerenciasDireccion.Visibility = Visibility.Collapsed;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtBuscarDireccion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lstSugerenciasDireccion_MouseDoubleClick(sender, null);
            }
        }

        private void txtBuscarPais_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lstSugerenciasPais_MouseDoubleClick(sender, null);
            }
        }

        private async void txtBuscarPais_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtBuscarPais.Text.Length < 3)
            {
                lstSugerenciasPais.Visibility = Visibility.Collapsed;
                return;
            }

            List<PaisAutocompleteResult> sugerencias = await placesService.AutocompletarPaisAsync(txtBuscarPais.Text);

            if (sugerencias.Any())
            {
                lstSugerenciasPais.ItemsSource = sugerencias;
                lstSugerenciasPais.Visibility = Visibility.Visible;
            }
            else
            {
                lstSugerenciasPais.Visibility = Visibility.Collapsed;
            }
        }

        private async void lstSugerenciasPais_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstSugerenciasPais.SelectedItem is PaisAutocompleteResult seleccion)
            {
                txtBuscarPais.Text = seleccion.Description;

                codigoPais = await placesService.ObtenerCodigoIsoDesdePlaceId(seleccion.PlaceId);
                lstSugerenciasPais.Visibility = Visibility.Collapsed;
            }
        }
    }
}
