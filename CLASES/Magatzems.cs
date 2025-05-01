using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class Magatzems
    {
        [FirestoreDocumentId]
        public string MagatzemID { get; set; }

        [FirestoreProperty]
        public string EmpresaPertanyentID { get; set; }

        [FirestoreProperty]
        public string NomMagatzem { get; set; }

        [FirestoreProperty]
        public Adreça Ubicacio { get; set; }

        [FirestoreProperty]
        public List<string> Zones { get; set; } = new List<string>();

        public int NumeroDeZonas => Zones?.Count ?? 0;


        [FirestoreProperty]
        public bool MagatzemPerDefecte { get; set; }

        public Magatzems() { }

        public Magatzems(string idMagatzem, string empresaPertanyent, string nomMagatzem, Adreça ubicacio, List<string> zones, bool magatzemPerDefecte)
        {
            MagatzemID = idMagatzem;
            EmpresaPertanyentID = empresaPertanyent;
            NomMagatzem = nomMagatzem;
            Ubicacio = ubicacio;
            Zones = zones;
            MagatzemPerDefecte = magatzemPerDefecte;
        }
    }

}
