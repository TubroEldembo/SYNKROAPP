using GalaSoft.MvvmLight.Command;
using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SYNKROAPP.ViewModel
{
    public class MoverProductoViewModel : INotifyPropertyChanged
    {
        private IDAO dao;
        private ProducteGeneral _productoSeleccionado;

        public MoverProductoViewModel(IDAO dao, ProducteGeneral producto, ZonaEmmagatzematge zonaOrigen, Magatzems magatzem)
        {
            this.dao = dao;
            _productoSeleccionado = producto;
            AlmacenOrigen = magatzem;
            ZonaOrigen = zonaOrigen;
            NombreProducto = producto.Nom;

            ConfirmarMovimientoCommand = new RelayCommand(ConfirmarMovimiento, PuedeConfirmarMovimiento);
        }

        public async Task CargarDatosIniciales()
        {
            var almacenes = await dao.ObtenerLosAlmacenesTotalesDeLaEmpresa(ZonaOrigen.EmpresaID);
            AlmacenesDisponibles = new ObservableCollection<Magatzems>(almacenes);
        }

        private string _nombreProducto;
        public string NombreProducto
        {
            get => _nombreProducto;
            set { _nombreProducto = value; OnPropertyChanged(nameof(NombreProducto)); }
        }

        private ObservableCollection<Magatzems> _almacenesDisponibles;
        public ObservableCollection<Magatzems> AlmacenesDisponibles
        {
            get => _almacenesDisponibles;
            set { _almacenesDisponibles = value; OnPropertyChanged(nameof(AlmacenesDisponibles)); }
        }

        private Magatzems _almacenSeleccionado;
        public Magatzems AlmacenSeleccionado
        {
            get => _almacenSeleccionado;
            set
            {
                _almacenSeleccionado = value;
                OnPropertyChanged(nameof(AlmacenSeleccionado));
                _ = CargarZonasDelAlmacenSeleccionado(_almacenSeleccionado);
            }
        }

        private Magatzems _almacenOrigen;
        public Magatzems AlmacenOrigen
        {
            get => _almacenOrigen;
            set { _almacenOrigen = value; OnPropertyChanged(nameof(ZonaOrigen)); }
        }

        private ZonaEmmagatzematge _zonaOrigen;
        public ZonaEmmagatzematge ZonaOrigen
        {
            get => _zonaOrigen;
            set { _zonaOrigen = value; OnPropertyChanged(nameof(ZonaOrigen)); }
        }

        private ObservableCollection<ZonaEmmagatzematge> _zonasDestino;
        public ObservableCollection<ZonaEmmagatzematge> ZonasDestino
        {
            get => _zonasDestino;
            set { _zonasDestino = value; OnPropertyChanged(nameof(ZonasDestino)); }
        }

        private ZonaEmmagatzematge _zonaDestinoSeleccionada;
        public ZonaEmmagatzematge ZonaDestinoSeleccionada
        {
            get => _zonaDestinoSeleccionada;
            set { _zonaDestinoSeleccionada = value; OnPropertyChanged(nameof(ZonaDestinoSeleccionada)); }
        }

        private int _cantidadAMover;
        public int CantidadAMover
        {
            get => _cantidadAMover;
            set { _cantidadAMover = value; OnPropertyChanged(nameof(CantidadAMover)); }
        }

        private async Task CargarZonasDelAlmacenSeleccionado(Magatzems magatzemSeleccionat)
        {
            if (!string.IsNullOrEmpty(magatzemSeleccionat?.MagatzemID))
            {
                var zonas = await dao.DetallesZonasAlmacen(magatzemSeleccionat);
                ZonasDestino = new ObservableCollection<ZonaEmmagatzematge>(zonas);
            }
        }

        private async void ConfirmarMovimiento()
        {
            try
            {
                DocumentSnapshot docSnap = await dao.GetProducteInventariPorID(_productoSeleccionado.ProducteID);
                var producteInventari = docSnap.Exists ? docSnap.ConvertTo<ProductesInventari>() : null;

                MovimentsInventari movimiento = new MovimentsInventari
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

                await dao.GuardarMovimientoInventariAsync(movimiento, producteInventari);
                MessageBox.Show("Movimiento registrado con éxito.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al guardar el movimiento: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool PuedeConfirmarMovimiento()
        {
            return ZonaDestinoSeleccionada != null && CantidadAMover > 0;
        }

        public ICommand ConfirmarMovimientoCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
