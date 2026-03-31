using CountriesApi.Models;

namespace CountriesApi.Repositories
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAllAsync();
        Task<Country?> GetByCodeAsync(string code);
        Task<Country?> GetByIdAsync(int id);
        Task InsertAsync(Country country);
    }
}