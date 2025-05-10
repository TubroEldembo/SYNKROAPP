using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace SYNKROAPP.ViewModel
{
    public class MercadoEmpresasViewModel : INotifyPropertyChanged
    {
        private readonly IDAO _dao;
        private string _textoBusqueda;
        private double _valorSliderUbicacion;
        private Empreses _miEmpresa;
        private ObservableCollection<Empreses> _empresasFiltradas;

        public MercadoEmpresasViewModel(IDAO dao, Empreses empresa)
        {
            _dao = dao;
            _miEmpresa = empresa;
            EmpresasFiltradas = new ObservableCollection<Empreses>();
        }

        public ObservableCollection<Empreses> EmpresasFiltradas
        {
            get => _empresasFiltradas;
            set
            {
                _empresasFiltradas = value;
                OnPropertyChanged(nameof(EmpresasFiltradas));
            }
        }

        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                if (_textoBusqueda != value)
                {
                    _textoBusqueda = value;
                    OnPropertyChanged(nameof(TextoBusqueda));
                    FiltrarEmpresas();
                }
            }
        }

        public double ValorSliderUbicacion
        {
            get => _valorSliderUbicacion;
            set
            {
                if (_valorSliderUbicacion != value)
                {
                    _valorSliderUbicacion = value;
                    OnPropertyChanged(nameof(ValorSliderUbicacion));
                    FiltrarEmpresas();
                }
            }
        }

        private string _nombre;
        public string NomEmpresa
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(NomEmpresa));
            }
        }

        private string _sector;
        public string Tipus
        {
            get => _sector;
            set
            {
                _sector = value;
                OnPropertyChanged(nameof(Tipus));
            }
        }

        private Adreça _adreca;
        public Adreça Adreca
        {
            get => _adreca;
            set
            {
                _adreca = value;
                OnPropertyChanged(nameof(Adreca));
            }
        }

        private ImageSource _logoPath;
        public ImageSource FotoEmpresalUrl
        {
            get => _logoPath;
            set
            {
                _logoPath = value;
                OnPropertyChanged(nameof(FotoEmpresalUrl));
            }
        }

        public string UbicacionTexto => $"Distancia: {ValorSliderUbicacion} km";
        public string TextoResultados => $"{EmpresasFiltradas?.Count ?? 0} resultados";

        public async Task CargarEmpresas()
        {
            EmpresasFiltradas.Clear();
            List<string> lstIDsEmpresasConProductos = await _dao.GetEmpresesAmbProductesEnVenda();

            foreach (string id in lstIDsEmpresasConProductos)
            {
                // Excluir la empresa del usuario actual
                if (id == _miEmpresa.EmpresaID) continue;

                Empreses empresa = await _dao.GetEmpresaByID(id);
                if (empresa != null)
                {
                    EmpresasFiltradas.Add(empresa);
                }

                //Cargar  Variable detalle Empresa
                FotoEmpresalUrl = _dao.LoadImageFromUrl(empresa.FotoEmpresalUrl);
                NomEmpresa = empresa.NomEmpresa;
                Tipus = empresa.Tipus;
                
            }

            OnPropertyChanged(nameof(EmpresasFiltradas));
            OnPropertyChanged(nameof(TextoResultados));
        }

        private void FiltrarEmpresas()
        {
            // Aquí aplicas los filtros (texto, ubicación, etc.)
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
