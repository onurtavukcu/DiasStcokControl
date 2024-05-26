
using DiasAPI.CheckDbExtension;
using DiasDataAccessLayer.EntityDbContext;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace DiasAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); 

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ProductDbContext>
             (
                options => options
                .UseNpgsql
                (
                    builder.Configuration.GetConnectionString("DiasStockControlDB"),
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName))
                    .LogTo(Console.WriteLine)
                );

            builder.Services.AddAndMigrateDatabase();

            builder.Services.AddHangfire(conf =>
            {
                conf.UseInMemoryStorage();
                conf.UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
            });

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
