using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public partial class EditarProductoViewModel : ObservableObject
{
    private readonly IDAO _dao;
    private readonly ProducteGeneral _productoOriginal;
    private DetallProducte detallProducte;

    public EditarProductoViewModel(IDAO dao, ProducteGeneral producto)
    {
        _dao = dao;
        _productoOriginal = producto;
    }

    public async Task CargarDetallProducte()
    {
        try
        {
            detallProducte = await _dao.GetDetallProducteAsync(_productoOriginal.ProducteID);

            Nom = _productoOriginal.Nom;
            Descripcio = _productoOriginal.Descripcio;
            Sku = _productoOriginal.SKU;
            Categoria = _productoOriginal.CategoriaID;
            Subcategoria = _productoOriginal.SubCategoriaID;
            Preu = detallProducte.Preu;
            EnVenda = detallProducte.EnVenda;

            ImagenProducto = _dao.LoadImageFromUrl(_productoOriginal.ImatgeURL);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error cargando detalle del producto: {ex.Message}");
        }
    }

    // PROPIEDADES con reevaluación automática de comandos
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmarEdicionCommand))]
    private string nom;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmarEdicionCommand))]
    private string descripcio;

    [ObservableProperty]
    private string sku;

    [ObservableProperty]
    private string categoria;

    [ObservableProperty]
    private string subcategoria;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmarEdicionCommand))]
    private double preu;

    [ObservableProperty]
    private bool enVenda;

    [ObservableProperty]
    private ImageSource imagenProducto;

    // COMANDOS
    [RelayCommand(CanExecute = nameof(PuedeConfirmarEdicion))]
    private async Task ConfirmarEdicion()
    {
        try
        {
            _productoOriginal.Nom = Nom;
            _productoOriginal.Descripcio = Descripcio;
            _productoOriginal.SKU = Sku;
            _productoOriginal.CategoriaID = Categoria;
            _productoOriginal.SubCategoriaID = Subcategoria;

            await _dao.UpdateProduct(_productoOriginal, Preu, EnVenda);

            MessageBox.Show("Producto actualizado correctamente.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar el producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool PuedeConfirmarEdicion()
    {
        return !string.IsNullOrWhiteSpace(Nom) &&
               !string.IsNullOrWhiteSpace(Descripcio) &&
               Preu > 0;
    }

    [RelayCommand]
    private async Task SeleccionarImagenAsync()
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp",
            Title = "Seleccionar imagen"
        };

        if (dlg.ShowDialog() == true)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dlg.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                ImagenProducto = bitmap;

                // SUBIR IMAGEN Y GUARDAR URL
                string nuevaUrl = await _dao.StoreImage(dlg.FileName, $"imagen{_productoOriginal.Nom}Actualizada.png");  // ← Debes implementar esto en tu IDAO
                _productoOriginal.ImatgeURL = nuevaUrl;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


    [RelayCommand]
    private void EliminarImagen()
    {
        ImagenProducto = null;
        _productoOriginal.ImatgeURL = string.Empty;
    }
}
