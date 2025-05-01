using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class CategoriaGenerica
    {
        [FirestoreProperty]
        public string CategoriaID { get; set; }

        [FirestoreProperty]
        public string Nom { get; set; }

        [FirestoreProperty]
        public string Descripcio { get; set; }

        public CategoriaGenerica() { }

        public CategoriaGenerica(string categoriaID, string nom, string descripcio)
        {
            CategoriaID = categoriaID;
            Nom = nom;
            Descripcio = descripcio;
        }
    }

}
