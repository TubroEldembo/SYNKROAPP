using Steema.TeeChart;
using Steema.TeeChart.Styles;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.VIEWMODEL;
using System.Drawing;
using System.Windows.Controls;

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

            // Configurar el texto del encabezado y subencabezado
            tchartPie.Header.Text = $"PRODUCTOS EN EL CATÁLOGO \n  {total} productos registrados";
            tchartPie.SubHeader.Text = $"{total} productos registrados";
            tchartPie.Header.Font.Bold = true;
            tchartPie.Header.Font.Color = Color.FromArgb(74, 87, 147);  // Asegúrate de que el color sea visible
            tchartPie.SubHeader.Font.Size = 10;
            tchartPie.SubHeader.Font.Color = Color.Black;  // Asegúrate de que el texto sea visible

            // Estilo de fondo del gráfico (asegúrate de que no se mezcle con el color del texto)
            tchartPie.Panel.Color = Color.FromArgb(245, 245, 245);  // Un fondo gris claro

            Pie pieSeries = new Pie(tchartPie.Chart);

            foreach (var kvp in productosXCategoria)
            {
                pieSeries.Add(kvp.Value, kvp.Key.ToUpper());
            }

            pieSeries.ShowInEditor = true;

            // Estilo de la leyenda
            tchartPie.Legend.Alignment = Steema.TeeChart.LegendAlignments.Right;
            tchartPie.Legend.LegendStyle = LegendStyles.Auto;
            tchartPie.Legend.ResizeChart = false;
            tchartPie.Legend.CustomPosition = false;
            tchartPie.Legend.Width = 420;
            tchartPie.Aspect.View3D = true;

            // Estilo gráfico
            pieSeries.Circled = true;
            pieSeries.Marks.Visible = true;
            pieSeries.Marks.Style = MarksStyles.Percent;
            pieSeries.ExplodeBiggest = 5;

            tchartPie.Chart.GetLegendText += (s, e) =>
            {
                string categoria = pieSeries.Labels[e.Index];
                int cantidad = (int)pieSeries[e.Index].Y;
                e.Text = $"{categoria}: {cantidad}";
            };
        }


        private void VincularBarChart()
        {
            var productosXAlmacen = _viewModel.ProductosPorAlmacen;
            tchartBar.Series.Clear();
            tchartBar.Header.Text = "PRODUCTOS POR ALMACÉN";
            tchartBar.Aspect.View3D = true;

            Bar barSeries = new Bar(tchartBar.Chart);
            barSeries.Marks.Visible = true;
            barSeries.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Value;
            barSeries.ColorEach = true;

            foreach (var kvp in productosXAlmacen)
            {
                barSeries.Add(kvp.Value, kvp.Key);
            }

            tchartBar.Axes.Bottom.Labels.Angle = 0;
            tchartBar.Axes.Bottom.Labels.Font.Size = 8;
            tchartBar.Axes.Bottom.Labels.MultiLine = true;
            tchartBar.Axes.Left.Title.Text = "Cantidad de productos";
            tchartBar.Legend.LegendStyle = LegendStyles.Auto;
            


            tchartBar.Chart.GetLegendText += (s, e) =>
            {
                string categoria = barSeries.Labels[e.Index];
                int cantidad = (int)barSeries[e.Index].Y;
                e.Text = $"{categoria}: {cantidad} productos";
            };

        }

        private async Task CargarYVincularGrafico()
        {
            _viewModel.IsCargandoVisible = true;

            // Llamar al método CargarDatosEmpresa en ViewModel
            await _viewModel.CargarDatosEmpresa();

            // Vincular los datos al gráfico
            VincularPieChart();
            VincularBarChart();

            _viewModel.IsCargandoVisible = false;
        }




    }
}
