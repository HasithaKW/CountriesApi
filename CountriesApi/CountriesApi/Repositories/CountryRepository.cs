using CountriesApi.Models;
using Microsoft.Data.SqlClient;

namespace CountriesApi.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CountryRepository> _logger;

        public CountryRepository(IConfiguration configuration, ILogger<CountryRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _logger = logger;
        }

        // Get all countries
        public async Task<List<Country>> GetAllAsync()
        {
            var countries = new List<Country>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = "SELECT Id,Code,Name,Capital,Population,Region,CreatedAt FROM Countries";

                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    countries.Add(MapToCountry(reader));
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error while fetching all countries");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching all countries");
                throw;
            }

            return countries;
        }

        // Get by Code
        public async Task<Country?> GetByCodeAsync(string code)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = "SELECT Id,Code,Name,Capital,Population,Region,CreatedAt FROM Countries WHERE Code = @Code";

                using var command = new SqlCommand(query, connection);
                command.Parameters.Add("@Code", System.Data.SqlDbType.NVarChar).Value = code;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapToCountry(reader);
                }

                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error while fetching country by code {Code}", code);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching country by code {Code}", code);
                throw;
            }
        }

        // Get by ID
        public async Task<Country?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = "SELECT Id,Code,Name,Capital,Population,Region,CreatedAt FROM Countries WHERE Id = @Id";

                using var command = new SqlCommand(query, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapToCountry(reader);
                }

                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error while fetching country by ID {Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching country by ID {Id}", id);
                throw;
            }
        }

        // Insert into DB
        public async Task InsertAsync(Country country)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO Countries (Code, Name, Capital, Population, Region)
                    VALUES (@Code, @Name, @Capital, @Population, @Region)";

                using var command = new SqlCommand(query, connection);

                command.Parameters.Add("@Code", System.Data.SqlDbType.NVarChar).Value = country.Code;
                command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = country.Name;
                command.Parameters.Add("@Capital", System.Data.SqlDbType.NVarChar).Value = (object?)country.Capital ?? DBNull.Value;
                command.Parameters.Add("@Population", System.Data.SqlDbType.BigInt).Value = (object?)country.Population ?? DBNull.Value;
                command.Parameters.Add("@Region", System.Data.SqlDbType.NVarChar).Value = (object?)country.Region ?? DBNull.Value;

                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error while inserting country {Code}", country.Code);

                // Handle duplicate key (UNIQUE constraint)
                if (ex.Number == 2627) // Unique constraint violation
                {
                    _logger.LogWarning("Duplicate country code detected: {Code}", country.Code);
                    return;
                }

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while inserting country {Code}", country.Code);
                throw;
            }
        }

        // Map DB row → Country object
        private Country MapToCountry(SqlDataReader reader)
        {
            return new Country
            {
                Id = (int)reader["Id"],
                Code = reader["Code"].ToString()!,
                Name = reader["Name"].ToString()!,
                Capital = reader["Capital"] as string,
                Population = reader["Population"] as long?,
                Region = reader["Region"] as string,
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }
    }
}