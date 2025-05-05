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
        [FirestoreDocumentId]
        public string EmpresaID { get; set; }  

        [FirestoreProperty]
        public bool EstatVerificacio { get; set; }

        [FirestoreProperty]
        public string NomEmpresa { get; set; } 

        [FirestoreProperty]
        public string Tipus { get; set; } 

        [FirestoreProperty]
        public List<string> Magatzems { get; set; } = new List<string>(); 

        [FirestoreProperty]
        public List<string> Usuaris { get; set; } = new List<string>(); 

        [FirestoreProperty]
        public Adreça Ubicacio { get; set; }

        [FirestoreProperty]
        public string FotoEmpresalUrl { get; set; }

        public Empreses() { }

        public Empreses(string empresaID, bool estatVerificacio, string nomEmpresa, string tipus, List<string> magatzems, List<string> usuaris, Adreça ubicacio, string fotoEmpresalUrl)
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
