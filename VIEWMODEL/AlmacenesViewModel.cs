using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.VIEWMODEL
{
    public class AlmacenesViewModel: INotifyPropertyChanged
    {
        private IDAO _dao;
        private List<Magatzems> _almacenes;
        private Empreses _empresa;
        private string _totalAlmacenes;
        private string _movimientosRecientes;
        private string _productosEnAlmacenes;
        private int _numeroDeZonas;
        private ObservableCollection<MovimentsInventari> _todosLosMovimientos;

        public AlmacenesViewModel(IDAO _dao, Empreses empresa)
        {
            this._dao = _dao;
            this._empresa = empresa;
            this._almacenes = new List<Magatzems>();
            CargarMovimientosAsync();
        }

        private List<MovimentsInventari> _movimientos;

        public List<MovimentsInventari> Movimientos
        {
            get => _movimientos;
            set
            {
                _movimientos = value;
                OnPropertyChanged(nameof(Movimientos));
            }
        }

        private ObservableCollection<MovimentsInventari> _movimientosFiltrados = new ObservableCollection<MovimentsInventari>();
        public ObservableCollection<MovimentsInventari> MovimientosFiltrados
        {
            get => _movimientosFiltrados;
            set
            {
                _movimientosFiltrados = value;
                OnPropertyChanged(nameof(MovimientosFiltrados));
            }
        }

        private TipusMoviment? _tipoMovimientoSeleccionado;
        public TipusMoviment? TipoMovimientoSeleccionado
        {
            get => _tipoMovimientoSeleccionado;
            set
            {
                _tipoMovimientoSeleccionado = value;
                OnPropertyChanged(nameof(TipoMovimientoSeleccionado));
                AplicarFiltros();
            }
        }

        public string TotalAlmacenes
        {
            get => _totalAlmacenes;
            set
            {
                if (_totalAlmacenes != value)
                {
                    _totalAlmacenes = value;
                    OnPropertyChanged(nameof(TotalAlmacenes));
                }
            }
        }

        public string MovimientosRecientes
        {
            get => _movimientosRecientes;
            set
            {
                if (_movimientosRecientes != value)
                {
                    _movimientosRecientes = value;
                    OnPropertyChanged(nameof(MovimientosRecientes));
                }
            }
        }

        public string ProductosEnAlmacenes
        {
            get => _productosEnAlmacenes;
            set
            {
                if (_productosEnAlmacenes != value)
                {
                    _productosEnAlmacenes = value;
                    OnPropertyChanged(nameof(ProductosEnAlmacenes));
                }
            }
        }

        public int NumeroDeZonas
        {
            get => _numeroDeZonas;
            set
            {
                if (_numeroDeZonas != value)
                {
                    _numeroDeZonas = value;
                    OnPropertyChanged(nameof(ProductosEnAlmacenes));
                }
            }
        }

        public List<Magatzems> Almacenes
        {
            get => _almacenes;
            set
            {
                if (_almacenes != value)
                {
                    _almacenes = value;
                    OnPropertyChanged(nameof(Almacenes));
                }
            }
        }

        public async Task CargarMovimientosAsync()
        {
            List<MovimentsInventari> movimientos = await _dao.ObtenerMovimientosInventarioPorEmpresa(_empresa.EmpresaID);
            _todosLosMovimientos = new ObservableCollection<MovimentsInventari>(movimientos);

            // Aplicar filtros iniciales
            AplicarFiltros();
        }
        public void AplicarFiltros()
        {
            if (_todosLosMovimientos == null)
                return;

            List<MovimentsInventari> movimientosFiltrados = _todosLosMovimientos.ToList();

            if (TipoMovimientoSeleccionado.HasValue)
            {
                movimientosFiltrados = movimientosFiltrados
                    .Where(m => m.Tipus == TipoMovimientoSeleccionado.Value)
                    .ToList();
            }

            // Actualizar la colección observable
            MovimientosFiltrados = new ObservableCollection<MovimentsInventari>(movimientosFiltrados);
        }
        public async Task CargarDetallesAlmacenes()
        {
            List<Magatzems> almacenes = await _dao.DetallesAlmacenes(_empresa);
            
            if (almacenes != null)
            {
                // Actualizamos la lista de almacenes en el ViewModel
                Almacenes = almacenes;

                // Total de almacenes como string
                TotalAlmacenes = almacenes.Count.ToString();

                // ✅ Calcular el número total de zonas
                NumeroDeZonas = almacenes.Sum(a => a.Zones?.Count ?? 0);

            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    

}
