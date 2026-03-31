namespace CountriesApi.Models
{
   public class Country
    {
        
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Capital { get; set; }
        public long? Population { get; set; }
        public string? Region { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}