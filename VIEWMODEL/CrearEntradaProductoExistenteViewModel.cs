using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.VIEWMODEL
{
    public class CrearEntradaProductoExistenteViewModel : INotifyPropertyChanged
    {
        private readonly IDAO _dao;
        private readonly ProducteAmbDetall _producto;
        private readonly string _empresaID;

        public CrearEntradaProductoExistenteViewModel(IDAO dao, ProducteAmbDetall producto, string empresaID)
        {
            _dao = dao;
            _producto = producto;
            _empresaID = empresaID;

            // Asegúrate de que la imagen se carga y se asigna correctamente
            BitmapImage bt = dao.LoadImageFromUrl(_producto.Producte.ImatgeURL);
            ImagenProducto = bt;

            // Asigna el producto al ProductoSeleccionado y asegúrate de que tenga imagen
            _producto.ImagenProducto = bt; // Esto es lo que faltaba
            ProductoSeleccionado = _producto;

        }

        // ✅ NUEVO: propiedad pública para el binding
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



        private int _cantidadAIngresar;
        public int CantidadAIngresar
        {
            get => _cantidadAIngresar;
            set { _cantidadAIngresar = value; OnPropertyChanged(nameof(CantidadAIngresar)); }
        }

        public ObservableCollection<Magatzems> ListaAlmacenes { get; set; } = new ObservableCollection<Magatzems>();

        private Magatzems _almacenSeleccionado;
        public Magatzems AlmacenSeleccionado
        {
            get => _almacenSeleccionado;
            set
            {
                _almacenSeleccionado = value;
                OnPropertyChanged(nameof(AlmacenSeleccionado));
                _ = CargarZonasPorAlmacenAsync();
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

        public ObservableCollection<ZonaEmmagatzematge> ListaZonas { get; set; } = new ObservableCollection<ZonaEmmagatzematge>();

        private ZonaEmmagatzematge _zonaSeleccionada;
        public ZonaEmmagatzematge ZonaSeleccionada
        {
            get => _zonaSeleccionada;
            set { _zonaSeleccionada = value; OnPropertyChanged(nameof(ZonaSeleccionada)); }
        }

        public ObservableCollection<string> ListaEstados { get; set; } = new ObservableCollection<string> { "Nuevo", "Seminuevo", "Desconocido" };

        private string _estadoSeleccionado;
        public string EstadoSeleccionado
        {
            get => _estadoSeleccionado;
            set { _estadoSeleccionado = value; OnPropertyChanged(nameof(EstadoSeleccionado)); }
        }

        public async Task InicializarAsync()
        {
            var almacenes = await _dao.ObtenerLosAlmacenesTotalesDeLaEmpresa(_empresaID);
            ListaAlmacenes.Clear();
            foreach (var alm in almacenes)
                ListaAlmacenes.Add(alm);

            ListaZonas.Clear();
            foreach (var alm in almacenes)
            {
                var zonas = await _dao.DetallesZonasAlmacen(alm);
                foreach (var zona in zonas)
                    ListaZonas.Add(zona);
            }
        }

        private async Task CargarZonasPorAlmacenAsync()
        {
            if (AlmacenSeleccionado == null) return;

            var zonas = await _dao.DetallesZonasAlmacen(AlmacenSeleccionado);
            ListaZonas.Clear();
            foreach (var zona in zonas)
                ListaZonas.Add(zona);
        }

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
                ZonaEmmagatzematge zona = ZonaSeleccionada;
                string inventariID;
                bool productExists;
                ProductesInventari inventari;

                // Primero verificamos si el producto ya existe en esta zona
                inventariID = await _dao.CheckProductInZona(
                    _producto.Producte.ProducteID,
                    _empresaID,
                    zona.MagatzemPertanyent,
                    zona.ZonaEmmagatzematgeID);

                productExists = !string.IsNullOrEmpty(inventariID);

                // Si no existe, creamos un nuevo registro de inventario
                if (!productExists)
                {
                    // Solo preparamos el objeto - NO guardamos en base de datos todavía
                    inventari = new ProductesInventari
                    {
                        ProducteID = _producto.Producte.ProducteID,
                        EmpresaID = _empresaID,
                        Quantitat = 0, // Inicializamos en 0, el movimiento de inventario actualizará la cantidad
                        Estat = "Nuevo",
                        ZonaID = zona.ZonaEmmagatzematgeID,
                        MagatzemID = zona.MagatzemPertanyent,
                        CodiReferencia = _producto.Producte.SKU
                    };

                    // Generamos un ID para el nuevo producto pero no lo guardamos aún
                    inventariID = GenerateInventoryId();
                    inventari.IDProducteInventari = inventariID;
                }
                else
                {
                    inventari = await _dao.GetProductoInventario(
                       _empresaID,
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
                        EmpresaIDOrigen = _empresaID,
                        EmpresaIDDesti = _empresaID,
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
