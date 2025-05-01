using Google.Cloud.Firestore;
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
    public class DetalleDe1ZonaViewModel: INotifyPropertyChanged
    {
        public IDAO _dao;
        public readonly ZonaEmmagatzematge _zonaSeleccionada;
        public DetalleDe1ZonaViewModel(IDAO dao, ZonaEmmagatzematge zonaSeleccionada)
        {
            _dao = dao;
            _zonaSeleccionada = zonaSeleccionada;
        }

        private string _nomZona;
        public string NomZona
        {
            get => _nomZona;
            set
            {
                _nomZona = value;
                OnPropertyChanged(nameof(NomZona));  
            }
        }

        private string _zonaID;
        public string ZonaID
        {
            get => _zonaID;
            set
            {
                _zonaID = value;
                OnPropertyChanged(nameof(ZonaID));
            }
        }

        private string _almacenPerteneciente;
        public string AlmacenPerteneciente
        {
            get => _almacenPerteneciente;
            set
            {
                _almacenPerteneciente = value;
                OnPropertyChanged(nameof(AlmacenPerteneciente));
            }
        }

        private int _nProductos;
        public int NProductos
        {
            get => _nProductos;
            set
            {
                _nProductos = value;
                OnPropertyChanged(nameof(NProductos));
            }
        }

        private int _capacitat;
        public int Capacitat
        {
            get=> _capacitat;
            set
            {
                _capacitat = value;
                OnPropertyChanged(nameof(Capacitat));
            }
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

        private string _almacenID;
        public string AlmacenID
        {
            get => _almacenID;
            set
            {
                _almacenID = value;
                OnPropertyChanged(nameof(AlmacenID));
            }
        }

        private ObservableCollection<ProducteInventariAmbNom> _productes = new();
        public ObservableCollection<ProducteInventariAmbNom> Productes
        {
            get => _productes;
            set
            {
                _productes = value;
                OnPropertyChanged(nameof(Productes));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CargarProductos()
        {
            List<ProductesInventari> productesInventari = await _dao.ProductosEn1Zona(_zonaSeleccionada);

            Productes.Clear();
            foreach (var item in productesInventari)
            {
                // Obtener el producto general por ID
                DocumentSnapshot docSnap = await _dao.GetProducteGeneralPorID(item.ProducteID); // Este método lo creas abajo
                var producteGeneral = docSnap.Exists ? docSnap.ConvertTo<ProducteGeneral>() : null;

                string nom = producteGeneral?.Nom ?? "Sin nombre";
                string subcategoria = producteGeneral?.SubCategoriaID ?? "Sin subcategoría";

                Productes.Add(new ProducteInventariAmbNom
                {
                    ProducteID = item.ProducteID,
                    Nom = nom,
                    Estat = item.Estat,
                    Categoria = producteGeneral.CategoriaID,
                    SubCategoria = subcategoria,
                    Quantitat = item.Quantitat

                });
            }

            NomZona = _zonaSeleccionada.Nom;
            ZonaID = _zonaSeleccionada.ZonaEmmagatzematgeID;
            AlmacenID = _zonaSeleccionada.MagatzemPertanyent;
            NProductos = _zonaSeleccionada.NProductos;
            Capacitat = _zonaSeleccionada.Capacitat;
        }
    }
}
