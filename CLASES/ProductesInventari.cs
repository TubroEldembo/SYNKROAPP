using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class ProductesInventari
    {
        [FirestoreDocumentId]
        public string IDProducteInventari { get; set; }

        [FirestoreProperty]
        public string ProducteID { get; set; }

        [FirestoreProperty]
        public string CodiReferencia { get; set; }  // Código de referencia del producto

        [FirestoreProperty]
        public string Estat { get; set; }  // Estado del producto (Nuevo, Roto, etc.)

        [FirestoreProperty]
        public int Quantitat { get; set; }  // Cantidad de productos en ese estado

        [FirestoreProperty]
        public string ZonaID { get; set; }  // Zona de almacenamiento

        [FirestoreProperty]
        public string MagatzemID { get; set; }

        [FirestoreProperty]
        public string EmpresaID { get; set; }


        // Constructor
        public ProductesInventari() { }

        public ProductesInventari(string? id, string producteID, string codiReferencia, string estat, int quantitat, string zonaID, string magatzemID, string empresaID)
        {
            IDProducteInventari = id;
            ProducteID = producteID;
            CodiReferencia = codiReferencia;
            Estat = estat;
            Quantitat = quantitat;
            ZonaID = zonaID;
            MagatzemID = magatzemID;
            EmpresaID = empresaID;
        }
    }
}
