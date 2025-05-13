using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SYNKROAPP.ViewModel
{
    public class DetalleDe1ZonaViewModel : INotifyPropertyChanged
    {
        private readonly IDAO _dao;
        private readonly ZonaEmmagatzematge _zonaSeleccionada;
        private readonly Magatzems _magatzemSeleccionat;
        private Dictionary<string, List<Subcategories>> _diccionarioSubcategorias;
        private List<ProducteInventariAmbNom> _todosLosProductes;

        public IDAO Dao => _dao;

        public ZonaEmmagatzematge ZonaSeleccionada => _zonaSeleccionada;

        public Magatzems MagatzemSeleccionat => _magatzemSeleccionat;


        public DetalleDe1ZonaViewModel(IDAO dao, ZonaEmmagatzematge zonaSeleccionada, Magatzems magatzemSeleccionat)
        {
            _dao = dao;
            _zonaSeleccionada = zonaSeleccionada;
            _magatzemSeleccionat = magatzemSeleccionat;
        }

        #region Propiedades públicas

        public ObservableCollection<Categories> Categories { get; set; } = new();
        public ObservableCollection<Subcategories> Subcategories { get; set; } = new();
        public ObservableCollection<string> Estats { get; set; } = new();

        private ObservableCollection<ProducteInventariAmbNom> _productesFiltrats = new();
        public ObservableCollection<ProducteInventariAmbNom> ProductesFiltrats
        {
            get => _productesFiltrats;
            set
            {
                _productesFiltrats = value;
                OnPropertyChanged(nameof(ProductesFiltrats));
            }
        }

        private Categories _categoriaSeleccionada;
        public Categories CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                OnPropertyChanged(nameof(CategoriaSeleccionada));
                ActualizarSubcategorias();
                AplicarFiltros();
            }
        }

        private Subcategories _subCategoriaSeleccionada;
        public Subcategories SubCategoriaSeleccionada
        {
            get => _subCategoriaSeleccionada;
            set
            {
                _subCategoriaSeleccionada = value;
                OnPropertyChanged(nameof(SubCategoriaSeleccionada));
                AplicarFiltros();
            }
        }

        private string _estatSeleccionat;
        public string EstatSeleccionat
        {
            get => _estatSeleccionat;
            set
            {
                _estatSeleccionat = value;
                OnPropertyChanged(nameof(EstatSeleccionat));
                AplicarFiltros();
            }
        }

        // Otras propiedades de zona
        public string NomZona { get; set; }
        public string ZonaID { get; set; }
        public string AlmacenPerteneciente { get; set; }
        public int NProductos { get; set; }
        public int Capacitat { get; set; }
        public int Quantitat { get; set; }
        public string AlmacenID { get; set; }

        #endregion

        #region Métodos de carga y filtro

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

                    var p = new ProducteInventariAmbNom
                    {
                        ProducteID = item.ProducteID,
                        Nom = producteGeneral?.Nom ?? "Sin nombre",
                        SKU = producteGeneral?.SKU ?? "N/A",
                        UrlImagen = producteGeneral?.ImatgeURL ?? "",
                        Descripcio = producteGeneral?.Descripcio ?? "",
                        Estat = item.Estat,
                        Categoria = producteGeneral?.CategoriaID,
                        SubCategoria = producteGeneral?.SubCategoriaID,
                        Quantitat = item.Quantitat
                    };

                    _todosLosProductes.Add(p);
                }

                // Actualizar datos de la zona
                NomZona = _zonaSeleccionada.Nom;
                ZonaID = _zonaSeleccionada.ZonaEmmagatzematgeID;
                AlmacenID = _zonaSeleccionada.MagatzemPertanyent;
                NProductos = _zonaSeleccionada.NProductos;
                Capacitat = _zonaSeleccionada.Capacitat;

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando productos: {ex.Message}");
            }
        }

        public async Task CargarListasParaCombosAsync()
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
                    var categoria = new Categories { CategoriaID = catGen.CategoriaID, Nom = catGen.Nom + " (Genérica)" };
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
                Estats.Add("Usado");
                Estats.Add("Defectuoso");
                Estats.Add("En reparación");

                EstatSeleccionat = "Todos";
                CategoriaSeleccionada = categoriaTodas; // Selección por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando listas: {ex.Message}");
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
                filtrados = filtrados.Where(p => p.Categoria == CategoriaSeleccionada.CategoriaID);
            }

            if (SubCategoriaSeleccionada != null && SubCategoriaSeleccionada.SubCategoriaID != "TODAS")
            {
                filtrados = filtrados.Where(p => p.SubCategoria == SubCategoriaSeleccionada.SubCategoriaID);
            }

            ProductesFiltrats = new ObservableCollection<ProducteInventariAmbNom>(filtrados);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
