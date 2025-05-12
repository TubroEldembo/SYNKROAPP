using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using SYNKROAPP.CLASES;
using Microsoft.Win32;
using System.Windows;
using SYNKROAPP.DAO;

namespace SYNKROAPP.Vistes.Vista_Productos
{
    public class ImportarProductosViewModel : INotifyPropertyChanged
    {
        private IDAO dao;
        private Empreses empresa;

        public ImportarProductosViewModel(IDAO dao, Empreses empresa)
        {
            this.dao = dao;
            this.empresa = empresa;
            ProductosImportados = new ObservableCollection<ProducteAmbDetall>();
            InicializarAsync();
        }

        public async Task InicializarAsync()
        {
            var almacenes = await dao.ObtenerLosAlmacenesTotalesDeLaEmpresa(empresa.EmpresaID);
            ListaAlmacenes.Clear();
            foreach (var alm in almacenes)
                ListaAlmacenes.Add(alm);

            ListaZonas.Clear();
            foreach (var alm in almacenes)
            {
                var zonas = await dao.DetallesZonasAlmacen(alm);
                foreach (var zona in zonas)
                    ListaZonas.Add(zona);
            }
        }

        public ObservableCollection<Magatzems> ListaAlmacenes { get; set; } = new ObservableCollection<Magatzems>();
        private Magatzems _almacenSeleccionado;
        public Magatzems AlmacenSeleccionado
        {
            get => _almacenSeleccionado;
            set
            {
                _almacenSeleccionado = value;
                OnPropertyChanged(nameof(AlmacenSeleccionado));
                _ = CargarZonasPorAlmacenAsync();
            }
        }
        public ObservableCollection<ZonaEmmagatzematge> ListaZonas { get; set; } = new ObservableCollection<ZonaEmmagatzematge>();

        private ZonaEmmagatzematge _zonaSeleccionada;
        public ZonaEmmagatzematge ZonaSeleccionada
        {
            get => _zonaSeleccionada;
            set { _zonaSeleccionada = value; OnPropertyChanged(nameof(ZonaSeleccionada)); }
        }

        private async Task CargarZonasPorAlmacenAsync()
        {
            if (AlmacenSeleccionado == null) return;

            var zonas = await dao.DetallesZonasAlmacen(AlmacenSeleccionado);
            ListaZonas.Clear();
            foreach (var zona in zonas)
                ListaZonas.Add(zona);
        }

        private ObservableCollection<ProducteAmbDetall> _productosImportados;
        public ObservableCollection<ProducteAmbDetall> ProductosImportados
        {
            get => _productosImportados;
            set
            {
                _productosImportados = value;
                OnPropertyChanged();
            }
        }

        private string _rutaArchivo;
        public string RutaArchivo
        {
            get => _rutaArchivo;
            set
            {
                _rutaArchivo = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> GuardarProductosImportadosConInventarioAsync()
        {
            bool importacionExitosa = false;

            foreach (ProducteAmbDetall producto in ProductosImportados)
            {
                try
                {
                    ZonaEmmagatzematge zona = ZonaSeleccionada;

                    // ➤ Paso 1: Crear producto general y su detalle

                    await dao.AddProductoGeneral(producto.Producte, producto.Detall);

                    // ➤ Paso 2: Comprobar si ya existe en inventario
                    string inventariID = await dao.CheckProductInZona(
                        producto.Producte.ProducteID,
                        empresa.EmpresaID,
                        zona.MagatzemPertanyent,
                        zona.ZonaEmmagatzematgeID);

                    bool productExists = !string.IsNullOrEmpty(inventariID);
                    ProductesInventari inventari;

                    // ➤ Paso 3: Crear o recuperar inventario
                    if (!productExists)
                    {
                        inventari = new ProductesInventari
                        {
                            ProducteID = producto.Producte.ProducteID,
                            EmpresaID = empresa.EmpresaID,
                            Quantitat = 1, // O según cantidad importada
                            Estat = "Nuevo",
                            ZonaID = zona.ZonaEmmagatzematgeID,
                            MagatzemID = zona.MagatzemPertanyent,
                            CodiReferencia = producto.Producte.SKU,
                            IDProducteInventari = GenerateInventoryId()
                        };

                        //await dao.AddInventariToZona(inventari);
                    }
                    else
                    {
                        inventari = await dao.GetProductoInventario(
                            empresa.EmpresaID,
                            zona.MagatzemPertanyent,
                            zona.ZonaEmmagatzematgeID,
                            inventariID);
                    }

                    // ➤ Paso 4: Registrar movimiento de inventario
                    if (!string.IsNullOrEmpty(inventari.IDProducteInventari))
                    {
                        var moviment = new MovimentsInventari
                        {
                            ProducteInventariID = inventari.IDProducteInventari,
                            EmpresaIDOrigen = empresa.EmpresaID,
                            EmpresaIDDesti = empresa.EmpresaID,
                            MagatzemIDOrigen = zona.MagatzemPertanyent,
                            MagatzemIDDesti = zona.MagatzemPertanyent,
                            ZonaOrigenID = zona.ZonaEmmagatzematgeID,
                            ZonaDestiID = zona.ZonaEmmagatzematgeID,
                            Tipus = TipusMoviment.Entrada,
                            Quantitat = inventari.Quantitat,
                            Data = DateTime.UtcNow,
                            Notes = productExists ? "Importación de producto existente" : "Importación nuevo producto"
                        };

                        await dao.GuardarMovimientoInventariAsync(moviment, inventari, productExists);
                        importacionExitosa = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar producto {producto.Producte.Nom}: {ex.Message}");
                }
            }


            MessageBox.Show("Productos e inventario importados correctamente.");
            return importacionExitosa;

        }

        public async Task SeleccionarArchivoYLeerAsync()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos JSON (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                RutaArchivo = openFileDialog.FileName;

                try
                {
                    await LeerProductosDesdeJsonAsync(RutaArchivo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer el archivo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async Task LeerProductosDesdeJsonAsync(string filePath)
        {
            ProductosImportados.Clear();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("No se ha encontrado el archivo: " + filePath);

            await Task.Run(() =>
            {
                var lista = dao.LlegeixProductesJson(filePath);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var producto in lista)
                        ProductosImportados.Add(producto);
                });
            });

            OnPropertyChanged(nameof(ProductosImportados));
        }
        private string GenerateInventoryId()
        {
            // Genera un ID único para el inventario (similar a lo que hace Firestore)
            return Guid.NewGuid().ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
