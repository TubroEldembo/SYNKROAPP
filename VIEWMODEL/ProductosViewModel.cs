using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.Vistes.Vista_Productos;
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

namespace SYNKROAPP.VIEWMODEL
{
    public class ProductosViewModel: INotifyPropertyChanged
    {
        private IDAO _dao;
        private Dictionary<string, List<string>> _diccionarioSubcategorias = new();
        private Dictionary<string, string> _mapCategoriaNombreToID = new();
        private Dictionary<string, string> _mapSubcategoriaNombreToID = new();
        private Empreses _empresa;

        public ProductosViewModel(IDAO dao, Empreses empresa) 
        {
            _dao = dao;
            _empresa = empresa;
            ListaProductos = new ObservableCollection<ProducteAmbDetall>();
            ListaCategorias = new ObservableCollection<string>();
            CargarDatosIniciales();
        }

        #region Propiedades del formulario

        private string _precioSeleccionado;
        public string PrecioSeleccionado
        {
            get => _precioSeleccionado;
            set
            {
                _precioSeleccionado = value;
                OnPropertyChanged(nameof(PrecioSeleccionado));
                FiltrarProductos();
            }
        }

        private string _codigoProducto;
        public string CodigoProducto
        {
            get => _codigoProducto;
            set { _codigoProducto = value; OnPropertyChanged(nameof(CodigoProducto)); }
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

        private bool _estaEnVenta;
        public bool EstaEnVenta
        {
            get => _estaEnVenta;
            set { _estaEnVenta = value; OnPropertyChanged(nameof(EstaEnVenta)); }
        }

        private double _precioProducto;
        public double PrecioProducto
        {
            get => _precioProducto;
            set { _precioProducto = value; OnPropertyChanged(nameof(PrecioProducto)); }
        }


        private ImageSource _imagenProducto;
        public ImageSource ImagenProducto
        {
            get => _imagenProducto;
            set { _imagenProducto = value; OnPropertyChanged(nameof(ImagenProducto)); }
        }

        private string _rutaImagen;
        public string RutaImagen
        {
            get => _rutaImagen;
            set { _rutaImagen = value; OnPropertyChanged(nameof(RutaImagen)); }
        }

        private bool _tieneImagen;
        public bool TieneImagen
        {
            get => _tieneImagen;
            set { _tieneImagen = value; OnPropertyChanged(nameof(TieneImagen)); }
        }

        private string _categoriaSeleccionada;
        public string CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set 
            { 
                _categoriaSeleccionada = value; 
                OnPropertyChanged(nameof(CategoriaSeleccionada)); FiltrarProductos(); 
            }
        }

        private string _subcategoriaSeleccionada;
        public string SubcategoriaSeleccionada
        {
            get => _subcategoriaSeleccionada;
            set { _subcategoriaSeleccionada = value; OnPropertyChanged(nameof(SubcategoriaSeleccionada)); }
        }

        public ObservableCollection<string> ListaCategorias { get; set; }

        #endregion


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


        private async void CargarDatosIniciales()
        {
            try
            {
                CategoriaSeleccionada = "Todas las categorías";
                PrecioSeleccionado = "Todos los precios";

                ListaCategorias.Clear();
                ListaCategorias.Add("Todas las categorías"); // ✅ Añadir al principio

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

                List<ProducteAmbDetall> productos = await _dao.GetProductosCatagalogoD1Empresa(_empresa.EmpresaID, false);

                foreach (ProducteAmbDetall prod in productos)
                {
                    ListaProductos.Add(new ProducteAmbDetall
                    {
                        Producte = prod.Producte,
                        Cantidad = prod.Cantidad,
                        Preu = prod.Preu,
                    });
                }

                ProductosFiltrados = new ObservableCollection<ProducteAmbDetall>(ListaProductos);

                CategoriaSeleccionada = "Todas las categorías"; // ✅ Valor por defecto

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

            // Filtrar por rango de precio
            if (!string.IsNullOrWhiteSpace(PrecioSeleccionado) && PrecioSeleccionado != "Todos los precios")
            {
                switch (PrecioSeleccionado)
                {
                    case "Menor a $100":
                        productos = productos.Where(p => p.Preu < 100);
                        break;
                    case "$100 - $500":
                        productos = productos.Where(p => p.Preu >= 100 && p.Preu <= 500);
                        break;
                    case "$500 - $1000":
                        productos = productos.Where(p => p.Preu > 500 && p.Preu <= 1000);
                        break;
                    case "Mayor a $1000":
                        productos = productos.Where(p => p.Preu > 1000);
                        break;
                }
            }

            // Actualizar la colección observable de productos filtrados
            ProductosFiltrados = new ObservableCollection<ProducteAmbDetall>(productos);
        }
        #endregion


        #region Métodos públicos

        public void SeleccionarImagen()
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
                    RutaImagen = dlg.FileName;
                    TieneImagen = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async void GuardarProducto()
        {
            if (string.IsNullOrWhiteSpace(NombreProducto) || string.IsNullOrWhiteSpace(CodigoProducto))
            {
                MessageBox.Show("Nombre y Código son obligatorios.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string urlImagen = null;
            if (!string.IsNullOrWhiteSpace(RutaImagen) && File.Exists(RutaImagen))
            {
                string nombreArchivo = Path.GetFileName(RutaImagen);
                string nombreFirebase = $"{CodigoProducto}_{NombreProducto}.jpg";
                urlImagen = await _dao.StoreImage(RutaImagen, nombreFirebase);
            }


            ProducteGeneral nuevo = new ProducteGeneral
            {
                ProducteID = Guid.NewGuid().ToString(), // ID aleatorio
                Nom = NombreProducto,
                CodiReferencia = CodigoProducto,
                Descripcio = DescripcionProducto,
                CategoriaID = CategoriaSeleccionada,
                SubCategoriaID = SubcategoriaSeleccionada,
                ImatgeURL = urlImagen,
                SKU = CodigoProducto,
            };

            DetallProducte detall = new DetallProducte
            {
                Preu = PrecioProducto,
                EnVenda = EstaEnVenta,
                EmpresaID = _empresa.EmpresaID
            };

            ProducteAmbDetall productoConDetalle = new ProducteAmbDetall
            {
                Producte = nuevo,
            };

            await _dao.AddProductoGeneral(nuevo, detall);

            ListaProductos.Add(productoConDetalle);
            FiltrarProductos();

            MessageBox.Show("Producto guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
