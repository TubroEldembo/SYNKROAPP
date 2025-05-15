using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.GOOGLE_PLACES_SERVICE;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

public partial class CrearAlmacenViewModel : ObservableObject
{
    private readonly IDAO _dao;
    private readonly Empreses _empresa;
    GooglePlacesService service = new GooglePlacesService();

    public CrearAlmacenViewModel(IDAO dao, Empreses empresa)
    {
        _dao = dao;
        _empresa = empresa;

        EmpresaSeleccionada = _empresa;

        // Configurar valores por defecto
        UbicacionNueva = true;
    }

    #region Propiedades

    [ObservableProperty]
    private string? nombreAlmacen;

    [ObservableProperty]
    private int capacidadTotal;

    [ObservableProperty]
    private Empreses? empresaSeleccionada;

    [ObservableProperty]
    private string? descripcion;

    [ObservableProperty]
    private bool ubicacionNueva = true;

    [ObservableProperty]
    private bool ubicacionEmpresa;

    [ObservableProperty]
    private string? textoBusqueda;

    [ObservableProperty]
    private ObservableCollection<string>? sugerenciasLocalizacion;

    [ObservableProperty]
    private string? sugerenciaSeleccionada;

    [ObservableProperty]
    private string? calle;

    [ObservableProperty]
    private string? numero;

    [ObservableProperty]
    private string? ciudad;

    [ObservableProperty]
    private string? codigoPostal;

    [ObservableProperty]
    private string? provincia;

    [ObservableProperty]
    private string? pais;

    [ObservableProperty]
    private double latitud;

    [ObservableProperty]
    private double longitud;

    // Propiedades para mostrar la dirección de la empresa
    [ObservableProperty]
    private string? direccionEmpresa;

    [ObservableProperty]
    private string? ciudadEmpresa;

    [ObservableProperty]
    private string? cpEmpresa;

    [ObservableProperty]
    private string? paisEmpresa;

    public ObservableCollection<Empreses> ListaEmpresas { get; } = new();

    #endregion

    #region Observadores de cambios en propiedades
    partial void OnNombreAlmacenChanged(string? value)
    {
        GuardarCommand.NotifyCanExecuteChanged();
        GuardarConfigCommand.NotifyCanExecuteChanged();
    }

    partial void OnCapacidadTotalChanged(int value)
    {
        GuardarCommand.NotifyCanExecuteChanged();
        GuardarConfigCommand.NotifyCanExecuteChanged();
    }

    partial void OnUbicacionNuevaChanged(bool value)
    {
        UbicacionEmpresa = !value;
        GuardarCommand.NotifyCanExecuteChanged();
        GuardarConfigCommand.NotifyCanExecuteChanged();
    }

    partial void OnUbicacionEmpresaChanged(bool value)
    {
        UbicacionNueva = !value;
        GuardarCommand.NotifyCanExecuteChanged();
        GuardarConfigCommand.NotifyCanExecuteChanged();
    }

    partial void OnSugerenciaSeleccionadaChanged(string? value)
    {
        //if (!string.IsNullOrEmpty(value))
        //{
        //    _ = ObtenerDetallesDireccionAsync(value);
        //}
    }

    #endregion

    #region Métodos

    public async Task BuscarUbicacionAsync()
    {
        if (string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            SugerenciasLocalizacion = null;
            return;
        }

        try
        {
            // Simulamos una llamada a la API de Google Places
            //var sugerencias = await (TextoBusqueda);
            //SugerenciasLocalizacion = new ObservableCollection<string>(sugerencias);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al buscar ubicaciones: {ex.Message}");
        }
    }

    //private async Task ObtenerDetallesDireccionAsync(string sugerencia)
    //{
    //    try
    //    {
    //        // Simulamos una llamada para obtener detalles de la dirección seleccionada
    //        var detallesDireccion = await service.AutocompletarCiudadAsync(sugerencia);

    //        // Actualizar propiedades con la información obtenida
    //        Calle = detallesDireccion.Calle;
    //        Numero = detallesDireccion.Numero;
    //        Ciudad = detallesDireccion.Ciudad;
    //        CodigoPostal = detallesDireccion.CodigoPostal;
    //        Provincia = detallesDireccion.Provincia;
    //        Pais = detallesDireccion.Pais;
    //        Latitud = detallesDireccion.Latitud;
    //        Longitud = detallesDireccion.Longitud;

