using Microsoft.Win32;
using SYNKROAPP.CLASES;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.Vistes
{
    /// <summary>
    /// Lógica de interacción para PantallaRegisterWPF.xaml
    /// </summary>
    public partial class PantallaRegisterWPF : Window
    {
        public PantallaRegisterWPF()
        {
            InitializeComponent();  
        }

        private void btnUploadImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if(openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                imgProfile.Source = bitmap;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Usuaris nuevoUsuario = new Usuaris
            {
                Nom = txtNombre.Text,
                Cognoms = txtApellidos.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Password,
                Rol = "Usuario", 
                EmpresaID = "", 
                FotoPerfilUrl = "" 
            };

            PantallaRegisterEmpresaWPF pantallaRegEmpresa = new PantallaRegisterEmpresaWPF(nuevoUsuario);
            pantallaRegEmpresa.Show();
            this.Close();
        }

        private void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFormCompletion();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckFormCompletion();
        }

        private void CheckFormCompletion()
        {

            bool isFormComplete = !string.IsNullOrWhiteSpace(txtNombre.Text) &&
                                  !string.IsNullOrWhiteSpace(txtApellidos.Text) &&
                                  !string.IsNullOrWhiteSpace(txtEmail.Text) &&
                                  !string.IsNullOrWhiteSpace(txtPassword.Password);

            btnSiguiente.IsEnabled = isFormComplete;
        }

    }
}
