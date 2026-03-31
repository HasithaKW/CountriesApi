using System.Text.Json.Serialization;

namespace CountriesApi.Models
{
   public class CountryApiResponse
    {
        [JsonPropertyName("name")]
        public CountryName? Name { get; set; }

        [JsonPropertyName("cca2")]
        public string? Code { get; set; }

        [JsonPropertyName("capital")]
        public List<string>? Capital { get; set; }

        [JsonPropertyName("population")]
        public long? Population { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }
    }

    // Nested class for the "name" object inside the API response
    public class CountryName
    {
     
        [JsonPropertyName("common")]
        public string? Common { get; set; }
    }
}
