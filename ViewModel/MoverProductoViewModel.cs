using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Collections.ObjectModel;
using System.Windows;

public partial class MoverProductoViewModel : ObservableObject
{
    private readonly IDAO dao;
    private readonly ProducteGeneral _productoSeleccionado;

    public MoverProductoViewModel(IDAO dao, ProducteGeneral producto, ZonaEmmagatzematge zonaOrigen, Magatzems magatzem, int quantitatProducte)
    {
        this.dao = dao;
        _productoSeleccionado = producto;
        AlmacenOrigen = magatzem;
        ZonaOrigen = zonaOrigen;
        NombreProducto = producto.Nom;
        QuantitatProducte = quantitatProducte;
        CodigoProducto = producto.CodiReferencia;

    }

    public async Task CargarDatosIniciales()
    {
        var almacenes = await dao.ObtenerLosAlmacenesTotalesDeLaEmpresa(ZonaOrigen.EmpresaID);
        AlmacenesDisponibles = new ObservableCollection<Magatzems>(almacenes);

        List<ZonaProductoViewModel> zonas = await dao.ObtenerZonasDeProducto(_productoSeleccionado.ProducteID);

        ZonasProducto = zonas.Select(z => new ZonaProductoViewModel
        {
            Almacen = z.Almacen,
            Zona = z.Zona,
            QuantitatDisponible = z.QuantitatDisponible
        }).ToList();

        int totalCantidad = ZonasProducto.Sum(z => z.QuantitatDisponible);
        Quantitat = totalCantidad;
    }

    [ObservableProperty]
    private int quantitat;

    [ObservableProperty]
    private List<ZonaProductoViewModel> zonasProducto;

    [ObservableProperty]
    private string nombreProducto;

    [ObservableProperty]
    private string codigoProducto;

    [ObservableProperty]
    private int quantitatProducte;

    [ObservableProperty]
    private ObservableCollection<Magatzems> almacenesDisponibles;

    partial void OnAlmacenSeleccionadoChanged(Magatzems value)
    {
        _ = CargarZonasDelAlmacenSeleccionado(value);
    }

    [ObservableProperty]
    private Magatzems almacenSeleccionado;

    [ObservableProperty]
    private Magatzems almacenOrigen;

    [ObservableProperty]
    private ZonaEmmagatzematge zonaOrigen;

    [ObservableProperty]
    private ObservableCollection<ZonaEmmagatzematge> zonasDestino;

    [ObservableProperty]
    private ZonaEmmagatzematge zonaDestinoSeleccionada;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmarMovimientoCommand))]
    private int cantidadAMover;

    private async Task CargarZonasDelAlmacenSeleccionado(Magatzems magatzemSeleccionat)
    {
        if (!string.IsNullOrEmpty(magatzemSeleccionat?.MagatzemID))
        {
            var zonas = await dao.DetallesZonasAlmacen(magatzemSeleccionat);

            // Filtrar la zona origen (por ID)
            var zonasFiltradas = zonas
                .Where(z => z.ZonaEmmagatzematgeID != ZonaOrigen?.ZonaEmmagatzematgeID)
                .ToList();

            ZonasDestino = new ObservableCollection<ZonaEmmagatzematge>(zonasFiltradas);
        }
    }


    [RelayCommand(CanExecute = nameof(PuedeConfirmarMovimiento))]
    private async Task ConfirmarMovimientoAsync()
    {
        try
        {
            DocumentSnapshot docSnap = await dao.GetProducteInventariPorID(_productoSeleccionado.ProducteID);
            ProductesInventari producteInventari = docSnap.Exists ? docSnap.ConvertTo<ProductesInventari>() : null;

            var movimiento = new MovimentsInventari
            {
                ProducteInventariID = producteInventari.IDProducteInventari,
                EmpresaIDOrigen = ZonaOrigen.EmpresaID,
                EmpresaIDDesti = ZonaDestinoSeleccionada.EmpresaID,
                MagatzemIDOrigen = ZonaOrigen.MagatzemPertanyent,
                MagatzemIDDesti = ZonaDestinoSeleccionada.MagatzemPertanyent,
                ZonaOrigenID = ZonaOrigen.ZonaEmmagatzematgeID,
                ZonaDestiID = ZonaDestinoSeleccionada.ZonaEmmagatzematgeID,
                Tipus = TipusMoviment.TrasllatIntern,
                Quantitat = CantidadAMover,
                Data = DateTime.UtcNow,
                Notes = "Movimiento interno desde interfaz"
            };

            await dao.GuardarMovimientoInventariAsync(movimiento, producteInventari, false);
            MessageBox.Show("Movimiento registrado con éxito.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Excepción al guardar el movimiento: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool PuedeConfirmarMovimiento()
    {
        return ZonaDestinoSeleccionada != null &&
               CantidadAMover > 0 &&
               ZonaDestinoSeleccionada.Capacitat >= CantidadAMover && CantidadAMover < Quantitat;
    }
}
