using CountriesApi.Models;
using CountriesApi.Repositories;
using CountriesApi.ExternalApis;

namespace CountriesApi.Services
{
    public class CountryService
    {
        private readonly ICountryRepository _repository;
        private readonly RestCountriesClient _apiClient;
        private readonly ILogger<CountryService> _logger;

        public CountryService(
            ICountryRepository repository,
            RestCountriesClient apiClient,
            ILogger<CountryService> logger)
        {
            _repository = repository;
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all countries");
                return new List<Country>();
            }
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching country by ID {Id}", id);
                return null;
            }
        }

        // Caching logic - check DB first, then external API
        public async Task<Country?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            code = code.ToUpper();

            try
            {
                // 1. Check DB first
                var existing = await _repository.GetByCodeAsync(code);
                if (existing != null)
                    return existing;

                // 2. Call external API
                var apiData = await _apiClient.GetCountryByCodeAsync(code);
                if (apiData == null)
                    return null;

                // 3. Map API response to Country model
                var country = new Country
                {
                    Code = apiData.Code ?? code,
                    Name = apiData.Name?.Common ?? "Unknown",
                    Capital = apiData.Capital?.FirstOrDefault(),
                    Population = apiData.Population,
                    Region = apiData.Region
                };

                // 4. Save to DB
                await _repository.InsertAsync(country);

                // 5️⃣ Return the same object (no need to query again)
                var savedCountry = await _repository.GetByCodeAsync(code);
                return savedCountry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByCodeAsync for code {Code}", code);
                return null;
            }
        }
    }
}