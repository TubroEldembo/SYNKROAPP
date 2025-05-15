using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using SYNKROAPP.Vistes.Vista_Productos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;

namespace SYNKROAPP.ViewModel
{
    public partial class DetalleDe1ZonaViewModel : ObservableObject
    {
        private readonly IDAO _dao;
        private readonly ZonaEmmagatzematge _zonaSeleccionada;
        private readonly Magatzems _magatzemSeleccionat;
        private Dictionary<string, List<Subcategories>> _diccionarioSubcategorias;
        private List<ProducteInventariAmbNom> _todosLosProductes;
        private int productosTotales;
        private int capacidadTotal;
        private Dictionary<string, int> productosPorAlmacen = new(); // Si lo necesitas por nombre de almacén


        public DetalleDe1ZonaViewModel(IDAO dao, ZonaEmmagatzematge zonaSeleccionada, Magatzems magatzemSeleccionat)
        {
            _dao = dao;
            _zonaSeleccionada = zonaSeleccionada;
            _magatzemSeleccionat = magatzemSeleccionat;

            // Inicializar colecciones
            Categories = new ObservableCollection<Categories>();
            Subcategories = new ObservableCollection<Subcategories>();
            Estats = new ObservableCollection<string>();
            ProductesFiltrats = new ObservableCollection<ProducteInventariAmbNom>();
        }

        public IDAO Dao => _dao;
        public ZonaEmmagatzematge ZonaSeleccionada => _zonaSeleccionada;
        public Magatzems MagatzemSeleccionat => _magatzemSeleccionat;

        #region Propiedades Observable

        [ObservableProperty]
        private ObservableCollection<Categories> categories;

        [ObservableProperty]
        private ObservableCollection<Subcategories> subcategories;

        [ObservableProperty]
        private ObservableCollection<string> estats;

        [ObservableProperty]
        private ObservableCollection<ProducteInventariAmbNom> productesFiltrats;

