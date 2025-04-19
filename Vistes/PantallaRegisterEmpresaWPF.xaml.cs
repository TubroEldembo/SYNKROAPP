using Microsoft.Win32;
using SYNKROAPP.AUTH;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.Vistes.Vista_Home;
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

                Empreses novaEmpresa = new Empreses
                {
                    EstatVerificacio = false,
                    NomEmpresa = txtNombreEmpresa.Text,
                    Tipus = txtTipoEmpresa.Text,
                    Magatzems = new List<int>(),
                    Usuaris = new List<int>(),
                    Ubicacio = direccioEmpresa,
                    FotoEmpresalUrl = "" // Si subes imagen, puedes poner aquí la URL
                };


                await dao.RegistrarUsuariAmbEmpresa(usuari, novaEmpresa);
                MessageBox.Show("Empresa i usuari registrats correctament!");
                PantallaHomeWPF homeWindow = new PantallaHomeWPF(usuari,auth, dao);
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
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                imgEmpresa.Source = bitmap;
            }
        }
    }
}
