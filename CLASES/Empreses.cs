using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData] 
    public class Empreses
    {
        [FirestoreProperty]
        public string EmpresaID { get; set; }  

        [FirestoreProperty]
        public bool EstatVerificacio { get; set; }

        [FirestoreProperty]
        public string NomEmpresa { get; set; } 

        [FirestoreProperty]
        public string Tipus { get; set; } 

        [FirestoreProperty]
        public List<int> Magatzems { get; set; } = new List<int>(); 

        [FirestoreProperty]
        public List<int> Usuaris { get; set; } = new List<int>(); 

        [FirestoreProperty]
        public Adreça Ubicacio { get; set; }

        [FirestoreProperty]
        public string FotoEmpresalUrl { get; set; }

        public Empreses() { }

        public Empreses(string empresaID, bool estatVerificacio, string nomEmpresa, string tipus, List<int> magatzems, List<int> usuaris, Adreça ubicacio, string fotoEmpresalUrl)
        {
            EmpresaID = empresaID;
            EstatVerificacio = estatVerificacio;
            NomEmpresa = nomEmpresa;
            Tipus = tipus;
            Magatzems = magatzems;
            Usuaris = usuaris;
            Ubicacio = ubicacio;
            FotoEmpresalUrl = fotoEmpresalUrl;
        }
    }
}
