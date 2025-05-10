using GalaSoft.MvvmLight.Command;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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

            // Comando para volver a la pantalla anterior
            VolverCommand = new RelayCommand(Volver);
        }

        private ObservableCollection<ProducteGeneral> _productosFiltrados;
        public ObservableCollection<ProducteGeneral> ProductosFiltrados
        {
            get => _productosFiltrados;
            set { _productosFiltrados = value; OnPropertyChanged(nameof(ProductosFiltrados)); }
        }

        private string _textoBusqueda;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; OnPropertyChanged(nameof(TextoBusqueda)); }
        }

        // Comando Volver
        public ICommand VolverCommand { get; }

        private void Volver()
        {
            // Aquí puedes navegar a la pantalla anterior
            // En el caso de WPF, puedes usar NavigationService si estás en un contexto de navegación
            MessageBox.Show("Volviendo a la pantalla anterior...");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
