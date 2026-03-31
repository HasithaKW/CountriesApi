using System.Text.Json;
using CountriesApi.Models;

namespace CountriesApi.ExternalApis
{
    public class RestCountriesClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RestCountriesClient> _logger;

        public RestCountriesClient(HttpClient httpClient, ILogger<RestCountriesClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CountryApiResponse?> GetCountryByCodeAsync(string code)
        {
            try
            {
                var url = $"https://restcountries.com/v3.1/alpha/{code}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API returned non-success status: {StatusCode} for code {Code}",
                        response.StatusCode, code);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<List<CountryApiResponse>>(json, options);

                return data?.FirstOrDefault();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error while calling REST Countries API for code {Code}", code);
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timeout while calling REST Countries API for code {Code}", code);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error for code {Code}", code);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching country for code {Code}", code);
                return null;
            }
        }
    }
}