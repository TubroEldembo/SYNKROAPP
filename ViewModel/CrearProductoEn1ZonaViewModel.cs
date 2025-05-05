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
    public class CrearProductoEn1ZonaViewModel : INotifyPropertyChanged
    {
        public IDAO _dao;
        private ZonaEmmagatzematge zona;
        private Dictionary<string, List<string>> _diccionarioSubcategorias = new();
        private Dictionary<string, string> _mapCategoriaNombreToID = new();
        private Dictionary<string, string> _mapSubcategoriaNombreToID = new();

        public CrearProductoEn1ZonaViewModel(IDAO dao, ZonaEmmagatzematge zona)
        {
            this._dao = dao;
            this.zona = zona;
        }

        private string _nomZona;
        public string NomZona
        {
            get => _nomZona;
            set { _nomZona = value; OnPropertyChanged(nameof(NomZona)); }
        }

        private string _nombreProducto;
        public string NombreProducto
        {
            get => _nombreProducto;
            set { _nombreProducto = value; OnPropertyChanged(nameof(NombreProducto)); }
        }

        private string _descripcionProducto;
        public string DescripcionProducto
        {
            get => _descripcionProducto;
            set { _descripcionProducto = value; OnPropertyChanged(nameof(DescripcionProducto)); }
        }

        private string _sku;
        public string SKU
        {
            get => _sku;
            set { _sku = value; OnPropertyChanged(nameof(SKU)); }
        }

        private bool _esNuevo;
        public bool EsNuevo
        {
            get => _esNuevo;
            set { _esNuevo = value; OnPropertyChanged(nameof(EsNuevo)); }
        }

        private bool _esSeminuevo;
        public bool EsSeminuevo
        {
            get => _esSeminuevo;
            set { _esSeminuevo = value; OnPropertyChanged(nameof(EsSeminuevo)); }
        }

        private bool _estaEnVenta;
        public bool EstaEnVenta
        {
            get => _estaEnVenta;
            set { _estaEnVenta = value; OnPropertyChanged(nameof(EstaEnVenta)); }
        }

        private double _precioProducto;
        public double PrecioProducto
        {
            get => _precioProducto;
            set { _precioProducto = value; OnPropertyChanged(nameof(PrecioProducto)); }
        }

        private int _cantidadInicial;
        public int CantidadInicial
        {
            get => _cantidadInicial;
            set { _cantidadInicial = value; OnPropertyChanged(nameof(CantidadInicial)); }
        }

        public ObservableCollection<string> ListaCategorias { get; set; } = new ObservableCollection<string>();
        private string _categoriaSeleccionada;
        public string CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set
            {
                _categoriaSeleccionada = value;
                OnPropertyChanged(nameof(CategoriaSeleccionada));
                FiltrarSubcategorias();
            }
        }

        private void FiltrarSubcategorias()
        {
            ListaSubcategoriasFiltradas.Clear();
            if (!string.IsNullOrEmpty(CategoriaSeleccionada) && _diccionarioSubcategorias.TryGetValue(CategoriaSeleccionada, out var subcategorias))
            {
                foreach (var sub in subcategorias)
                {
                    ListaSubcategoriasFiltradas.Add(sub);
                }
            }
        }

        public ObservableCollection<string> ListaSubcategoriasFiltradas { get; set; } = new();

        private string _subcategoriaSeleccionada;
        public string SubcategoriaSeleccionada
        {
            get => _subcategoriaSeleccionada;
            set { _subcategoriaSeleccionada = value; OnPropertyChanged(nameof(SubcategoriaSeleccionada)); }
        }

        public ObservableCollection<string> ListaAlmacenes { get; set; } = new ObservableCollection<string>();
        private string _almacenSeleccionado;
        public string AlmacenSeleccionado
        {
            get => _almacenSeleccionado;
            set { _almacenSeleccionado = value; OnPropertyChanged(nameof(AlmacenSeleccionado)); }
        }

        public ObservableCollection<string> ListaZonas { get; set; } = new ObservableCollection<string>();
        private string _zonaSeleccionada;
        public string ZonaSeleccionada
        {
            get => _zonaSeleccionada;
            set { _zonaSeleccionada = value; OnPropertyChanged(nameof(ZonaSeleccionada)); }
        }

        public async Task InicializarCamposAsync()
        {
            try
            {
                NomZona = zona.Nom;
                ListaAlmacenes.Clear();
                ListaZonas.Clear();

                ListaAlmacenes.Add(zona.MagatzemPertanyent);
                ListaZonas.Add(zona.ZonaEmmagatzematgeID);

                AlmacenSeleccionado = zona.MagatzemPertanyent;
                ZonaSeleccionada = zona.ZonaEmmagatzematgeID;

                // Genéricas
                var (categoriasGenericas, subcategoriasGenericas) = await _dao.ObtenirCategoriesGeneriques();
                foreach (var cat in categoriasGenericas)
                {
                    ListaCategorias.Add(cat.Nom);
                    _mapCategoriaNombreToID[cat.Nom] = cat.CategoriaID;

                    if (subcategoriasGenericas.TryGetValue(cat.CategoriaID, out var subs))
                    {
                        _diccionarioSubcategorias[cat.Nom] = subs.Select(s => s.Nom).ToList();
                        foreach (var sub in subs)
                        {
                            _mapSubcategoriaNombreToID[sub.Nom] = sub.SubCategoriaID;
                        }
                    }
                }

                // Personalizadas
                var (categoriasPers, subcategoriasPers) = await _dao.ObtenirCategoriesPersonalitzades(zona.EmpresaID);
                foreach (var cat in categoriasPers)
                {
                    ListaCategorias.Add(cat.Nom);
                    _mapCategoriaNombreToID[cat.Nom] = cat.CategoriaID;

                    if (subcategoriasPers.TryGetValue(cat.CategoriaID, out var subs))
                    {
                        _diccionarioSubcategorias[cat.Nom] = subs.Select(s => s.Nom).ToList();
                        foreach (var sub in subs)
                        {
                            _mapSubcategoriaNombreToID[sub.Nom] = sub.SubCategoriaID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar los campos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task<bool> GuardarProductoEnZona()
        {
            if (string.IsNullOrWhiteSpace(NombreProducto) || string.IsNullOrWhiteSpace(SKU) || PrecioProducto <= 0 || CantidadInicial < 0)
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos correctamente.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            try
            {
                ProducteGeneral producteGeneral = new ProducteGeneral
                {
                    Nom = NombreProducto,
                    CodiReferencia = SKU,
                    Descripcio = DescripcionProducto,
                    CategoriaID = CategoriaSeleccionada,
                    SubCategoriaID = SubcategoriaSeleccionada,
                    SKU = SKU,
                };

                DetallProducte detall = new DetallProducte
                {
                    Preu = PrecioProducto,
                    Disponible = CantidadInicial > 0,
                    EnVenda = EstaEnVenta
                };

                ProductesInventari inventari = new ProductesInventari
                {
                    IDProducteInventari = "",
                    Quantitat = CantidadInicial,
                    EmpresaID = zona.EmpresaID,
                    Estat = EsNuevo ? "Nuevo" : (EsSeminuevo ? "Seminuevo" : "Desconocido"),
                    MagatzemID = zona.MagatzemPertanyent,
                    ZonaID = zona.ZonaEmmagatzematgeID,
                    CodiReferencia = SKU
                };

                bool resultat = await _dao.AddProducts2Zone(producteGeneral, detall, inventari);

                if (resultat)
                {
                    MessageBox.Show("Producto agregado correctamente a la zona.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Error al guardar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al guardar el producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public void GenerarSKU()
        {
            SKU = $"SKU-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
