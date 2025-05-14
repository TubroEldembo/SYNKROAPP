using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.VIEWMODEL;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.VIEWMODEL
{
    public partial class GestionarCatalogoViewModel : ObservableObject
    {
        private readonly IDAO dao;
        private readonly Empreses empresa;

        public GestionarCatalogoViewModel(IDAO dao, Empreses empresa)
        {
            this.dao = dao;
            this.empresa = empresa;
            CargarDatosSimulados();
            AplicarFiltros();

            // Detectar cambios y aplicar filtros automáticamente
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName is nameof(TextoBusqueda) or nameof(CategoriaSeleccionada) or nameof(EstadoSeleccionado))
                    AplicarFiltros();
            };
        }


        // --- 🔍 PROPIEDADES DE FILTRO ---
        [ObservableProperty]
        private string textoBusqueda;

        [ObservableProperty]
        private string categoriaSeleccionada;

        [ObservableProperty]
        private string estadoSeleccionado;
        [ObservableProperty]
        private ImageSource imagenProducto;

        [ObservableProperty]
        private string nom;

        [ObservableProperty]
        private string descripcio;

        [ObservableProperty]
        private string sku;

        [ObservableProperty]
        private string categoria;

        [ObservableProperty]
        private string subcategoria;

        [ObservableProperty]
        private double preu;

        [ObservableProperty]
        private ProducteAmbDetall? productoSeleccionadoParaMenu;



        public ObservableCollection<string> Categorias { get; set; } = new();
        public ObservableCollection<string> Estados { get; set; } = new() { "Disponible", "Agotado", "Archivado" };

        public ObservableCollection<ProducteAmbDetall> ProductosOriginales { get; set; } = new();
        public ObservableCollection<ProducteAmbDetall> ProductosFiltrados { get; set; } = new();

     
        [RelayCommand]
        private void MostrarMenuContextual(ProducteAmbDetall producto)
        {
            ProductoSeleccionadoParaMenu = producto;
            // Aquí no se abre el Popup, solo se notifica del cambio
        }

        private async void CargarDatosSimulados()
        {
            List<ProducteAmbDetall> productesAmbDetall = await dao.GetProductosCatagalogoD1Empresa(empresa.EmpresaID, true);
            DetallProducte detallProducte;
            foreach (var item in productesAmbDetall)
            {
                detallProducte = await dao.GetDetallProducteAsync(item.Producte.ProducteID);
                BitmapImage bitmap = dao.LoadImageFromUrl(item.Producte.ImatgeURL);
                item.Nom = item.Producte.Nom;
                item.Sku = item.Producte.SKU;
                item.ImagenProducto = bitmap;
                ProductosFiltrados.Add(item);
            }
        }

        private void AplicarFiltros()
        {
            var filtrados = ProductosFiltrados.Where(p =>
                (string.IsNullOrWhiteSpace(TextoBusqueda) || p.Producte.Nom.ToLower().Contains(TextoBusqueda.ToLower())) &&
                (string.IsNullOrWhiteSpace(CategoriaSeleccionada) || p.Producte.CategoriaID == CategoriaSeleccionada)
            ).ToList();

            ProductosFiltrados.Clear();
            foreach (var p in filtrados)

                ProductosFiltrados.Add(p);
        }
    }
}