        // Categoría seleccionada con notificación manual para ejecutar lógica adicional
        private Categories _categoriaSeleccionada;
        public Categories CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                if (SetProperty(ref _categoriaSeleccionada, value))
                {
                    ActualizarSubcategorias();
                    AplicarFiltros();
                }
            }
        }

        // Subcategoría seleccionada con notificación manual
        private Subcategories _subCategoriaSeleccionada;
        public Subcategories SubCategoriaSeleccionada
        {
            get => _subCategoriaSeleccionada;
            set
            {
                if (SetProperty(ref _subCategoriaSeleccionada, value))
                {
                    AplicarFiltros();
                }
            }
        }

        // Estado seleccionado con notificación manual
        private string _estatSeleccionat;
        public string EstatSeleccionat
        {
            get => _estatSeleccionat;
            set
            {
                if (SetProperty(ref _estatSeleccionat, value))
                {
                    AplicarFiltros();
                }
            }
        }

        // Propiedades de la zona
        [ObservableProperty]
        private string nomZona;

        [ObservableProperty]
        private string zonaID;

        [ObservableProperty]
        private string almacenPerteneciente;

        [ObservableProperty]
        private int nProductos;

        [ObservableProperty]
        private int capacitat;

        [ObservableProperty]
        private int quantitat;

        [ObservableProperty]
        private string almacenID;

        #endregion

        #region Métodos de carga y filtro

        [RelayCommand]
        public async Task CargarProductos()
        {
            try
            {
                var productesInventari = await _dao.ProductosEn1Zona(_zonaSeleccionada);
                _todosLosProductes = new List<ProducteInventariAmbNom>();

                foreach (var item in productesInventari)
                {
                    var docSnap = await _dao.GetProducteGeneralPorID(item.ProducteID);
                    var producteGeneral = docSnap.Exists ? docSnap.ConvertTo<ProducteGeneral>() : null;

                    ProducteInventariAmbNom p = new ProducteInventariAmbNom
                    {
                        ProducteID = item.ProducteID,
                        Nom = producteGeneral?.Nom ?? "Sin nombre",
                        SKU = producteGeneral?.SKU ?? "N/A",
                        UrlImagen = producteGeneral?.ImatgeURL ?? "",
                        Descripcio = producteGeneral?.Descripcio ?? "",
                        Estat = item.Estat,
                        Categoria = producteGeneral?.CategoriaID,
                        SubCategoria = producteGeneral?.SubCategoriaID,
                        Quantitat = item.Quantitat,
                    };
                    _todosLosProductes.Add(p);
                }

                // Actualizar datos de la zona
                NomZona = _zonaSeleccionada.Nom;
                ZonaID = _zonaSeleccionada.ZonaEmmagatzematgeID;
                AlmacenID = _zonaSeleccionada.MagatzemPertanyent;
                AlmacenPerteneciente = _zonaSeleccionada.MagatzemPertanyent;

                productosTotales = 0;
                capacidadTotal = 0;

                List<ProductesInventari> productosZona = await _dao.ProductosEn1Zona(ZonaSeleccionada);

                foreach (ProductesInventari prouctes in productosZona)
                {
                    productosTotales += prouctes.Quantitat;
                }

                NProductos = productosTotales; 

                Capacitat = _zonaSeleccionada.Capacitat;

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public async Task CargarListasParaCombos()
        {
            try
            {
                Categories.Clear();
                _diccionarioSubcategorias = new Dictionary<string, List<Subcategories>>();

                // Opción 'Todas las categorías'
                var categoriaTodas = new Categories { CategoriaID = "TODAS", Nom = "Todas las categorías" };
                Categories.Add(categoriaTodas);

                var (categoriasGenericas, subcategoriasGenericasDictionary) = await _dao.ObtenirCategoriesGeneriques();

                foreach (var catGen in categoriasGenericas)
                {
                    var categoria = new Categories { CategoriaID = catGen.CategoriaID, Nom = catGen.Nom };
                    Categories.Add(categoria);

                    if (subcategoriasGenericasDictionary.TryGetValue(catGen.CategoriaID, out var subList))
                    {
                        _diccionarioSubcategorias[catGen.CategoriaID] = subList.Select(s => new Subcategories
                        {
                            SubCategoriaID = s.SubCategoriaID,
                            Nom = s.Nom,
                            CategoriaID = catGen.CategoriaID
                        }).ToList();
                    }
                }

                string empresaID = _zonaSeleccionada.EmpresaID;
                if (!string.IsNullOrEmpty(empresaID))
                {
                    var (categoriasPers, subcatsPersDict) = await _dao.ObtenirCategoriesPersonalitzades(empresaID);

                    foreach (Categories catPers in categoriasPers)
                    {
                        Categories.Add(catPers);
                        if (subcatsPersDict.TryGetValue(catPers.CategoriaID, out var subcats))
                        {
                            _diccionarioSubcategorias[catPers.CategoriaID] = subcats;
                        }
                    }
                }

                Estats.Clear();
                Estats.Add("Todos");
                Estats.Add("Nuevo");
                Estats.Add("Seminuevo");
                Estats.Add("Defectuoso");
                Estats.Add("En reparación");

                EstatSeleccionat = "Todos";
                CategoriaSeleccionada = categoriaTodas; // Selección por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando listas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarSubcategorias()
        {
            Subcategories.Clear();

            // Opción 'Todas las subcategorías'
            Subcategories todasSub = new Subcategories { SubCategoriaID = "TODAS", Nom = "Todas las subcategorías" };
            Subcategories.Add(todasSub);

            if (CategoriaSeleccionada != null &&
                CategoriaSeleccionada.CategoriaID != "TODAS" &&
                _diccionarioSubcategorias.TryGetValue(CategoriaSeleccionada.CategoriaID, out var lista))
            {
                foreach (Subcategories subcat in lista)
                {
                    Subcategories.Add(subcat);
                }
            }

            SubCategoriaSeleccionada = todasSub;
        }

        private void AplicarFiltros()
        {
            if (_todosLosProductes == null) return;

            var filtrados = _todosLosProductes.AsEnumerable();

            if (EstatSeleccionat != "Todos" && !string.IsNullOrWhiteSpace(EstatSeleccionat))
            {
                filtrados = filtrados.Where(p => p.Estat == EstatSeleccionat);
            }

            if (CategoriaSeleccionada != null && CategoriaSeleccionada.CategoriaID != "TODAS")
            {
                filtrados = filtrados.Where(p => p.Categoria == CategoriaSeleccionada.Nom);
            }

            if (SubCategoriaSeleccionada != null && SubCategoriaSeleccionada.SubCategoriaID != "TODAS")
            {
                filtrados = filtrados.Where(p => p.SubCategoria == SubCategoriaSeleccionada.Nom);
            }

            ProductesFiltrats = new ObservableCollection<ProducteInventariAmbNom>(filtrados);
        }

        #endregion

        #region Comandos adicionales

        [RelayCommand]
        public void RefrescarFiltros()
        {
            AplicarFiltros();
        }

        [RelayCommand]
        public void LimpiarFiltros()
        {
            if (Categories.Count > 0)
            {
                CategoriaSeleccionada = Categories.FirstOrDefault(c => c.CategoriaID == "TODAS");
            }
            if (Estats.Count > 0)
            {
                EstatSeleccionat = "Todos";
            }
            AplicarFiltros();
        }
        #endregion
    }

}