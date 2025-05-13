using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.ViewModel
{
    public class CrearProductoEn1ZonaViewModel : INotifyPropertyChanged
    {
        public IDAO _dao;
        private ZonaEmmagatzematge zona;
        private Dictionary<string, List<string>> _diccionarioSubcategorias = new();
        private Dictionary<string, string> _mapCategoriaNombreToID = new();
        private Dictionary<string, string> _mapSubcategoriaNombreToID = new();

        public CrearProductoEn1ZonaViewModel(IDAO dao, ZonaEmmagatzematge zona)
        {
            this._dao = dao;
            this.zona = zona;
            ListaProductos = new ObservableCollection<ProducteAmbDetall>();
        }

        private string _nomZona;
        public string NomZona
        {
            get => _nomZona;
            set { _nomZona = value; OnPropertyChanged(nameof(NomZona)); }
        }

        private string _nombreProducto;
        public string NombreProducto
        {
            get => _nombreProducto;
            set { _nombreProducto = value; OnPropertyChanged(nameof(NombreProducto)); }
        }

        private string _descripcionProducto;
        public string DescripcionProducto
        {
            get => _descripcionProducto;
            set { _descripcionProducto = value; OnPropertyChanged(nameof(DescripcionProducto)); }
        }

        private string _sku;
        public string SKU
        {
            get => _sku;
            set { _sku = value; OnPropertyChanged(nameof(SKU)); }
        }

        private int _cantidadInicial;
        public int CantidadAIngresar
        {
            get => _cantidadInicial;
            set { _cantidadInicial = value; OnPropertyChanged(nameof(CantidadAIngresar)); }
        }

        private ProducteAmbDetall _productoSeleccionado;
        public ProducteAmbDetall ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                OnPropertyChanged(nameof(ProductoSeleccionado));
            }
        }


        public ObservableCollection<string> ListaCategorias { get; set; } = new ObservableCollection<string>();
        private string _categoriaSeleccionada;
        public string CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                OnPropertyChanged(nameof(CategoriaSeleccionada)); FiltrarSubcategorias(); FiltrarProductos();
            }
        }

        private void FiltrarSubcategorias()
        {
            ListaSubcategoriasFiltradas.Clear();
            if (!string.IsNullOrEmpty(CategoriaSeleccionada) && _diccionarioSubcategorias.TryGetValue(CategoriaSeleccionada, out var subcategorias))
            {
                foreach (var sub in subcategorias)
                {
                    ListaSubcategoriasFiltradas.Add(sub);
                }
            }
        }

        public ObservableCollection<string> ListaSubcategoriasFiltradas { get; set; } = new();

        private string _subcategoriaSeleccionada;
        public string SubcategoriaSeleccionada
        {
            get => _subcategoriaSeleccionada;
            set { _subcategoriaSeleccionada = value; OnPropertyChanged(nameof(SubcategoriaSeleccionada)); }
        }

        public ObservableCollection<string> ListaAlmacenes { get; set; } = new ObservableCollection<string>();
        private string _almacenSeleccionado;
        public string AlmacenSeleccionado
        {
            get => _almacenSeleccionado;
            set { _almacenSeleccionado = value; OnPropertyChanged(nameof(AlmacenSeleccionado)); }
        }

        public ObservableCollection<string> ListaZonas { get; set; } = new ObservableCollection<string>();
        private ZonaEmmagatzematge _zonaSeleccionada;
        public ZonaEmmagatzematge ZonaSeleccionada
        {
            get => _zonaSeleccionada;
            set
            {
                _zonaSeleccionada = value;
                OnPropertyChanged(nameof(ZonaSeleccionada));
            }
        }


        private ImageSource _imagenProducto;
        public ImageSource ImagenProducto
        {
            get => _imagenProducto;
            set { _imagenProducto = value; OnPropertyChanged(nameof(ImagenProducto)); }
        }


        #region Productos

        public ObservableCollection<ProducteAmbDetall> ListaProductos { get; set; }

        private ObservableCollection<ProducteAmbDetall> _productosFiltrados;
        public ObservableCollection<ProducteAmbDetall> ProductosFiltrados
        {
            get => _productosFiltrados;
            set { _productosFiltrados = value; OnPropertyChanged(nameof(ProductosFiltrados)); }
        }

        private string _textoBusqueda;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; OnPropertyChanged(nameof(TextoBusqueda)); FiltrarProductos(); }
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

                CargarDatosIniciales();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar los campos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void CargarDatosIniciales()
        {
            try
            {
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

                List<ProducteAmbDetall> productos = await _dao.GetProductosCatagalogoD1Empresa(zona.EmpresaID, false); // deberás implementar este método

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
                    productos = productos.Where(p => p.Producte?.CategoriaID == CategoriaSeleccionada).ToList();
                }

            }

            // Actualizar la colección observable de productos filtrados
            ProductosFiltrados = new ObservableCollection<ProducteAmbDetall>(productos);
        }

        #endregion

        public async Task<bool> GuardarEntradaAsync()
        {
            bool inventarioAgregado = false;
            if (CantidadAIngresar <= 0 || string.IsNullOrWhiteSpace(ZonaSeleccionada?.ZonaEmmagatzematgeID))
            {
                MessageBox.Show("Complete los campos correctamente.");
                return false;
            }

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
                        Notes = productExists ? "Entrada adicional de producto existente" : "Nueva entrada de producto"
                    };

                    // La transacción creará o actualizará el producto según corresponda
                    await _dao.GuardarMovimientoInventariAsync(moviment, inventari, productExists);
                    inventarioAgregado = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la entrada: {ex.Message}");
            }

            return inventarioAgregado;
        }

        private string GenerateInventoryId()
        {
            // Genera un ID único para el inventario (similar a lo que hace Firestore)
            return Guid.NewGuid().ToString();
        }

        public void GenerarSKU()
        {
            SKU = $"SKU-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
