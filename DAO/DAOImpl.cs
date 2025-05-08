using Firebase.Storage;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using SYNKROAPP.CLASES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SYNKROAPP.DAO
{
    public class DAOImpl: IDAO
    {
        FirestoreDb db;
        private string token;
        private string projectName;

        public DAOImpl(string token, string projectName)
        {
            this.token = token;
            this.projectName = projectName;
            SetCredentials();
            db = FirestoreDb.Create(projectName);
        }

        private void SetCredentials()
        {
            string pathToServiceAccount = "credencials.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToServiceAccount);
        }

        public async Task RegistrarUsuariAmbEmpresa(Usuaris unUsuari, Empreses unaEmpresa)
        {
            try
            {
                if (unUsuari == null)
                    throw new Exception("L'usuari no és vàlid.");

                if (unaEmpresa == null)
                    throw new Exception("L'empresa no és vàlida.");

                // 1. Crear documento empresa con ID autogenerado
                DocumentReference empresaRef = db.Collection("Empreses").Document();
                unaEmpresa.EmpresaID = empresaRef.Id;
                await empresaRef.SetAsync(unaEmpresa);

                // 2. Crear documento usuario con ID autogenerado
                DocumentReference usuariRef = db.Collection("Usuaris").Document();
                unUsuari.UsuariID = usuariRef.Id;
                unUsuari.EmpresaID = empresaRef.Id;

                await usuariRef.SetAsync(unUsuari);

                // 3. Añadir el ID del usuario a la lista Usuaris de la empresa
                await empresaRef.UpdateAsync("Usuaris", FieldValue.ArrayUnion(usuariRef.Id));

                MessageBox.Show("Usuari i empresa registrats correctament!", "Èxit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durant el registre: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Task AddUsuari(Usuaris unUsuari)
        {
            throw new NotImplementedException();
        }
        public Task AddEmpresa(Empreses unaEmpresa)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmpresa(Empreses unaEmpresa)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUsuari(Usuaris unUsuari)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuaris> GetUsuariByEmail(string targetEmail)
        {
            CollectionReference colRef = db.Collection("Usuaris");
            Query query = colRef.WhereEqualTo("Email", targetEmail);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                MessageBox.Show($"No se encontró ningún usuario con el correo: {targetEmail}");
                return null;
            }

            DocumentSnapshot doc = snapshot.Documents.First();

            Console.WriteLine($"Documento encontrado: {doc.Id}");
            foreach (var kv in doc.ToDictionary())
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            Usuaris usuario = doc.ConvertTo<Usuaris>();
            return usuario;
        }

        public Task UpdateEmpresa(Empreses unaEmpresa)
        {
            throw new NotImplementedException();
        }

        public async Task<Empreses> GetEmpresaByID(string empresaID)
        {
            try
            {
                var docRef = db.Collection("Empreses").Document(empresaID);
                var snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Empreses empresa = snapshot.ConvertTo<Empreses>();
                    return empresa;
                }
                else
                    return null;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error al obtener empresa con ID {empresaID}", ex);
            }
        }

        public Task UpdateUsuari(Usuaris unUsuari)
        {
            throw new NotImplementedException();
        }

        public async Task AddAlmacen(Empreses unaEmpresa, Magatzems unMagatzem)
        {
            try
            {
                if (unaEmpresa == null || string.IsNullOrEmpty(unaEmpresa.EmpresaID))
                    throw new Exception("L'empresa no és valida o no té ID");
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<Magatzems> CrearAlmacen(Empreses unaEmpresa, Magatzems nouMagatzem)
        {
            try
            {
                if (string.IsNullOrEmpty(unaEmpresa.EmpresaID))
                    throw new Exception("La ID de la empresa es obligatoria.");

                if (nouMagatzem == null)
                    throw new Exception("Los datos del almacén no son válidos.");

                DocumentReference empresaRef = db.Collection("Empreses").Document((string)unaEmpresa.EmpresaID);
                CollectionReference almacenesRef = empresaRef.Collection("Magatzems");

                QuerySnapshot snapshot = await almacenesRef.WhereEqualTo("NomMagatzem", nouMagatzem.NomMagatzem).GetSnapshotAsync();

                if (snapshot.Count > 0)
                {
                    DocumentSnapshot existingDoc = snapshot.Documents.First();
                    return existingDoc.ConvertTo<Magatzems>();
                }

                DocumentReference magatzemDocRef = almacenesRef.Document(); // ID autogenerado
                nouMagatzem.MagatzemID = magatzemDocRef.Id;
                nouMagatzem.EmpresaPertanyentID = unaEmpresa.EmpresaID;

                await magatzemDocRef.SetAsync(nouMagatzem);
                await empresaRef.UpdateAsync("Magatzems", FieldValue.ArrayUnion(magatzemDocRef.Id));

                return nouMagatzem;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el almacén: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }


        public async Task<ZonaEmmagatzematge> CrearZonaAlmacen(Magatzems unMagatzem, ZonaEmmagatzematge novaZona)
        {
            try
            {
                if (string.IsNullOrEmpty(unMagatzem.MagatzemID))
                {
                    throw new Exception("Falta la ID del almacén");
                }

                DocumentReference empresaRef = db.Collection("Empreses").Document(unMagatzem.EmpresaPertanyentID);
                DocumentReference magatzemRef = empresaRef.Collection("Magatzems").Document(unMagatzem.MagatzemID);
                CollectionReference zonesRef = magatzemRef.Collection("ZonesEmmagatzematge");

                QuerySnapshot snapshot = await zonesRef.WhereEqualTo("NomZona", novaZona.Nom).GetSnapshotAsync();
                if (snapshot.Count > 0)
                {
                    DocumentSnapshot existingZone = snapshot.Documents.First();
                    return existingZone.ConvertTo<ZonaEmmagatzematge>();
                }

                DocumentReference zonaDocRef = zonesRef.Document();
                novaZona.ZonaEmmagatzematgeID = zonaDocRef.Id;

                await zonaDocRef.SetAsync(novaZona);
                await magatzemRef.UpdateAsync("Zones", FieldValue.ArrayUnion(zonaDocRef.Id));

                return novaZona;
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error al crear la zona: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<List<Magatzems>> DetallesAlmacenes(Empreses empresa)
        {
            try
            {
                if (string.IsNullOrEmpty(empresa.EmpresaID))
                    throw new Exception("El ID de la empresa no es válido");

                DocumentReference empresaRef = db.Collection("Empreses").Document((string)empresa.EmpresaID);
                CollectionReference almacenesRef = empresaRef.Collection("Magatzems");

                // Obtener todos los documentos (almacenes) de la colección
                QuerySnapshot snapshot = await almacenesRef.GetSnapshotAsync();

                if (snapshot.Documents.Any())
                {
                    List<Magatzems> almacenes = new List<Magatzems>();

                    foreach (var document in snapshot.Documents)
                    {
                        // Convertir cada documento a un objeto Magatzems
                        Magatzems almacen = document.ConvertTo<Magatzems>();
                        almacenes.Add(almacen);
                    }

                    return almacenes;

                }
                else
                {
                    MessageBox.Show("No se encontraron almacenes para esta empresa.");
                    return new List<Magatzems>(); // Retornar una lista vacía si no hay almacenes
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error al obtener los almacenes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null; // En caso de error, retornar null
            }
        }


        public async Task<List<ZonaEmmagatzematge>> DetallesZonasAlmacen(Magatzems unMagatzem)
        {
            try
            {
                if (string.IsNullOrEmpty(unMagatzem.MagatzemID))
                    throw new Exception("El ID del almacén no es válido");

                DocumentReference empresaRef = db.Collection("Empreses").Document(unMagatzem.EmpresaPertanyentID);
                DocumentReference almacenesRef = empresaRef.Collection("Magatzems").Document(unMagatzem.MagatzemID);
                CollectionReference zonesRef = almacenesRef.Collection("ZonesEmmagatzematge");

                QuerySnapshot snapshot = await zonesRef.GetSnapshotAsync();

                if (snapshot.Documents.Any())
                {
                    List<ZonaEmmagatzematge> zonesMagatzem = new List<ZonaEmmagatzematge>();
                    foreach (var document in snapshot.Documents)
                    {
                        ZonaEmmagatzematge zonaMagatzem = document.ConvertTo<ZonaEmmagatzematge>();
                        zonesMagatzem.Add(zonaMagatzem);
                    }

                    return zonesMagatzem;
                }
                else
                {
                    MessageBox.Show("No se encontraron zonas de almacenamiento para esta almacén.");
                    return new List<ZonaEmmagatzematge>(); // Retornar una lista vacía si no hay almacenes
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error al obtener las zonas de almacenamiento: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null; // En caso de error, retornar null
            }
        }

        public async Task<bool> AddProducts2Zone(ProducteGeneral producteGeneral, DetallProducte detallProducte, ProductesInventari inventari)
        {
            try
            {
                // Validaciones iniciales
                if (producteGeneral == null || inventari == null || detallProducte == null)
                    throw new Exception("Datos del producto no válidos.");

                if (string.IsNullOrEmpty(inventari.EmpresaID) ||
                    string.IsNullOrEmpty(inventari.MagatzemID) ||
                    string.IsNullOrEmpty(inventari.ZonaID))
                    throw new Exception("Faltan IDs de Empresa, Magatzem o Zona.");

                // Paso 1: Crear ProducteGeneral
                DocumentReference productRef = await db.Collection("ProducteGeneral").AddAsync(producteGeneral);
                string productID = productRef.Id;

                // Paso 2: Agregar detall como subcolección del producto
                detallProducte.ProducteID = productID;
                await productRef.Collection("DetallProducte").AddAsync(detallProducte);

                // Paso 3: Preparar Inventari con el ProducteID
                inventari.ProducteID = productID;

                // Paso 4: Verificar que la zona ya existe
                DocumentReference zonaRef = db
                    .Collection("Empreses").Document(inventari.EmpresaID)
                    .Collection("Magatzems").Document(inventari.MagatzemID)
                    .Collection("ZonesEmmagatzematge").Document(inventari.ZonaID);

                DocumentSnapshot zonaSnapshot = await zonaRef.GetSnapshotAsync();
                if (!zonaSnapshot.Exists)
                {
                    throw new Exception("La zona especificada no existe. No se puede añadir el producto.");
                }

                // Paso 5: Insertar el inventario como subcolección de esa zona
                DocumentReference inventariRef = zonaRef
                    .Collection("ProductesInventari")
                    .Document(); // ID autogenerado

                string inventariID = inventariRef.Id;
                inventari.IDProducteInventari = inventariID;

                await inventariRef.SetAsync(inventari);

                // Paso 6: Añadir ID a la lista de productos de la zona
                await zonaRef.UpdateAsync("Productes", FieldValue.ArrayUnion(inventariID));

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el producto: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<List<ProductesInventari>> ProductosEn1Zona(ZonaEmmagatzematge zona)
        {
            DocumentReference zonaRef = db.Collection("Empreses").Document(zona.EmpresaID)
                                          .Collection("Magatzems").Document(zona.MagatzemPertanyent)
                                          .Collection("ZonesEmmagatzematge").Document(zona.ZonaEmmagatzematgeID);

            var productesSnapshot = await zonaRef.Collection("ProductesInventari").GetSnapshotAsync();

            if (productesSnapshot.Documents.Any())
            {
                List<ProductesInventari> lstProductesInventari = new List<ProductesInventari>();

                foreach (var doc in productesSnapshot.Documents)
                {
                    ProductesInventari producte = doc.ConvertTo<ProductesInventari>();
                    lstProductesInventari.Add(producte);
                }

                return lstProductesInventari;
            }
            else
            {
                MessageBox.Show("No se encontraron productos en esta zona del almacén.");
                return new List<ProductesInventari>(); // Retornar una lista vacía si no hay almacenes
            }
        }

        public async Task<DocumentSnapshot> GetProducteGeneralPorID(string producteID)
        {
            var docRef = db.Collection("ProducteGeneral").Document(producteID);
            return await docRef.GetSnapshotAsync();
        }


        /// <summary>
        /// Se utiliza diccionario para luego a la hora de filtrar manualmente las subcategorias de un categoria,
        /// de esta manera ya asociamos la subcategoria directamente a la categoria. Y no hace irlo filtrando
        /// manualmente
        /// </summary>
        /// <returns></returns>
        public async Task<(List<CategoriaGenerica>, Dictionary<string, List<SubcategoriaGenerica>>)> ObtenirCategoriesGeneriques()
        {
            List<CategoriaGenerica> catGenList = new List<CategoriaGenerica>();
            Dictionary<string, List<SubcategoriaGenerica>> subCatDict = new Dictionary<string, List<SubcategoriaGenerica>>();

            var snapshot = await db.Collection("CategoriesGeneriques").GetSnapshotAsync();

            foreach (var doc in snapshot.Documents)
            {
                var categoria = doc.ConvertTo<CategoriaGenerica>();
                categoria.CategoriaID = doc.Id;
                catGenList.Add(categoria);

                var subcats = await doc.Reference.Collection("Subcategories").GetSnapshotAsync();
                subCatDict[categoria.CategoriaID] = subcats.Documents
                    .Select(s => s.ConvertTo<SubcategoriaGenerica>())
                    .ToList();

            }

            return (catGenList, subCatDict);

        }

        /// <summary>
        /// Se utiliza diccionario para luego a la hora de filtrar manualmente las subcategorias de un categoria,
        /// de esta manera ya asociamos la subcategoria directamente a la categoria. Y no hace irlo filtrando
        /// manualmente
        /// </summary>
        /// <returns></returns>
        public async Task<(List<Categories>, Dictionary<string, List<Subcategories>>)> ObtenirCategoriesPersonalitzades(string empresaID)
        {
            List<Categories> catList = new List<Categories>();
            Dictionary<string, List<Subcategories>> subCatDict = new Dictionary<string, List<Subcategories>>();

            var snapshot = await db.Collection("Empreses")
                .Document(empresaID)
                .Collection("Categories")
                .GetSnapshotAsync();

            foreach (var doc in snapshot.Documents)
            {
                Categories categoria = doc.ConvertTo<Categories>();
                categoria.CategoriaID = doc.Id;
                catList.Add(categoria);

                var subcats = await doc.Reference.Collection("Subcategories").GetSnapshotAsync();
                subCatDict[categoria.CategoriaID] = subcats.Documents
                    .Select(s => s.ConvertTo<Subcategories>())
                    .ToList();

            }
            return (catList, subCatDict);
        }

        public async Task<List<Magatzems>> ObtenerLosAlmacenesTotalesDeLaEmpresa(string empresaID)
        {
            try
            {
                List<Magatzems> almacenes = new List<Magatzems>();
                CollectionReference magatzemRef = db.Collection("Empreses").Document(empresaID).Collection("Magatzems");
                var snapshot = await magatzemRef.GetSnapshotAsync();

                foreach (var doc in snapshot.Documents)
                {
                    if (doc.Exists)
                    {
                        Magatzems almmacen = doc.ConvertTo<Magatzems>();
                        almacenes.Add(almmacen);
                    }
                }

                return almacenes;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error al obtener los almacenes de la empresa {empresaID}", ex);
            }
          
        }

        public async Task<Dictionary<string, int>> ObtenerProductosEnVentaPorCategoria(string empresaID)
        {
            Dictionary<string, int> productosPorCategoria = new Dictionary<string, int>();

            var magatzemSnap = await db.Collection("Empreses")
                                       .Document(empresaID)
                                       .Collection("Magatzems")
                                       .GetSnapshotAsync();

            foreach (var magatzemDoc in magatzemSnap.Documents)
            {
                // Obtener todas las zonas del magatzem
                var zonesSnap = await magatzemDoc.Reference
                                                 .Collection("ZonesEmmagatzematge")
                                                 .GetSnapshotAsync();

                foreach (var zonaDoc in zonesSnap.Documents)
                {
                    // Obtener productos de inventario de cada zona
                    var inventariSnap = await zonaDoc.Reference
                                             .Collection("ProductesInventari")
                                             .GetSnapshotAsync();

                    foreach (var inventariDoc in inventariSnap.Documents)
                    {
                        ProductesInventari inventari = inventariDoc.ConvertTo<ProductesInventari>();
                        if (inventari?.ProducteID == null)
                            continue;

                        //  Obtener el ProducteGeneral correspondiente
                        var producteGeneralDoc = await db.Collection("ProducteGeneral")
                                                .Document(inventari.ProducteID)
                                                .GetSnapshotAsync();

                        if (!producteGeneralDoc.Exists)
                            continue;

                        ProducteGeneral producteGeneral = producteGeneralDoc.ConvertTo<ProducteGeneral>();
                        string categoriaID = producteGeneral?.CategoriaID ?? "Sin categoría";

                        //  Obtener el detalle del producto y verificar si está en venta
                        var detallSnap = await producteGeneralDoc.Reference
                                                        .Collection("DetallProducte")
                                                        .GetSnapshotAsync();

                        DetallProducte detall = detallSnap.Documents.FirstOrDefault()?.ConvertTo<DetallProducte>();

                        if (detall != null && detall.EnVenda)
                        {
                            if (productosPorCategoria.ContainsKey(categoriaID))
                                productosPorCategoria[categoriaID]++;
                            else
                                productosPorCategoria[categoriaID] = 1;
                        }

                    }
                }
            }

            return productosPorCategoria;

        }

        public async Task<List<ZonaProductoViewModel>> ObtenerZonasDeProducto(string producteID)
        {
            List<ZonaProductoViewModel> zonasConProducto = new List<ZonaProductoViewModel>();

            CollectionReference empresesRef = db.Collection("Empreses");
            QuerySnapshot empresasSnap = await empresesRef.GetSnapshotAsync();

            foreach (var empresaDoc in empresasSnap.Documents)
            {
                string empresaId = empresaDoc.Id;

                // Accede a cada magatzem de la empresa
                CollectionReference magatzemsRef = empresaDoc.Reference.Collection("Magatzems");
                QuerySnapshot magatzemsSnap = await magatzemsRef.GetSnapshotAsync();

                foreach (var magatzemDoc in magatzemsSnap.Documents)
                {
                    string magatzemId = magatzemDoc.Id;

                    // Accede a las zonas de cada magatzem
                    CollectionReference zonasRef = magatzemDoc.Reference.Collection("ZonesEmmagatzematge");
                    QuerySnapshot zonasSnap = await zonasRef.GetSnapshotAsync();

                    foreach (var zonaDoc in zonasSnap.Documents)
                    {
                        string zonaNombre = zonaDoc.GetValue<string>("Nom");

                        // Obtener todos los documentos de la colección ProductesInventari
                        QuerySnapshot productesSnap = await zonaDoc.Reference
                            .Collection("ProductesInventari")
                            .GetSnapshotAsync();

                        foreach (var inventariDoc in productesSnap.Documents)
                        {
                            // Verificar si el campo ProducteGeneralID coincide con el que buscamos
                            string producteGeneralIdEnInventari = inventariDoc.GetValue<string>("ProducteID");

                            if (producteGeneralIdEnInventari == producteID)
                            {
                                int quantitat = inventariDoc.GetValue<int>("Quantitat");

                                zonasConProducto.Add(new ZonaProductoViewModel
                                {
                                    Almacen = magatzemId,
                                    Zona = zonaNombre,
                                    QuantitatDisponible = quantitat
                                });
                            }
                        }
                    }
                }
            }

            return zonasConProducto;

        }

        #region STORAGE IMAGE
        public async Task<string> StoreImage(string localPath, string nameToStore)
        {
            const string BUCKET = "projecto-synkroapp.firebasestorage.app";

            using var stream = File.Open(localPath, FileMode.Open);
            var file = new FirebaseStorage(BUCKET)
                .Child("imatges")
                .Child($"{nameToStore}")
                .PutAsync(stream);

            return await file;
        }

        public BitmapImage LoadImageFromUrl(string url) 
        {
            try
            {
                var image = new Image();
                var fullFilePath = @$"{url}";

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute); 
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
                return null;
            }
        }


        #endregion

        #region MOVIMIENTO DE PRODUCTOS
        public async Task GuardarMovimientoInventariAsync(MovimentsInventari movimentTraslado, ProductesInventari producte)
        {
            // Sirve para hacer multiples operaciones a la vez, de esta manera no tendremos
            // que ir coleccion x coleccion obteniendo los datos.


            await db.RunTransactionAsync(async transaction =>
            {
                // 1r OPERACIONES DE LECTURA 
                DocumentReference movimentRef = db.Collection("Empreses")
                    .Document(movimentTraslado.EmpresaIDOrigen)
                    .Collection("MovimentsInventari")
                    .Document();

                // Inicializar variables para almacenar los snapshots
                DocumentSnapshot origenSnapshot = null;
                DocumentSnapshot destinoSnapshot = null;

                // Referencias a los documentos que necesitamos leer
                DocumentReference productoZonaOrigenRef = null;
                DocumentReference productoZonaDestinoRef = null;

                switch (movimentTraslado.Tipus)
                {
                    case TipusMoviment.Entrada:
                        // Solo leer la zona de destino
                        productoZonaDestinoRef = db.Collection("Empreses")
                            .Document(movimentTraslado.EmpresaIDOrigen)
                            .Collection("Magatzems")
                            .Document(movimentTraslado.MagatzemIDDesti)
                            .Collection("ZonesEmmagatzematge")
                            .Document(movimentTraslado.ZonaDestiID)
                            .Collection("ProductesInventari")
                            .Document(movimentTraslado.ProducteInventariID);

                        destinoSnapshot = await transaction.GetSnapshotAsync(productoZonaDestinoRef);
                    break;

                    case TipusMoviment.Sortida:
                        // Solo leer la zona de origen
                        productoZonaOrigenRef = db.Collection("Empreses")
                            .Document(movimentTraslado.EmpresaIDOrigen)
                            .Collection("Magatzems")
                            .Document(movimentTraslado.MagatzemIDOrigen)
                            .Collection("ZonesEmmagatzematge")
                            .Document(movimentTraslado.ZonaOrigenID)
                            .Collection("ProductesInventari")
                            .Document(movimentTraslado.ProducteInventariID);

                        origenSnapshot = await transaction.GetSnapshotAsync(productoZonaOrigenRef);
                    break;

                    case TipusMoviment.TrasllatIntern:
                        // Leer ambas zonas
                        productoZonaOrigenRef = db.Collection("Empreses")
                            .Document(movimentTraslado.EmpresaIDOrigen)
                            .Collection("Magatzems")
                            .Document(movimentTraslado.MagatzemIDOrigen)
                            .Collection("ZonesEmmagatzematge")
                            .Document(movimentTraslado.ZonaOrigenID)
                            .Collection("ProductesInventari")
                            .Document(movimentTraslado.ProducteInventariID);

                        productoZonaDestinoRef = db.Collection("Empreses")
                            .Document(movimentTraslado.EmpresaIDOrigen)
                            .Collection("Magatzems")
                            .Document(movimentTraslado.MagatzemIDDesti)
                            .Collection("ZonesEmmagatzematge")
                            .Document(movimentTraslado.ZonaDestiID)
                            .Collection("ProductesInventari")
                            .Document(movimentTraslado.ProducteInventariID);

                        origenSnapshot = await transaction.GetSnapshotAsync(productoZonaOrigenRef);
                        destinoSnapshot = await transaction.GetSnapshotAsync(productoZonaDestinoRef);
                    break;
                }

                // 2. DESPUÉS TODAS LAS ESCRITURAS (WRITES)

                // Guardar el movimiento
                movimentTraslado.MovimentID = movimentRef.Id;
                transaction.Set(movimentRef, movimentTraslado);

                // Actualizar inventarios según tipo de movimiento
                switch (movimentTraslado.Tipus)
                {
                    case TipusMoviment.Entrada:
                        // Actualizar zona destino
                        ActualizarZonaEnTransaccion(
                            transaction,
                            producte,
                            productoZonaDestinoRef,
                            destinoSnapshot,
                            movimentTraslado.ProducteInventariID,
                            movimentTraslado.Quantitat,
                            movimentTraslado.ZonaDestiID);
                        break;

                    case TipusMoviment.Sortida:
                        // Actualizar zona origen
                        ActualizarZonaEnTransaccion(
                            transaction,
                            producte,
                            productoZonaOrigenRef,
                            origenSnapshot,
                            movimentTraslado.ProducteInventariID,
                            -movimentTraslado.Quantitat,
                            movimentTraslado.ZonaOrigenID);
                        break;

                    case TipusMoviment.TrasllatIntern:
                        // Actualizar ambas zonas
                        ActualizarZonaEnTransaccion(
                            transaction,
                            producte,
                            productoZonaOrigenRef,
                            origenSnapshot,
                            movimentTraslado.ProducteInventariID,
                            -movimentTraslado.Quantitat,
                            movimentTraslado.ZonaOrigenID);

                        ActualizarZonaEnTransaccion(
                            transaction,
                            producte,
                            productoZonaDestinoRef,
                            destinoSnapshot,
                            movimentTraslado.ProducteInventariID,
                            movimentTraslado.Quantitat,
                            movimentTraslado.ZonaDestiID);
                        break;
                }

            });
        }

        private void ActualizarZonaEnTransaccion(
                Transaction transaction,
                ProductesInventari producte,
                DocumentReference productoZonaRef,
                DocumentSnapshot docSnapshot,
                string producteGeneralID,
                int cantidadCambio,
                string zonaID)
        {
            if (docSnapshot.Exists)
            {
                Dictionary<string, object> productoData = docSnapshot.ToDictionary();
                int cantidadActual = Convert.ToInt32(productoData["Quantitat"]);
                int nuevaCantidad = cantidadActual + cantidadCambio;

                // Asegurarse de que la cantidad no sea negativa
                if (nuevaCantidad < 0)
                {
                    throw new Exception($"No hay suficiente stock en la zona {zonaID} para el producto {producteGeneralID}");
                }

                // Actualizamos solo el campo de cantidad
                transaction.Update(productoZonaRef, new Dictionary<string, object>
                {
                    { "Quantitat", nuevaCantidad }
                });
            }
            else if (cantidadCambio > 0)
            {
                // Si el producto no existe en la zona pero estamos añadiendo (solo para entradas o traslados)
                ProductesInventari nuevoProducto = new ProductesInventari
                {
                    ProducteID = producte.ProducteID,
                    Quantitat = cantidadCambio,
                    Estat = producte.Estat, // Puedes ajustar esto según tu lógica
                    CodiReferencia = producte.CodiReferencia, // Si no lo tienes, deja vacío o genera uno
                    ZonaID = zonaID,
                    MagatzemID = producte.MagatzemID,
                    EmpresaID = producte.EmpresaID
                };

                // Creamos un nuevo documento para el producto en la zona
                transaction.Set(productoZonaRef, nuevoProducto);
            }
            else
            {
                // Intentamos restar de un producto que no existe
                throw new Exception($"No existe el producto {producteGeneralID} en la zona {zonaID}");
            }
        }

        public async Task<DocumentSnapshot> GetProducteInventariPorID(string producteID)
        {
            CollectionReference empresesRef = db.Collection("Empreses");
            QuerySnapshot empresasSnap = await empresesRef.GetSnapshotAsync();

            foreach (var empresaDoc in empresasSnap.Documents)
            {
                var magatzemsSnap = await empresaDoc.Reference.Collection("Magatzems").GetSnapshotAsync();

                foreach (var magatzemDoc in magatzemsSnap.Documents)
                {
                    var zonasSnap = await magatzemDoc.Reference.Collection("ZonesEmmagatzematge").GetSnapshotAsync();

                    foreach (var zonaDoc in zonasSnap.Documents)
                    {
                        var productesSnap = await zonaDoc.Reference
                            .Collection("ProductesInventari")
                            .WhereEqualTo("ProducteID", producteID)
                            .Limit(1) // Solo queremos el primero
                            .GetSnapshotAsync();

                        if (productesSnap.Count > 0)
                        {
                            return productesSnap.Documents[0];  // Primer documento que coincide
                        }
                    }
                }
            }

            return null; // No encontrado
        }

        #endregion


    }
}
