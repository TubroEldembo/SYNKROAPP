using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.ViewModel
{
    public partial class CrearProductoEn1ZonaViewModel : ObservableObject
    {
        private readonly IDAO _dao;
        private ZonaEmmagatzematge zona;
        private Dictionary<string, List<string>> _diccionarioSubcategorias = new();
        private Dictionary<string, string> _mapCategoriaNombreToID = new();
        private Dictionary<string, string> _mapSubcategoriaNombreToID = new();

        public CrearProductoEn1ZonaViewModel(IDAO dao, ZonaEmmagatzematge zona)
        {
            _dao = dao;
            this.zona = zona;
            ListaProductos = new ObservableCollection<ProducteAmbDetall>();

            // Initialize with default
            CantidadAIngresar = 1;

            FiltrarProductos();
        }

        [ObservableProperty]
        private string nomZona;

        [ObservableProperty]
        private string nombreProducto;

        [ObservableProperty]
        private string descripcionProducto;

        [ObservableProperty]
        private string sku;

        [ObservableProperty]
        private int cantidadAIngresar;

        // Modified property with manual change notification
        private ProducteAmbDetall _productoSeleccionado;
        public ProducteAmbDetall ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                if (SetProperty(ref _productoSeleccionado, value))
                {
                    // Update dependent properties or perform actions when product changes
                    OnPropertyChanged(nameof(CanAgregarProducto));

                    // You can also update other properties based on the selected product
                    if (value != null)
                    {
                        NombreProducto = value.Producte?.Nom;
                        DescripcionProducto = value.Producte?.Descripcio;
                        Sku = value.Producte?.SKU;
                        ImagenProducto = value.ImagenProducto;
                    }
                }
            }
        }

        [ObservableProperty]
        private ObservableCollection<string> listaCategorias = new();

        [ObservableProperty]
        private string categoriaSeleccionada;

        [ObservableProperty]
        private ObservableCollection<string> listaSubcategoriasFiltradas = new();

        [ObservableProperty]
        private string subcategoriaSeleccionada;

        [ObservableProperty]
        private ObservableCollection<string> listaAlmacenes = new();

        [ObservableProperty]
        private string almacenSeleccionado;

        [ObservableProperty]
        private ObservableCollection<string> listaZonas = new();

        [ObservableProperty]
        private ZonaEmmagatzematge zonaSeleccionada;

        [ObservableProperty]
        private ImageSource imagenProducto;

        [ObservableProperty]
        private ObservableCollection<ProducteAmbDetall> listaProductos;

        [ObservableProperty]
        private ObservableCollection<ProducteAmbDetall> productosFiltrados;

        [ObservableProperty]
        private string textoBusqueda;

        partial void OnTextoBusquedaChanged(string value)
        {
            FiltrarProductos();
        }

        partial void OnCategoriaSeleccionadaChanged(string value)
        {
            // Update subcategories when category changes
            if (!string.IsNullOrEmpty(value) && _diccionarioSubcategorias.ContainsKey(value))
            {
                ListaSubcategoriasFiltradas.Clear();
                foreach (var subcategoria in _diccionarioSubcategorias[value])
                {
                    ListaSubcategoriasFiltradas.Add(subcategoria);
                }
            }
            else
            {
                ListaSubcategoriasFiltradas.Clear();
            }

            // Also filter products when category changes
            FiltrarProductos();
        }

        public async Task InicializarCamposAsync()
        {
            try
            {
                NomZona = zona.Nom;
                ListaAlmacenes.Clear();
                ListaZonas.Clear();

                ListaAlmacenes.Add(zona.MagatzemPertanyent);
                ListaZonas.Add(zona.ZonaEmmagatzematgeID);

                AlmacenSeleccionado = zona.MagatzemPertanyent;
                ZonaSeleccionada = zona;

                // Genéricas
                var (categoriasGenericas, subcategoriasGenericas) = await _dao.ObtenirCategoriesGeneriques();
                foreach (var cat in categoriasGenericas)
                {
                    ListaCategorias.Add(cat.Nom);
                    _mapCategoriaNombreToID[cat.Nom] = cat.CategoriaID;

                    if (subcategoriasGenericas.TryGetValue(cat.CategoriaID, out var subs))
                    {
                        _diccionarioSubcategorias[cat.Nom] = subs.Select(s => s.Nom).ToList();
                        foreach (var sub in subs)
                        {
                            _mapSubcategoriaNombreToID[sub.Nom] = sub.SubCategoriaID;
                        }
                    }
                }

                // Personalizadas
                var (categoriasPers, subcategoriasPers) = await _dao.ObtenirCategoriesPersonalitzades(zona.EmpresaID);
                foreach (var cat in categoriasPers)
                {
                    ListaCategorias.Add(cat.Nom);
                    _mapCategoriaNombreToID[cat.Nom] = cat.CategoriaID;

                    if (subcategoriasPers.TryGetValue(cat.CategoriaID, out var subs))
                    {
                        _diccionarioSubcategorias[cat.Nom] = subs.Select(s => s.Nom).ToList();
                        foreach (var sub in subs)
                        {
                            _mapSubcategoriaNombreToID[sub.Nom] = sub.SubCategoriaID;
                        }
                    }
                }

                await CargarDatosInicialesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar los campos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                ListaProductos.Clear();

                List<ProducteAmbDetall> productos = await _dao.GetProductosCatagalogoD1Empresa(zona.EmpresaID, false);

                foreach (ProducteAmbDetall prod in productos)
                {
                    BitmapImage bitmap = _dao.LoadImageFromUrl(prod.Producte.ImatgeURL);

                    ListaProductos.Add(new ProducteAmbDetall
                    {
                        Producte = prod.Producte,
                        Cantidad = prod.Cantidad,
                        Preu = prod.Preu,
                        ImagenProducto = bitmap,
                    });
                }

                ProductosFiltrados = new ObservableCollection<ProducteAmbDetall>(ListaProductos);

                // Select the first product if available
                if (ProductosFiltrados.Count > 0)
                {
                    ProductoSeleccionado = ProductosFiltrados[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos: " + ex.Message);
            }
        }

        private void FiltrarProductos()
        {
            if (ListaProductos == null)
                return;

            IEnumerable<ProducteAmbDetall> productos = ListaProductos;

            // Filtrar por texto de búsqueda
            if (!string.IsNullOrWhiteSpace(TextoBusqueda))
            {
                string textoBusqueda = TextoBusqueda.ToLower();
                productos = productos.Where(p =>
                    (p.Producte?.Nom?.ToLower()?.Contains(textoBusqueda) ?? false) ||
                    (p.Producte?.CodiReferencia?.ToLower()?.Contains(textoBusqueda) ?? false) ||
                    (p.Producte?.Descripcio?.ToLower()?.Contains(textoBusqueda) ?? false));
            }

            // Filtrar por categoría
            if (!string.IsNullOrWhiteSpace(CategoriaSeleccionada))
            {
                if (_mapCategoriaNombreToID.TryGetValue(CategoriaSeleccionada, out var categoriaId))
                {
                    productos = productos.Where(p => p.Producte?.CategoriaID == categoriaId).ToList();
                }
            }

            // Actualizar la colección observable de productos filtrados
            ProductosFiltrados = new ObservableCollection<ProducteAmbDetall>(productos);

            // Maintain selection if possible or select first item
            if (ProductoSeleccionado != null)
            {
                var matchingProduct = ProductosFiltrados.FirstOrDefault(p =>
                    p.Producte?.ProducteID == ProductoSeleccionado.Producte?.ProducteID);

                if (matchingProduct != null)
                {
                    ProductoSeleccionado = matchingProduct;
                }
                else if (ProductosFiltrados.Count > 0)
                {
                    ProductoSeleccionado = ProductosFiltrados[0];
                }
                else
                {
                    ProductoSeleccionado = null;
                }
            }
            else if (ProductosFiltrados.Count > 0)
            {
                ProductoSeleccionado = ProductosFiltrados[0];
            }
        }

        [RelayCommand]
        public async Task<bool> GuardarEntradaAsync()
        {
            if (!CanAgregarProducto())
            {
                MessageBox.Show("Complete los campos correctamente. Debe seleccionar un producto y especificar una cantidad válida.");
                return false;
            }

            bool inventarioAgregado = false;

            try
            {
                var zona = ZonaSeleccionada;
                string inventariID;
                bool productExists;
                ProductesInventari inventari;

                // Primero verificamos si el producto ya existe en esta zona
                inventariID = await _dao.CheckProductInZona(
                    ProductoSeleccionado.Producte.ProducteID,
                    ZonaSeleccionada.EmpresaID,
                    zona.MagatzemPertanyent,
                    zona.ZonaEmmagatzematgeID);

                productExists = !string.IsNullOrEmpty(inventariID);

                // Si no existe, creamos un nuevo registro de inventario
                if (!productExists)
                {
                    // Crear nuevo producto en inventario
                    inventari = new ProductesInventari
                    {
                        ProducteID = ProductoSeleccionado.Producte.ProducteID,
                        EmpresaID = ZonaSeleccionada.EmpresaID,
                        Quantitat = 0,
                        Estat = "Nuevo", // O el estado que corresponda
                        ZonaID = zona.ZonaEmmagatzematgeID,
                        MagatzemID = zona.MagatzemPertanyent,
                        CodiReferencia = ProductoSeleccionado.Producte.SKU
                    };

                    // Generamos un ID para el nuevo producto pero no lo guardamos aún
                    inventariID = GenerateInventoryId();
                    inventari.IDProducteInventari = inventariID;
                }
                else
                {
                    // Si existe, obtenemos el objeto actual
                    inventari = await _dao.GetProductoInventario(
                        zona.EmpresaID,
                        zona.MagatzemPertanyent,
                        zona.ZonaEmmagatzematgeID,
                        inventariID);
                }

                // Si tenemos un ID válido (existente o recién creado)
                if (!string.IsNullOrEmpty(inventariID))
                {
                    MovimentsInventari moviment = new MovimentsInventari
                    {
                        ProducteInventariID = inventariID,
                        EmpresaIDOrigen = zona.EmpresaID,
                        EmpresaIDDesti = zona.EmpresaID,
                        MagatzemIDOrigen = zona.MagatzemPertanyent,
                        MagatzemIDDesti = zona.MagatzemPertanyent,
                        ZonaOrigenID = zona.ZonaEmmagatzematgeID,
                        ZonaDestiID = zona.ZonaEmmagatzematgeID,
                        Tipus = TipusMoviment.Entrada,
                        Quantitat = CantidadAIngresar,
                        Data = DateTime.UtcNow,
                        Notes = "Ingreso desde el registro del producto"
                    };

                    await _dao.GuardarMovimientoInventariAsync(moviment, inventari, productExists);

                    inventarioAgregado = true;

                    MessageBox.Show($"Producto: {ProductoSeleccionado.Producte.Nom} ingresado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el producto: {ex.Message}");
            }

            return inventarioAgregado;
        }

        public bool CanAgregarProducto()
        {
            ProductoSeleccionado.Cantidad = CantidadAIngresar;
            return ProductoSeleccionado != null &&
                   CantidadAIngresar > 0 &&
                   (ZonaSeleccionada?.Capacitat == null ||
                    ProductoSeleccionado.Cantidad < ZonaSeleccionada.Capacitat);
        }

        private string GenerateInventoryId() => Guid.NewGuid().ToString();

        [RelayCommand]
        public void SelectProduct(ProducteAmbDetall product)
        {
            if (product != null)
            {
                ProductoSeleccionado = product;
            }
        }
    }
}