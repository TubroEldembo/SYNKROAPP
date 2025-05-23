﻿using SYNKROAPP.ViewModel;
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

namespace SYNKROAPP.Vistes.Vista_Movimientos
{
    /// <summary>
    /// Lógica de interacción para PantallaMoverProductosWPF.xaml
    /// </summary>
    public partial class PantallaMoverProductosWPF : Window
    {
        private MoverProductoViewModel viewModel;
        public PantallaMoverProductosWPF(MoverProductoViewModel moverProductoViewModel)
        {
            InitializeComponent();

            this.viewModel = moverProductoViewModel;
            this.DataContext = viewModel;

            _ = viewModel.CargarDatosIniciales();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
