using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

public partial class AlmacenesViewModel : ObservableObject
{
    private readonly IDAO _dao;
    private readonly Empreses _empresa;
    private ObservableCollection<MovimentsInventari> _todosLosMovimientos;

    public AlmacenesViewModel(IDAO dao, Empreses empresa)
    {
        _dao = dao;
        _empresa = empresa;
        Almacenes = new List<Magatzems>();
        _ = CargarMovimientosAsync();
    }

    [ObservableProperty]
    private ObservableCollection<MovimentsInventari> movimientosFiltrados = new();

    [ObservableProperty]
    private TipusMoviment? tipoMovimientoSeleccionado;

    partial void OnTipoMovimientoSeleccionadoChanged(TipusMoviment? value)
    {
        AplicarFiltros();
    }

    [ObservableProperty]
    private string totalAlmacenes;

    [ObservableProperty]
    private int movimientosRecientes;

    [ObservableProperty]
    private string productosEnAlmacenes;

    [ObservableProperty]
    private int numeroDeZonas;

    [ObservableProperty]
    private List<Magatzems> almacenes;

    [ObservableProperty]
    private Magatzems? almacenSeleccionado;


    public Action? AbrirPantallaCrearAlmacen { get; set; }
    public Action<Magatzems>? AbrirPantallaZonasAlmacen { get; set; }

    [RelayCommand]
    private void CrearAlmacen()
    {
        AbrirPantallaCrearAlmacen?.Invoke();
    }

    partial void OnAlmacenSeleccionadoChanged(Magatzems? value)
    {
        if (value != null)
        {
            AbrirPantallaZonasAlmacen?.Invoke(value);
        }
    }


    public async Task CargarMovimientosAsync()
    {
        var movimientos = await _dao.ObtenerMovimientosInventarioPorEmpresa(_empresa.EmpresaID);
        _todosLosMovimientos = new ObservableCollection<MovimentsInventari>(movimientos);
        MovimientosRecientes = _todosLosMovimientos.Count;

        AplicarFiltros();
    }

    public void AplicarFiltros()
    {
        if (_todosLosMovimientos == null)
            return;

        var filtrados = _todosLosMovimientos.AsEnumerable();

        if (TipoMovimientoSeleccionado.HasValue)
        {
            filtrados = filtrados.Where(m => m.Tipus == TipoMovimientoSeleccionado.Value);
        }

        MovimientosFiltrados = new ObservableCollection<MovimentsInventari>(filtrados);
    }

    public async Task CargarDetallesAlmacenes()
    {
        var almacenes = await _dao.DetallesAlmacenes(_empresa);

        if (almacenes != null)
        {
            Almacenes = almacenes;
            TotalAlmacenes = almacenes.Count.ToString();
            NumeroDeZonas = almacenes.Sum(a => a.Zones?.Count ?? 0);
        }
    }
}
