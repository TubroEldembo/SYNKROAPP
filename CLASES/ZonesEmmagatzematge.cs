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
        [FirestoreProperty]
        public int ZonaEmmagatzematgeID { get; set; }  // idZonaEmmagatzematge

        [FirestoreProperty]
        public string Nom { get; set; }  // nom

        [FirestoreProperty]
        public int MagatzemPertanyent { get; set; }  // idMagatzem (relación con Magatzem)

        [FirestoreProperty]
        public List<int> Productes { get; set; } = new List<int>();  // Relación con ProducteInventari (lista de IDs)

        public ZonaEmmagatzematge() { }

        public ZonaEmmagatzematge(int idZonaEmmagatzematge, string nom, int magatzemPertanyent, List<int> productes)
        {
            ZonaEmmagatzematgeID = idZonaEmmagatzematge;
            Nom = nom;
            MagatzemPertanyent = magatzemPertanyent;
            Productes = productes;
        }
    }
}
