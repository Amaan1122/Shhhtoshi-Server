
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shhhtoshi.Api.DB;
using System.Text;

namespace ShhhToshiApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add DB Context

            //builder.Services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
            //});

            // Use environment variable for connection string
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Configure the application to listen on a specific port
            // Use the PORT environment variable or default to 5000
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

            // Adding CORS policy

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApplication",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAngularApplication");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
