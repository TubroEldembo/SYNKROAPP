using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class Subcategories
    {
        [FirestoreProperty]
        public string SubCategoriaID { get; set; }

        [FirestoreProperty]
        public string Nom { get; set; }

        [FirestoreProperty]
        public string Descripcio { get; set; }

        [FirestoreProperty]
        public string CategoriaID { get; set; }  // string, no int

        [FirestoreProperty]
        public string EmpresaID { get; set; } // útil para filtros o seguridad

        public Subcategories() { }

        public Subcategories(string subCategoriaID, string nom, string descripcio, string categoriaID, string empresaID)
        {
            SubCategoriaID = subCategoriaID;
            Nom = nom;
            Descripcio = descripcio;
            CategoriaID = categoriaID;
            EmpresaID = empresaID;
        }
    }


}
