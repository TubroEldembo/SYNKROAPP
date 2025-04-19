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
        private string _totalAlmacenes;
        private string _movimientosRecientes;
        private string _productosEnAlmacenes;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    

}
