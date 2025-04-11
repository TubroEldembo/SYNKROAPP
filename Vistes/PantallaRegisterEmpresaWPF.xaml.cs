using Microsoft.Win32;
using SYNKROAPP.CLASES;
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
    /// Lógica de interacción para PantallaRegisterEmpresaWPF.xaml
    /// </summary>
    public partial class PantallaRegisterEmpresaWPF : Window
    {
        private Usuaris usuari;
        public PantallaRegisterEmpresaWPF(Usuaris usuari)
        {
            InitializeComponent();
            this.usuari = usuari;  
        }

        private void btnRegistrarEmpresa_Click(object sender, RoutedEventArgs e)
        {

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
