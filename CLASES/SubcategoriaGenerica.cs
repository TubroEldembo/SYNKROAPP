using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class SubcategoriaGenerica
    {
        [FirestoreProperty]
        public string SubCategoriaID { get; set; }

        [FirestoreProperty]
        public string Nom { get; set; }

        [FirestoreProperty]
        public string Descripcio { get; set; }

        public SubcategoriaGenerica() { }

        public SubcategoriaGenerica(string subCategoriaID, string nom, string descripcio)
        {
            SubCategoriaID = subCategoriaID;
            Nom = nom;
            Descripcio = descripcio;
        }
    }

}
