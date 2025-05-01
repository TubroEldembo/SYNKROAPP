using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class Categories
    {
        [FirestoreProperty]
        public string CategoriaID { get; set; }  // Ahora es string

        [FirestoreProperty]
        public string Nom { get; set; }

        [FirestoreProperty]
        public string Descripcio { get; set; }

        [FirestoreProperty]
        public string EmpresaID { get; set; } // Para saber a qué empresa pertenece

        public Categories() { }

        public Categories(string categoriaID, string nom, string descripcio, string empresaID)
        {
            CategoriaID = categoriaID;
            Nom = nom;
            Descripcio = descripcio;
            EmpresaID = empresaID;
        }
    }

}
