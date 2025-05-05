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
using System.Windows;

namespace SYNKROAPP.ViewModel
{
    public class DetalleDe1ZonaViewModel: INotifyPropertyChanged
    {
        public IDAO _dao;
        public readonly ZonaEmmagatzematge _zonaSeleccionada;
        public ObservableCollection<Categories> Categories { get; set; } = new();
        public ObservableCollection<Subcategories> Subcategories { get; set; } = new();

        private Dictionary<string, List<Subcategories>> _diccionarioSubcategorias;


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


        private ObservableCollection<string> _estats = new();
        public ObservableCollection<string> Estats
        {
            get => _estats;
            set
            {
                _estats = value;
                OnPropertyChanged(nameof(Estats));
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
            }
        }

        private void ActualizarSubcategorias()
        {
            Subcategories.Clear();

            if (_categoriaSeleccionada != null && _diccionarioSubcategorias.TryGetValue(_categoriaSeleccionada.CategoriaID, out var lista))
            {
                foreach (var subcat in lista)
                {
                    Subcategories.Add(subcat);
                }
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

        public async Task CargarListasParaCombosAsync()
        {
            try
            {
                // Inicializar el diccionario de subcategorías
                _diccionarioSubcategorias = new Dictionary<string, List<Subcategories>>();
                Categories.Clear();

                // 1. Cargar categorías genéricas
                var (categoriasGenericas, subcategoriasGenericasDictionary) = await _dao.ObtenirCategoriesGeneriques();

                // Convertir categorías genéricas al formato Categories y agregarlas
                foreach (var categoriaGenerica in categoriasGenericas)
                {
                    var categoria = new Categories
                    {
                        CategoriaID = categoriaGenerica.CategoriaID,
                        Nom = categoriaGenerica.Nom + " (Genérica)" // Opcional: añadir indicador que es genérica
                                                                    // Otros campos necesarios
                    };

                    Categories.Add(categoria);

                    // Convertir y añadir subcategorías genéricas al diccionario
                    var listaSubcategorias = new List<Subcategories>();
                    if (subcategoriasGenericasDictionary.TryGetValue(categoriaGenerica.CategoriaID, out var subList))
                    {
                        foreach (var subcategoriaGenerica in subList)
                        {
                            listaSubcategorias.Add(new Subcategories
                            {
                                SubCategoriaID = subcategoriaGenerica.SubCategoriaID,
                                Nom = subcategoriaGenerica.Nom,
                                CategoriaID = categoriaGenerica.CategoriaID
                                // Otros campos necesarios
                            });
                        }

                        _diccionarioSubcategorias[categoriaGenerica.CategoriaID] = listaSubcategorias;
                    }
                }

                // 2. Cargar categorías personalizadas si hay una empresa válida
                string empresaID = _zonaSeleccionada.EmpresaID;

                if (!string.IsNullOrEmpty(empresaID))
                {
                    var (categoriasPersonalizadas, subcategoriasPersonalizadasDictionary) = await _dao.ObtenirCategoriesPersonalitzades(empresaID);

                    // Agregar categorías personalizadas
                    foreach (var categoriaPersonalizada in categoriasPersonalizadas)
                    {
                        // Opcional: marcar como personalizada en el nombre
                        // categoriaPersonalizada.Nom += " (Personalizada)";

                        Categories.Add(categoriaPersonalizada);

                        // Añadir subcategorías personalizadas al diccionario
                        if (subcategoriasPersonalizadasDictionary.TryGetValue(categoriaPersonalizada.CategoriaID, out var subcategorias))
                        {
                            _diccionarioSubcategorias[categoriaPersonalizada.CategoriaID] = subcategorias;
                        }
                    }
                }

                // 3. Cargar estados disponibles
                Estats.Clear();
                Estats.Add("Todos");
                Estats.Add("Nuevo");
                Estats.Add("Usado");
                Estats.Add("Defectuoso");
                Estats.Add("En reparación");
                EstatSeleccionat = "Todos"; // Estado por defecto

                // Si ya hay una categoría seleccionada, actualizar las subcategorías
                if (CategoriaSeleccionada != null)
                {
                    ActualizarSubcategorias();
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show($"Error cargando listas para combos: {ex.Message}");
            }
        }


    }
}
