using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SYNKROAPP.ViewModel
{
    public class CrearZonaViewModel: INotifyPropertyChanged
    {
        private readonly IDAO _dao;
        private readonly Magatzems _magatzem;

        private string _nombreZona;
        private string _almacenPerteneciente;
        private string _zonaID;
        private int _capacitat;
        private int _nProductos;

        public CrearZonaViewModel(IDAO dao, Magatzems magatzem)
        {
            _dao = dao;
            _magatzem = magatzem;
            _almacenPerteneciente = magatzem?.NomMagatzem ?? "(Sin nombre)";
        }

        public string Nom
        {
            get => _nombreZona;
            set
            {
                _nombreZona = value;
                OnPropertyChanged(nameof(Nom));
            }
        }

        public int Capacitat
        {
            get => _capacitat;
            set
            {
                _capacitat = value;
                OnPropertyChanged(nameof(Capacitat));
            }
        }

        public string AlmacenPerteneciente
        {
            get => _almacenPerteneciente;
            set
            {
                _almacenPerteneciente = value;
                OnPropertyChanged(nameof(AlmacenPerteneciente));
            }
        }

        public string ZonaID
        {
            get => _zonaID;
            set
            {
                _zonaID = value;
                OnPropertyChanged(nameof(ZonaID));
            }
        }

        public int NProductos
        {
            get => _nProductos;
            set
            {
                _nProductos = value;
                OnPropertyChanged(nameof(NProductos));
            }
        }

        public async Task<bool> GuardarZona()
        {
            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Capacitat.ToString()))
            {
                MessageBox.Show("Por favor, rellene todos los campos obligatorios.", 
                                "Validación", MessageBoxButton.OK, MessageBoxImage.Warning
                               );
                return false;
            }

            ZonaEmmagatzematge nuevaZona = new ZonaEmmagatzematge
            {
                Nom = Nom,
                Capacitat = Capacitat, 
                MagatzemPertanyent = _magatzem.MagatzemID,
                EmpresaID = _magatzem.EmpresaPertanyentID,
            };

            var resultat = await _dao.CrearZonaAlmacen(_magatzem, nuevaZona);

            if (resultat != null)
            {
                _nProductos = resultat.Productes.Count;
                _zonaID = resultat.ZonaEmmagatzematgeID;

                MessageBox.Show("Zona de almacén creada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
