using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.ViewModel
{
    class VistaMovimientosViewModel: INotifyPropertyChanged
    {
        private readonly IDAO dao;
        private readonly Empreses empresa;
        private ObservableCollection<MovimentsInventari> _todosLosMovimientos;

        public VistaMovimientosViewModel(IDAO dao, Empreses empresa)
        {
            this.dao = dao;
            this.empresa = empresa;

            // Inicializar fechas por defecto (último mes)
            FechaDesde = DateTime.Now.AddMonths(-1);
            FechaHasta = DateTime.Now;

            // Cargar datos al iniciar
            CargarMovimientosAsync();


        }

        #region Propiedades
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

        private DateTime _fechaDesde;
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set
            {
                _fechaDesde = value;
                OnPropertyChanged(nameof(FechaDesde));
                AplicarFiltros();
            }
        }

        private DateTime _fechaHasta;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set
            {
                _fechaHasta = value;
                OnPropertyChanged(nameof(FechaHasta));
                AplicarFiltros();
            }
        }
        #endregion

        #region MÉTODOS
        public async Task CargarMovimientosAsync()
        {
            var movimientos = await dao.ObtenerMovimientosInventarioPorEmpresa(empresa.EmpresaID);
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

            // Filtrar por fecha desde
            movimientosFiltrados = movimientosFiltrados
                .Where(m => m.Data.Date >= FechaDesde.Date)
                .ToList();

            // Filtrar por fecha hasta
            movimientosFiltrados = movimientosFiltrados
                .Where(m => m.Data.Date <= FechaHasta.Date)
                .ToList();

            // Actualizar la colección observable
            MovimientosFiltrados = new ObservableCollection<MovimentsInventari>(movimientosFiltrados);
        }
        public void LimpiarFiltros()
        {
            TipoMovimientoSeleccionado = null;
            FechaDesde = DateTime.Now.AddMonths(-1);
            FechaHasta = DateTime.Now;

            // Los setters de las propiedades llamarán a AplicarFiltros()
        }
        #endregion


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
