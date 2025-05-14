using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.VIEWMODEL
{
    public class DetalleDelProductoViewModel : INotifyPropertyChanged
    {
        public IDAO dao;
        private ProducteGeneral _productoSeleccionado;
        private string estado;
        private string descricpcion;
        private string categoria;
        private string subcategoria;
        private DetallProducte detallProducte;

        public DetalleDelProductoViewModel(IDAO dao, ProducteGeneral productoSeleccionado, string estado, string descripcion, string categoria, string subcategoria, Magatzems magatzem ,ZonaEmmagatzematge zona)
        {
            this.dao = dao;
            _productoSeleccionado = productoSeleccionado;
            this.estado = estado;
            this.categoria = categoria;
            this.subcategoria = subcategoria;
            this.descricpcion = descripcion;
            this._zona = zona;
            this._magatzem = magatzem;

        }


        // Propiedades para los datos del producto
        private string _nombreProducto;
        public string NombreProducto
        {
            get => _nombreProducto;
            set
            {
                _nombreProducto = value;
                OnPropertyChanged(nameof(NombreProducto));
            }
        }

        private double _preu;
        public double Preu
        {
            get => _preu;
            set { _preu = value; OnPropertyChanged(nameof(Preu)); }
        }

        private bool _enVenda;
        public bool EnVenda
        {
            get => _enVenda;
            set { _enVenda = value; OnPropertyChanged(nameof(EnVenda)); }
        }

        private int _quantitat;
        public int Quantitat
        {
            get => _quantitat;
            set
            {
                _quantitat = value;
                OnPropertyChanged(nameof(Quantitat));
            }
        }

        private ProducteGeneral _producto;
        public ProducteGeneral ProductoSeleccionado
        {
            get => _producto;
            set
            {
                _producto = value;
                OnPropertyChanged(nameof(ProductoSeleccionado));
            }
        }

        private Magatzems _magatzem;
        public Magatzems Magatzem
        {
            get => _magatzem;
            set
            {
                _magatzem = value;
                OnPropertyChanged(nameof(Magatzem));
            }
        }

        private ZonaEmmagatzematge _zona;
        public ZonaEmmagatzematge Zona
        {
            get => _zona;
            set
            {
                _zona = value;
                OnPropertyChanged(nameof(Zona));
            }
        }

        private string _sku;
        public string SKU
        {
            get => _sku;
            set
            {
                _sku = value;
                OnPropertyChanged(nameof(SKU));
            }
        }

        private string _estadoProducto;
        public string EstadoProducto
        {
            get => _estadoProducto;
            set
            {
                _estadoProducto = value;
                OnPropertyChanged(nameof(EstadoProducto));
            }
        }

        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                OnPropertyChanged(nameof(Descripcion));
            }
        }

        private string _categoria;
        public string Categoria
        {
            get => _categoria;
            set
            {
                _categoria = value;
                OnPropertyChanged(nameof(Categoria));
            }
        }

        private string _subcategoria;
        public string Subcategoria
        {
            get => _subcategoria;
            set
            {
                _subcategoria = value;
                OnPropertyChanged(nameof(Subcategoria));
            }
        }

        private ImageSource _imagenProducto;
        public ImageSource ImagenProducto
        {
            get => _imagenProducto;
            set
            {
                _imagenProducto = value;
                OnPropertyChanged(nameof(ImagenProducto));
            }
        }

        private List<ZonaProductoViewModel> _zonasProducto;
        public List<ZonaProductoViewModel> ZonasProducto
        {
            get => _zonasProducto;
            set
            {
                _zonasProducto = value;
                OnPropertyChanged(nameof(ZonasProducto));
            }
        }

        // Método para cargar los detalles del producto
        public async Task CargarDetallesProducto()
        {
            detallProducte = await dao.GetDetallProducteAsync(_productoSeleccionado.ProducteID);

            // Aquí se asignan los valores de la propiedad
            NombreProducto = _productoSeleccionado.Nom;
            SKU = _productoSeleccionado.SKU;
            EstadoProducto = estado.ToUpper();
            Descripcion = descricpcion;
            Categoria = categoria.ToUpper();
            Subcategoria = subcategoria;
            ProductoSeleccionado = _productoSeleccionado;
            Zona = _zona;   

            EnVenda = detallProducte.EnVenda;
            Preu = detallProducte.Preu;


            BitmapImage bitmap = dao.LoadImageFromUrl(_productoSeleccionado.ImatgeURL);
            ImagenProducto = bitmap;

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

        // Notificación de cambios de propiedad
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Clase para representar las zonas del producto (esto puede adaptarse a tu modelo de datos)
   
}
