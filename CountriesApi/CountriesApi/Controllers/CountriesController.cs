using Microsoft.AspNetCore.Mvc;
using CountriesApi.Services;
using CountriesApi.Models;

namespace CountriesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly CountryService _service;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(CountryService service, ILogger<CountriesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<List<Country>>> GetAll()
        {
            try
            {
                var countries = await _service.GetAllAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all countries");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/countries/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            try
            {
                var country = await _service.GetByIdAsync(id);

                if (country == null)
                    return NotFound($"Country with ID {id} not found");

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching country with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/countries/code/LK
        [HttpGet("code/{code}")]
        public async Task<ActionResult<Country>> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Country code is required");

            try
            {
                var country = await _service.GetByCodeAsync(code);

                if (country == null)
                    return NotFound($"Country with code {code} not found");

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching country with code {code}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}