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
    class VistaResumenViewModel: INotifyPropertyChanged
    {
        private IDAO dao;
        private Empreses empresa;
        public Dictionary<string, int> ProductosPorCategoria { get; private set; } = new Dictionary<string, int>();


        public VistaResumenViewModel(IDAO dao, Empreses empresa)
        {
            this.dao = dao;
            this.empresa = empresa;
        }

        private string _nomEmpresa;
        public string NomEmpresa
        {
            get => _nomEmpresa;
            set
            {
                _nomEmpresa = value;
                OnPropertyChanged(nameof(NomEmpresa));
            }
        }

        private int _totalProductos;
        public int TotalProductos
        {
            get => _totalProductos;
            set
            {
                _totalProductos = value;
                OnPropertyChanged(nameof(TotalProductos));
            }
        }

        private int _productosEnMovimiento;
        public int ProductosEnMovimiento
        {
            get => _productosEnMovimiento;
            set
            {
                _productosEnMovimiento = value;
                OnPropertyChanged(nameof(ProductosEnMovimiento));
            }
        }


        private int _totalAlmacenes;
        public int TotalAlmacenes
        {
            get => _totalAlmacenes;
            set
            {
                _totalAlmacenes = value;
                OnPropertyChanged(nameof(TotalAlmacenes));
            }
        }

        private string _capacidadUsadaPorcentaje = "0%";
        public string CapacidadUsadaPorcentaje
        {
            get => _capacidadUsadaPorcentaje;
            set
            {
                _capacidadUsadaPorcentaje = value;
                OnPropertyChanged(nameof(CapacidadUsadaPorcentaje));
            }
        }

        public async Task CargarDatosEmpresa()
        {
            NomEmpresa = empresa.NomEmpresa;

            List<Magatzems> almacenes = await dao.ObtenerLosAlmacenesTotalesDeLaEmpresa(empresa.EmpresaID);
            TotalAlmacenes = almacenes.Count;


            int totalProductos = 0;
            //int productosEnMovimiento = 0; // depende de cómo definas "en movimiento"
            int capacidadTotal = 0;
            int productosTotales = 0;

            foreach (Magatzems almacen in almacenes) 
            {
                List<ZonaEmmagatzematge> zonas = await dao.DetallesZonasAlmacen(almacen);
                
                foreach(ZonaEmmagatzematge zona in zonas)
                {
                    int productosZona = zona.Productes?.Count ?? 0;
                    productosTotales += productosZona;
                    capacidadTotal += zona.Capacitat;
                }
            }

            TotalProductos = productosTotales;
            //ProductosEnMovimiento = producosEnMovimiento;

            if (capacidadTotal > 0)
            {
                double porcentaje = (double)productosTotales / capacidadTotal * 100;
                CapacidadUsadaPorcentaje = $"{Math.Round(porcentaje, 2)}%";
            }
            else
                CapacidadUsadaPorcentaje = "0%";

            ProductosPorCategoria = await dao.ObtenerProductosEnVentaPorCategoria(empresa.EmpresaID);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
