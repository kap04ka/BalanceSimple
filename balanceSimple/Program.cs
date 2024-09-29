using balanceSimple.Services;

namespace balanceSimple
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.Services.AddTransient<ICalculatorService, CalculatorService>();

            // Configure the HTTP request pipeline.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder => builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.MapControllers();

            app.Run();
        }
    }
}