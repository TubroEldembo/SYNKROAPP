using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.VIEWMODEL
{
    public partial class AgregarProductoACatalogoVM : ObservableObject
    {
        private readonly IDAO _dao;
        private readonly string _empresaID;
        private DetallProducte detallProducte;

        public AgregarProductoACatalogoVM(IDAO dao, string empresaID)
        {
            _dao = dao;
            _empresaID = empresaID;

            ProductosFiltrados = new ObservableCollection<ProducteAmbDetall>();
            ListaCategorias = new ObservableCollection<string>();

            _ = InicializarAsync();
        }

        [ObservableProperty]
        private ObservableCollection<ProducteAmbDetall> productosFiltrados;

        [ObservableProperty]
        private ObservableCollection<string> listaCategorias;

        [ObservableProperty]
        private ProducteAmbDetall? productoSeleccionado;

        partial void OnProductoSeleccionadoChanged(ProducteAmbDetall? value)
        {
            if (value != null)
            {
                try
                {
                    BitmapImage bt = _dao.LoadImageFromUrl(value.Producte.ImatgeURL);
                    ImagenProducto = bt;

                    // Cargar detallProducte y actualizar estado del comando
                    _ = CargarDetallYActualizarComandoAsync(value.Producte.ProducteID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la imagen: {ex.Message}");
                }
            }
            else
            {
                ImagenProducto = null;
                detallProducte = null;
                SeleccionarProductoCommand.NotifyCanExecuteChanged();
            }
        }

        private async Task CargarDetallYActualizarComandoAsync(string producteId)
        {
            detallProducte = await _dao.GetDetallProducteAsync(producteId);
            SeleccionarProductoCommand.NotifyCanExecuteChanged();
        }

        [ObservableProperty]
        private ImageSource? imagenProducto;

        [ObservableProperty]
        private string? busquedaProducto;

        partial void OnBusquedaProductoChanged(string? value)
        {
            _ = FiltrarProductosAsync();
        }

        [ObservableProperty]
        private string? categoriaSeleccionada;

        partial void OnCategoriaSeleccionadaChanged(string? value)
        {
            _ = FiltrarProductosAsync();
        }

        public async Task InicializarAsync()
        {
            try
            {
                var productos = await _dao.GetProductosCatagalogoD1Empresa(_empresaID, false);

                var categorias = productos.Select(p => p.Producte.CategoriaID).Distinct().ToList();

                ListaCategorias.Clear();
                ListaCategorias.Add("Todas");
                foreach (var categoria in categorias)
                {
                    if (!string.IsNullOrEmpty(categoria))
                        ListaCategorias.Add(categoria);
                }

                CategoriaSeleccionada = "Todas";
                await FiltrarProductosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos iniciales: {ex.Message}");
            }
        }

        private async Task FiltrarProductosAsync()
        {
            try
            {
                var productos = await _dao.GetProductosCatagalogoD1Empresa(_empresaID, false);

                if (!string.IsNullOrWhiteSpace(BusquedaProducto))
                {
                    string busqueda = BusquedaProducto.ToLower();
                    productos = productos.Where(p =>
                        p.Producte.Nom.ToLower().Contains(busqueda) ||
                        p.Producte.SKU.ToLower().Contains(busqueda) ||
                        (p.Producte.CategoriaID != null && p.Producte.CategoriaID.ToLower().Contains(busqueda))
                    ).ToList();
                }

                if (!string.IsNullOrEmpty(CategoriaSeleccionada) && CategoriaSeleccionada != "Todas")
                {
                    productos = productos.Where(p => p.Producte.CategoriaID == CategoriaSeleccionada).ToList();
                }

                ProductosFiltrados.Clear();
                foreach (var producto in productos)
                {
                    ProductosFiltrados.Add(producto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar los productos: {ex.Message}");
            }
        }

        [RelayCommand(CanExecute = nameof(PuedeSeleccionarProducto))]
        public async void SeleccionarProducto()
        {
            if (ProductoSeleccionado == null) return;

            await _dao.UpdateProduct(ProductoSeleccionado.Producte, 1, true);

            MessageBox.Show("Producto agregado al catálogo.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool PuedeSeleccionarProducto()
        {
            return ProductoSeleccionado != null && (detallProducte == null || detallProducte.EnVenda != true);
        }
    }
}
