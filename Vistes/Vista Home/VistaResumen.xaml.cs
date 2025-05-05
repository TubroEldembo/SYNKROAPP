using Steema.TeeChart.Styles;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.VIEWMODEL;
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
    /// Lógica de interacción para VistaResumen.xaml
    /// </summary>
    public partial class VistaResumen : UserControl
    {
        private IDAO dao;
        private Empreses empresa;
        private VistaResumenViewModel _viewModel;

        public VistaResumen(IDAO dao, Empreses empresa)
        {
            InitializeComponent();

            this.dao = dao;
            this.empresa = empresa;

            _viewModel = new VistaResumenViewModel(dao, empresa);
            DataContext = _viewModel;

           _ = CargarYVincularGrafico();
        }

        private void VincularPieChart()
        {
            var productosXCategoria = _viewModel.ProductosPorCategoria;
            tchartPie.Series.Clear();

            int total = productosXCategoria.Values.Sum();
            tchartPie.Header.Text = $"PRODUCTOS EN EL CATALOGO ({total} en total)";

            Pie pieSeries = new Pie(tchartPie.Chart);

            foreach (var kvp in productosXCategoria)
            {
                // Personaliza directamente el label
                string leyendaPersonalizada = $"{kvp.Key}: {kvp.Value}";
                pieSeries.Add(kvp.Value, leyendaPersonalizada);
            }

            // Mostrar la leyenda con los nuevos labels
            pieSeries.ShowInEditor = true;

            // Estilo de la leyenda
            tchartPie.Legend.Alignment = Steema.TeeChart.LegendAlignments.Right;
            tchartPie.Legend.ResizeChart = false;
            tchartPie.Legend.CustomPosition = false;
            tchartPie.Legend.Width = 420;

            // Estilo gráfico
            pieSeries.Circled = true;
            pieSeries.Marks.Visible = true;
            pieSeries.Marks.Style = MarksStyles.Percent;
            pieSeries.ExplodeBiggest = 5;
        }

        private async Task CargarYVincularGrafico()
        {
            // Llamar al método CargarDatosEmpresa en ViewModel
            await _viewModel.CargarDatosEmpresa();

            // Vincular los datos al gráfico
            VincularPieChart();
        }




    }
}
