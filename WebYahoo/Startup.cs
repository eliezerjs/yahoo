using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StockService.Data;
using StockService.Services;


namespace StockService
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Adiciona o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de estoque", Version = "v1" });
            });

            services.AddDbContext<StockDbContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

            services.AddHttpClient<YahooFinanceService>();

            services.AddDbContext<StockDbContext>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            // Adiciona o middleware para servir o Swagger gerado como um endpoint JSON.
            app.UseSwagger();

            // Adiciona o middleware para servir a UI do Swagger gerada.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de estoque v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
