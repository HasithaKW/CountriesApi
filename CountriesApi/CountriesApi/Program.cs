using CountriesApi.Repositories;
using CountriesApi.ExternalApis;
using CountriesApi.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddHttpClient<RestCountriesClient>();
builder.Services.AddScoped<CountryService>();



// Add controllers

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
