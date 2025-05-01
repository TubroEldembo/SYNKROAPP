using Google.Cloud.Firestore;
using System;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class ProducteGeneral
    {
        [FirestoreDocumentId]
        public string ProducteID { get; set; }

        [FirestoreProperty]
        public string Nom { get; set; }

        [FirestoreProperty]
        public string CodiReferencia { get; set; } 

        [FirestoreProperty]
        public string Descripcio { get; set; }

        [FirestoreProperty]
        public string CategoriaID { get; set; }

        [FirestoreProperty]
        public string SubCategoriaID { get; set; }

        [FirestoreProperty]
        public string SKU { get; set; }

        public ProducteGeneral() { }

        public ProducteGeneral(string producteID, string nom, string codiReferencia, string descripcio,
                               string categoriaID, string subCategoriaID, string sKU)
        {
            ProducteID = producteID ?? Guid.NewGuid().ToString();
            Nom = nom;
            CodiReferencia = codiReferencia;
            Descripcio = descripcio;
            CategoriaID = categoriaID;
            SubCategoriaID = subCategoriaID;
            SKU = sKU;
        }
    }
}