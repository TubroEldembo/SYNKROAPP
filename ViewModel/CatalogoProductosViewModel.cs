using GalaSoft.MvvmLight.Command;
using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static Steema.TeeChart.Styles.SquarifiedMap;

namespace SYNKROAPP.ViewModel
{
    public class CatalogoProductosViewModel : INotifyPropertyChanged
    {
        private IDAO dao;
        private Empreses empresa;

        public CatalogoProductosViewModel(IDAO dao, Empreses empresa)
        {
            this.dao = dao;
            this.empresa = empresa;
            ProductosFiltrados = new ObservableCollection<ProducteGeneral>();
            NomEmpresa = empresa.NomEmpresa;
        }

        private ObservableCollection<ProducteGeneral> _productosFiltrados;
        public ObservableCollection<ProducteGeneral> ProductosFiltrados
        {
            get => _productosFiltrados;
            set { _productosFiltrados = value; OnPropertyChanged(nameof(ProductosFiltrados)); }
        }

        private string _nomEmpresa;
        public string NomEmpresa
        {
            get => _nomEmpresa;
            set
            {
                _nomEmpresa = value;
                OnPropertyChanged(NomEmpresa);
            }
        }


        public async Task CargarProductos()
        {
            ProductosFiltrados.Clear();

            List<ProducteAmbDetall> productesAmbDetall = await dao.GetProductosCatagalogoD1Empresa(empresa.EmpresaID, true);

            foreach (var item in productesAmbDetall)
            {
                ProductosFiltrados.Add(item.Producte);
            }

            OnPropertyChanged(nameof(ProductosFiltrados));
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
