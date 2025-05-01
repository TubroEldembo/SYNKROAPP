using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    [FirestoreData]
    public class Usuaris
    {
        [FirestoreDocumentId]
        public string UsuariID { get; set; }

        [FirestoreProperty]
        public string Nom { get; set; }

        [FirestoreProperty]
        public string Cognoms { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }

        [FirestoreProperty]
        public string Rol { get; set; }

        [FirestoreProperty]
        public string EmpresaID { get; set; }

        [FirestoreProperty]
        public string FotoPerfilUrl { get; set; } 

        public Usuaris() { }

        public Usuaris(string usuariID, string nom, string cognoms, string email, string password, string rol, string empresaID, string fotoPerfilUrl = "")
        {
            UsuariID = usuariID;
            Nom = nom;
            Cognoms = cognoms;
            Email = email;
            Password = password;
            Rol = rol;
            EmpresaID = empresaID;
            FotoPerfilUrl = fotoPerfilUrl;
        }
    }
}
