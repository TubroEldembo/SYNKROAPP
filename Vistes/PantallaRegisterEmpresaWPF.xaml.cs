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
        public PantallaRegisterEmpresaWPF(Usuaris usuari)
        {
            InitializeComponent();
            this.usuari = usuari;
            cmbTipoVia.ItemsSource = Enum.GetValues(typeof(TipusVia));
            cmbTipoVia.SelectedIndex = 0; // O el que quieras como predeterminado
        }

        private async void btnRegistrarEmpresa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var auth = await Autentication.SignUp(usuari.Email, usuari.Password);
                dao = DAOFactory.CreateDAO(auth.FirebaseToken, "projecto-synkroapp");

                TipusVia tipusSeleccionat = (TipusVia)cmbTipoVia.SelectedItem;
                Adreça direccioEmpresa = new Adreça
                {
                    AdreçaID = Math.Abs(Guid.NewGuid().GetHashCode()),
                    TipusVia = tipusSeleccionat,
                    Nom = txtNomCalle.Text,
                    Numero = int.Parse(txtNumero.Text),
                    CodiPostal = txtCodigoPostal.Text,
                    Provincia = txtProvincia.Text,
                    Pais = txtPais.Text
                };

                string urlImagen = "";
                if (!string.IsNullOrWhiteSpace(rutaImagenEmpresa) && File.Exists(rutaImagenEmpresa))
                {
                    string nombreArchivo = System.IO.Path.GetFileName(rutaImagenEmpresa);
                    string nombreFirebase = $"{txtNombreEmpresa.Text.Replace(" ", "_")}_{Guid.NewGuid()}.jpg"; // Aseguras nombre único
                    urlImagen = await dao.StoreImage(rutaImagenEmpresa, nombreFirebase); // Subes a Firebase Storage
                }

                Empreses novaEmpresa = new Empreses
                {
                    EstatVerificacio = false,
                    NomEmpresa = txtNombreEmpresa.Text,
                    Tipus = txtTipoEmpresa.Text,
                    Magatzems = new List<string>(),
                    Usuaris = new List<string>(),
                    Ubicacio = direccioEmpresa,
                    FotoEmpresalUrl = urlImagen 
                };


                await dao.RegistrarUsuariAmbEmpresa(usuari, novaEmpresa);
                MessageBox.Show("Empresa i usuari registrats correctament!");
                PantallaHomeWPF homeWindow = new PantallaHomeWPF(usuari,auth, dao, novaEmpresa);
                homeWindow.Show();
                this.Close();

            }
            catch(Exception ex)  
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

    }
}
