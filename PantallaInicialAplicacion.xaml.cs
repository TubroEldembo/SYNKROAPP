using SYNKROAPP.Vistes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SYNKROAPP
{
    /// <summary>
    /// Interaction logic for PantallaInicialAplicacion.xaml
    /// </summary>
    public partial class PantallaInicialAplicacion : Window
    {
        public PantallaInicialAplicacion()
        {
            InitializeComponent();
        }
      
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            PantallaLoginWPF pantallaLogin = new PantallaLoginWPF();
            pantallaLogin.Show();
            this.Close();
        }

        private void btnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            PantallaRegisterWPF pantallaRegisterUser = new PantallaRegisterWPF();
            pantallaRegisterUser.Show();
            this.Close();
        }
    }
}