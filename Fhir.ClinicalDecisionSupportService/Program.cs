using Fhir.ClinicalDecisionSupportService.Models;
using Fhir.ClinicalDecisionSupportService.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;

namespace Fhir.ClinicalDecisionSupportService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables().Build();

            builder.Services.Configure<CorsOptions>(options =>
            {
                options.AddPolicy(
                    "WithCredentialsAnyOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST", "OPTIONS");
                    });
            });

            // Add services to the container.
            builder.Services.AddScoped<IFhirDataService, FhirDataService>();
            builder.Services.AddScoped<ICardService, CardService>();

            builder.Services.AddControllers();

             builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo     {
                    Version = _configuration["Application:Version"],
                    Title = _configuration["Application:Title"],
                    Description = _configuration["Application:Description"],
                    TermsOfService = new Uri(_configuration["Application:TermsUri"]),
                    Contact = new OpenApiContact {
                                    Name = _configuration["Application:ContactName"],
                                    Url = new Uri(_configuration["Application:ContactUri"]),
                                    Email = _configuration["Application:ContactEmail"]},


                    License = new OpenApiLicense {
                                    Name = _configuration["Application:License"],
                                    Url = new Uri(_configuration["Application:LicenseUrl"]) }

                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}