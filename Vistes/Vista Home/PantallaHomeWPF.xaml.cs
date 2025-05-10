using Firebase.Auth;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.Vistes.Vista_Mercado.VistaEmpresas;
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

namespace SYNKROAPP.Vistes.Vista_Home
{
    /// <summary>
    /// Lógica de interacción para PantallaHomeWPF.xaml
    /// </summary>
    public partial class PantallaHomeWPF : Window
    {
        private Usuaris loggedUser;
        private FirebaseAuth auth;
        private FirebaseAuthLink authLink;
        private IDAO dao;
        private bool isMenuVisible = false;
        private Empreses empresa;


        public PantallaHomeWPF(Usuaris usuari, FirebaseAuthLink auth, IDAO dao, Empreses empresa)
        {
            InitializeComponent();

            this.loggedUser = usuari;
            this.auth = auth;
            this.dao = dao;
            this.empresa = empresa;

            contentArea.Content = new VistaResumen(dao, empresa);
        }

        public PantallaHomeWPF(Usuaris loggedUser, FirebaseAuth auth, FirebaseAuthLink authLink, IDAO dao, Empreses empresa)
        {
            InitializeComponent();

            this.loggedUser = loggedUser;
            this.auth = auth;
            this.authLink = authLink;
            this.dao = dao;
            this.empresa = empresa;

            BitmapImage logoUrl = dao.LoadImageFromUrl(empresa.FotoEmpresalUrl);
            logoImageURL.Source = logoUrl;

            contentArea.Content = new VistaResumen(dao, empresa);
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuVisible)
            {
                isMenuVisible = false;
                sliderBar.Visibility = Visibility.Collapsed;
                overlay.Visibility = Visibility.Collapsed;
            }

            contentArea.Content = new VistaResumen(dao, empresa);

        }

        private void btnRegistrarArticulos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAlmacenes_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuVisible)
            {
                isMenuVisible = false;
                sliderBar.Visibility = Visibility.Collapsed;
                overlay.Visibility = Visibility.Collapsed;
            }

            var vista = new Vista_Almacenes.VistaAlmacenes(dao, empresa);

            // Suscribimos al evento para quitar el UserControl cuando se cierre
            vista.OnCerrar += () =>
            {
                contentArea.Content = null; // Esto "cierra" el UserControl
            };

            contentArea.Content = vista;
            

        }

        private void btnMovimientos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMercado_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuVisible)
            {
                isMenuVisible = false;
                sliderBar.Visibility = Visibility.Collapsed;
                overlay.Visibility = Visibility.Collapsed;
            }

            contentArea.Content = new PantallaEmpresasEnElMercadoWPF(dao, empresa);
        }

        private void btnComandas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnToggleSlider_Click(object sender, RoutedEventArgs e)
        {
            isMenuVisible = !isMenuVisible;
            sliderBar.Visibility = isMenuVisible ? Visibility.Visible : Visibility.Collapsed;
            overlay.Visibility = isMenuVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMenuVisible = false;
            sliderBar.Visibility = Visibility.Collapsed;
            overlay.Visibility = Visibility.Collapsed;
        }
    }
}
