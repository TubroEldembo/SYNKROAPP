using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    public enum TipusVia
    {
        Carrer,
        Avinguda,
        Travesia,
        Placa,
        Carretera
    }

    [FirestoreData]
    public class Adreça
    {
        [FirestoreProperty]
        public int AdreçaID { get; set; }

        [FirestoreProperty]
        public string CodiPostal { get; set; }

        //[FirestoreProperty]
        //public string Provincia { get; set; }

        [FirestoreProperty]
        public string Pais { get; set; }

        [FirestoreProperty]
        public string Direccio { get; set; }  // ← NUEVO

        [FirestoreProperty]
        public double Latitud { get; set; }   // ← NUEVO

        [FirestoreProperty]
        public double Longitud { get; set; }  // ← NUEVO

        public Adreça() { }

        public Adreça(int adreçaID, TipusVia tipusVia, string nom, int numero, string codiPostal, string provincia, string pais, string direccio, double latitud, double longitud)
        {
            AdreçaID = adreçaID;
            CodiPostal = codiPostal;
            //Provincia = provincia;
            Pais = pais;
            Direccio = direccio;
            Latitud = latitud;
            Longitud = longitud;

        }
     

    }
}
