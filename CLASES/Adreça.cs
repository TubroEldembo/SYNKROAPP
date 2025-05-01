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

        // Guardamos el enum como string en Firestore
        [FirestoreProperty(Name = "TipusVia")]
        public string TipusViaString
        {
            get => TipusVia.ToString();
            set => TipusVia = Enum.TryParse(value, out TipusVia tipus) ? tipus : TipusVia.Carrer;
        }

        // Propiedad que no se almacena en Firestore, pero se usa en C#
        public TipusVia TipusVia { get; set; }

        [FirestoreProperty]
        public string Nom { get; set; }

        [FirestoreProperty]
        public int Numero { get; set; }

        [FirestoreProperty]
        public string CodiPostal { get; set; }

        [FirestoreProperty]
        public string Provincia { get; set; }

        [FirestoreProperty]
        public string Pais { get; set; }

        public Adreça() { }

        public Adreça(int adreçaID, TipusVia tipusVia, string nom, int numero, string codiPostal, string provincia, string pais)
        {
            AdreçaID = adreçaID;
            TipusVia = tipusVia;
            Nom = nom;
            Numero = numero;
            CodiPostal = codiPostal;
            Provincia = provincia;
            Pais = pais;
        }
        public override string ToString()
        {
            return $" {TipusVia} {Nom}, {Numero}, {CodiPostal}, {Provincia}, {Pais}";
        }

    }
}
