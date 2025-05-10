using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    using Google.Cloud.Firestore;

    [FirestoreData]
    public class DetallProducte
    {
        [FirestoreDocumentId]
        public string DetallProducteID { get; set; }

        [FirestoreProperty]
        public string EmpresaID { get; set; }


        [FirestoreProperty]
        public double Preu { get; set; }

        [FirestoreProperty]
        public bool Disponible { get; set; }

        [FirestoreProperty]
        public bool EnVenda { get; set; }

        [FirestoreProperty]
        public string ProducteID { get; set; } // Just one reference to ProducteGeneral

        public DetallProducte() { }

        public DetallProducte(string? detallProducteID, double preu, bool disponible, bool enVenda, string empresaID)
        {
            DetallProducteID = detallProducteID ?? Guid.NewGuid().ToString(); // Genera un ID si es null
            Preu = preu;
            Disponible = disponible;
            EnVenda = enVenda;
            EmpresaID = empresaID;
        }
    }

}