    //        // Ocultar sugerencias después de seleccionar
    //        SugerenciasLocalizacion = null;
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show($"Error al obtener detalles de la dirección: {ex.Message}");
    //    }
    //}

    private bool PuedeGuardar()
    {
        // Validar campos básicos requeridos
        if (string.IsNullOrWhiteSpace(NombreAlmacen) ||
            CapacidadTotal <= 0 ||
            EmpresaSeleccionada == null)
            return false;

        // Validar campos de ubicación
        if (UbicacionNueva)
        {
            return !string.IsNullOrWhiteSpace(Calle) &&
                   !string.IsNullOrWhiteSpace(Ciudad) &&
                   !string.IsNullOrWhiteSpace(CodigoPostal) &&
                   !string.IsNullOrWhiteSpace(Provincia) &&
                   !string.IsNullOrWhiteSpace(Pais);
        }

        // Si usa la ubicación de la empresa, no se necesitan más validaciones
        return true;
    }

    private async Task<string> GuardarAlmacenAsync()
    {
        try
        {
            // Crear objeto de almacén
            Magatzems almacen = new Magatzems
            {
                MagatzemID = Guid.NewGuid().ToString(),
                NomMagatzem = NombreAlmacen,
                EmpresaPertanyentID = EmpresaSeleccionada?.EmpresaID,
                //Ubicacio = EmpresaSe
            };

            // Crear objeto de dirección
            Adreça ubicacion = new Adreça
            {
                //DireccionID = Guid.NewGuid().ToString(),
                //Calle = UbicacionNueva ? Calle : DireccionEmpresa,
                //Numero = UbicacionNueva ? Numero : "",
                //Ciudad = UbicacionNueva ? Ciudad : CiudadEmpresa,
                //CodigoPostal = UbicacionNueva ? CodigoPostal : CpEmpresa,
                //Provincia = UbicacionNueva ? Provincia : "",
                //Pais = UbicacionNueva ? Pais : PaisEmpresa,
                //Latitud = UbicacionNueva ? Latitud : 0,
                //Longitud = UbicacionNueva ? Longitud : 0
            };

            // Guardar en la base de datos
            //await _dao.GuardarAlmacenAsync(almacen, ubicacion);

            return almacen.MagatzemID;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar el almacén: {ex.Message}");
            return null;
        }
    }

    #endregion

    #region Comandos

    [RelayCommand]
    private async Task TextoBusquedaCambiadoAsync()
    {
        await BuscarUbicacionAsync();
    }

    [RelayCommand]
    private void CambiarAUbicacionNueva()
    {
        UbicacionNueva = true;
    }

    [RelayCommand]
    private void CambiarAUbicacionEmpresa()
    {
        UbicacionEmpresa = true;
    }

    [RelayCommand(CanExecute = nameof(PuedeGuardar))]
    private async Task GuardarAsync()
    {
        var almacenID = await GuardarAlmacenAsync();
        if (!string.IsNullOrEmpty(almacenID))
        {
            MessageBox.Show("Almacén guardado correctamente.");
            CerrarDialogo?.Invoke(this, new DialogoResult { Resultado = true, AlmacenID = almacenID });
        }
    }

    [RelayCommand(CanExecute = nameof(PuedeGuardar))]
    private async Task GuardarConfigAsync()
    {
        var almacenID = await GuardarAlmacenAsync();
        if (!string.IsNullOrEmpty(almacenID))
        {
            MessageBox.Show("Almacén guardado correctamente. Configurando zonas...");
            CerrarDialogo?.Invoke(this, new DialogoResult
            {
                Resultado = true,
                AlmacenID = almacenID,
                ConfigurarZonas = true
            });
        }
    }

    [RelayCommand]
    private void Cancelar()
    {
        CerrarDialogo?.Invoke(this, new DialogoResult { Resultado = false });
    }

    #endregion

    // Evento para cerrar el diálogo y pasar información
    public event EventHandler<DialogoResult> CerrarDialogo;

    // Clase para devolver información cuando se cierra el diálogo
    public class DialogoResult
    {
        public bool Resultado { get; set; }
        public string AlmacenID { get; set; }
        public bool ConfigurarZonas { get; set; }
    }
}
