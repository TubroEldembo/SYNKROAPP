using Google.Cloud.Firestore;
using System;

namespace SYNKROAPP.CLASES
{
    public enum TipusMoviment
    {
        Entrada, // Compra
        Sortida, // Venta
        TrasllatIntern // Movimiento interno (en tu p
    }

    [FirestoreData]
    public class MovimentsInventari
    {
        [FirestoreDocumentId]
        public string MovimentID { get; set; }

        [FirestoreProperty]
        public string ProducteInventariID { get; set; }

        [FirestoreProperty]
        public string EmpresaIDOrigen { get; set; }

        [FirestoreProperty]
        public string EmpresaIDDesti { get; set; }

        [FirestoreProperty]
        public string MagatzemIDOrigen { get; set; }

        [FirestoreProperty]
        public string MagatzemIDDesti { get; set; }

        [FirestoreProperty]
        public string ZonaOrigenID { get; set; }

        [FirestoreProperty]
        public string ZonaDestiID { get; set; }

        // Usamos una propiedad string intermedia para Firebase
        [FirestoreProperty("Tipus")]
        public string TipusString
        {
            get => Tipus.ToString();
            set => Tipus = Enum.TryParse(value, out TipusMoviment tipus) ? tipus : TipusMoviment.Entrada;
        }

        public TipusMoviment Tipus { get; set; }

        [FirestoreProperty]
        public int Quantitat { get; set; }

        [FirestoreProperty]
        public DateTime Data { get; set; }

        [FirestoreProperty]
        public string UsuariID { get; set; }

        [FirestoreProperty]
        public string Notes { get; set; }

        public MovimentsInventari() { }

        public MovimentsInventari(string producteInventariID, string empresaID, string magatzemIDOrigen, string magatzemIDDesti, string zonaOrigenID, string zonaDestiID,
                           TipusMoviment tipus, int quantitat, DateTime data, string usuariID, string notes = "")
        {
            ProducteInventariID = producteInventariID;
            EmpresaIDOrigen = empresaID;
            EmpresaIDDesti = empresaID;
            MagatzemIDOrigen = magatzemIDOrigen;
            MagatzemIDDesti = magatzemIDDesti;
            ZonaOrigenID = zonaOrigenID;
            ZonaDestiID = zonaDestiID;
            Tipus = tipus;
            Quantitat = quantitat;
            Data = data;
            UsuariID = usuariID;
            Notes = notes;
        }

    }
}
