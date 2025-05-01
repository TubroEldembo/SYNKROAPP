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
    public class ZonasAlmacenViewModel : INotifyPropertyChanged
    {
        public IDAO _dao;
        public readonly Magatzems _almacenSeleccionado;

        public ZonasAlmacenViewModel(IDAO _dao, Magatzems almacenSeleccionado)
        {
            this._dao = _dao;
            _almacenSeleccionado = almacenSeleccionado;
        }

        // Propiedades de la interfaz

        private string _detalleAlmacen;
        public string DetalleAlmacen
        {
            get => _detalleAlmacen;
            set
            {
                _detalleAlmacen = value;
                OnPropertyChanged(nameof(DetalleAlmacen));
            }
        }

        private string _direccion;
        public string Direccion
        {
            get => _direccion;
            set
            {
                _direccion = value;
                OnPropertyChanged(nameof(Direccion));
            }
        }

        private string _magatzemID;
        public string MagatzemID
        {
            get => _magatzemID;
            set
            {
                _magatzemID = value;
                OnPropertyChanged(nameof(MagatzemID));
            }
        }

        private int _productosTotales;
        public int ProductosTotales
        {
            get => _productosTotales;
            set
            {
                _productosTotales = value;
                OnPropertyChanged(nameof(ProductosTotales));
            }
        }

        private int _numeroZonas;
        public int NumeroDeZonas
        {
            get => _numeroZonas;
            set
            {
                _numeroZonas = value;
                OnPropertyChanged(nameof(NumeroDeZonas));
            }
        }

        private ObservableCollection<ZonaEmmagatzematge> _zonas = new();
        public ObservableCollection<ZonaEmmagatzematge> Zonas
        {
            get => _zonas;
            set
            {
                _zonas = value;
                OnPropertyChanged(nameof(Zonas));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CargarZonasAsync()
        {
            List<ZonaEmmagatzematge> zonasAlmacen = await _dao.DetallesZonasAlmacen(_almacenSeleccionado);

            if (zonasAlmacen != null)
            {
                Zonas = new ObservableCollection<ZonaEmmagatzematge>(zonasAlmacen);

                // Aquí puedes actualizar otras propiedades si es necesario
                ProductosTotales = zonasAlmacen.Sum(z => z.Productes?.Count ?? 0);  
                NumeroDeZonas = zonasAlmacen.Count;
                Direccion = _almacenSeleccionado.Ubicacio.Nom;
                DetalleAlmacen = _almacenSeleccionado.NomMagatzem;
                MagatzemID = _almacenSeleccionado.MagatzemID;
                
            }
        }
    }
}
