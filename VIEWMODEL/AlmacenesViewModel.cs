using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
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
        private string _totalAlmacenes;
        private string _movimientosRecientes;
        private string _productosEnAlmacenes;
        private int _numeroDeZonas;

        public AlmacenesViewModel(IDAO _dao)
        {
            this._dao = _dao;
            this._almacenes = new List<Magatzems>();
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

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task CargarDetallesAlmacenes(Usuaris empresa)
        {
            List<Magatzems> almacenes = await _dao.DetallesAlmacenes(empresa);

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    

}
