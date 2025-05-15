using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.VIEWMODEL
{
    public partial class CrearEntradaProductoExistenteViewModel : ObservableObject
    {
        private readonly IDAO _dao;
        private readonly ProducteAmbDetall _producto;
        private readonly string _empresaID;

        public CrearEntradaProductoExistenteViewModel(IDAO dao, ProducteAmbDetall producto, string empresaID)
        {
            _dao = dao;
            _producto = producto;
            _empresaID = empresaID;

            BitmapImage bt = dao.LoadImageFromUrl(_producto.Producte.ImatgeURL);
            ImagenProducto = bt;
            ProductoSeleccionado = _producto;

            ListaEstados = new ObservableCollection<string> { "Nuevo", "Seminuevo", "Desconocido" };
        }

        public ObservableCollection<Magatzems> ListaAlmacenes { get; } = new();
        public ObservableCollection<ZonaEmmagatzematge> ListaZonas { get; } = new();

        public ObservableCollection<string> ListaEstados { get; }

        [ObservableProperty]
        private ProducteAmbDetall productoSeleccionado;

        [ObservableProperty]
        private int nProductes;

        [ObservableProperty]
        private int cantidadAIngresar;

        [ObservableProperty]
        private Magatzems? almacenSeleccionado;

        partial void OnAlmacenSeleccionadoChanged(Magatzems? value)
        {
            if (value != null)
                _ = CargarZonasPorAlmacenAsync();
        }

        [ObservableProperty]
        private ZonaEmmagatzematge? zonaSeleccionada;

        [ObservableProperty]
        private string? estadoSeleccionado = "Nuevo";

        [ObservableProperty]
        private ImageSource? imagenProducto;

        public async Task InicializarAsync()
        {
            var almacenes = await _dao.ObtenerLosAlmacenesTotalesDeLaEmpresa(_empresaID);
            ListaAlmacenes.Clear();
            foreach (var a in almacenes)
                ListaAlmacenes.Add(a);

            ListaZonas.Clear();
            foreach (var alm in almacenes)
            {
                var zonas = await _dao.DetallesZonasAlmacen(alm);
                foreach (var z in zonas)
                    ListaZonas.Add(z);

            }
        }

        private async Task CalcularCantidadProductoEnZonaAsync()
        {
            if (ZonaSeleccionada == null) return;

            var productosZona = await _dao.ProductosEn1Zona(ZonaSeleccionada);
            int productosTotales = 0;

            foreach (ProductesInventari prouctes in productosZona)
            {
                productosTotales += prouctes.Quantitat;
            }

            NProductes = productosTotales;
        }

        private async Task CargarZonasPorAlmacenAsync()
        {
            if (AlmacenSeleccionado == null) return;

            var zonas = await _dao.DetallesZonasAlmacen(AlmacenSeleccionado);
            ListaZonas.Clear();
            foreach (var z in zonas)
                ListaZonas.Add(z);

        }
        partial void OnCantidadAIngresarChanged(int value)
        {
            GuardarEntradaCommand.NotifyCanExecuteChanged();
        }

        partial void OnZonaSeleccionadaChanged(ZonaEmmagatzematge? value)
        {
            GuardarEntradaCommand.NotifyCanExecuteChanged();
            _ = CalcularCantidadProductoEnZonaAsync();
        }


        [RelayCommand(CanExecute = nameof(PuedeGuardarEntrada))]
        public async Task GuardarEntradaAsync()
        {
            try
            {
                if (ZonaSeleccionada == null) return;

                string inventariID = await _dao.CheckProductInZona(
                    _producto.Producte.ProducteID,
                    _empresaID,
                    ZonaSeleccionada.MagatzemPertanyent,
                    ZonaSeleccionada.ZonaEmmagatzematgeID);

                bool productExists = !string.IsNullOrEmpty(inventariID);

                ProductesInventari inventari;

                if (!productExists)
                {
                    inventari = new ProductesInventari
                    {
                        ProducteID = _producto.Producte.ProducteID,
                        EmpresaID = _empresaID,
                        Quantitat = 0,
                        Estat = EstadoSeleccionado ?? "Nuevo",
                        ZonaID = ZonaSeleccionada.ZonaEmmagatzematgeID,
                        MagatzemID = ZonaSeleccionada.MagatzemPertanyent,
                        CodiReferencia = _producto.Producte.SKU,
                        IDProducteInventari = Guid.NewGuid().ToString()
                    };
                    inventariID = inventari.IDProducteInventari;
                }
                else
                {
                    inventari = await _dao.GetProductoInventario(
                        _empresaID,
                        ZonaSeleccionada.MagatzemPertanyent,
                        ZonaSeleccionada.ZonaEmmagatzematgeID,
                        inventariID);
                }

                MovimentsInventari moviment = new MovimentsInventari
                {
                    ProducteInventariID = inventariID,
                    EmpresaIDOrigen = _empresaID,
                    EmpresaIDDesti = _empresaID,
                    MagatzemIDOrigen = ZonaSeleccionada.MagatzemPertanyent,
                    MagatzemIDDesti = ZonaSeleccionada.MagatzemPertanyent,
                    ZonaOrigenID = ZonaSeleccionada.ZonaEmmagatzematgeID,
                    ZonaDestiID = ZonaSeleccionada.ZonaEmmagatzematgeID,
                    Tipus = TipusMoviment.Entrada,
                    Quantitat = CantidadAIngresar,
                    Data = DateTime.UtcNow,
                    Notes = productExists ? "Entrada adicional de producto existente" : "Nueva entrada de producto"
                };

                await _dao.GuardarMovimientoInventariAsync(moviment, inventari, productExists);
                MessageBox.Show("Entrada registrada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la entrada: {ex.Message}");
            }
        }

        private bool PuedeGuardarEntrada()
        {
            return ProductoSeleccionado != null &&
            CantidadAIngresar > 0 &&
            ZonaSeleccionada != null &&
            (ZonaSeleccionada.NProductos + CantidadAIngresar) <= ZonaSeleccionada.Capacitat;
        }
    }
}
