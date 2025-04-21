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
        [FirestoreProperty]
        public int IdMagatzem { get; set; }

        [FirestoreProperty]
        public int EmpresaPertanyent { get; set; }

        [FirestoreProperty]
        public string NomMagatzem { get; set; }

        [FirestoreProperty]
        public Adreça Ubicacio { get; set; }

        [FirestoreProperty]
        public List<int> Zones { get; set; } = new List<int>();

        [FirestoreProperty]
        public bool MagatzemPerDefecte { get; set; }

        public Magatzems() { }

        public Magatzems(int idMagatzem, int empresaPertanyent, string nomMagatzem, Adreça ubicacio, List<int> zones, bool magatzemPerDefecte)
        {
            IdMagatzem = idMagatzem;
            EmpresaPertanyent = empresaPertanyent;
            NomMagatzem = nomMagatzem;
            Ubicacio = ubicacio;
            Zones = zones;
            MagatzemPerDefecte = magatzemPerDefecte;
        }
    }

}
