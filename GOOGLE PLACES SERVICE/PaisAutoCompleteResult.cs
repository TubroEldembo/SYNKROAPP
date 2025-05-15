using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.GOOGLE_PLACES_SERVICE
{
    public class PaisAutocompleteResult
    {
        public string Description { get; set; }
        public string PlaceId { get; set; }

        public override string ToString()
        {
            return Description; // Así se mostrará en el ListBox
        }
    }

}
