using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class ZonaEmmagatzematge
    {
        [FirestoreDocumentId]
        public string ZonaEmmagatzematgeID { get; set; }  // idZonaEmmagatzematge

        [FirestoreProperty]
        public string Nom { get; set; }  // nom

        [FirestoreProperty]
        public string MagatzemPertanyent { get; set; }  // idMagatzem (relación con Magatzem)

        [FirestoreProperty]
        public string EmpresaID { get; set; }  // idMagatzem (relación con Magatzem)

        [FirestoreProperty]
        public int Capacitat { get; set; }  


        [FirestoreProperty]
        public List<string> Productes { get; set; } = new List<string>();

        public int NProductos => Productes?.Count ?? 0;

        public string AlmacenPerteneciente => MagatzemPertanyent;

        public ZonaEmmagatzematge() { }

        public ZonaEmmagatzematge(string idZonaEmmagatzematge, string empresaID ,string nom, string magatzemPertanyent, List<string> productes, int capacitat)
        {
            ZonaEmmagatzematgeID = idZonaEmmagatzematge;
            EmpresaID = empresaID;
            Nom = nom;
            MagatzemPertanyent = magatzemPertanyent;
            Productes = productes;
            Capacitat = capacitat;
        }
    }
}
