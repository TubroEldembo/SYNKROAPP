using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using SYNKROAPP.CLASES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public Task AddEmpresa(Usuaris unaEmpresa)
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

        public Task UpdateUsuari(Usuaris unUsuari)
        {
            throw new NotImplementedException();
        }

        public async Task AddAlmacen(Usuaris unaEmpresa, Magatzems unMagatzem)
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

        public async Task<Magatzems> CrearAlmacen(Usuaris unaEmpresa, Magatzems nouMagatzem)
        {
            try
            {
                if (string.IsNullOrEmpty(unaEmpresa.EmpresaID))
                    throw new Exception("La ID de la empresa es obligatoria.");

                if (nouMagatzem == null)
                    throw new Exception("Los datos del almacén no son válidos.");

                DocumentReference empresaRef = db.Collection("Empreses").Document(unaEmpresa.EmpresaID);
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

        public async Task<List<Magatzems>> DetallesAlmacenes(Usuaris empresa)
        {
            try
            {
                if (string.IsNullOrEmpty(empresa.EmpresaID))
                    throw new Exception("El ID de la empresa no es válido");

                DocumentReference empresaRef = db.Collection("Empreses").Document(empresa.EmpresaID);
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
                var categoria = doc.ConvertTo<Categories>();
                categoria.CategoriaID = doc.Id;
                catList.Add(categoria);

                var subcats = await doc.Reference.Collection("Subcategories").GetSnapshotAsync();
                subCatDict[categoria.CategoriaID] = subcats.Documents
                    .Select(s => s.ConvertTo<Subcategories>())
                    .ToList();

            }
            return (catList, subCatDict);
        }
    }
}
