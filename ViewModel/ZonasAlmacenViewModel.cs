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
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public partial class ZonasAlmacenViewModel : ObservableObject
    {
        private readonly IDAO _dao;
        private readonly Magatzems _almacenSeleccionado;

        public ZonasAlmacenViewModel(IDAO dao, Magatzems almacenSeleccionado)
        {
            _dao = dao;
            _almacenSeleccionado = almacenSeleccionado;
        }

        public IDAO Dao => _dao;
        public Magatzems AlmacenSeleccionado => _almacenSeleccionado;

        [ObservableProperty]
        private string nombreAlmacen;

        [ObservableProperty]
        private int nProductos;

        [ObservableProperty]
        private string direccion;

        [ObservableProperty]
        private string magatzemID;

        [ObservableProperty]
        private int productosTotales;

        [ObservableProperty]
        private int numeroDeZonas;

        [ObservableProperty]
        private ObservableCollection<ZonaEmmagatzematge> zonas = new();

        [RelayCommand]
        public async Task CargarZonasAsync()
        {
            List<ZonaEmmagatzematge> zonasAlmacen = await _dao.DetallesZonasAlmacen(_almacenSeleccionado);

           
            if (zonasAlmacen != null)
            {
                productosTotales = 0;

                foreach (ZonaEmmagatzematge zona in zonasAlmacen)
                {
                    List<ProductesInventari> productosZona = await _dao.ProductosEn1Zona(zona);
                    foreach (ProductesInventari producte in  productosZona)
                    {
                        zona.NProductos = productosZona.Sum(p => p.Quantitat); // <-- AQUÍ
                        productosTotales += zona.NProductos; // Acumula bien aquí también
                    }
                }

                Zonas = new ObservableCollection<ZonaEmmagatzematge>(zonasAlmacen);

                ProductosTotales = productosTotales;
                NumeroDeZonas = zonasAlmacen.Count;
                //Direccion = _almacenSeleccionado.Ubicacio.Nom;
                NombreAlmacen = _almacenSeleccionado.NomMagatzem;
                MagatzemID = _almacenSeleccionado.MagatzemID;
            }
        }
    }

}
