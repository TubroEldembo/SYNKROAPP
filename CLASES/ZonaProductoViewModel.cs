using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.CLASES
{
    public class ZonaProductoViewModel
    {
        public string Almacen { get; set; } // de ZonaEmmagatzematge.MagatzemPertanyent
        public string Zona { get; set; }    // de ZonaEmmagatzematge.Nom
        public int QuantitatDisponible { get; set; } // lo obtienes desde Firestore, DAO, etc.
    }

}
