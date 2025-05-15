using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.GOOGLE_PLACES_SERVICE
{
    class GooglePlacesService
    {
        private readonly string apiKey = "AIzaSyCkwiE3hkfm81sNW_sLR16cv4Dh8B9EU-g";
        private readonly HttpClient client = new HttpClient();

        public async Task<List<string>> AutocompletarCiudadAsync(string input, string pais)
        {
            string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={input}&key={apiKey}&language=es&components=country:{pais}";

            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);

            var resultados = new List<string>();
            foreach (var prediccion in json["predictions"])
            {
                resultados.Add(prediccion["description"].ToString());
            }

            return resultados;
        }

        public async Task<List<PaisAutocompleteResult>> AutocompletarPaisAsync(string input)
        {
            string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={input}&types=country&key={apiKey}&language=es";

            string response = await client.GetStringAsync(url);
            JObject json = JObject.Parse(response);

            var resultados = new List<PaisAutocompleteResult>();
            foreach (var prediccion in json["predictions"])
            {
                resultados.Add(new PaisAutocompleteResult
                {
                    Description = prediccion["description"]?.ToString(),
                    PlaceId = prediccion["place_id"]?.ToString()
                });
            }

            return resultados;
        }

        public async Task<string> ObtenerCodigoIsoDesdePlaceId(string placeId)
        {
            string url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={apiKey}&language=es";

            string response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);

            var addressComponents = json["result"]?["address_components"];
            if (addressComponents != null)
            {
                foreach (var comp in addressComponents)
                {
                    var types = comp["types"]?.Select(t => t.ToString()).ToList();
                    if (types != null && types.Contains("country"))
                    {
                        return comp["short_name"]?.ToString(); // Este es el código ISO, como "ES"
                    }
                }
            }

            return null;
        }


        public async Task<(double lat, double lng)> ObtenerCoordenadasAsync(string direccion)
        {
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(direccion)}&key={apiKey}";
            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);

            var location = json["results"][0]["geometry"]["location"];
            double lat = (double)location["lat"];
            double lng = (double)location["lng"];

            return (lat, lng);
        }
    }
}
