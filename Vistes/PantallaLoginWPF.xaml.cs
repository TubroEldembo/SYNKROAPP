using Firebase.Auth;
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

namespace SYNKROAPP.Vistes
{
    /// <summary>
    /// Lógica de interacción para PantallaLoginWPF.xaml
    /// </summary>
    public partial class PantallaLoginWPF : Window
    {
        IDAO? dao;
        Usuaris loggedUser;

        public PantallaLoginWPF()
        {
            InitializeComponent();
        }

        private void txtRegister_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PantallaRegisterWPF pantallaRegister = new PantallaRegisterWPF();
            pantallaRegister.Show(); 
            this.Close();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userEmail = txtUser.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Si us plau, introdueix el correu i la contrasenya.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                FirebaseAuthLink authLink = await Autentication.SignIn(userEmail, password);
                if (authLink != null)
                {
                    FirebaseAuth auth = authLink;

                    dao = DAOFactory.CreateDAO(auth.FirebaseToken, "projecto-synkroapp");

                    Usuaris loggedUser = await dao.GetUsuariByEmail(auth.User.Email);

                    if(loggedUser == null)
{
                        loggedUser = new Usuaris(
                            auth.User.LocalId,
                            auth.User.FirstName ?? "",
                            auth.User.LastName ?? "",
                            auth.User.Email,
                            "",             // No almacenes la contraseña
                            "Empleado",
                            ""              // No tienes EmpresaID aquí, así que déjalo vacío
                        );
                        await dao.AddUsuari(loggedUser);
                    }

                    Empreses empresa = await dao.GetEmpresaByID(loggedUser.EmpresaID);


                    PantallaHomeWPF homeWindow = new PantallaHomeWPF(loggedUser, auth, authLink, dao, empresa);
                    homeWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error en l'autenticació.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"S'ha produït un error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void txtForgotPsswd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
