using Firebase.Auth;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.Vistes.Vista_Mercado.VistaEmpresas;
using SYNKROAPP.Vistes.Vista_Movimientos;
using SYNKROAPP.Vistes.Vista_Productos;
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
using System.Windows.Media.Animation;
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
        private bool isMenuOpen = false;

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

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {

            if (isMenuOpen)
            {
                ToggleMenu();
            }

            contentArea.Content = new VistaResumen(dao, empresa);

        }

        private void btnRegistrarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuOpen)
            {
                ToggleMenu();
            }

            contentArea.Content = new VistaProductos(dao, empresa);
        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuOpen)
            {
                ToggleMenu();
            }

            var vista = new VistaImportarProductos(dao, empresa);

            // Suscribimos al evento para quitar el UserControl cuando se cierre
            contentArea.Content = vista;
        }

        private void btnAlmacenes_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuOpen)
            {
                ToggleMenu();
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
            if (isMenuOpen)
            {
                ToggleMenu();
            }

            VistaMovimientos vista = new VistaMovimientos(dao, empresa);

            contentArea.Content = vista;
        }

        private void btnMercado_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuOpen)
            {
                ToggleMenu();
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
            ToggleMenu();
        }
        private void overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Cerrar el menú si se hace clic fuera de él
            if (isMenuOpen)
            {
                ToggleMenu();
            }
        }

        private void ToggleMenu()
        {
            if (isMenuOpen)
            {
                // Cerrar menú
                Storyboard sb = this.FindResource("SlideOutMenu") as Storyboard;
                if (sb != null)
                {
                    sb.Begin(sliderBar);
                }

                // Ocultar overlay después de la animación
                overlay.Visibility = Visibility.Collapsed;
                isMenuOpen = false;

                // Cambiar el contenido del botón
                btnToggleSlider.Content = "☰";
            }
            else
            {
                // Abrir menú
                Storyboard sb = this.FindResource("SlideInMenu") as Storyboard;
                if (sb != null)
                {
                    sb.Begin(sliderBar);
                }

                // Mostrar overlay
                overlay.Visibility = Visibility.Visible;
                isMenuOpen = true;

                // Cambiar el contenido del botón
                btnToggleSlider.Content = "✕";
            }
        }
    }
}
